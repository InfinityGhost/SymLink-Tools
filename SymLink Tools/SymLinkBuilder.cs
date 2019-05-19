using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SymLink_Tools
{
    public class SymLinkBuilder
    {
        public SymLinkBuilder()
        {
            CMD.OutputDataReceived += OutputDataHandler;
            CMD.ErrorDataReceived += OutputDataHandler;
        }

        public event EventHandler<string> Output;

        #region Objects / Static Objects

        private Process CMD = new Process
        {
            StartInfo = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe"),
            EnableRaisingEvents = true,
        };

        public static string DefaultCommand = "/c mklink ";

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the arguments for creating a file link.
        /// </summary>
        /// <param name="mainLocation">Actual location of the file.</param>
        /// <param name="newLocation">New Symlink location.</param>
        /// <returns>Arguments</returns>
        private string GetFileArgument(string mainLocation, string newLocation)
        {
            return $@"{DefaultCommand} ""{newLocation}"" ""{mainLocation}"" ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainRoot">Name of the original folder root.</param>
        /// <param name="newRoot">Name of the new folder root for the Symlink.</param>
        /// <param name="subfileName">Name of the subfile.</param>
        /// <returns></returns>
        private string GetSubfileArgument(string mainRoot, string newRoot, string subfileName)
        {
            return $@"{DefaultCommand} ""{newRoot}\{subfileName}"" ""{mainRoot}\{subfileName}"" ";
        }

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
            CMD.StartInfo.Arguments = args;
            CMD.StartInfo.RedirectStandardOutput = true;
            CMD.StartInfo.RedirectStandardError = true;
            CMD.StartInfo.UseShellExecute = false;
            CMD.StartInfo.CreateNoWindow = true;
            CMD.Start();

            CMD.BeginErrorReadLine();
            CMD.BeginOutputReadLine();

            Debugger.Log(0, this.GetType().Name, $"Running: {CMD.StartInfo.FileName} {CMD.StartInfo.Arguments}");
        }

        private void OutputDataHandler(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                Output?.Invoke(sender, e.Data);
        }

        #endregion

        #region Public Methods

        public void CreateFile(string realPath, string newPath)
        {
            Output?.Invoke(this, $"Creating symlink for {realPath} to {newPath}");
            RunArguments(GetFileArgument(realPath, newPath));

            new Windows.DirectoryListing(newPath).Show();
        }

        public void CreateSubfiles(string realPath, string newPath)
        {
            Output?.Invoke(this, $"Creating symlinks from {realPath}");

            var files = Directory.GetFiles(realPath);
            foreach (string file in files)
                RunArguments(GetSubfileArgument(realPath, newPath, file));
            Output?.Invoke(this, $"Successfully created {files.Length} symlinks in {newPath}.");

            var symFiles = files.ToList().ConvertAll(file => $"{newPath}\\{new FileInfo(file).Name}");
            new Windows.DirectoryListing(symFiles);
        }

        public void CreateFolder(string realPath, string newPath)
        {
            Output?.Invoke(this, $"Creating symlink for {realPath} to {newPath}");
            RunArguments(GetFolderArgument(realPath, newPath));

            new Windows.DirectoryListing(newPath).Show();
        }

        public void CreateSubfolders(string realPath, string newPath)
        {
            Output?.Invoke(this, $"Creating symlinks from {realPath}");
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
