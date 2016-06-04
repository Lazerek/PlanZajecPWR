using System.Windows.Controls;
using PlanZajec.ViewModels;

namespace PlanZajec
{
    /// <summary>
    /// Klasa typu window wyświetlająca prowadzących
    /// </summary>
    public partial class ProwadzacyMenu : UserControl
    {
        public ProwadzacyViewModel ViewModel;
        /// <summary>
        /// Domyślny konstruktor zawierający prowadzących
        /// </summary>
        public ProwadzacyMenu()
        {
            InitializeComponent();
            ViewModel = new ProwadzacyViewModel();
            DataContext = ViewModel;
        }
        /// <summary>
        /// Filtorwanie prowadzących po wpisanym tekście
        /// </summary>

        private void SzukajTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.FiltrujProwadzacych(szukajTextBox.Text);
        }
    }
}
