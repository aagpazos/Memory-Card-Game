using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VersionVSClase
{
    public class Card : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string nombreCarta { get; set; }
        public bool abierta_;
        public Image carta;
        public Image cartaAtras;


        public bool abierta
        {
            get { return abierta_; }
            set { abierta_ = value; OnPropertyChanged("abierta"); }
        }


        public Card(Random random)
        {
            string[] randomName = {
                "Ciri.jpg",
                "Eredin.jpg",
                "Jaskier.jpg",
                "Triss.jpg",
                "Geralt.jpg",
                "Yennefer.jpg",
                "Beauclair.jpg",
                "KaerMorhen.png",
                "Nilfgaard.jpg",
                "Novigrad.jpg",
                "Oxenfurt.jpg" };

            nombreCarta = randomName[random.Next(randomName.Length)];
            abierta = false;
            carta = IniciarImagen(nombreCarta);
            cartaAtras = IniciarImagen("back.jpg");
            carta.Height = cartaAtras.Height = 150;

        }

        public Card(Card cartaACopiar)
        {
            nombreCarta = cartaACopiar.nombreCarta;
            abierta = cartaACopiar.abierta;
            carta = IniciarImagen(cartaACopiar.nombreCarta);
            cartaAtras = IniciarImagen("back.jpg");
            carta.Height = cartaAtras.Height = 150;
        }

        public Image IniciarImagen(String rutaCarta)
        {
            string ruta = "/imagenes/";
            ruta = string.Concat(ruta, rutaCarta);
            Image imagenCarta = new Image();
            BitmapImage imagen = new BitmapImage();
            imagen.BeginInit();
            imagen.UriSource = new Uri(ruta, UriKind.Relative);
            imagen.EndInit();
            imagenCarta.Stretch = Stretch.Fill;
            imagenCarta.Source = imagen;
            return imagenCarta;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
               new PropertyChangedEventArgs(propertyName));
        }

   

    }

    static class ShuffleClass
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
