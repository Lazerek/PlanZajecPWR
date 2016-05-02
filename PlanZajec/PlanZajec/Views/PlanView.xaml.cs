using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PlanZajec.ViewModels;

namespace PlanZajec.Views
{
    /// <summary>
    ///     Interaction logic for PlanView.xaml
    /// </summary>
    public partial class PlanView : UserControl
    {
        private  PlanViewModel viewModel;
        private static PlanView pv;
        public PlanView()
        {
            InitializeComponent();
            //Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //var t = DesiredSize.Width;

            viewModel = new PlanViewModel();
            DataContext = viewModel;
            NarysujPlan();
            pv = this;
           
           
        }

        private void RysujPlan()
        {
            for (var i = 0; i < viewModel.Kafelki.Count; i++)
            {
                var tempR = 0;
                double tempC = 0;
                switch (viewModel.ListaGrupZajeciowych[i].Dzień)
                {
                    case "pn":
                        tempR = 0;
                        break;
                    case "wt":
                        tempR = 1;
                        break;
                    case "śr":
                        tempR = 2;
                        break;
                    case "cz":
                        tempR = 3;
                        break;
                    case "pt":
                        tempR = 4;
                        break;
                    case "sb":
                        tempR = 5;
                        break;
                    case "nd":
                        tempR = 6;
                        break;
                }
                var godzina = viewModel.ListaGrupZajeciowych[i].Godzina;
                var tempS = godzina.Split(':');

                double t = 3200;
                tempC = (t / (60 * 14) * (int.Parse(tempS[0]) * 60 + int.Parse(tempS[1]))) - 7 * 60 - (t - 400);

                var tempChild = viewModel.Kafelki[i];
                TabelaGrup.Children.Add(tempChild);
                Grid.SetRow(tempChild, tempR);
                var margin = tempChild.Margin;
                margin.Left = (int)tempC;
                tempChild.Margin = margin;
            }
        }

        private void NarysujPlan()
        {
            for (var i = 0; i < viewModel.Kafelki.Count; i++)
            {
                var kafelek = viewModel.Kafelki[i];
                int numerDnia = 0;
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
                string godzinaRozpoczeciaString = viewModel.ListaGrupZajeciowych[i].Godzina;
                int godzinaRozpoczecia = int.Parse(godzinaRozpoczeciaString.Split(':')[0])-1 + ((int.Parse(godzinaRozpoczeciaString.Split(':')[1])) <= 30 ? 0 : 1);
                string godzinaZakonczeniaString = viewModel.ListaGrupZajeciowych[i].GodzinaKoniec;
                int godzinaZakonczenia = int.Parse(godzinaZakonczeniaString.Split(':')[0])-1 + ((int.Parse(godzinaZakonczeniaString.Split(':')[1])) <= 30 ? 0 : 1);
                int czasTrwania = godzinaZakonczenia - godzinaRozpoczecia;
                
                //var t = TabelaGrup.ColumnDefinitions.First(e => Grid.GetColumn(e) == godzinaRozpoczecia);
                //var uiElement = TabelaGrup.Children;//.Cast<UIElement>().ToArray();//.First(e => Grid.GetRow(e) == numerDnia && Grid.GetColumn(e) == godzinaRozpoczecia);
                

                TabelaGrup.Children.Add(kafelek);
                Grid.SetRow(kafelek, numerDnia);
                Grid.SetColumn(kafelek, godzinaRozpoczecia-6);
                Grid.SetColumnSpan(kafelek, czasTrwania);
            }
        }

        private void usun()
        {
            foreach(var gr in viewModel.Kafelki)
            {
                TabelaGrup.Children.Remove(gr);
            }
           
        }
        public static void Aktualizuj()
        {
            pv.viewModel = new PlanViewModel();
            pv.DataContext = pv.viewModel;
            pv.usun();
            pv.NarysujPlan();
        }
    }
}
