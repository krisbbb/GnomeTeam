using System;
using Avalonia.Controls;

namespace FileCart.ViewModels
{
    public class FileDialogService : IFileDialogService
    {
        public FileDialogResult GetFile(string title, string initialFile)
        {
            var result = new FileDialogResult();

            var dialog = new OpenFileDialog();

            dialog.Title = title;
            dialog.InitialDirectory = initialFile;

            var success = dialog.ShowAsync().GetAwaiter().GetResult();

            if(success != true) { return result; }

            return result;
        }
    }
}
