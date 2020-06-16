using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace VersionVSClase
{
    public class CambioNumCartasArgs : EventArgs
    {
        public int NumCartas { get; set; }
        public CambioNumCartasArgs(int n) { NumCartas = n; }
    }

    public class CambioTiempoArgs : EventArgs
    {
        public int Tiempo { get; set; }
        public CambioTiempoArgs(int n) { Tiempo = n; }
    }

    public class EliminarCartaArgs : EventArgs
    {
        public Card carta { get; set; }
        public EliminarCartaArgs(Card c) { carta = c; }
    }

    public delegate void CambioNumEventHandler(Object sender, CambioNumCartasArgs e);
    public delegate void CambioTiempoEventHandler(Object sender, CambioTiempoArgs e);
    public delegate void EliminarCartaEventHandler(Object sender, EliminarCartaArgs e);

    public partial class Preferencias : Window
    {
        public event CambioNumEventHandler CambioNum;
        public event CambioTiempoEventHandler CambioTemp;
        public event EliminarCartaEventHandler EliminarCard;

        void OnCambioNum(int n)
        {
            if (CambioNum != null)
                CambioNum(this, new CambioNumCartasArgs(n));
        }

        void OnCambioTiempo(int n)
        {
            if (CambioTemp != null)
                CambioTemp(this, new CambioTiempoArgs(n));
        }

        void OnEliminarCarta(Card c)
        {
            if (EliminarCard != null)
                EliminarCard(this, new EliminarCartaArgs(c));
        }

        public Preferencias(MainWindow owner, ObservableCollection<Card> baraja)
        {
            this.Owner = owner;
            InitializeComponent();
            lista.ItemsSource = baraja;


        }

        public int NumCartas
        {
            get { return (int)sliderCartas.Value; }
            set { sliderCartas.Value = value; }
        }

        public int Tiempo
        {
            get { return (int)sliderTiempo.Value; }
            set { sliderTiempo.Value = value; }
        }

        private void SliderTiempo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OnCambioTiempo((int)sliderTiempo.Value);
        }

        private void SliderCartas_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OnCambioNum((int)sliderCartas.Value);
        }

        private void lista_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(lista.SelectedItem != null)
            {
                Eliminar.IsEnabled = true;
            }
            else
            {
                Eliminar.IsEnabled = false;
            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            OnEliminarCarta((Card)lista.SelectedItem);
        }
    }
}
