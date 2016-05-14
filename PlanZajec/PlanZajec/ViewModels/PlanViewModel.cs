using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Wpf;

namespace PlanZajec.ViewModels
{
    public class PlanViewModel:ViewModel
    {
        public long IdPlanu
        {
            get; private set;
        }

        private Plany plan;

        public List<GrupyZajeciowe> ListaGrupZajeciowych { get; private set; }
        public List<GrupaZajeciowaTemplate> Kafelki { get; private set; }
        public PlanViewModel(long id)
        {
            IdPlanu = id;
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Kafelki =new List<GrupaZajeciowaTemplate>();
                plan = uw.Plany.Get(id);
                ListaGrupZajeciowych = plan.GrupyZajeciowe.ToList();
                foreach(GrupyZajeciowe gr in ListaGrupZajeciowych)
                {
                    var temp = new GrupaZajeciowaTemplate(new GrupaTamplateViewModel(gr.KodGrupy));
                    
                    var okno= Application.Current.Windows.OfType<MainWindow>().First();
                    var a = okno.DataContext as PlanView;
                    System.Diagnostics.Debug.WriteLine(a);
                    Kafelki.Add(temp);
                }
                //ListaGrupZajeciowych[0];
            }
        }
        public void aktualizuj()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Kafelki = new List<GrupaZajeciowaTemplate>();
                ListaGrupZajeciowych = uw.Plany.Get(IdPlanu).GrupyZajeciowe.ToList();
                foreach (GrupyZajeciowe gr in ListaGrupZajeciowych)
                {
                    var temp = new GrupaZajeciowaTemplate(new GrupaTamplateViewModel(gr.KodGrupy));


                    var okno = Application.Current.Windows.OfType<MainWindow>().First();
                    var a = okno.DataContext as PlanView;
                    System.Diagnostics.Debug.WriteLine(a);
                    Kafelki.Add(temp);
                    NotifyPropertyChange("ListaGrupZajeciowych");
                    NotifyPropertyChange("Kafelki");
                }
                //ListaGrupZajeciowych[0];
            }
        }

        //TODO: Fail in projecting GrupaTamplateViewModel, it's hard to quck refactor - Tip: Refector klas to binding!
        /*
        private void TempOnContextMenuOpening(object sender, ContextMenuEventArgs contextMenuEventArgs)
        {
            GrupaZajeciowaTemplate grupaTemplate = sender as GrupaZajeciowaTemplate;
            GrupyZajeciowe grupa = grupaTemplate. .DataContext as GrupyZajeciowe;
            FrameworkElement fe = e.Source as FrameworkElement;

            fe.ContextMenu = GetContextMenu(grupa);
        }

        private ContextMenu GetContextMenu(UnitOfWork uw, GrupyZajeciowe grupa)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem menuRemove = new MenuItem
            {
                Header = "Usuń z planu",
                Icon = new Image() {Source = new BitmapImage(new Uri("Images/minusIcon.png", UriKind.Relative))}
            };
            menuRemove.Click += new RoutedEventHandler(OnRemoveFromPlanHandler);
            menu.Items.Add((menuRemove));
            return menu;
        }

        private void OnRemoveFromPlanHandler(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                unitOfWork.Plany.UsunGrupeZajeciowaZPlanu(grupaToAddOrDeleteFromPlan);
            }
            PlanView.RefreshSchedule();
        }
        */
    }
}
