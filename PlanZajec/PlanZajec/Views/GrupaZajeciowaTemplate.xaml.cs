﻿using PlanZajec.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for GrupaZajeciowaTemplate.xaml
    /// </summary>
    public partial class GrupaZajeciowaTemplate : UserControl
    {
        private GrupaTamplateViewModel viewModel;
        public GrupaZajeciowaTemplate(GrupaTamplateViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            Start.Text = viewModel.GrupZaj.Godzina;
            Koniec.Text = viewModel.GrupZaj.GodzinaKoniec;

            Nazwa.Text = viewModel.nazwa;
            Nazwa.ToolTip = viewModel.nazwa;

            if(!viewModel.GrupZaj.Tydzien.Equals("//"))
                Tydzen.Text = viewModel.GrupZaj.Tydzien;
            Budynek.Text = viewModel.GrupZaj.Budynek;
            Sala.Text = viewModel.GrupZaj.Sala;
            String typZajec = viewModel.GrupZaj.TypZajec;
            Color colResult;
            switch (typZajec)
            {
                case "Wykład": colResult = Colors.Green; break;
                case "Zajęcia laboratoryjne": colResult = Colors.Orange; break;
                case "Projekt": colResult = Colors.Orchid; break;
                case "Seminarium": colResult = Colors.LightBlue; break;
                case "Praktyka": colResult = Colors.Yellow; break;
                default: colResult = Colors.Black; break;
            }
            SolidColorBrush solidResult = new SolidColorBrush(colResult);
            Kafel.Background = solidResult;
        }

        private void GrupaZajeciowaTemplate_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Nazwa.MaxWidth = e.NewSize.Width;
        }

        private void Usun_DoubleClik()
        {
            System.Diagnostics.Debug.WriteLine("Yolo");
        }
    }
}
