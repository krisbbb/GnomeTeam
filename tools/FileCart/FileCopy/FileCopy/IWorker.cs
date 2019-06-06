using System;
using System.IO;
using System.Threading.Tasks;

namespace FileCopy
{
    public interface IWorker
    {
        Task<(int SourceSize, int TargetSize)> FileSize();
        Task CopyFile();
    }
}
