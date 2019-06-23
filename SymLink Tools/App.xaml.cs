using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SymLink_Tools.Tools;

namespace SymLink_Tools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Report errors to log
            AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledException;

            var args = e.Args;

            var window = new MainWindow();
            if (args.Length >= 1 && !string.IsNullOrWhiteSpace(args[0]))
                window.RealPath = args[0];
            if (args.Length >= 2 && !string.IsNullOrWhiteSpace(args[1]))
                window.SymLinkPath = args[1];
            
            MainWindow.Show();
        }

        private void GlobalUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default;
            ex = (Exception)e.ExceptionObject;

            var crashDump = new List<string>
            {
                $"-------",
                $"Exception occured at {DateTime.Now}",
                $"Source: {ex.Source}",
                $"Message: {ex.Message}",
                $"HelpLink: {ex.HelpLink}",
                $"StackTrace: {Environment.NewLine}{ex.StackTrace}",
                $"TargetSite: {ex.TargetSite.Name}",
                $"HResult: {ex.HResult}",
            };

            try
            {
                Log(crashDump);
            }
            catch
            {
                Clipboard.SetText(crashDump.Combine());
                MessageBox.Show(crashDump.Combine());
            }
        }
        
        public static void Log(string line) => File.AppendAllText(LogPath, Prefix + line + Environment.NewLine);

        public static void Log(IEnumerable<string> lines) => Log(lines.Combine());

        private static string LogPath = $"{Directory.GetCurrentDirectory()}\\log.txt";
        private static string Prefix => DateTime.Now.ToLongTimeString() + ": ";
    }
}
