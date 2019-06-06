using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.IO;
using Xunit;
using Moq;
using FileCopy;

namespace FileCopyTest
{
    public class UnitTest1
    {
        [Fact]
        public void PathCombineTest()
        {
            //Given
            var (workerMock, watcherMock, source, file, dest) = GetDefaultTestObjects();

            //When
            var sut = new FileCopy.FileCopy(workerMock.Object, watcherMock.Object, source, file, dest);

            //Then
            Assert.Equal("c:/test.txt", sut.SourcePath);
            Assert.Equal("c:/tmp/test.txt", sut.TargetPath);
        }

        [Fact]
        public void BasicStateTest()
        {
            //Given
            var (workerMock, watcherMock, source, file, dest) = GetDefaultTestObjects();
            var sut = new FileCopy.FileCopy(workerMock.Object, watcherMock.Object, source, file, dest);

            //When
            sut.Start();

            //Then
            Assert.Equal(FileCopyStatus.Running, sut.Status);
        }

        [Fact]
        public async void StartAndStopTest()
        {
            //Given
            var (workerMock, watcherMock, source, file, dest) = GetDefaultTestObjects();
            var sut = new FileCopy.FileCopy(workerMock.Object, watcherMock.Object, source, file, dest);
            var observer = sut.ProgressUpdates;
        
            //When
            sut.Start();
            WatcherSubject[watcherMock].OnNext(WatcherChangeTypes.Changed);
            WorkerFileSizeManager[workerMock].Sources[0].SetResult((SourceSize: 10, TargetSize: 5));
            WorkerFileCopyManager[workerMock].Sources[0].SetResult(true); //Indicates that copy is finished

        
            //Then
            Assert.Equal(FileCopyStatus.Finished, sut.Status);
        }

        //Percent Calculation
        [Fact]
        public async void PercentCalculationTest()
        {
            //Given
            var (workerMock, watcherMock, source, file, dest) = GetDefaultTestObjects();
            workerMock.Setup(w => w.FileSize()).Returns(Task.FromResult((SourceSize: 10, TargetSize: 5)));
            var sut = new FileCopy.FileCopy(workerMock.Object, watcherMock.Object, source, file, dest);
            var observer = sut.ProgressUpdates;
        
            //When
            sut.Start();
            WatcherSubject[watcherMock].OnNext(WatcherChangeTypes.Changed);
        
            //Then

            //Observable should put up a 50% value result
            var result = sut.ProgressUpdates.First();
            Assert.Equal(10, result.SourceSize);
            Assert.Equal(5, result.CurrentSize);
        }


        //Throttling

        //Exception Handling

        //Create default setup
        public (Mock<IWorker>, Mock<IWatcher>, string, string, string) GetDefaultTestObjects()
        {
            var workerMock = new Mock<IWorker>();
            var fileSizeManager = new TaskCompletionSourceManager<(int SourceSize, int TargetSize)>();
            var fileCopyManager = new TaskCompletionSourceManager<bool>();
            workerMock.Setup(w => w.FileSize()).Returns(fileSizeManager.NewTask());
            workerMock.Setup(w => w.CopyFile()).Returns(fileCopyManager.NewTask());
            WorkerFileSizeManager[workerMock] = fileSizeManager;
            WorkerFileCopyManager[workerMock] = fileCopyManager;

            var watcherMock = new Mock<IWatcher>();
            var watcherSubject = new Subject<WatcherChangeTypes>();
            watcherMock.Setup(w => w.FileChanged).Returns(watcherSubject.AsObservable());
            WatcherSubject[watcherMock] = watcherSubject;

            var source = "c:/";
            var file = "test.txt";
            var dest = "c:/tmp/";
            
            return (workerMock, watcherMock, source, file, dest);
        }


        //Tests to test the test system

        public Dictionary<Mock<IWorker>, TaskCompletionSourceManager<(int SourceSize, int TargetSize)>> WorkerFileSizeManager =
            new Dictionary<Mock<IWorker>, TaskCompletionSourceManager<(int SourceSize, int TargetSize)>>();

        public Dictionary<Mock<IWorker>, TaskCompletionSourceManager<bool>> WorkerFileCopyManager =
            new Dictionary<Mock<IWorker>, TaskCompletionSourceManager<bool>>();        

        public Dictionary<Mock<IWatcher>, Subject<WatcherChangeTypes>> WatcherSubject =
            new Dictionary<Mock<IWatcher>, Subject<WatcherChangeTypes>>();
    }

    public class TaskCompletionSourceManager<T>
    {
        public List<TaskCompletionSource<T>> Sources { get; set; } = new List<TaskCompletionSource<T>>();
        public Task<T> NewTask()
        {
            var source = new TaskCompletionSource<T>();
            Sources.Add(source);
            return source.Task;
        }
    }
}
