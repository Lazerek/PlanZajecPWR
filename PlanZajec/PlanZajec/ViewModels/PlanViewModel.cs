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
        public List<GrupyZajeciowe> ListaGrupZajeciowych { get; private set; }
        public List<GrupaZajeciowaTemplate> Kafelki { get; private set; }
        public PlanViewModel(long id)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Kafelki=new List<GrupaZajeciowaTemplate>();
                ListaGrupZajeciowych = uw.Plany.Get(id).GrupyZajeciowe.ToList();
                foreach(GrupyZajeciowe gr in ListaGrupZajeciowych)
                {
                    var temp = new GrupaZajeciowaTemplate(gr.KodGrupy);
                    
                    var okno= Application.Current.Windows.OfType<MainWindow>().First();
                    var a = okno.DataContext as PlanView;
                    System.Diagnostics.Debug.WriteLine(a);
                    Kafelki.Add(temp);
                }
                //ListaGrupZajeciowych[0];
            }
        }
    }
}
