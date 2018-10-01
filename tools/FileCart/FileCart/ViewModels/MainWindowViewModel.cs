using System;
using System.Collections.Generic;
using System.Text;

namespace FileCart.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Source => "D:\\Foo\\Bar\\";
        public string Destination => "C:\\Bar\\Foo\\";
    }
}
