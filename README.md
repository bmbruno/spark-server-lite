# SparkServer Lite

A simple blog management and hosting platform built on SQLite and ASP.NET Core MVC (.NET 8).

## About

SparkServer Lite provides a simple blog engine that can be hosted anywhere a .NET Core application can run. A few features highlights:

* Blogs posts are written with Markdown.
* Basic media management built in.
* Minimal setup & configuration to get started.
* Self-contained application; SQLite-based data store.
* Obvious URL structure (`/posts`, `/posts/2025/10`, `/blogtags`, etc.).
* Open source: extend however you see fit.

## Setup

To set up SparkServer Lite:

1) Ensure a .NET 8 runtime is installed.

2) Run SQL build scripts (from a terminal) to create the SQLite database (includes test data for local development).

   * Windows: `/SQLScripts/RunAllCreate.bat`
   * MacOS: `sh /SQLScripts/RunAllCreateMacOS`
	
3) Run `dotnet build` then `dotnet run` to start the local server.

4) Visit `https://<hostname>/admin` to get started managing posts.

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

#### System configuration

* **Debug**: enables additional debug messages; also skips SSO login (use with caution!).
* **DatabaseConnectionString**: connection string to a SQLite database file.
* **SiteURL**: resolvable URL of the site, including protocal and hostname (`https://www.myblog.com`).
* **BlogItemsPerPage**: number of blog posts to display on each list page.
 
#### Backend server configuration

* **ServerWWWRoot**: path to the website content folder; usually 'wwwroot'
* **MediaFolderServerPath**: relative path to the media folder as viewed from the app directory.
* **BlogBannerServerPath**: relative path to the default blog banners folder as viewed from the app directory.
* **LibraryMediaServerPath**: relative path to the library image folder as viewed from the app directory.
* **DefaultBlogBannerPath**: absolute addressable path to the default blog banner image.
* **DefaultBlogBannerThumbnailPath**: absolute addressable path to the default blog banner thumbnail image.

#### SSO configuration

These settings are only used with the QuickSSO system.

* **SSOSigningKey**: signing key from the QuickSSO system.
* **SSOSiteID**: siteID value provided by the QuickSSO system.
* **SSOLoginURL**: should be set to `http://sso.brandonbruno.com/Authenticate/`
* **SSOLogoutURL**: should be set to `http://sso.brandonbruno.com/Logout/`

### Content Settings descriptors

#### Site-wide content

* **SiteTitle**: displayable name of the website.
* **SiteSubtitle**: displayable description of the website. Used in metadata fields, under main logo, and footer.
* **SiteLogoURL**: URL of the site logo image.

#### Footer content

* **Copyright**: copyright string (copyright mark and year are already provided).
* **Description**: description of the website.
* **Blurb**: message, summary, overview, or other text about the website. Appears in right column of footer.

## Contact The Author

For questions / comments / issues, contact me:

* [Brandon Bruno on LinkedIn](https://www.linkedin.com/in/brandonbruno)
* [bmbruno@gmail.com](mailto:bmbruno@gmail.com)
