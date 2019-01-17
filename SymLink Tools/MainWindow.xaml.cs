using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using SymLink_Tools.Tools;

namespace SymLink_Tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            Console.Updated += Status.Update;

            SymLink.Output += Console.Log;
        }

        private SymLink SymLink = new SymLink();

        #region Properties

        private string _realfolderpath;
        public string RealFolderPath
        {
            set
            {
                _realfolderpath = value.Trim('"', '\\');
                NotifyPropertyChanged();
            }
            get => _realfolderpath;
        }

        private string _symlinkfolderpath;
        public string SymLinkFolderPath
        {
            set
            {
                _symlinkfolderpath = value.Trim('"', '\\');
                NotifyPropertyChanged();
            }
            get => _symlinkfolderpath;
        }

        #endregion

        #region Main Tab: Buttons

        private void OpenRealFolder(object sender = null, EventArgs e = null) => Process.Start(RealFolderPath);

        private void CreateSymlinks(object sender = null, EventArgs e = null)
        {
            if (RootFoldersRadio.IsChecked ?? false)
                SymLink.CreateFolder(RealFolderPath, SymLinkFolderPath);
            if (SubfoldersRadio.IsChecked ?? false)
                SymLink.CreateSubfolders(RealFolderPath, SymLinkFolderPath);
        }

        private void PrintFolders(object sender = null, EventArgs e = null)
        {
            if (RootFoldersRadio.IsChecked ?? false)
            {
                Console.Log($"{SymLinkFolderPath}");
                new Windows.DirectoryListing(SymLinkFolderPath).ShowDialog();
            }
            if (SubfoldersRadio.IsChecked ?? false)
            {
                var directories = Directory.GetDirectories(RealFolderPath).ToList()
                    .ConvertAll(dir => $"{SymLinkFolderPath}\\{new DirectoryInfo(dir).Name}");
                new Windows.DirectoryListing(directories).ShowDialog();
            }
        }

        private void FindReal(object sender = null, EventArgs e = null)
        {
            RealFolderPath = StorageHelper.FindFolder();
        }

        private void FindSym(object sender = null, EventArgs e = null)
        {
            SymLinkFolderPath = StorageHelper.FindFolder();
        }

        #endregion

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Misc.

        private void Close(object sender = null, EventArgs e = null)
        {
            this.Hide();
            Environment.Exit(0x0);
        }

        private void WindowRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Debugger.IsAttached)
                ConsoleTab.SetValue(VisibilityProperty, Visibility.Collapsed);
        }

        #endregion

        private void WindowRoot_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0x0);
        }
    }
}
