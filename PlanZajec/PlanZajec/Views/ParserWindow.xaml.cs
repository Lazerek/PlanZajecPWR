using System.Linq;
using System.Windows;
using Wpf;

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
    }
}
