using System.IO;
using System.Windows.Forms;

namespace SymLink_Tools.Tools
{
    public static class StorageHelper
    {
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
