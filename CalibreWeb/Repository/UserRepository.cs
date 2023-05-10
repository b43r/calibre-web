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
        private const string UserFile = "users.json";
        private const int SALT_SIZE = 24;
        private const int HASH_SIZE = 24;
        private const int ITERATIONS = 100000;

        private Dictionary<string, AppUser> userDict;

        /// <summary>
        /// Create a new instance of the user repository.
        /// </summary>
        public UserRepository() {

            string userFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UserFile);
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

                    if (updated)
                    {
                        string jsonData = JsonConvert.SerializeObject(userList, Formatting.Indented);
                        File.WriteAllText(userFile, jsonData);
                    }

                    userDict = userList.ToDictionary(k => k.Name.ToLower(), v => v);
                }
                catch
                {
                    // ignore
                }
            }
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
            if (userDict != null && userDict.TryGetValue(username.ToLower(), out var user))
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
