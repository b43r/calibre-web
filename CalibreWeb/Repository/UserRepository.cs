/*
 * CalibreWeb
 * 
 * Copyright (C) 2018..2021 by Simon Baer
 *
 * This program is free software; you can redistribute it and/or modify it under the terms
 * of the GNU General Public License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with this program;
 * If not, see http://www.gnu.org/licenses/.
 * 
 */

using System.Security.Cryptography;

using Newtonsoft.Json;

using CalibreWeb.Models;

namespace CalibreWeb.Repository
{
    public class UserRepository
    {
        private const string Base64Prefix = "b64/";
        private const string UserFileName = "users.json";
        private const int SALT_SIZE = 24;
        private const int HASH_SIZE = 24;
        private const int ITERATIONS = 100000;

        private Dictionary<string, AppUser> userDict;
        private string userFile;

        /// <summary>
        /// Create a new instance of the user repository.
        /// </summary>
        public UserRepository() {

            userFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UserFileName);

            if (File.Exists(userFile))
            {
                try
                {
                    var userList = JsonConvert.DeserializeObject<AppUser[]>(File.ReadAllText(userFile));

                    bool updated = false;
                    foreach (var user in userList)
                    {
                        if (!user.Password.StartsWith(Base64Prefix) || 
                            !user.Salt.StartsWith(Base64Prefix))
                        {
                            updated = true;
                            var salt = GetSalt();
                            user.Salt = Base64Prefix + Convert.ToBase64String(salt);
                            user.Password = Base64Prefix + Convert.ToBase64String(CreateHash(user.Password, salt));
                        }
                    }

                    userDict = userList.ToDictionary(k => k.Name.ToLower(), v => v);

                    if (updated)
                    {
                        SaveUsers();
                    }
                }
                catch
                {
                    // ignore
                }
            }
        }

        /// <summary>
        /// save the list of users to a file.
        /// </summary>
        private void SaveUsers()
        {
            var userList = userDict.Values.ToList();
            string jsonData = JsonConvert.SerializeObject(userList, Formatting.Indented);
            File.WriteAllText(userFile, jsonData);
        }

		/// <summary>
		/// Returns a list of all users.
		/// </summary>
		/// <returns>list of AppUser</returns>
		public List<AppUser> GetUsers()
        {
            if (userDict != null)
            {
                return userDict.Values.ToList();
			}

            return new List<AppUser>();
        }

        /// <summary>
        /// Returns the user with the given name.
        /// </summary>
        /// <returns>AppUser or null</returns>
        public AppUser GetUser(string userName)
        {
            if (userDict != null)
            {
                if (userDict.TryGetValue(userName.ToLower(), out var appUser))
                {
                    return appUser;
                }
            }

            return null;
        }

        /// <summary>
        /// Check username and password against user repository.
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">password</param>
        /// <param name="role">returns the user role</param>
        /// <returns>true if user exists and password matches</returns>
        public bool CheckLogin(string username, string password, out string role)
        {
            if (userDict != null && userDict.TryGetValue((username ?? "").ToLower(), out var user))
            {
                var salt = Convert.FromBase64String(user.Salt.Substring(Base64Prefix.Length));
                if (Convert.ToBase64String(CreateHash(password, salt)) == user.Password.Substring(Base64Prefix.Length))
                {
                    if (user.Role.Equals(AppUserRole.Admin, StringComparison.OrdinalIgnoreCase) ||
                        user.Role.Equals(AppUserRole.Download, StringComparison.OrdinalIgnoreCase))
                    {
                        role = user.Role;
                    }
                    else
                    {
                        role = AppUserRole.None;
                    }
                    
                    return true;
                }
            }

            role = AppUserRole.None;
            return false;
        }

        /// <summary>
        /// Add a new user to the user list.
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">password</param>
        /// <param name="role">role </param>
        public void AddUser(string username, string password, string role)
        {
            var salt = GetSalt();
            var appUser = new AppUser
            {
                Name = username,
                Salt = Base64Prefix + Convert.ToBase64String(salt),
                Password = Base64Prefix + Convert.ToBase64String(CreateHash(password, salt)),
                Role = role
            };

            userDict.Add(username.ToLower(), appUser);
            SaveUsers();
        }

        /// <summary>
        /// Delete the user with the given name from the user list.
        /// </summary>
        /// <param name="username">name of user to delete</param>
        public void DeleteUser(string username)
        {
            userDict.Remove(username.ToLower());
            SaveUsers();
        }

        /// <summary>
        /// Update the role of the given user.
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="role">role</param>
        public bool UpdateRole(string username, string role)
        {
            var user = GetUser(username);
            if (user != null)
            {
                user.Role = role;
                SaveUsers();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a new random salt value.
        /// </summary>
        /// <returns>byte array</returns>
        private static byte[] GetSalt() => RandomNumberGenerator.GetBytes(SALT_SIZE);

        /// <summary>
        /// Create a hash using the given salt.
        /// </summary>
        /// <param name="input">text to hash</param>
        /// <param name="salt">salt to use</param>
        /// <returns>byte array</returns>
        private static byte[] CreateHash(string input, byte[] salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, ITERATIONS);
            return pbkdf2.GetBytes(HASH_SIZE);
        }
    }
}
