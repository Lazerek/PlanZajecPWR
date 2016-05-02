using System.Windows.Controls;
using PlanZajec.ViewModels;

namespace PlanZajec.Views
{
    /// <summary>
    ///     Interaction logic for PlanView.xaml
    /// </summary>
    public partial class PlanView : UserControl
    {
        private static PlanView pv;
        private PlanViewModel viewModel;

        public PlanView()
        {
            InitializeComponent();
            viewModel = new PlanViewModel();
            DataContext = viewModel;
            NarysujPlan();
            pv = this;
        }

        private void NarysujPlan()
        {
            for (var i = 0; i < viewModel.Kafelki.Count; i++)
            {
                var kafelek = viewModel.Kafelki[i];
                var numerDnia = 0;
                switch (viewModel.ListaGrupZajeciowych[i].Dzień)
                {
                    case "pn":
                        numerDnia = 0;
                        break;
                    case "wt":
                        numerDnia = 1;
                        break;
                    case "śr":
                        numerDnia = 2;
                        break;
                    case "cz":
                        numerDnia = 3;
                        break;
                    case "pt":
                        numerDnia = 4;
                        break;
                    case "sb":
                        numerDnia = 5;
                        break;
                    case "nd":
                        numerDnia = 6;
                        break;
                }
                var godzinaRozpoczeciaString = viewModel.ListaGrupZajeciowych[i].Godzina;
                var godzinaRozpoczecia = int.Parse(godzinaRozpoczeciaString.Split(':')[0]) - 1 +
                                         (int.Parse(godzinaRozpoczeciaString.Split(':')[1]) <= 30 ? 0 : 1);
                var godzinaZakonczeniaString = viewModel.ListaGrupZajeciowych[i].GodzinaKoniec;
                var godzinaZakonczenia = int.Parse(godzinaZakonczeniaString.Split(':')[0]) - 1 +
                                         (int.Parse(godzinaZakonczeniaString.Split(':')[1]) <= 30 ? 0 : 1);
                var czasTrwania = godzinaZakonczenia - godzinaRozpoczecia;

                TabelaGrup.Children.Add(kafelek);
                Grid.SetRow(kafelek, numerDnia);
                Grid.SetColumn(kafelek, godzinaRozpoczecia - 6);
                Grid.SetColumnSpan(kafelek, czasTrwania);
            }
        }

        private void Usun()
        {
            foreach (var gr in viewModel.Kafelki)
            {
                TabelaGrup.Children.Remove(gr);
            }
        }

        public static void Aktualizuj()
        {
            pv.viewModel = new PlanViewModel();
            pv.DataContext = pv.viewModel;
            pv.Usun();
            pv.NarysujPlan();
        }
    }
}