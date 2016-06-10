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
    }
}
