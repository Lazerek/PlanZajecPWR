using PlanZajec.ViewModels;
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

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for PlanView.xaml
    /// </summary>
    public partial class PlanView : UserControl
    {
        private PlanViewModel viewModel;
      

        public PlanView()
        {
           
            InitializeComponent();
            TabelaGrup.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            viewModel = new PlanViewModel();
            this.DataContext = viewModel;
           
            for (int i = 0; i < viewModel.Kafelki.Count; i++)
            {
                int tempR = 0;
                double tempC = 0;
                switch(viewModel.ListaGrupZajeciowych[i].Dzień)
                {
                    case "pn": tempR = 0;
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
                string godzina = viewModel.ListaGrupZajeciowych[i].Godzina;
                var tempS = godzina.Split(':');

              
                double t = 3200;
                tempC = (t/(60*14)*(int.Parse(tempS[0]) * 60 + int.Parse(tempS[1])))-7*60-(t-400);
                
                var tempChild = viewModel.Kafelki[i];
                    TabelaGrup.Children.Add(tempChild);
                Grid.SetRow(tempChild, tempR);
                Thickness margin = tempChild.Margin;
                margin.Left = ((int)tempC);
                tempChild.Margin = margin;

            }
        }
    }
}
