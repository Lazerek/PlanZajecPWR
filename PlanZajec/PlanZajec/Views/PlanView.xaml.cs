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
        private readonly PlanViewModel viewModel;
      
        public PlanView()
        {
            InitializeComponent();
            TabelaGrup.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            viewModel = new PlanViewModel();
            DataContext = viewModel;
           
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

              
                double  t = 3200;
                tempC = ( t/(60*14)*(int.Parse( tempS[0]) * 60 + int.Parse( tempS[1])))-7*60-( t-400);
                
                var tempChild = viewModel.Kafelki[i];
                    TabelaGrup.Children.Add(tempChild);
                Grid.SetRow(tempChild, tempR);
                var margin = tempChild.Margin;
                margin.Left = (int) tempC;
                tempChild.Margin = margin;
            }
        }
    }
}
