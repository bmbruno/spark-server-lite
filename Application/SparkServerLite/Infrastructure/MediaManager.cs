using SparkServerLite.Interfaces;
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

        /// <summary>
        /// Creates a potentially unique 12-character ID for a folder.
        /// </summary>
        /// <returns>Generated folder ID.</returns>
        private string GenerateUniqueFolderID()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12);
        }

    }
}