﻿namespace TeslaUtilities
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;

    using TeslaUtilities.Music;

    public partial class MainWindow : Window
    {
        private string selectedPlayListFileName;

        private string selectedTargetFolderName;

        public MainWindow()
        {
            InitializeComponent();
        }

        //TODO use MVVM instead of events
        private void CopyFiles(object sender, RoutedEventArgs e)
        {
            IPlayListReader reader = new GeneralPlayListReader();
            var files = reader.GetMusicFiles(new FileInfo(selectedPlayListFileName));
            var targetFolder = new DirectoryInfo(selectedTargetFolderName);

            FileRenamer.Process(targetFolder, files);
        }

        /// <summary>
        /// Selects the play list file.
        /// </summary>
        private void SelectPlayListFile(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            var supportedExtensions = GeneralPlayListReader.GetSupportedReaderExtensions();
            dialog.Filter = BuildDialogFilter(supportedExtensions);
            dialog.Multiselect = false;
            dialog.Title = "Select Playlist File";
            //TODO use MyMusic for first run, and remembered path for subsequent runs
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedPlayListFileName = dialog.FileName;
                PlayListName.Text = selectedPlayListFileName;
            }
        }

        /// <summary>
        /// Builds a dialog filter based on the list of extensions
        /// </summary>
        /// <param name="extensions">The file extensions to list in the dialog</param>
        /// <returns>Returns a dialog filter (eg  "M3U Files|*.m3u")</returns>
        private string BuildDialogFilter(string[] extensions)
        {
            //  "M3U Files|*.m3u";
            return string.Join("|", extensions.Select(e => e.ToUpper() + " Files|*." + e));
        }

        /// <summary>
        /// Selects the target folder.
        /// </summary>
        private void SelectTargetFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = true;
            dialog.Description = "Select folder in which to copy the music files";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedTargetFolderName = dialog.SelectedPath;

                TargetFolder.Text = selectedTargetFolderName;
            }
        }
    }
}
