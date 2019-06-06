using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace FileCopy
{
    public class FileCopy : IFileCopy
    {
        public FileCopy(IWorker worker, IWatcher watcher, string sourceDir, string sourceFile, string targetDir)
        {
            _worker = worker;
            _watcher = watcher;
            SourceDir = sourceDir;
            SourceFile = sourceFile;
            TargetDir = targetDir;
            SourcePath = Path.Combine(sourceDir, sourceFile);
            TargetPath = Path.Combine(targetDir, sourceFile);
            Status = FileCopyStatus.NotStarted;
        }

        public FileCopyStatus Status { get; private set; }

        public async Task Start()
        {
            Status = FileCopyStatus.Running;
            await _worker.CopyFile();
            Status = FileCopyStatus.Finished;
        }

        public Exception Error { get; private set; }

        public string SourceDir { get; private set; }
        public string SourceFile { get; private set; }
        public string TargetDir { get; private set; }

        public string SourcePath { get; private set; }
        public string TargetPath { get; private set; }

        public IObservable<FileCopySize> ProgressUpdates
        {
            get
            {
                // return Observable.FromAsync(async () => _worker.FileSize()).Merge(_watcher.FileChanged)
                // .Select(x => new FileCopySize { CurrentSize = x.TargetSize, SourceSize = x.SourceSize });

                return _watcher.FileChanged
                    .Select(x => Observable.FromAsync(() => _worker.FileSize()))
                    .Select(x => x.Select(y => new FileCopySize { SourceSize = y.SourceSize , CurrentSize = y.TargetSize }))
                    .Concat();
            }
        }

        private readonly IWorker _worker;
        private readonly IWatcher _watcher;
    }
}
