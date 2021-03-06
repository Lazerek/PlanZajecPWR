﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PlanZajec.ViewModels
{
    /// <summary>
    /// Klasa abstrakcyjna udostępniająca możliwość powiadamiania składowych o zmianach w viewmodelu
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Właściwość event informująca elementy zbidowane z własnosią o zmienie parametru bindowania.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Powiadamia elementy zbidowane z własnosią o zmienie parametru bindowania.
        /// </summary>
        /// <param name="propertyName">
        /// Własność, która ulagła zmianie
        /// </param>
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
