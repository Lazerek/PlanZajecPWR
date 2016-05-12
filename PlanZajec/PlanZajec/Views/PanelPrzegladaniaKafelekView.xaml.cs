using PlanZajec.CommonInformations;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for PanelPrzegladaniaKafelekView.xaml
    /// </summary>
    public partial class PanelPrzegladaniaKafelekView : UserControl
    {
        private readonly Image addIcon;
        private readonly Image minusIcon;
        private GrupyZajeciowe grupaToAddOrDeleteFromPlan;

        public PanelPrzegladaniaKafelekView()
        {
            InitializeComponent();
            addIcon = new Image() { Source = new BitmapImage(new Uri("Images/addIcon.png", UriKind.Relative)) };
            minusIcon = new Image() { Source = new BitmapImage(new Uri("Images/minusIcon.png", UriKind.Relative)) };           
        }

        private void PrepareOpenContextMenu(object sender, ContextMenuEventArgs e)
        {
            KafelekGrup kafelek = sender as KafelekGrup;
            GrupyZajeciowe grupa = kafelek?.DataContext as GrupyZajeciowe;         
            FrameworkElement fe = e.Source as FrameworkElement;

            fe.ContextMenu = GetContextMenu(grupa);
        }

        private ContextMenu GetContextMenu(GrupyZajeciowe grupa)
        {
            System.Diagnostics.Debug.WriteLine("@@@Run|->GetContextMenu)");
            if(grupa == null || ActChosenPlanSingleton.Instance.IdPlanu < 0)
            {
                return null;
            }
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
                    menuAddOrRemovFromPlan.Click += OnRemoveFromPlanHandler;             
                }
                else
                {
                    menuAddOrRemovFromPlan.Header = "Dodaj do planu";
                    menuAddOrRemovFromPlan.Icon = addIcon;
                    menuAddOrRemovFromPlan.Click += OnAddToPlanHandler;
                    CommandManager.InvalidateRequerySuggested();
                }
            }
            theMenu.Items.Add(menuAddOrRemovFromPlan);
            return theMenu;
        }

        private void OnAddToPlanHandler(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                unitOfWork.Plany.DodajGrupeZajeciowaDoPlanu(grupaToAddOrDeleteFromPlan);
            }
            PlanView.RefreshSchedule();
        }

        private void OnRemoveFromPlanHandler(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                unitOfWork.Plany.UsunGrupeZajeciowaZPlanu(grupaToAddOrDeleteFromPlan);
            }
            PlanView.RefreshSchedule();
        }
    }
}
