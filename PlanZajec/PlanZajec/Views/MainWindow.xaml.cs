﻿using PlanZajec.DataAccessLayer;
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
    /// Klasa wyświetlająca okno główne planu
    /// </summary>
    public partial class MainWindow : Window
    {
        private PanelFiltrow _panelFiltrow;
        public PanelGlowny PanelGlownyOkna { get; private set; }

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
            PanelGlownyOkna = new PanelGlowny(this);
            PGlowny.Children.Add(PanelGlownyOkna);
            //add panel filtrow
            _panelFiltrow = new PanelFiltrow(this);
            PFiltrow.Children.Add(_panelFiltrow);
        }
        /// <summary>
        /// Metoda zamykająca aplikację przy wyjściu
        /// </summary>

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Metoda pokazująca okno z parserem
        /// </summary>

        public void ShowParserWindow(object sender, EventArgs e)
        {
            ParserWindow pw = new ParserWindow();
            pw.Show();
        }
        /// <summary>
        /// Metoda pokazująca okno HTML
        /// </summary>

        public void ShowHTMLWindow(object sender, EventArgs e)
        {
            OknoDanychEdukacji ode = new OknoDanychEdukacji();
            ode.ShowDialog();
        }

        /// <summary>
        /// Metoda pozwalająca dodać nowy plan z poziomu menu
        /// </summary>

        public void menuNowyPlan(object sender, EventArgs e)
        {
            AddPlanWindow addPlan = new AddPlanWindow();
            addPlan.ShowDialog();
        }

        /// <summary>
        /// Metoda pozwalająca utworzyć alternatywny plan na podstawie istniejącego
        /// </summary>

        public void menuNowyAlternatywnyPlan(object sender, EventArgs e)
        {
            ListaPlanow lp = preparePlanList();
            lp.ShowDialog();
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
        /// <param name="plan">Pierwotny plan</param>
        private void AddPlan(string Title, Plany plan)
        {
            using (var unit = new UnitOfWork(new PlanPwrContext()))
            {
                Plany nowyPlan = new Plany { NazwaPlanu = Title };
                var staryPlan = unit.Plany.Get(plan.IdPlanu);
                foreach (GrupyZajeciowe g in staryPlan.GrupyZajeciowe)
                {
                    nowyPlan.GrupyZajeciowe.Add(g);
                }
                unit.Plany.Add(nowyPlan);
                unit.SaveChanges();
                PlanyViewModel.Instance.DodajPlan(nowyPlan);
            }
        }

        /// <summary>
        /// Metoda pozwalająca otworzyć zapisany plik z planem
        /// </summary>

        public void menuOtworz(object sender, EventArgs e)
        {
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

        public void menuZapisz(object sender, EventArgs e)
        {
            ZapisWindow zw = new ZapisWindow();
            zw.ShowDialog();
        }

        /// <summary>
        /// Metoda otwierająca okno z zapisem planu
        /// </summary>

        public void menuZapiszJako(object sender, EventArgs e)
        {
            ZapisWindow zw = new ZapisWindow();
            zw.ShowDialog();
        }

        /// <summary>
        /// Metoda otwierająca okno do drukowania planu
        /// </summary>

        public void menuDrukuj(object sender, EventArgs e)
        {
            DrukujWindow dw = new DrukujWindow();
            dw.ShowDialog();
        }

        /// <summary>
        /// Metoda ekposrtująca plik jako pdf
        /// </summary>

        public void menuEksportujPDF(object sender, EventArgs e)
        {
            Eksportuj eks = new Eksportuj(true);
            eks.ShowDialog();
        }

        /// <summary>
        /// Metoda eksportująca plan jako plik graficzny
        /// </summary>

        public void menuEksportujPlikGraficzny(object sender, EventArgs e)
        {
            Eksportuj eks = new Eksportuj(false);
            eks.ShowDialog();
        }

        /// <summary>
        /// Metoda zakończająca działanie programu
        /// </summary>

        public void menuZakoncz(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Metoda otwierająca okno z informacjami
        /// </summary>

        public void menuInformacje(object sender, EventArgs e)
        {
            OknoInformacji o = new OknoInformacji();
            o.ShowDialog();
        }
        /// <summary>
        /// Metoda przełądowująca komponenty okna
        /// </summary>
        public void ReloadWindowComponents()
        {
            //reload palen glowny
            PGlowny.Children.RemoveAt(0);
            PanelGlownyOkna = new PanelGlowny(this);
            PGlowny.Children.Add(PanelGlownyOkna);
            //relaod panel filtrow
            PFiltrow.Children.RemoveAt(0);
            _panelFiltrow = new PanelFiltrow(this);
            PFiltrow.Children.Add(_panelFiltrow);
        }
    }
}
