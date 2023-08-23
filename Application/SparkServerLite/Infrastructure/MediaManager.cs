using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System.IO;

namespace SparkServerLite.Infrastructure
{
    public class MediaManager
    {
        private readonly IAppSettings _settings;

        public MediaManager(IAppSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Generates a unique media folder path and creates it on disk (year/id).
        /// </summary>
        /// <returns>Unique media folder path. Should be stored in the datastore alonside the Blog post data.</returns>
        /// <param name="year">Year component of the CreateDate DateTime for the blog post.</param>
        public string CreateMediaFolderForBlog(int year)
        {
            string basePath = Path.Combine(_settings.MediaFolderRootPath, year.ToString());

            // Check if year folder exists; create if not
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            // Generate a unique folder ID for this blog
            string folderID = GenerateUniqueFolderID();
            string folderPath = Path.Combine(basePath, folderID);

            // Check if this folder ID already exists(collision); if yes, re - generate / re - check until unique
            while (Directory.Exists(folderPath))
            {
                folderID = GenerateUniqueFolderID();
                folderPath = Path.Combine(basePath, folderID);
            }

            Directory.CreateDirectory(folderPath);

            return Path.Combine(year.ToString(), folderID).Replace(@"\\", "/");
        }

        public List<MediaItem> GetMediaForBlog(string folderPath)
        {
            List<MediaItem> mediaList = new List<MediaItem>();
            folderPath = Path.Combine(_settings.MediaFolderRootPath, folderPath);

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Media folder not found: {folderPath}");

            string[] files = Directory.GetFiles(folderPath);

            if (files.Length == 0) { return mediaList; }

            foreach (string file in files)
            {
                mediaList.Add(new MediaItem() { 
                    Filename = Path.GetFileName(file),
                    Filetype = Path.GetExtension(file),
                    Path = FormatForURL(file),
                    ThumbnailPath = "thumbnail" // TODO: call GetThumbnailFilename()
                });
            }

            return mediaList;
        }

        /// <summary>
        /// Creates a potentially unique 12-character ID for a folder.
        /// </summary>
        /// <returns>Generated folder ID.</returns>
        private string GenerateUniqueFolderID()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12);
        }

        private string GetThumbnailFilename(string file)
        {
            // TODO: insert "_thumb" into filename
            return string.Empty;
        }

        /// <summary>
        /// Formats a server-side media path to be used as a relative URL path.
        /// </summary>
        /// <param name="input">File path from the media root.</param>
        /// <returns>Filepath formatted for use as a relative URL.</returns>
        private string FormatForURL(string input)
        {
            if (input.StartsWith("."))
                input = input.Substring(1, input.Length - 1);

            return input.Replace(@"\", "/");
        }
    }
}