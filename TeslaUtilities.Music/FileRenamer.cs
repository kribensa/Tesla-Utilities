namespace TeslaUtilities.Music
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    public class FileRenamer
    {
        /// <summary>
        /// Renames a file from ORIGINALNAME.EXT or nnnn-ORIGINALNAME.EXT and prepends a new numerical index
        /// </summary>
        /// <param name="file">The file to rename</param>
        /// <param name="index">The index to use as a prefix.</param>
        /// <param name="digitCount">The number of digits to use for the prefix</param>
        public string GetNewFileName(FileInfo file, int index, int digitCount)
        {
            string newPrefix = index.ToString(CultureInfo.InvariantCulture).PadLeft(digitCount, '0') + "-";

            const string RegexPattern = @"^\d+\-";
            string newName = Regex.IsMatch(file.Name, RegexPattern)
                                 ? Regex.Replace(file.Name, RegexPattern, newPrefix)
                                 : newPrefix + file.Name;
            return newName;
        }

        /// <summary>
        /// Copies the file into the target folder renamed with appropriate prefix, and returns the full name of the copied file.
        /// </summary>
        /// <param name="targetFolder">The folder in which to copy the renamed file.</param>
        /// <param name="file">The file to rename.</param>
        /// <param name="index">The index to use as a prefix.</param>
        /// <param name="digitCount">The number of digits to use for the prefix</param>
        public string CopyRenamedFile(DirectoryInfo targetFolder, FileInfo file, int index, int digitCount)
        {
            if (!targetFolder.Exists)
                targetFolder.Create();

            string newName = this.GetNewFileName(file, index, digitCount);
            string newPath = Path.Combine(targetFolder.FullName, newName);

            File.Copy(file.FullName, newPath);

            return newPath;
        }

        /// <summary>
        /// Processes the specified target folder.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        /// <param name="files">The music files to rename and copy to the target folder.</param>
        public static void Process(DirectoryInfo targetFolder, IEnumerable<FileInfo> files)
        {
            const string Header = "#TeslaM3U";
            var targetM3UData = new StringBuilder();
            targetM3UData.AppendLine(Header);

            var namer = new FileRenamer();
            int index = 1;
            foreach (var file in files)
            {
                var newName = namer.CopyRenamedFile(targetFolder, file, index++, 5);
                targetM3UData.AppendLine(newName);
            }
            
            //Write m3u file to target folder
            var targetM3UFile = Path.Combine(targetFolder.FullName, "Tesla.m3u");
            File.WriteAllText(targetM3UFile, targetM3UData.ToString());
        }
    }
}
