using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SymLink_Tools.Windows
{
    /// <summary>
    /// Interaction logic for DirectoryListing.xaml
    /// </summary>
    public partial class DirectoryListing : Window
    {
        public DirectoryListing()
        {
            InitializeComponent();
        }

        public DirectoryListing(string directory) : this()
        {
            CreateObject(directory);
        }
        
        public DirectoryListing(IEnumerable<string> directories) : this()
        {
            CreateObjects(directories);
        }

        #region Methods

        private void CreateObjects(IEnumerable<string> directories)
        {
            foreach (var directory in directories)
                CreateObject(directory);
        }

        public void CreateObject(string directory)
        {
            List.Children.Add(element: new ListBoxItem { Content = directory });
        }

        private void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            try
            {
                Process.Start((string)item.Content);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion

    }
}
