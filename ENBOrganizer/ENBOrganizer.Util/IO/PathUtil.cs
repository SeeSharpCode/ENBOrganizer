using System.IO;
using System.Linq;

namespace ENBOrganizer.Util.IO
{
    public static class PathUtil
    {
        public static bool IsValidFileSystemName(string fileName)
        {
            return !fileName.ToCharArray().Any(c => Path.GetInvalidFileNameChars().Contains(c));
        }

        public static void CopyTo(this FileSystemInfo fileSystemInfo, string targetPath)
        {
            if (fileSystemInfo is DirectoryInfo)
                ((DirectoryInfo)fileSystemInfo).CopyTo(targetPath);
            else
                ((FileInfo)fileSystemInfo).CopyTo(targetPath);
        }
    }
}
