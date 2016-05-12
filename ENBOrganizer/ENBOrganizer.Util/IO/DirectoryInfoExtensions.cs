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

        public static void Rename(this DirectoryInfo directory, string name)
        {
            string renamedPath = Path.Combine(directory.Parent.FullName, name);

            DirectoryInfo renamedDirectory = new DirectoryInfo(renamedPath);

            if (renamedDirectory.Exists)
                renamedDirectory.Delete(true);

            directory.CopyTo(renamedPath);
            directory.Delete(true);
        }
    }
}
