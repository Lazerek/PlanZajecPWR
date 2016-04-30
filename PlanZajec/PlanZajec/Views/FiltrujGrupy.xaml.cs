﻿using System;
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
using PlanZajec.ViewModels;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for FiltrujGrupy.xaml
    /// </summary>
    public partial class FiltrujGrupy : UserControl
    {
        private PrzegladanieGrupViewModel viewModel;
        public FiltrujGrupy()
        {
            InitializeComponent();
            cb_wsz.IsChecked = true;
        }

        public void wyczyscFiltr(object sender, EventArgs e)
        {
            nazwaK.Text = "";
            nazwaP.Text = "";
            nazwaKG.Text = "";
            nazwaKK.Text = "";
            nazwaPot.Text = "";
            cb_wsz.IsChecked = true;
            cb_wyk.IsChecked = false;
            cb_lab.IsChecked = false;
            cb_pro.IsChecked = false;
            cb_sem.IsChecked = false;
        }

        private void filtruj_Click(object sender, RoutedEventArgs e)
        {
            PrzegladanieGrupViewModel.temp.Filtruj(nazwaK.Text, nazwaPot.Text, nazwaKG.Text, nazwaKK.Text, nazwaP.Text);
        }

        private void filtrujEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                PrzegladanieGrupViewModel.temp.Filtruj(nazwaK.Text, nazwaPot.Text, nazwaKG.Text, nazwaKK.Text, nazwaP.Text);
        }
    }
}
