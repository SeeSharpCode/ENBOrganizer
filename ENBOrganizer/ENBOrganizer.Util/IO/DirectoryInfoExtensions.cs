using System;
using System.IO;

namespace ENBOrganizer.Util.IO
{
    public static class DirectoryInfoExtensions
    {
        public static void CopyTo(this DirectoryInfo sourceDirectory, string targetDirectoryPath)
        {
            DirectoryInfo targetDirectory = new DirectoryInfo(targetDirectoryPath);

            if (sourceDirectory.FullName.EqualsIgnoreCase(targetDirectory.FullName))
                throw new IOException("Attempt to copy directory failed because source and target directory paths are the same.");

            if (!targetDirectory.Exists)
                targetDirectory.Create();

            foreach (FileInfo file in sourceDirectory.GetFiles())
                file.CopyTo(Path.Combine(targetDirectoryPath, file.Name), true);

            foreach (DirectoryInfo subdirectory in sourceDirectory.GetDirectories())
                CopyTo(subdirectory, Path.Combine(targetDirectory.FullName, subdirectory.Name));
        }

        public static void DeleteRecursive(this DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles();
            DirectoryInfo[] directories = directory.GetDirectories();

            foreach (FileInfo file in files)
            {
                file.Attributes = FileAttributes.Normal;
                file.Delete();
            }

            foreach (DirectoryInfo subdirectory in directories)
            {
                subdirectory.Attributes = FileAttributes.Normal;
                DeleteRecursive(subdirectory);
            }

            directory.Delete();
        }

        public static void Rename(this DirectoryInfo directory, string name)
        {
            string renamedPath = Path.Combine(directory.Parent.FullName, name);

            if (Directory.Exists(renamedPath))
                Directory.Delete(renamedPath, true);

            directory.CopyTo(renamedPath);
            directory.Delete(true);
        }

        public static bool CanWrite(this DirectoryInfo directory)
        {
            try
            {
                string testFilePath = Path.Combine(directory.FullName, Path.GetRandomFileName());
                File.Create(testFilePath, 1, FileOptions.DeleteOnClose);

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
