using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PlanZajec.ViewModels;
using Wpf;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for ParserWindow.xaml
    /// </summary>
    public partial class ParserWindow : Window
    {
        public ParserWindow()
        {
            InitializeComponent();
        }

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
