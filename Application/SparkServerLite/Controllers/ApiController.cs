using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SparkServerLite.Infrastructure;
using SparkServerLite.Infrastructure.Enums;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using Image = SixLabors.ImageSharp.Image;

namespace SparkServerLite.Controllers
{
    public class ApiController : Controller
    {
        private readonly IBlogRepository<Blog> _blogRepo;
        private readonly IBlogTagRepository<BlogTag> _blogTagRepo;
        private readonly IAuthorRepository<Author> _authorRepo;
        private readonly IAppSettings _settings;

        public ApiController(IBlogRepository<Blog> blogRepo, IBlogTagRepository<BlogTag> blogTagRepo, IAuthorRepository<Author> authorRepo, IAppSettings settings)
        {
            _blogRepo = blogRepo;
            _blogTagRepo = blogTagRepo;
            _authorRepo = authorRepo;
            _settings = settings;
        }

        [HttpPost]
        public JsonResult MarkdownToHTML(string markdown)
        {
            JsonPayload json = new JsonPayload();
            json.Status = JsonStatus.OK.ToString();

            markdown = WebUtility.UrlDecode(markdown);

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
            MediaManager manager = new MediaManager(_settings);

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
            List<MediaItem> list = manager.GetMediaForBlog(blog.MediaFolder);

            // Blank out server path for client-side data
            foreach (MediaItem item in list)
            {
                item.ServerPath = null;
            }

            json.Status = JsonStatus.OK.ToString();
            json.Message = string.Empty;
            json.Data = list;

            return Json(json);
        }

        [HttpPost]
        public JsonResult UploadMedia(IFormCollection form)
        {
            MediaManager media = new MediaManager(_settings);
            JsonPayload json = new JsonPayload();
            int filesUploaded = 0;
            string mediaFolder = string.Empty;

            if (form.Files.Count == 0)
            {
                json.Status = JsonStatus.ERROR.ToString();
                json.Message = "No files selected. Please select one or more pictures to upload.";
                json.Data = null;

                return Json(json);
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

            for (int i = 0; i < form.Files.Count; i++)
            {
                // Lightly sanitize the filename (prevent folder injection)
                string fileName = form.Files[i].FileName.Replace(@"/", string.Empty).Replace(@"\", string.Empty);
                string filePath = Path.Combine(_settings.MediaFolderServerPath, existingBlog.MediaFolder, fileName);

                // Overwrite existing media automatically
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);                    

                // Save image to disk
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    form.Files[i].CopyTo(fileStream);
                }

                // Create thumbnail
                using (Image image = Image.Load(filePath))
                {
                    try
                    {
                        // TODO: calculate new size based on image aspect ratio and current orientation

                        image.Mutate(x => x.Resize(200, 200));
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
            MediaManager manager = new MediaManager(_settings);

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

        public JsonResult GetNextBlogBanner()
        {
            JsonPayload json = new JsonPayload();

            string latestBlogBanner = _blogRepo.GetLatestBlogBanner(_settings.BlogBannerWebPath);

            if (String.IsNullOrEmpty(latestBlogBanner))
            {
                json.Status = JsonStatus.ERROR.ToString();
                json.Message = "No latest blog banner found.";
                return Json(json);
            }

            // Strip extension from the file; increment to next EXPECTED (ie sequential) banner image filename
            int nextBannerNumber = Convert.ToInt32(latestBlogBanner.Split(".")[0]) + 1;

            // Increment to the next banner (see if file exists); if next files doesn't exists, return to 01 position
            string formatPath = Path.Combine(_settings.BlogBannerServerPath, "{0:00}.jpg");
            string newFilename = string.Format(formatPath, nextBannerNumber);

            if (!System.IO.File.Exists(newFilename))
            {
                nextBannerNumber = 1;
            }

            // Format using web-friendly path to blog-banner folder
            formatPath = $"{_settings.BlogBannerWebPath}/{{0:00}}.jpg";
            newFilename = string.Format(formatPath, nextBannerNumber);

            json.Status = JsonStatus.OK.ToString();
            json.Data = newFilename;

            return Json(json);
        }
    }
}