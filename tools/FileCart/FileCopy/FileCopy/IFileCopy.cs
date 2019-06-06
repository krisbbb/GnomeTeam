using System;
using System.IO;
using System.Threading.Tasks;

namespace FileCopy
{
    public interface IFileCopy
    {
        FileCopyStatus Status { get; }
        Task Start();
        Exception Error { get; }
        IObservable<FileCopySize> ProgressUpdates { get; }
    }

    public enum FileCopyStatus
    {
        NotStarted = 0,
        Running,
        Error,
        Finished,
    }

    public class FileCopySize
    {
        public int CurrentSize { get; set; }
        public int SourceSize { get; set; }
    }
}
