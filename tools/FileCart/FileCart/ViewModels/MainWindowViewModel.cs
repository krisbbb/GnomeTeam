using System;
using System.Collections.Generic;
using System.Text;

namespace FileCart.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() :
        this(new FileDialogService())
        {}

        public MainWindowViewModel(IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;
        }

        public string Source { get; private set; } = "D:\\Foo\\Bar\\";
        public string Destination { get; private set; } = "C:\\Bar\\Foo\\";
        public string SecondDestination { get; private set; } = "C:\\Bar\\Foo\\";

        public enum DirectoryType
        {
            Source,
            Destination,
            SecondDestination
        }

        public void UpdateDirectory(DirectoryType dir)
        {
            var initialFile = "";
            switch (dir)
            {
                case DirectoryType.Source:
                  initialFile = Source;
                  break;
                case DirectoryType.Destination:
                  initialFile = Destination;
                  break;
                case DirectoryType.SecondDestination:
                  initialFile = SecondDestination;
                  break;
                default:
                  break;
            };
            var result = _fileDialogService.GetFile(Title[dir], initialFile);

            if (!result.Success) { return; }

            switch (dir)
            {
                case DirectoryType.Source:
                  Source = result.File;
                  break;
                case DirectoryType.Destination:
                  Destination = result.File;
                  break;
                case DirectoryType.SecondDestination:
                  SecondDestination = result.File;
                  break;
                default:
                  break;
            }
        }

        public void Exit(int? code = null)
        {
            Environment.Exit(code ?? 0);
        }

        private Dictionary<DirectoryType, string> Title = new Dictionary<DirectoryType, string>{
            {DirectoryType.Source, "Select Directory to Copy From"},
            {DirectoryType.Destination, "Select Directory to Copy To"},
            {DirectoryType.SecondDestination, "Select Directory to Also Copy To"},
        };

        private readonly IFileDialogService _fileDialogService;
    }
}
