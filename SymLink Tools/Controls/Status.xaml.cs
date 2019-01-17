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

namespace SymLink_Tools.Controls
{
    /// <summary>
    /// Interaction logic for Status.xaml
    /// </summary>
    public partial class Status : UserControl, INotifyPropertyChanged
    {
        public Status()
        {
            InitializeComponent();
        }

        #region Properties

        private string _statustext;
        public string StatusText
        {
            set
            {
                _statustext = value;
                NotifyPropertyChanged();
            }
            get => _statustext;
        }

        #endregion

        #region Methods

        public void Update(string text) => StatusText = text;
        public void Update(object sender, string text) => StatusText = text;

        #endregion

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }
}
