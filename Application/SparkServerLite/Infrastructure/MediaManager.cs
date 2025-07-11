﻿using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System;
using System.IO;
using System.Runtime;

namespace SparkServerLite.Infrastructure
{
    public class MediaManager
    {
        private readonly IAppSettings _settings;
        private readonly IWebHostEnvironment _host;

        private const string _thumbnailAffix = "-thumb";

        public MediaManager(IAppSettings settings, IWebHostEnvironment host)
        {
            _settings = settings;
            _host = host;
        }

        /// <summary>
        /// Generates a unique media folder path and creates it on disk (year/id).
        /// </summary>
        /// <returns>Unique media folder path. Should be stored in the datastore alonside the Blog post data.</returns>
        /// <param name="year">Year component of the CreateDate DateTime for the blog post.</param>
        public string CreateMediaFolderForBlog(int year)
        {
            string basePath = Path.Combine(_host.ContentRootPath, _settings.ServerWWWRoot, _settings.MediaFolderPath, year.ToString());

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
        /// Gets a list of media items (and associated thumbnails) as relative URL file paths for the given Blog media folder.
        /// </summary>
        /// <param name="folderPath">Folder on disk where the files live.</param>
        /// <returns>List of MediaItems</returns>
        /// <exception cref="DirectoryNotFoundException">Thrown if no directory exists on-disk for the given folderPath.</exception>
        public List<MediaItem> GetMediaForBlog(string folderPath)
        {
            List<MediaItem> mediaList = new List<MediaItem>();
            string serverPath = Path.Combine(_host.ContentRootPath, _settings.ServerWWWRoot, _settings.MediaFolderPath, folderPath);

            if (!Directory.Exists(serverPath))
                throw new DirectoryNotFoundException($"[MediaManager.GetMediaForBlog] Media folder not found: {folderPath}");

            string[] files = Directory.GetFiles(serverPath);

            if (files.Length == 0) { return mediaList; }

            foreach (string file in files)
            {
                // Filter out thumbnail images from this list
                if (file.EndsWith($"{_thumbnailAffix}{Path.GetExtension(file)}"))
                    continue;

                string filename = Path.GetFileName(file);

                // Web browsers should request media from the root of the website - hence the leading slash (the 'wwwroot' folder is never known to clients)
                string webPath = Path.Combine("/", _settings.MediaFolderPath, folderPath, filename);

                mediaList.Add(new MediaItem() { 
                    Filename = filename,
                    Filetype = Path.GetExtension(file),
                    ServerPath = file,
                    WebPath = FormatForURL(webPath),
                    ThumbnailPath = FormatForURL(GetThumbnailFilename(webPath))
                });
            }

            return mediaList;
        }

        /// <summary>
        /// Gets a list of all images in the library folder. Does not return thumbnails.
        /// </summary>
        /// <returns>List of MediaItems.</returns>
        public List<MediaItem> GetLibraryMedia()
        {
            List<MediaItem> mediaList = new();

            string[] allFiles = Directory.GetFiles(Path.Combine(_host.ContentRootPath, _settings.ServerWWWRoot, _settings.LibraryMediaPath));

            foreach (string file in allFiles)
            {
                // Filter out thumbnail images from this list
                if (file.EndsWith($"{_thumbnailAffix}{Path.GetExtension(file)}"))
                    continue;

                string filename = Path.GetFileName(file);
                string webPath = Path.Combine($"/{_settings.LibraryMediaPath}", filename);

                mediaList.Add(new MediaItem()
                {
                    Filename = filename,
                    Filetype = Path.GetExtension(file),
                    ServerPath = file,
                    WebPath = FormatForURL(webPath),
                    ThumbnailPath = FormatForURL(GetThumbnailFilename(webPath))
                });
            }

            return mediaList;
        }

        /// <summary>
        /// Creates a the standard thumbnail file name for a file. Format is filename + "-thumb" + extensions.
        /// </summary>
        /// <param name="filePath">Full file path.</param>
        /// <returns>File path with the thumbnail filename.</returns>
        public string GetThumbnailFilename(string filePath)
        {
            string filename = Path.GetFileName(filePath);
            string extension = Path.GetExtension(filePath);

            return filePath.Replace(extension, $"{_thumbnailAffix}{extension}");
        }

        /// <summary>
        /// Deteles a media item and it's associated thumbnail (if it exists).
        /// </summary>
        /// <param name="serverFilePath">Server-based path to the media item.</param>
        public void DeleteMedia(string serverFilePath)
        {
            // Delete image
            if (File.Exists(serverFilePath))
                File.Delete(serverFilePath);

            // Delete thumbnail
            string thumbnail = GetThumbnailFilename(serverFilePath);

            if (File.Exists(thumbnail))
                File.Delete(thumbnail);
        }

        /// <summary>
        /// Creates a potentially unique 12-character ID for a folder.
        /// </summary>
        /// <returns>Generated folder ID.</returns>
        private string GenerateUniqueFolderID()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12);
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