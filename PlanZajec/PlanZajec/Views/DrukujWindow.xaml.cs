using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PrintDialog = System.Windows.Controls.PrintDialog;
using PlanZajec.ViewModels;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa typu Window pozwalająca wydrukować plan
    /// </summary>
    public partial class DrukujWindow : Window
    {
        public ObservableCollection<Plany> plany { get; private set; }
        private long[] tab;
        /// <summary>
        /// Konstruktor tworzący okienko wyświetlające możliwe plany
        /// </summary>
        public DrukujWindow()
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
        /// Przycisk drukowania
        /// </summary>
        /// <param name="sender">/param>
        /// <param name="e"></param>
        private void Drukuj(object sender, RoutedEventArgs e)
        {
            PlanViewModel pvm = new PlanViewModel(tab[PlanyComboBox.SelectedIndex]);
            PlanView pv = new PlanView(pvm);
            // PrintDialog printDialog = new PrintDialog();
            PrintDialog Objprint = new PrintDialog();
            if (Objprint.ShowDialog() == true)
            {
                System.Printing.PrintCapabilities capabilities = Objprint.PrintQueue.GetPrintCapabilities(Objprint.PrintTicket);
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / pv.ActualWidth, capabilities.PageImageableArea.ExtentHeight / pv.ActualHeight);
                pv.LayoutTransform = new ScaleTransform(scale, scale);
                Size size = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                pv.Measure(size);
                pv.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), size));
                Objprint.PrintVisual(pv, "Plan");
            }
        }
        private void Anuluj(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
