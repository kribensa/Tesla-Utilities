namespace TeslaUtilities.Music.PlayListReaders
{
    using System.IO;

    internal static class M3UReaderExtensions
    {
        /// <summary>
        /// Determines whether the specified file information is valid.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        /// <returns><c>true</c> if the specified file information is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValid(this FileInfo fileInfo)
        {
            // Do not copy hidden files - they will consist of things like album art, etc which the Telsa currently ignores anyway
            return (fileInfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden;
        }
    }
}