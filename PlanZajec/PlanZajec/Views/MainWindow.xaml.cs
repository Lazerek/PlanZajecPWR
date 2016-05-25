using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.ComponentModel;
using System.Windows;
using PlanZajec.Views;
using System;
using PlanZajec.CommonInformations;
using PlanZajec.Parser;
using PlanZajec.ViewModels;

namespace Wpf
{
    /// <summary>
    /// Klasa wyświetlająca okno główne planu
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Domyślny konstruktor inicjalizujący okno główne
        /// </summary>
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
            PGlowny.Children.Add(new PanelGlowny());
            PFiltrow.Children.Add(new PanelFiltrow(this));
        }
        /// <summary>
        /// Metoda zamykająca aplikację przy wyjściu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Metoda pokazująca okno z parserem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowParserWindow(object sender, EventArgs e)
        {
            ParserWindow pw = new ParserWindow();
            pw.Show();
        }
        /// <summary>
        /// Metoda pokazująca okno HTML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowHTMLWindow(object sender, EventArgs e)
        {
            OknoDanychEdukacji ode = new OknoDanychEdukacji();
            ode.Show();
        }

        /// <summary>
        /// Metoda pozwalająca dodać nowy plan z poziomu menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuNowyPlan(object sender, EventArgs e)
        {
            //TODO
        }

        /// <summary>
        /// Metoda pozwalająca utworzyć alternatywny plan na podstawie istniejącego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuNowyAlternatywnyPlan(object sender, EventArgs e)
        {
            ListaPlanow lp = preparePlanList();
            lp.Show();
        }
        /// <summary>
        /// Metoda przygotowująca listę planów
        /// </summary>
        /// <returns>Lista Planów</returns>
        private ListaPlanow preparePlanList()
        {
            var res = new ListaPlanow();
            res.DodajPlan += AddPlan;
            return res;
        }
        /// <summary>
        /// Metoda dodająca plan
        /// </summary>
        /// <param name="Title">Nazwa planu</param>
        /// <param name="id">Id planu</param>
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

        /// <summary>
        /// Metoda pozwalająca otworzyć zapisany plik z planem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Metoda otwierająca okno z zapisem planu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuZapisz(object sender, EventArgs e)
        {
            //TODO


            ZapisWindow zw = new ZapisWindow();
            zw.ShowDialog();


        }

        /// <summary>
        /// Metoda otwierająca okno z zapisem planu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuZapiszJako(object sender, EventArgs e)
        {
            //TODO
            ZapisWindow zw = new ZapisWindow();
            zw.ShowDialog();

        }

        /// <summary>
        /// Metoda otwierająca okno do drukowania planu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuDrukuj(object sender, EventArgs e)
        {
            DrukujWindow dw = new DrukujWindow();
            dw.ShowDialog();
        }

        /// <summary>
        /// Metoda ekposrtująca plik jako pdf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuEksportujPDF(object sender, EventArgs e)
        {
            //TODO
        }

        /// <summary>
        /// Metoda eksportująca plan jako plik graficzny
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuEksportujPlikGraficzny(object sender, EventArgs e)
        {
            //TODO
        }

        /// <summary>
        /// Metoda zakończająca działanie programu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuZakoncz(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Metoda otwierająca okno z informacjami
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuInformacje(object sender, EventArgs e)
        {
            //TODO
        }
        /// <summary>
        /// Metoda przełądowująca komponenty okna
        /// </summary>
        public void ReloadWindowComponents()
        {
            PGlowny.Children.RemoveAt(0);
            PGlowny.Children.Add(new PanelGlowny());
            PFiltrow.Children.RemoveAt(0);
            PFiltrow.Children.Add(new PanelFiltrow(this));
        }
    }
}
