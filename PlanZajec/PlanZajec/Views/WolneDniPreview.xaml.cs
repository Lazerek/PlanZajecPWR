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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for WolneDniPreview.xaml
    /// </summary>
    public partial class WolneDniPreview : UserControl
    {
        public WolneDniPreview()
        {
            InitializeComponent();
        }

        public void WyswietlWyklucznieaZPlanu(long idPlanu)
        {
            WolnyCzasListBox.Items.Clear();
            string[] wolneDni;
            using (var dataBaseUnitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                wolneDni = dataBaseUnitOfWork.Plany.Get(idPlanu).GetWolneDni();
            }
            if (wolneDni == null) return;
            foreach (var czas in wolneDni)
            {
                var tempCzas = czas.Split(':');
                WolnyCzasListBox.Items.Add(tempCzas[2] + " od " + tempCzas[0] + "do " + tempCzas[1]);
            }
        }
    }
}
