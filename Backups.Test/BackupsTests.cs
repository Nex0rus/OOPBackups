using Backups.Entities;
using Backups.Interfaces;
using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.DomainInterfaces;
using Backups.Models;
using Xunit;
using Zio.FileSystems;

namespace Backups.Test
{
    public class BackupsTests
    {
        [Fact]
        public void ExecuteSplitBackupTaskStorageNumberIsCorrect()
        {
            var backup = new Backup();
            var splitAlgo = new SplitStorageAlgorithm();
            var restorePointName = new StreamIdentificator("Restore_Point");
            var splitTask = new BackupTask(backup, splitAlgo, restorePointName);
            splitTask.ToString();
            var fs = new MemoryFileSystem();
            var homeDir = new StreamIdentificator("/Home");
            var inMemoryFS = new InMemoryFileSystem(fs, homeDir);
            IStreamIdentificator nextDir = homeDir.Append("NewDirectory");
            IStreamIdentificator backupDir = homeDir.Append("BackupDir");
            var destMemoryFS = new InMemoryFileSystem(fs, backupDir);
            inMemoryFS.CreateFolder(backupDir);
            inMemoryFS.CreateFolder(nextDir);
            IStreamIdentificator firstFile = nextDir.Append("aboba.txt");
            inMemoryFS.CreateFile(firstFile);
            inMemoryFS.CreateFolder(nextDir.Append("B"));
            IStreamIdentificator secondFile = nextDir.Append("B").Append("amogus.txt");
            inMemoryFS.CreateFile(secondFile);
            var firstBackupObject = new BackupObject(firstFile, inMemoryFS);
            var secondBackupObject = new BackupObject(secondFile, inMemoryFS);
            splitTask.AddBackupObject(firstBackupObject);
            splitTask.AddBackupObject(secondBackupObject);
            IArchivator archivator = new ZipArchivator();
            splitTask.ExecuteTask(archivator, DateTime.Now, destMemoryFS);
            splitTask.RemoveBackupObject(firstBackupObject.StreamIdentificator);
            splitTask.ExecuteTask(archivator, DateTime.Now, destMemoryFS);
            IReadOnlyCollection<IRestorePoint> rps = splitTask.RestorePoints;
            int restorePointCount = 0;
            int repoObjectCount = 0;
            foreach (IRestorePoint restorePoint in rps)
            {
                restorePointCount += 1;
                repoObjectCount += restorePoint.Storage.GetRepoComponents().Count;
            }

            Assert.Equal(2, restorePointCount);
            Assert.Equal(3, repoObjectCount);
        }
    }
}