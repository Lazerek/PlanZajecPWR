﻿using PlanZajec.DataModel;
using PlanZajec.ViewModels;
using System;
using System.Windows;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa typu window wyświetlająca listę planów
    /// </summary>
    public partial class ListaPlanow : Window
    {
        public event Action<string, Plany> DodajPlan;

        public Plany plan;
        private ListaPlanowViewModel viewModel;

        /// <summary>
        /// Domyślny konstruktor pobierająca dane z viewmodelu listy planów
        /// </summary>
        public ListaPlanow()
        {
            InitializeComponent();
            viewModel = new ListaPlanowViewModel();
            this.DataContext = viewModel;
        }
        /// <summary>
        /// Metoda anulowania okna
        /// </summary>

        public void Anuluj(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Metoda tworzenia nowego planu
        /// </summary>

        public void Utworz(object sender, EventArgs e)
        {
            if(plList.SelectedItems.Count > 0)
            {
                if (!Nazwa.Text.Equals(""))
                {
                    plan = (Plany)plList.SelectedItem;
                    DodajPlan?.Invoke(Nazwa.Text, plan);
                    MessageBox.Show("Utworzono nowy alternatywny plan.", "Dodano plan");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nie wybrano nazwy dla nowego planu.", "Brak nazwy planu");
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano podstawowego planu.", "Nie wybrano planu");
            }
        }
    }
}
