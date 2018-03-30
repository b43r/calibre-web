# CalibreWeb
ASP.NET Core web-app for browsing and downloading ebooks stored in a Calibre database.

CalibreWeb works similar than the built-in Calibre content server: It displays your ebooks stored in a Calibre database on a HTML page. If you want to give someone access to you ebook catalogue, if you don't want to permanently run Calibre on your web-server, and if your web-server runs on Windows, the CalibreWeb is the right thing for you!

## Features

- Display cover images
- Display Calibre metadata title, author, language and description
- Download of all available formats
- List of authors
- Display all books of an author

Note that you **cannot** read your ebboks with CalibreWeb, but you can download them.

![CalibreWeb](https://github.com/b43r/calibre-web/raw/master/calibre-web.png "CalibreWeb screenshot")

## Getting started

Build CalibreWeb yourself from source or download the latest binary release.

Follow Microsoft documentation on how to host an ASP.NET Core web-app on IIS (https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?tabs=aspnetcore2x).

Edit *appsettings.json* and configure the path to your Calibre database:
```
  "Calibre": {
    "CataloguePath": "C:\\inetpub\\CalibreDB"
  }
```

With "Calibre database" I mean the directory containing *metadata.db* and all sub-directories containing your ebooks.
**Important:** Make sure the IIS application pool user has write access to this directory, otherwise the web-app will be very slow!
