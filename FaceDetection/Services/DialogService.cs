using System;
using System.IO;
using FaceDetection.Services.Interfaces;
using Microsoft.Win32;

namespace FaceDetection.Services
{
    public class DialogService : IDialogService
    {
        private readonly OpenFileDialog openFileDialog;

        public DialogService()
        {
            openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                CheckFileExists = true,
                CheckPathExists = true,
                RestoreDirectory = true
            };
        }
        /// <summary>
        /// Displays a file selection dialog.
        /// </summary>
        /// <param name="caption">The dialog caption.</param>
        /// <param name="filter">The files types to display in the file dialog.</param>
        /// <returns>The full path of the selected file.
        /// Returns an empty string if a file is not selected.</returns>
        public string PickFile(string caption, string filter = "All files (*.*)|*.*")
        {
            openFileDialog.Title = caption;
            openFileDialog.Filter = filter;

            if (openFileDialog.ShowDialog() == true) return openFileDialog.FileName;

            return string.Empty;
        }

        /// <summary>
        /// Displays a file selection dialog.
        /// </summary>
        /// <param name="caption">The dialog caption.</param>
        /// <param name="filter">The files types to display in the file dialog.</param>
        /// <returns>Returns a Stream with no backing store (Stream.Null).</returns>
        public Stream OpenFile(string caption, string filter = "All files (*.*)|*.*")
        {
            openFileDialog.Title = caption;
            openFileDialog.Filter = filter;

            if (openFileDialog.ShowDialog() == true) return openFileDialog.OpenFile();

            return Stream.Null;
        }
    }
}
