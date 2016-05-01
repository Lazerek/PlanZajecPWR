using PlanZajec.CommonInformations;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
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
    /// Interaction logic for PanelPrzegladaniaKafelekView.xaml
    /// </summary>
    public partial class PanelPrzegladaniaKafelekView : UserControl
    {

        private Image addIcon ;
        private Image minusIcon;

        public PanelPrzegladaniaKafelekView()
        {
            InitializeComponent();
            //DataContext = new PanelPrzegladaniaKafelekViewModel();
            addIcon = new Image() { Source = new BitmapImage(new Uri("Images/addIcon.png", UriKind.Relative)) };
            minusIcon = new Image() { Source = new BitmapImage(new Uri("Images/minusIcon.png", UriKind.Relative)) };           
        }

        private void klinkieteMenu(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Kliknieto!");
        }

        private void PrepareOpenContextMenu(object sender, ContextMenuEventArgs e)
        {
            KafelekGrup kafelek = sender as KafelekGrup;
            GrupyZajeciowe grupa = kafelek.DataContext as GrupyZajeciowe;
            System.Diagnostics.Debug.WriteLine("@@@PanelPrzegladaniaKafelekView|Greg|->"+ grupa);

            FrameworkElement fe = e.Source as FrameworkElement;
            fe.ContextMenu = GetContextMenu(grupa);
        }


        private ContextMenu GetContextMenu(GrupyZajeciowe grupa)
        {
            System.Diagnostics.Debug.WriteLine("@@@Greg|->GetContextMenu)");
            ContextMenu theMenu = new ContextMenu();
            MenuItem menuAddOrRemovFromPlan = new MenuItem();

            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                GrupyZajeciowe grupaUW = unitOfWork.GrupyZajeciowe.Get(grupa.KodGrupy);
                grupaToAddOrDeleteFromPlan = grupaUW;
                if(unitOfWork.GrupyZajeciowe.InThePlan(grupa.KodGrupy, ActChosenPlanSingleton.Instance.IdPlanu))
                {
                    menuAddOrRemovFromPlan.Header = "Usuń z planu";
                    menuAddOrRemovFromPlan.Icon = minusIcon;
                    menuAddOrRemovFromPlan.Click += new RoutedEventHandler(OnRemoveFromPlanHandler);             
                }
                else
                {
                    menuAddOrRemovFromPlan.Header = "Dodaj do planu";
                    menuAddOrRemovFromPlan.Icon = addIcon;
                    menuAddOrRemovFromPlan.Click += new RoutedEventHandler(OnAddToPlanHandler);
                    CommandManager.InvalidateRequerySuggested();
                }
                theMenu.Items.Add(menuAddOrRemovFromPlan);
            }            
            return theMenu;

        }

        private GrupyZajeciowe grupaToAddOrDeleteFromPlan = null;

        private void OnAddToPlanHandler(object sender, RoutedEventArgs e)
        {
            KafelekGrup kafel = sender as KafelekGrup;
            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                unitOfWork.Plany.DodajGrupeZajeciowaDoPlanu(grupaToAddOrDeleteFromPlan);
               

            }
            PlanView.Aktualizuj();
        }

        private void OnRemoveFromPlanHandler(object sender, RoutedEventArgs e)
        {
            KafelekGrup kafel = sender as KafelekGrup;
            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                unitOfWork.Plany.UsunGrupeZajeciowaZPlanu(grupaToAddOrDeleteFromPlan);
            }
        }
    }
}
