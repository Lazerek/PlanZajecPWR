using System;
using System.Collections.Generic;
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

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa okna informacji o programie
    /// </summary>
    public partial class OknoInformacji : Window
    {
        /// <summary>
        /// Konstuktor tworzący okno
        /// </summary>
        public OknoInformacji()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Metoda zamykająca okno przy wciśnięciu przycisku ok
        /// </summary>
        private void OkClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}