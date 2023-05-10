# CalibreWeb
ASP.NET Core web-app for browsing and downloading ebooks stored in a Calibre database.

CalibreWeb works similar to the built-in Calibre content server: It displays your ebooks stored in a Calibre database on a HTML page.

If you want to give someone access to you ebook catalogue, but don't want to permanently run Calibre on your Windows web-server, then CalibreWeb is the right thing for you!

## Features

- Display cover images
- Display Calibre metadata (title, author, language and description)
- Download of ebooks in all available formats
- List of authors
- Display all books of an author
- Optionally protect e-books with a login page

Note that you **cannot** use CalibreWeb to **read** your ebooks in a web-browser!

![CalibreWeb](https://github.com/b43r/calibre-web/raw/master/calibre-web.png "CalibreWeb screenshot")

## Getting started

Build CalibreWeb yourself from source or [download the latest binary release](https://github.com/b43r/calibre-web/releases/download/2.0/CalibreWeb_2_0.zip).

Follow Microsoft documentation on how to host a .NET 6.0 web-app on IIS (https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-6.0).

Edit *appsettings.json* and configure the path to your Calibre database:
```
  "Calibre": {
    "CataloguePath": "C:\\inetpub\\CalibreDB",
    "RequireLogin": false
  }
```

With "Calibre database" I mean the directory containing *metadata.db* and all sub-directories containing your ebooks.
**Important:** Make sure the IIS application pool user has write access to this directory, otherwise the web-app will be very slow!

If you use the default configuration with ```"RequireLogin": false``` everyone has access to your e-books. If you want to restrict access to certain users, read the following paragraph.

## Require login

If you set the parameter ```"RequireLogin": true``` in *appsettings.json*, all users accessing the web page are required to login with a username and password. Users are configured in a text-file *users.json*:
```
[
  {
    "name": "john.doe@example.org",
    "password": "123456",
    "salt": "",
    "role": "download"
  },
  {
    ...
  }
]
```
If you add a new user, enter the password in plaintext and add an empty "salt" value. After you have restart the application, a random "salt" will be generated and the plaintext password will be replaced with it's hash.

Possible values for "role":
. <empty> = view list of all books but downloading is not allowed
- *download* = view a list of all books and show download links
- *admin* = same as *download* (reserved for future use)
  
**Important:** You need to restart the application pool every time you have changed the *users.json* file!
