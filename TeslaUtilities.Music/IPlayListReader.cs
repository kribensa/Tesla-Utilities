namespace TeslaUtilities.Music
{
    using System.Collections.Generic;
    using System.IO;

    public interface IPlayListReader
    {
        IEnumerable<FileInfo> GetMusicFiles(FileInfo playlistFile);
    }
}