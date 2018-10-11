using System;

namespace FileCart.ViewModels
{
    public interface IFileDialogService
    {
        FileDialogResult GetFile(string title, string initialFile);
    }

    public class FileDialogResult
    {
        public bool Success { get; set; }
        public string File { get; set; }
    }
}
