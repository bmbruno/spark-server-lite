# Spark Server Lite

A simple blog management and hosting platform built on SQLite and ASP.NET Core MVC (.NET 6).

## About

Blogs posts are written with Markdown.

## Technical Information

### Standard Media Sizes

* Blog Image Banner: 2000px x 1000px @ 96dpi
* Blog Banner Thumb: 600px x 300px @ 96dpi

Default images (as JPG) for blog banners should be named with sequential integers (01, 02, 03... 22, 23, etc.) in the following folder:

```
/images/banner-images/xx.jpg
/images/banner-images/xx_thumb.jpg
```

### App Settings descriptors

* **Sitename**: Displayable name of the website.
* **SiteURL**: Resolvable URL of the site, including protocal and hostname (`https://www.myblog.com`).
* **MediaFolderServerPath**: Relative server-addressable path to the media folder.
* **MediaFolderWebPath**: Absolute addressable path to the media from the root web directory.
* **DatabaseConnectionString**: Connection string for the SQLite database.
* **BlogItemsPerPage**: Number of blog items to display per page.
* **BlogBannerServerPath**: Absolute addressable path to the default blog banners folder.
* **BlogBannerWebPath**: Absolute addressable path to the blog banners from the root web directory.
* **DefaultBlogBannerPath**: Absolute addressable path to the default blog banner image.
* **DefaultBlogBannerThumbnailPath**: Absolute addressable path to the default blog banner thumbnail image.