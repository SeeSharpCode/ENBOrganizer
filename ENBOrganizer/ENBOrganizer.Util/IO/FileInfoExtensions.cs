using ENBOrganizer.Util.UI;
using System.IO;
using System.Windows.Forms;

namespace ENBOrganizer.Util.IO
{
    public static class FileInfoExtensions
    {
        private const string OverwritePrompt = "'{0}' already exists here. Overwrite?";

        public static void Rename(this FileInfo file, string targetFileName)
        {
            string targetPath = Path.Combine(file.Directory.FullName, targetFileName + file.Extension);

            if (!File.Exists(targetPath))
                file.MoveTo(targetPath);
            else if (MessageBoxUtil.PromptForDecision(string.Format(OverwritePrompt, Path.GetFileName(targetFileName))) == DialogResult.Yes)
            {
                file.CopyTo(targetPath, true);
                file.Delete();
            }
        }
    }
}