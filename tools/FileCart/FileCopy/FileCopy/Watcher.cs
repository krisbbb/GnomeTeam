using System;
using System.IO;

namespace FileCopy
{
    public class Watcher : IWatcher
    {
        public Watcher()
        {
            
        }
        
        public IObservable<WatcherChangeTypes> FileChanged
        {
            get { return null; }
        }
    }
}
