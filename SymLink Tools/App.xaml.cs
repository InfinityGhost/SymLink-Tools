using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SymLink_Tools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var args = e.Args;

            var window = new MainWindow();
            if (args.Length >= 1 && !string.IsNullOrWhiteSpace(args[0]))
                window.RealPath = args[0];
            if (args.Length >= 2 && !string.IsNullOrWhiteSpace(args[1]))
                window.SymLinkPath = args[1];

            MainWindow.Show();
        }
    }
}
