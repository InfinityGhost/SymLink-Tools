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

        private SymLinkBuilder SymLink = new SymLinkBuilder();

        #region Properties

        private string _realpath;
        public string RealPath
        {
            set
            {
                _realpath = value.Trim('"', '\\');
                NotifyPropertyChanged();
            }
            get => _realpath;
        }

        private string _symlinkpath;
        public string SymLinkPath
        {
            set
            {
                _symlinkpath = value.Trim('"', '\\');
                NotifyPropertyChanged();
            }
            get => _symlinkpath;
        }

        #endregion

        #region Main Tab: Buttons

        private void OpenRealFolder(object sender = null, EventArgs e = null) => Process.Start(RealPath);

        private void CreateSymlinks(object sender = null, EventArgs e = null)
        {
            if (FileRadio.IsChecked ?? false)
                SymLink.CreateFile(RealPath, SymLinkPath);
            else if (RootFoldersRadio.IsChecked ?? false)
                SymLink.CreateFolder(RealPath, SymLinkPath);
            else if (SubfoldersRadio.IsChecked ?? false)
                SymLink.CreateSubfolders(RealPath, SymLinkPath);
            else
                Console.Log("Error: No symlink type selected!");
        }

        private void PrintFolders(object sender = null, EventArgs e = null)
        {
            if (FileRadio.IsChecked ?? false)
            {
                Console.Log($"{SymLinkPath}");
                new Windows.DirectoryListing(SymLinkPath).ShowDialog();
            }
            if (SubfilesRadio.IsChecked ?? false)
            {
                var files = Directory.GetFiles(RealPath).ToList()
                    .ConvertAll(file => $"{SymLinkPath}\\{new FileInfo(file).Name}");
                new Windows.DirectoryListing(files);
            }
            if (RootFoldersRadio.IsChecked ?? false)
            {
                Console.Log($"{SymLinkPath}");
                new Windows.DirectoryListing(SymLinkPath).ShowDialog();
            }
            if (SubfoldersRadio.IsChecked ?? false)
            {
                var directories = Directory.GetDirectories(RealPath).ToList()
                    .ConvertAll(dir => $"{SymLinkPath}\\{new DirectoryInfo(dir).Name}");
                new Windows.DirectoryListing(directories).ShowDialog();
            }
        }

        private void FindReal(object sender = null, EventArgs e = null)
        {
            if (FileRadio.IsChecked ?? false)
                RealPath = StorageHelper.FindFile();
            else
                RealPath = StorageHelper.FindFolder();
        }

        private void FindSym(object sender = null, EventArgs e = null)
        {
            if (FileRadio.IsChecked ?? false)
                SymLinkPath = StorageHelper.CreateFile().Replace(".symlink", string.Empty);
            else
                SymLinkPath = StorageHelper.FindFolder();
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

        private void Close(object sender, EventArgs e) => Close();

        private void WindowRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Debugger.IsAttached)
                ConsoleTab.SetValue(VisibilityProperty, Visibility.Collapsed);
        }

        #endregion
    }
}
