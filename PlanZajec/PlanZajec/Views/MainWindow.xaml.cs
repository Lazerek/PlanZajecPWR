using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.ComponentModel;
using System.Windows;
using PlanZajec.Views;
using System;
using System.Linq;
using PlanZajec.CommonInformations;
using PlanZajec.Parser;
using PlanZajec.ViewModels;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PanelFiltrow _panelFiltrow;
        private PanelGlowny _panelGlowny;
        public MainWindow()
        {
            //DATABASE LOAD
            DataBaseReturnPoint.PrePrareDB();
            using(var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                if( unitOfWork.Plany.Count() < 1)
                {
                    Plany plan = new Plany() { NazwaPlanu = "Główny"};
                    unitOfWork.Plany.Add(plan);
                    unitOfWork.SaveChanges();
                    ActChosenPlanSingleton.Instance.SetPlan (plan);
                }
                else
                {
                    ActChosenPlanSingleton.Instance.SetPlan(unitOfWork.Plany.GetFirstOrDefault());
                }
            }
            //Initialize window
            InitializeComponent();
            _panelGlowny = new PanelGlowny(this);
            PGlowny.Children.Add(_panelGlowny);
            //add panel filtrow
            _panelFiltrow = new PanelFiltrow(this);
            PFiltrow.Children.Add(_panelFiltrow);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //MENU
        //Menu - Wyświetlenie parsera
        public void ShowParserWindow(object sender, EventArgs e)
        {
            ParserWindow pw = new ParserWindow();
            pw.Show();
        }
        public void ShowHTMLWindow(object sender, EventArgs e)
        {
            OknoDanychEdukacji ode = new OknoDanychEdukacji();
            ode.Show();
        }

        //Menu - Utworzenie nowego planu
        public void menuNowyPlan(object sender, EventArgs e)
        {
            //TODO
        }

        //Menu - Utworzenie nowego planu na podstawie istniejacego planu
        public void menuNowyAlternatywnyPlan(object sender, EventArgs e)
        {
            ListaPlanow lp = preparePlanList();
            lp.Show();
        }

        private ListaPlanow preparePlanList()
        {
            var res = new ListaPlanow();
            res.DodajPlan += AddPlan;
            return res;
        }

        private void AddPlan(string Title, int id)
        {
            Plany plan;
            using (var unit = new UnitOfWork(new PlanPwrContext()))
            {
                plan = new Plany { NazwaPlanu = Title };
                var plist = unit.Plany.GetAll().GetEnumerator();
                int i = 0;
                while (plist.MoveNext())
                {
                    if(id == i)
                    {
                        foreach(GrupyZajeciowe g in plist.Current.GrupyZajeciowe){
                            plan.GrupyZajeciowe.Add(g);
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
                unit.Plany.Add(plan);
                unit.SaveChanges();
                WyborPlanuViewModel.Instance.DodajPlan(plan);
                UsunPlanViewModel.Instance.DodajPlan(plan);
            }
        }

        //Menu - Otwarcie planu z pliku
        public void menuOtworz(object sender, EventArgs e)
        {
            //TODO
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Plan zajęć (*.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                ZapisDoBazy.Importuj(dlg);
                this.ReloadWindowComponents();
            }
        }

        //Menu - Zapis planów do plików
        public void menuZapisz(object sender, EventArgs e)
        {
            //TODO


            ZapisWindow zw = new ZapisWindow();
            zw.ShowDialog();


        }

        //Menu - Funkcja Zapisz jako
        public void menuZapiszJako(object sender, EventArgs e)
        {
            //TODO
            ZapisWindow zw = new ZapisWindow();
            zw.ShowDialog();

        }

        //Menu - Metoda drukująca plan
        public void menuDrukuj(object sender, EventArgs e)
        {
            DrukujWindow dw = new DrukujWindow();
            dw.ShowDialog();
        }

        //Menu - Metoda eksportująca plan do pliku PDF
        public void menuEksportujPDF(object sender, EventArgs e)
        {
            //TODO
        }

        //Menu - Metoda eksportująca plan do pliku graficznego
        public void menuEksportujPlikGraficzny(object sender, EventArgs e)
        {
            //TODO
        }

        //Menu - Metoda kończąca działanie programu
        public void menuZakoncz(object sender, EventArgs e)
        {
            this.Close();
        }

        //Menu - Metoda wyświetlająca okienko z informacjami o programie
        public void menuInformacje(object sender, EventArgs e)
        {
            //TODO
        }

        public void ReloadWindowComponents()
        {
            //reload palen glowny
            PGlowny.Children.RemoveAt(0);
            _panelGlowny = new PanelGlowny(this);
            PGlowny.Children.Add(_panelGlowny);
            //relaod panel filtrow
            PFiltrow.Children.RemoveAt(0);
            _panelFiltrow = new PanelFiltrow(this);
            PFiltrow.Children.Add(_panelFiltrow);
        }

        public void UpdateOnSelectedPlanChange(long? number)
        {
            _panelFiltrow.UpdateOnSelectedPlanChange(number);
        }
    }
}
