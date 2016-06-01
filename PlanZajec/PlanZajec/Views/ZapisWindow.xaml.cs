using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.Parser;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace PlanZajec.Views
{
    /// <summary>
    ///  Klasa pozwalająca zapisać plan
    /// </summary>


    public partial class ZapisWindow : Window
    {
        public ObservableCollection<Plany> plany { get; private set; }
        private long[] tab;
        /// <summary>
        /// Domyślny konstruktor wyświetlający okno i pobierający listę planów do comboboxa
        /// </summary>
        public ZapisWindow()
        {
            InitializeComponent();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                plany = new ObservableCollection<Plany>(uw.Plany.GetAll().ToList());
            }
            tab = new long[plany.Count()];
            int i = 0;
            foreach (Plany plan in plany)
            {
                PlanyComboBox.Items.Add(plan.NazwaPlanu);
                tab[i] = plan.IdPlanu;
                i++;
            }
            PlanyComboBox.SelectedIndex = 0;
        }
        /// <summary>
        /// Metoda zapisująca plan po wyciśnięciu przycisku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            int index = PlanyComboBox.SelectedIndex;
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Plan zajęć (*.txt)|*.txt"
            };
            if (dialog.ShowDialog() == true)
            {
                ZapisDoBazy.export(dialog, tab[index]);
            }
        }

         /// <summary>
        /// Metoda zamykająca okno bez zapisu planu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
