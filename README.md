# Spark Server Lite

A simple blog management and hosting platform built on SQLite and ASP.NET Core MVC (.NET 6).

## About

Blogs posts are written with Markdown.

## Setup

To set up SparkServerLite:

1) Ensure a .NET 6 runtime is installed.

2) Run SQL build scripts (from a terminal) to create the SQLite database (includes test data for local development).

   * Windows: `/SQLScripts/RunAllCreate.bat`
   * MacOS: `sh /SQLScripts/RunAllCreateMacOS`
	
3) Run `dotnet build` then `dotnet run` to start the local server.

## Technical Information

### Standard Media Sizes

* Blog Image Banner: 2000px x 1000px @ 96dpi
* Blog Banner Thumb: 600px x 300px @ 96dpi

Default images (as JPG) for blog banners should be named with sequential integers (01, 02, 03... 22, 23, etc.) in the following folder:

```
/images/banner-images/xx.jpg
/images/banner-images/xx_thumbnail.jpg
```

### App Settings descriptors

#### Site Content

* **Sitename**: Displayable name of the website.
* **SiteDescription**: Displayable description of the website. Used in metadata fields, under main logo, and footer.
* **SiteURL**: Resolvable URL of the site, including protocal and hostname (`https://www.myblog.com`).
 
#### Backend Server configuration

* **MediaFolderServerPath**: Relative server-addressable path to the media folder.
* **MediaFolderWebPath**: Absolute addressable path to the media from the root web directory.
* **DatabaseConnectionString**: Connection string for the SQLite database.
* **BlogItemsPerPage**: Number of blog items to display per page.
 

* **BlogBannerServerPath**: Absolute addressable path to the default blog banners folder.
* **BlogBannerWebPath**: Absolute addressable path to the blog banners from the root web directory.
* **DefaultBlogBannerPath**: Absolute addressable path to the default blog banner image.
* **DefaultBlogBannerThumbnailPath**: Absolute addressable path to the default blog banner thumbnail image.

#### SSO Configuration

These settings are only used with the QuickSSO system.

* **SSOSigningKey**: Signing key from the QuickSSO system.
* **SSOSiteID**: SiteID value provided by the QuickSSO system.
* **SSOLoginURL**: should be set to `http://sso.brandonbruno.com/Authenticate/`
* **SSOLogoutURL**: should be set to `http://sso.brandonbruno.com/Logout/`
