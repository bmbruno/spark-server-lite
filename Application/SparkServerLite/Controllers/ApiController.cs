using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SparkServerLite.Infrastructure;
using SparkServerLite.Infrastructure.Enums;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System.Collections.Generic;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using Image = SixLabors.ImageSharp.Image;

namespace SparkServerLite.Controllers
{
    public class ApiController : Controller
    {
        private readonly IBlogRepository<Blog> _blogRepo;
        private readonly IAppSettings _settings;
        private readonly IWebHostEnvironment _host;

        private readonly string[] validFileExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public ApiController(IBlogRepository<Blog> blogRepo, IAppSettings settings, IWebHostEnvironment host)
        {
            _blogRepo = blogRepo;
            _settings = settings;
            _host = host;
        }

        [HttpPost]
        public JsonResult MarkdownToHTML(IFormCollection form)
        {
            JsonPayload json = new JsonPayload();
            json.Status = JsonStatus.OK.ToString();

            string markdown = WebUtility.UrlDecode(form["markdown"]);

            try
            {
                json.Data = FormatHelper.MarkdownToHTML(markdown, _settings.SiteURL);
            }
            catch (Exception exc)
            {
                json.Status = JsonStatus.EXCEPTION.ToString();
                json.Message = exc.ToString();
            }

            return Json(json);
        }

        [HttpGet]
        public JsonResult BlogMedia(int blogID)
        {
            JsonPayload json = new JsonPayload();
            MediaManager manager = new MediaManager(_settings, _host);

            // Load blog and validate data
            Blog blog = _blogRepo.Get(blogID);

            if (blog == null)
            {
                json.Status = JsonStatus.ERROR.ToString();
                json.Message = $"No blog found for ID {blogID}.";
                return Json(json);
            }

            if (String.IsNullOrEmpty(blog.MediaFolder))
            {
                json.Status = JsonStatus.OK.ToString();
                json.Message = $"No media folder stored for Blog ID {blogID}.";
                json.Data = new string[0];
                return Json(json);
            }

            // Load list of files from media folder
            try
            {
                List<MediaItem> list = manager.GetMediaForBlog(blog.MediaFolder);

                // Blank out server path for client-side data
                foreach (MediaItem item in list)
                {
                    item.ServerPath = null;
                }

                json.Status = JsonStatus.OK.ToString();
                json.Message = string.Empty;
                json.Data = list;
            }
            catch (Exception exc)
            {
                json.Status = JsonStatus.EXCEPTION.ToString();
                json.Message = exc.Message;
                json.Data = null;
            }

            return Json(json);
        }

        [HttpPost]
        public JsonResult UploadMedia(IFormCollection form)
        {
            MediaManager media = new MediaManager(_settings, _host);
            JsonPayload json = new JsonPayload();
            int filesUploaded = 0;
            string mediaFolder = string.Empty;

            if (form.Files.Count == 0)
            {
                json.Status = JsonStatus.ERROR.ToString();
                json.Message = "No files selected. Please select one or more files to upload.";
                json.Data = null;

                return Json(json);
            }

            // Validate file types
            foreach (IFormFile file in form.Files)
            {
                if (!validFileExtensions.Contains(Path.GetExtension(file.FileName.ToLower())))
                {
                    json.Status = JsonStatus.ERROR.ToString();
                    json.Message = $"File '{file.FileName}' must be an image type! Allowed types are: jpg, gif, png, webp";
                    json.Data = null;

                    return Json(json);
                }
            }

            int blogID = Convert.ToInt32(form["blogID"]);

            // Check if this blog already has a media folder; if so, use that
            Blog existingBlog = _blogRepo.Get(blogID);

            if (String.IsNullOrEmpty(existingBlog.MediaFolder))
            {
                // If not, create a new media folder(and save the value to the Blog record)
                existingBlog.MediaFolder = media.CreateMediaFolderForBlog(existingBlog.CreateDate.Year);
                _blogRepo.Update(existingBlog);
            }

            foreach (IFormFile file in form.Files)
            {
                // Lightly sanitize the filename (prevent folder injection)
                string fileName = file.FileName.Replace(@"/", string.Empty).Replace(@"\", string.Empty);
                string filePath = Path.Combine(_host.ContentRootPath, _settings.ServerWWWRoot, _settings.MediaFolderPath, existingBlog.MediaFolder, fileName);

                // Overwrite existing media automatically
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);                    

                // Save image to disk
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                // Create thumbnail
                using (Image image = Image.Load(filePath))
                {
                    try
                    {
                        // Preserve aspect ratio
                        double maxWidth = 600;
                        double maxHeight = 600;

                        var ratioX = (double)maxWidth / image.Width;
                        var ratioY = (double)maxHeight / image.Height;
                        var ratio = Math.Min(ratioX, ratioY);

                        var newWidth = (int)(image.Width * ratio);
                        var newHeight = (int)(image.Height * ratio);

                        image.Mutate(x => x.Resize(newWidth, newHeight));
                    }
                    catch (Exception exc)
                    {
                        json.Status = JsonStatus.EXCEPTION.ToString();
                        json.Message = exc.Message.ToString();
                        return Json(json);
                    }

                    // Get thumbnail file path
                    string thumbnailPath = media.GetThumbnailFilename(filePath);
                    image.SaveAsJpeg(thumbnailPath);
                }
            }

            json.Status = JsonStatus.OK.ToString();
            json.Message = $"{filesUploaded} files uploaded.";

            return Json(json);
        }

        [HttpPost]
        public JsonResult UploadLibraryMedia(IFormCollection form)
        {
            MediaManager media = new MediaManager(_settings, _host);
            JsonPayload json = new JsonPayload();
            int filesUploaded = 0;
            string libraryFolder = string.Empty;

            if (form.Files.Count == 0)
            {
                json.Status = JsonStatus.ERROR.ToString();
                json.Message = "No files selected. Please select one or more files to upload.";
                json.Data = null;

                return Json(json);
            }

            // Validate file types
            foreach (IFormFile file in form.Files)
            {
                if (!validFileExtensions.Contains(Path.GetExtension(file.FileName.ToLower())))
                {
                    json.Status = JsonStatus.ERROR.ToString();
                    json.Message = $"File '{file.FileName}' must be an image type! Allowed types are: jpg, gif, png, webp";
                    json.Data = null;

                    return Json(json);
                }
            }

            foreach (IFormFile file in form.Files)
            {
                // Lightly sanitize the filename (prevent folder injection)
                string fileName = file.FileName.Replace(@"/", string.Empty).Replace(@"\", string.Empty);
                string filePath = Path.Combine(_host.ContentRootPath, _settings.ServerWWWRoot, _settings.LibraryMediaPath, fileName);

                // Overwrite existing media automatically
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                // Save image to disk
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                // Create thumbnail
                using (Image image = Image.Load(filePath))
                {
                    try
                    {
                        // Preserve aspect ratio
                        double maxWidth = 600;
                        double maxHeight = 600;

                        var ratioX = (double)maxWidth / image.Width;
                        var ratioY = (double)maxHeight / image.Height;
                        var ratio = Math.Min(ratioX, ratioY);

                        var newWidth = (int)(image.Width * ratio);
                        var newHeight = (int)(image.Height * ratio);

                        image.Mutate(x => x.Resize(newWidth, newHeight));
                    }
                    catch (Exception exc)
                    {
                        json.Status = JsonStatus.EXCEPTION.ToString();
                        json.Message = exc.Message.ToString();
                        return Json(json);
                    }

                    // Get thumbnail file path
                    string thumbnailPath = media.GetThumbnailFilename(filePath);
                    image.SaveAsJpeg(thumbnailPath);
                }
            }

            json.Status = JsonStatus.OK.ToString();
            json.Message = $"{filesUploaded} files uploaded.";

            return Json(json);
        }

        [HttpPost]
        public JsonResult DeleteMedia(int blogID, string filename)
        {
            JsonPayload json = new JsonPayload();
            MediaManager manager = new(_settings, _host);

            Blog blog = _blogRepo.Get(blogID);

            if (blog == null)
            {
                json.Status = JsonStatus.ERROR.ToString();
                json.Message = $"No blog found for ID {blogID}.";
                return Json(json);
            }

            List<MediaItem> list = manager.GetMediaForBlog(blog.MediaFolder);

            MediaItem? item = list.FirstOrDefault(u => u.Filename.ToLower() == filename.ToLower());

            if (item == null)
            {
                json.Status = JsonStatus.ERROR.ToString();
                json.Message = $"No media item found for blog/filename {blogID}/{filename}.";
                return Json(json);
            }

            manager.DeleteMedia(item.ServerPath);

            json.Status = JsonStatus.OK.ToString();
            json.Message = $"Media item '{filename}' deleted.";

            return Json(json);
        }

        [HttpPost]
        public JsonResult DeleteLibraryMedia(string filename)
        {
            JsonPayload json = new JsonPayload();
            MediaManager manager = new MediaManager(_settings, _host);

            string libraryMediaPath = Path.Combine(_host.ContentRootPath, _settings.ServerWWWRoot, _settings.LibraryMediaPath, filename);

            if (String.IsNullOrEmpty(libraryMediaPath))
            {
                json.Status = JsonStatus.ERROR.ToString();
                json.Message = $"No media found at the following path: {libraryMediaPath}";
                return Json(json);
            }

            manager.DeleteMedia(libraryMediaPath);

            json.Status = JsonStatus.OK.ToString();
            json.Message = $"Media item '{filename}' deleted.";

            return Json(json);
        }

        public JsonResult GetNextBlogBanner()
        {
            JsonPayload json = new JsonPayload();
            MediaManager media = new MediaManager(_settings, _host);

            try
            {
                string latestBlogBanner = _blogRepo.GetLatestBlogBanner(_settings.BlogBannerPath);

                if (String.IsNullOrEmpty(latestBlogBanner))
                {
                    json.Status = JsonStatus.ERROR.ToString();
                    json.Message = "No latest blog banner found.";
                    return Json(json);
                }

                // Strip extension from the file; increment to next EXPECTED (ie sequential) banner image filename
                int nextBannerNumber = Convert.ToInt32(latestBlogBanner.Split(".")[0]) + 1;

                // Increment to the next banner (see if file exists); if next files doesn't exists, return to 01 position
                string formatPath = Path.Combine(_host.ContentRootPath, _settings.ServerWWWRoot, _settings.BlogBannerPath, "{0:00}.jpg");
                string newFilename = string.Format(formatPath, nextBannerNumber);

                if (!System.IO.File.Exists(newFilename))
                {
                    nextBannerNumber = 1;
                }

                // Format using web-friendly path to blog-banner folder
                formatPath = $"/{_settings.BlogBannerPath}/{{0:00}}.jpg";
                newFilename = string.Format(formatPath, nextBannerNumber);

                string newFilenameThumbnail = media.GetThumbnailFilename(newFilename);

                json.Status = JsonStatus.OK.ToString();
                json.Data = new string[] { newFilename, newFilenameThumbnail };

            }
            catch (Exception exc)
            {
                json.Status = JsonStatus.EXCEPTION.ToString();
                json.Message = exc.Message.ToString();
            }

            return Json(json);
        }

        [HttpGet]
        public JsonResult BlogMediaFolderList()
        {
            // TODO: return a list of blog media folders with the following columns: Blog Title, Date Published, Media Folder ID
            JsonPayload json = new();

            return Json(json);
        }

        public JsonResult LibraryList()
        {
            JsonPayload json = new();
            MediaManager media = new(_settings, _host);

            try
            {
                List<MediaItem> library = media.GetLibraryMedia();

                // Blank out server path for client-side data
                foreach (MediaItem item in library)
                    item.ServerPath = null;

                json.Status = JsonStatus.OK.ToString();
                json.Message = null;
                json.Data = library;
            }
            catch (Exception exc)
            {
                json.Status = JsonStatus.EXCEPTION.ToString();
                json.Message = $"Error loading library media. Exception: {exc.Message}";
                json.Data = null;
            }

            return Json(json);
        }
    }
}