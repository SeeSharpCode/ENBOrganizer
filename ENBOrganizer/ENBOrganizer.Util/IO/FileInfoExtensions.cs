using System.IO;

namespace ENBOrganizer.Util.IO
{
    public static class FileInfoExtensions
    {
        public static void Rename(this FileInfo file, string targetFileName)
        {
            string targetPath = Path.Combine(file.Directory.FullName, targetFileName + file.Extension);

            if (File.Exists(targetPath))
            {
                file.CopyTo(targetPath, true);
                file.Delete();
            }
            else
                file.MoveTo(targetPath);
        }
    }
}