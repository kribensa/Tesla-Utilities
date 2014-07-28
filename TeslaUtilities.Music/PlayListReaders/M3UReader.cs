namespace TeslaUtilities.Music.PlayListReaders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class M3UReader : IPlayListReader
    {
        /// <summary>
        /// Reads the m3u file and returns the enclosed files in order.
        /// If a folder is denoted in the m3u file, it will return the files from that folder in string order
        /// </summary>
        /// <param name="playlistFile">The m3u play list file</param>
        public IEnumerable<FileInfo> GetMusicFiles(FileInfo playlistFile)
        {
            if (playlistFile == null) yield break;

            var playListLines = File.ReadAllLines(playlistFile.FullName);
            
            // Ignore empty lines, comments or M3U Extended directives 
            foreach (var dataLine in playListLines
                .Select(line => line.Trim())
                .Where(dataLine => !string.IsNullOrWhiteSpace(dataLine) && !dataLine.StartsWith("#")))
            {
                // We could have an absolute file or folder name, a file or folder name relative to the m3u, or a URL
                if (Path.IsPathRooted(dataLine))
                {
                    // Absolute file or folder
                    foreach (var file in GetFiles(dataLine))
                        yield return file;
                }
                else if (dataLine.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                {
                    // We're going to ignore URLs for this version
                    // TODO support URLs
                    continue;
                }
                else
                {
                    // Relative file or folder 
                    foreach (var file in GetFiles(Path.Combine(playlistFile.DirectoryName, dataLine)))
                        yield return file;
                }
            }
        }

        /// <summary>
        /// Gets the files in the supplied folder (or the file itself if a file).
        /// </summary>
        /// <param name="folderOrFilename">The folder or filename.</param>
        private static IEnumerable<FileInfo> GetFiles(string folderOrFilename)
        {
            if (Directory.Exists(folderOrFilename))
            {
                foreach (var fileInfo in Directory.GetFiles(folderOrFilename, "*.*", SearchOption.AllDirectories)
                        .Select(f => new FileInfo(f))
                        .Where(i => i.IsValid()))
                    yield return fileInfo;
            }
            else if (File.Exists(folderOrFilename))
            {
                var fileInfo = new FileInfo(folderOrFilename);
                if (fileInfo.IsValid())
                    yield return fileInfo;
            }
        }
    }
}
