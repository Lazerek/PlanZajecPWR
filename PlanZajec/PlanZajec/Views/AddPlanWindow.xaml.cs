using System.Windows;
using System.Windows.Input;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa typu windows pozwalająca dodać nowy plan
    /// </summary>
    public partial class AddPlanWindow : Window
    {
        public string PlanTitle { get; set; }
        /// <summary>
        /// Domyślny konstruktor
        /// </summary>
        public AddPlanWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        /// <summary>
        /// Metoda dodająca plan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dodaj(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(PlanTitle))
            {
                MessageBox.Show("Plan musi mieć nazwę!");
            }
            else
            {
                DialogResult = true;
            }
        }
        /// <summary>
        /// Metoda pozwalająca anulować dodawanie planu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Anuluj(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Metoda tworząca scape dla okna głównego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sprawdzPrzyciski(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dodajButton.Focus(); //Przeniesienie zaznaczenie z textboxa na button
                Dodaj(this, new RoutedEventArgs());
                return;
            }
            if (e.Key == Key.Escape)
            {
                this.Close();
                return;
            }
        }
    }
}
