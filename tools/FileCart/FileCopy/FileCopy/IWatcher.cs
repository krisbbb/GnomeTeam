using System;
using System.IO;

namespace FileCopy
{
    public interface IWatcher
    {
        IObservable<WatcherChangeTypes> FileChanged { get; }
    }
}
