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
            Parser.Parser.Run();
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().First();
            if (window != null)
            {
                foreach (PrzegladanieGrup przegladanieGrup in window.LMenu.Children.OfType<PrzegladanieGrup>())
                {
                    przegladanieGrup.DgUsers.Items.Refresh();
                }
            }
        }
    }
}
