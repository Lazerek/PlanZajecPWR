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
    /// Klasa typu window wyświetlająca kafelki z panelu
    /// </summary>
    public partial class PanelPrzegladaniaKafelekView : UserControl
    {
        private readonly Image addIcon;
        private readonly Image minusIcon;
        private GrupyZajeciowe grupaToAddOrDeleteFromPlan;

        /// <summary>
        /// Domyślny konstruktor
        /// </summary>
        public PanelPrzegladaniaKafelekView()
        {
            InitializeComponent();
            addIcon = new Image() { Source = new BitmapImage(new Uri("Images/addIcon.png", UriKind.Relative)) };
            minusIcon = new Image() { Source = new BitmapImage(new Uri("Images/minusIcon.png", UriKind.Relative)) };           
        }
        /// <summary>
        /// Metoda przygotowująca do otwarcia menu kontekstowego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrepareOpenContextMenu(object sender, ContextMenuEventArgs e)
        {
            KafelekGrup kafelek = sender as KafelekGrup;
            GrupyZajeciowe grupa = kafelek?.DataContext as GrupyZajeciowe;         
            FrameworkElement fe = e.Source as FrameworkElement;

            fe.ContextMenu = GetContextMenu(grupa);
        }

        /// <summary>
        /// Metoda zwracająca menu
        /// </summary>
        /// <param name="grupa">Grupa zajęciowa</param>
        /// <returns>Kontekstowe menu</returns>
        private ContextMenu GetContextMenu(GrupyZajeciowe grupa)
        {
            ContextMenu theMenu = new ContextMenu();
            if (grupa == null || ActChosenPlanSingleton.Instance.IdPlanu < 0)
            {
                theMenu.Visibility = Visibility.Hidden;
                return theMenu;
            }
            
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
            }
            theMenu.Items.Add(menuAddOrRemovFromPlan);
            return theMenu;
        }
        /// <summary>
        /// Obsługa na dodanie kafelka do planu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddToPlanHandler(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                unitOfWork.Plany.DodajGrupeZajeciowaDoPlanu(grupaToAddOrDeleteFromPlan);
            }
            PlanView.RefreshSchedule();
        }
        /// <summary>
        /// Obsługa na usunięciu z planu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
