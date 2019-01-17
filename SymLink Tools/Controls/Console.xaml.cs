using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : UserControl, INotifyPropertyChanged
    {
        public Console()
        {
            InitializeComponent();
        }

        public event EventHandler<string> Updated;

        #region Properties

        public string Prefix => DateTime.Now.ToLongTimeString() + ": ";

        private string _buffer;
        public string Buffer
        {
            set
            {
                _buffer = value;
                NotifyPropertyChanged();
            }
            get => _buffer;
        }

        public bool IsBufferEmpty => string.IsNullOrWhiteSpace(Buffer);

        #endregion

        #region Methods

        public void Log(object sender, string line) => Log(line);
        public void Log(string line)
        {
            if (!IsBufferEmpty)
                Buffer += Environment.NewLine;
            Buffer += Prefix + line;
            Updated?.Invoke(this, line);
            Debug.WriteLine(Prefix + line);

            if (!Debugger.IsAttached)
                try
                {
                    File.AppendAllText($"{Directory.GetCurrentDirectory()}\\log.txt", Prefix + line + Environment.NewLine);
                }
                catch
                {
                    Updated?.Invoke(this, "ERROR: Failed to write to log.");
                }
        }

        public void Clear() => Buffer = string.Empty;
        public void Copy() => Clipboard.SetText(Buffer);

        #endregion

        #region Context Menu

        private void CopyBuffer(object sender = null, EventArgs e = null) => Copy();
        private void ClearBuffer(object sender = null, EventArgs e = null) => Clear();

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
