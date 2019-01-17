using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SymLink_Tools
{
    public class SymLink
    {
        public SymLink()
        {
            CMDProcess.OutputDataReceived += CMDProcess_OutputDataReceived;
        }

        public event EventHandler<string> Output;

        #region Objects / Static Objects

        private Process CMDProcess = new Process
        {
            StartInfo = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe"),
            EnableRaisingEvents = true,
        };

        public static string DefaultCommand = "/c mklink ";

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainRoot">Name of the original folder root.</param>
        /// <param name="newRoot">Name of the new folder root for the Symlink.</param>
        /// <param name="subfolderName">Name of the subfolder.</param>
        /// <returns></returns>
        private string GetSubfolderArgument(string mainRoot, string newRoot, string subfolderName)
        {
            return $@"{DefaultCommand} /d ""{newRoot}\{subfolderName}"" ""{mainRoot}\{subfolderName}"" ";
        }

        /// <summary>
        /// Gets the arguments for creating a folder link.
        /// </summary>
        /// <param name="mainLocation">Actual location of the folder.</param>
        /// <param name="newLocation">New Symlink location.</param>
        /// <returns>Arguments</returns>
        private string GetFolderArgument(string mainLocation, string newLocation)
        {
            return $@"{DefaultCommand} /d ""{newLocation}"" ""{mainLocation}"" ";
        }

        private void RunArguments(string args)
        {
            CMDProcess.StartInfo.Arguments = args;
            CMDProcess.Start();
            Output?.Invoke(this, $"Running: {CMDProcess.StartInfo.FileName} {CMDProcess.StartInfo.Arguments}");
        }

        private void CMDProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output?.Invoke(sender, e.Data.ToString());
        }

        #endregion

        #region Public Methods

        public void CreateFolder(string realPath, string newPath)
        {
            Output?.Invoke(this, $"Creating symlink for {realPath} to {newPath}");
            RunArguments(GetFolderArgument(realPath, newPath));
            Output?.Invoke(this, $"Successfully created Symlink at {newPath}.");

            new Windows.DirectoryListing(newPath).Show();
        }

        public void CreateSubfolders(string realPath, string newPath)
        {
            Output?.Invoke(this, $"Creating subfolders from {realPath}");
            var directories = Directory.GetDirectories(realPath);
            foreach (string subfolder in directories)
                RunArguments(GetSubfolderArgument(realPath, newPath, new DirectoryInfo(subfolder).Name));   
            Output?.Invoke(this, $"Successfully created {directories.Length} subfolders in {newPath}.");
;
            var symDirectories = directories.ToList().ConvertAll(dir => $"{newPath}\\{new DirectoryInfo(dir).Name}");
            new Windows.DirectoryListing(symDirectories).Show();
        }

        #endregion
    }
}
