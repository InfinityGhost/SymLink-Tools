using System.IO;
using System.Windows.Forms;

namespace SymLink_Tools.Tools
{
    public static class StorageHelper
    {
        public static string FindFile()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select a file",
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileName;
            else
                return string.Empty;
        }

        public static string CreateFile()
        {
            var dialog = new SaveFileDialog
            {
                Title = "Create a file",
                DefaultExt = ".symlink",
                Filter = "Symbolic link|*.symlink",
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileName;
            else
                return string.Empty;
        }

        public static string FindFolder()
        {
            var openDirectory = new FolderBrowserDialog
            {
                Description = "Select a folder.",
            };
            if (openDirectory.ShowDialog() == DialogResult.OK)
                return openDirectory.SelectedPath;
            else
                return string.Empty;
        }
    }
}
