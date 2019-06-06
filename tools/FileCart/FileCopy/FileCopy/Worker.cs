using System;
using System.IO;
using System.Threading.Tasks;

namespace FileCopy
{
    public class Worker : IWorker
    {
        public Worker(string sourcePath, string targetPath)
        {
            _sourcePath = sourcePath;
            _targetPath = targetPath;
        }
        
        public Task<(int SourceSize, int TargetSize)> FileSize()
        {
            //DEBUG: Dummy implementation during development
            var taskSource = new TaskCompletionSource<(int SourceSize, int TargetSize)>();
            taskSource.SetResult((0, 0));
            return taskSource.Task;
        }

        public Task CopyFile()
        {
            return Task.CompletedTask;
        }


        private readonly string _sourcePath, _targetPath;
    }
}
