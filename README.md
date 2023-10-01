# SparkServer Lite

A simple blog management and hosting platform built on SQLite and ASP.NET Core MVC (.NET 6).

## About

Blogs posts are written with Markdown.

## Setup

To set up SparkServer Lite:

1) Ensure a .NET 6 runtime is installed.

2) Run SQL build scripts (from a terminal) to create the SQLite database (includes test data for local development).

   * Windows: `/SQLScripts/RunAllCreate.bat`
   * MacOS: `sh /SQLScripts/RunAllCreateMacOS`
	
3) Run `dotnet build` then `dotnet run` to start the local server.

## Authentication

SparkServer Lite is configured to use a custom-built SSO system. The `AccountController` can be updated to use your login system of choice.

For local development and testing, enabling `debug` mode (see [App Settings descriptors](https://github.com/bmbruno/spark-server-lite#app-settings-descriptors)) will automatically log UserID 1 into the system. **WARNING**: do not enable debug mode on production deployments.

## Technical Information

### Standard Media Sizes

* Blog Image Banner: 2000px x 1000px @ 96dpi
* Blog Banner Thumb: 600px x 300px @ 96dpi

### Default Blog Banners

Blog posts can have default images configured. The "Get Next Banner" button on the Blog Edit page will automatically pull the next unused default image, returning to the first image once reaching the last.

Images to be used as default blog banners should be placed in the following folder and named with sequential integers (01, 02, 03... 22, 23, etc.):

```
/images/banner-images/xx.jpg
/images/banner-images/xx-thumb.jpg
```

There should a numbered file _and_ a thumbnail version.

### App Settings descriptors

#### System

* **Debug**: enables additional debug messages; also skips SSO login (use with caution!)

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
