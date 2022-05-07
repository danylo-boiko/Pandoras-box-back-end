namespace Storage.Core.Helpers
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Consts;

    public static class StoragePathsHelper
    {
        public static string EnvironmentName { get; set; } = string.Empty;

        public static string GetAvatarPath(string filename)
        {
            var imagesPath = GetAvatarsPath();
            var avatarImagesDir = Path.Combine(imagesPath, $"{filename}");
            return avatarImagesDir;
        }

        public static string GetChannelVideoPath(string filename)
        {
            var videosPath = GetVideosPath();
            var result = Path.Combine(videosPath, $"{filename}");
            return result;
        }

        private static string GetAvatarsPath()
        {
            var imagesPath = GetImagesPath();
            var avatarImagesDir = Path.Combine(imagesPath, StorageDirectories.Level3.Avatars);

            if (!Directory.Exists(avatarImagesDir))
            {
                Directory.CreateDirectory(avatarImagesDir);
            }

            return avatarImagesDir;
        }

        private static string GetVideosPath()
        {
            var imagesPath = GetImagesPath();
            var avatarImagesDir = Path.Combine(imagesPath, StorageDirectories.Level2.Videos);

            if (!Directory.Exists(avatarImagesDir))
            {
                Directory.CreateDirectory(avatarImagesDir);
            }

            return avatarImagesDir;
        }

        private static string GetImagesPath()
        {
            var storagePath = GetMediaFilesPath();
            var imagesDir = Path.Combine(storagePath, StorageDirectories.Level2.Images);

            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }

            return imagesDir;
        }

        private static string GetMediaFilesPath()
        {
            var storagePath = GetBaseStoragePath();
            var mediaFilesDir = Path.Combine(storagePath, StorageDirectories.Level1.MediaFiles);

            if (!Directory.Exists(mediaFilesDir))
            {
                Directory.CreateDirectory(mediaFilesDir);
            }

            return mediaFilesDir;
        }

        private static string GetBaseStoragePath()
        {
            var os = GetOsVersion();
            string homePath;

            if (os == OSPlatform.Linux)
            {
                homePath = GetStoragePathForLinux();
            }
            else if (os == OSPlatform.OSX)
            {
                var username = Environment.GetEnvironmentVariable("USERNAME") ??
                               Environment.GetEnvironmentVariable("USER");
                homePath = GetStoragePathForOSX(username);
            }
            else if (os == OSPlatform.Windows)
            {
                homePath = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            var appDir = $"pandoras-box-{EnvironmentName.ToLower()}";

            var storageDir = Path.Combine(homePath, appDir);

            if (!Directory.Exists(storageDir))
            {
                Directory.CreateDirectory(storageDir);
            }

            return storageDir;
        }

        private static OSPlatform GetOsVersion()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX;
            }

            return OSPlatform.Windows;
        }

        private static string GetStoragePathForLinux()
        {
            return "/srv/aspnetcorestorage";
        }

        private static string GetStoragePathForOSX(string username)
        {
            return $"/Users/{username}/projects/aspnetstorage";
        }
    }
}
