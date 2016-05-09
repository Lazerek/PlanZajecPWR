using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    }
}
