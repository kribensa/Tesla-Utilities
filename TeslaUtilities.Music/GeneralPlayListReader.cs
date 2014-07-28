namespace TeslaUtilities.Music
{
    using System.Collections.Generic;
    using System.IO;

    using TeslaUtilities.Music.PlayListReaders;

    public class GeneralPlayListReader : IPlayListReader
    {
        /// <summary>
        /// Gets the music files from the supplied playlist file.
        /// </summary>
        /// <param name="playlistFile">The playlist file.</param>
        public IEnumerable<FileInfo> GetMusicFiles(FileInfo playlistFile)
        {
            if (playlistFile == null)
                return null;

            IPlayListReader reader;
            switch (playlistFile.Extension.ToLowerInvariant())
            {
                case "m3u":
                    reader = new M3UReader();
                    break;

                case "wpl":
                    //TODO reader = new WplReader();

                case "zpl":
                    //TODO reader = new ZplReader();

                default:
                    return null;
            }

            return reader.GetMusicFiles(playlistFile);
        }

        /// <summary>
        /// Gets the supported reader extensions.
        /// </summary>
        public static string[] GetSupportedReaderExtensions()
        {
            // TODO get these from available classes
            return new[] { "m3u" };
        }
    }
}