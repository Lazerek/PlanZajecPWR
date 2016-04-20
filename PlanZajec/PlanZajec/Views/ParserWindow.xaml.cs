using System.Windows;

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
            
        }
    }
}
