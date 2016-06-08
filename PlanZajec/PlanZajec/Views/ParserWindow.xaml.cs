using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Wpf;
using Application = System.Windows.Application;

namespace PlanZajec.Views
{
    /// <summary>
    /// klasa typu window wyświetlająca okno z parserem
    /// </summary>
    public partial class ParserWindow : Window
    {

        public ParserWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Metoda uruchamiająca parser przy wciśnieciu przycisku
        /// </summary>

        private void RunParserButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool loaded = Parser.Parser.Run();
            if (loaded)
            {
                MainWindow window = Application.Current.Windows.OfType<MainWindow>().First();
                window.ReloadWindowComponents();
            }
        }

        private void SzukajButton_OnClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                SzukajTextBlock.Text = fbd.SelectedPath;
            }
        }
    }
}
