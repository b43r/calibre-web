# CalibreWeb
ASP.NET Core web-app for browsing and downloading ebooks stored in a Calibre database.

CalibreWeb works similar than the built-in Calibre content server: It displays your ebooks stored in a Calibre database on a HTML page. If you want to give someone access to you ebook catalogue, if you don't want to permanently run Calibre on your web-server, and if your web-server runs on Windows, the CalibreWeb is the right thing for you!

Note that you **cannot** read your ebboks with CalibreWeb, but you can download them.

![CalibreWeb](https://github.com/b43r/calibre-web/raw/master/calibre-web.png "CalibreWeb screenshot")

## Getting started ##

Build CalibreWeb yourself from source or download the latest binary release.

Edit *appsettings.json* and configure the path to your Calibre database:
```
  "Calibre": {
    "CataloguePath": "C:\\inetpub\\CalibreDB"
  }
```
