using System.Data.Entity.Core.Common.EntitySql;
using System.Windows;
using PlanZajec.EdukacjaIntegration;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa do obsługi okna edycji danych
    /// </summary>
    public partial class OknoDanychEdukacji : Window
    {
        /// <summary>
        /// Domyślny konstruktor
        /// </summary>
        public OknoDanychEdukacji()
        { 
            InitializeComponent();
        }
        /// <summary>
        /// Klasa pobierająca dane
        /// </summary>

        private void Sciagnij(object sender, RoutedEventArgs e)
        {
            EdukacjaConnector eduConnector = new EdukacjaConnector(Login.Text, PasswordToEdukacja.Password);
            eduConnector.LinesConnector = Parser.Parser.RunParserForAllLine;
            eduConnector.Run();
        }
        /// <summary>
        /// Klasa do anulowania działania
        /// </summary>

        private void Anuluj(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
