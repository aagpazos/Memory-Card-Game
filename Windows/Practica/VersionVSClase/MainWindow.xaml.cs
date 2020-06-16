using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace VersionVSClase
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int numeroCartas = 6;
        ObservableCollection<Card> baraja;
        DispatcherTimer timer, timerTexto;
        public int tiempoVueltasCartas = 5;
        public int tiempoTexto;
        public int cartasAbiertas = 0;
        public Image logo;
        public Label etiq;
        public Preferencias preferencias;
        public MediaPlayer mediaPlayer;
        public bool reproduciendo;
        Random random;


        public MainWindow()
        {
            InitializeComponent();
            baraja = new ObservableCollection<Card>();
            random = new Random();

            timerTexto = new DispatcherTimer();
            timerTexto.Tick += OnTick_Timer;
            timerTexto.Interval = new TimeSpan(0, 0, 0, 1, 0);

            logo = new Image();
            BitmapImage imagen = new BitmapImage();
            imagen.BeginInit();
            imagen.UriSource = new Uri("/imagenes/logo.png", UriKind.Relative);
            imagen.EndInit();
            logo.Stretch = Stretch.Fill;
            logo.Source = imagen;
            logo.Width = 400;
            Canvas.SetLeft(logo, 150);
            Canvas.SetTop(logo, 120);
            myCanvas.Children.Add(logo);

            mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri("Soundtrack.mp3", UriKind.Relative));
            mediaPlayer.MediaEnded += new EventHandler(Media_Ended);
            mediaPlayer.Play();
            reproduciendo = true;

            replay.IsEnabled = false;
            etiq = new Label { Width = 20, Height = 40, FontSize = 20 };
            Canvas.SetLeft(etiq, myCanvas.ActualWidth);



        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            IniciarJuego();
        }

        public void IniciarJuego()
        {
            cartasAbiertas = 0;
            play.IsEnabled = false;

            timer = new DispatcherTimer();
            timer.Tick += OnTick_TimerCartas;
            timer.Interval = new TimeSpan(0, 0, 0, tiempoVueltasCartas, 0);
            if (preferencias != null)
            {
                preferencias.sliderTiempo.IsEnabled = false;
                preferencias.lista.IsEnabled = false;
            }

            tiempoTexto = tiempoVueltasCartas;
            myCanvas.Children.Remove(logo);
            if (baraja.Count != 0)
            {
                baraja.Clear();
                myCanvas.Children.Clear();
            }

            if (myCanvas.Children.Contains(etiq) == false)
            {
                myCanvas.Children.Add(etiq);
            }
            
            for (int i = 0; i < (numeroCartas / 2); i++)
            {
                Card card = new Card(random);
                baraja.Add(card);
                Card cardCopy = new Card(card);
                baraja.Add(cardCopy);

            }
            baraja.Shuffle();

            int j = 0, k = -1;
            int margenLeft = 35;
            int margenTop = 15;
            int margenEntreCartas = 15;

            foreach (Card carta in baraja)
            {
                k++;
                if (k == 4)
                {
                    k = 0;
                    j++;
                }
                if ((baraja.Count == 10 && j == 2) || (baraja.Count == 6 && j == 1))
                    margenLeft = 200;
                if (baraja.Count == 8 || baraja.Count == 6)
                    margenTop = 90;

                Canvas.SetLeft(carta.carta, margenLeft + k * (carta.carta.Height + margenEntreCartas));
                Canvas.SetTop(carta.carta, margenTop + j * (carta.carta.Height + margenEntreCartas));
                Canvas.SetLeft(carta.cartaAtras, margenLeft + k * (carta.carta.Height + margenEntreCartas));
                Canvas.SetTop(carta.cartaAtras, margenTop + j * (carta.carta.Height + margenEntreCartas));
                carta.abierta = true;
                myCanvas.Children.Add(carta.carta);
            }
            timerTexto.Start();
            OnTextTimer();
            timer.Start();


        }


        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int terceraCarta = -1, nCard1 = -1, nCard2 = -1;
            foreach (Card carta in baraja)
            {
                if (carta.carta == (Image)sender)
                {
                    myCanvas.Children.Remove(carta.carta);
                    myCanvas.Children.Add((Image)carta.cartaAtras);
                    carta.abierta = false;
                    cartasAbiertas--;
                }
                else if (carta.cartaAtras == (Image)sender)
                {
                    myCanvas.Children.Remove(carta.cartaAtras);
                    myCanvas.Children.Add(carta.carta);
                    carta.abierta = true;
                    cartasAbiertas++;
                    if (cartasAbiertas == 3)
                        terceraCarta = baraja.IndexOf(carta);
                }
            }

            if (cartasAbiertas == 3)
            {
                for (int i = 0; i < baraja.Count; i++)
                {
                    Card c = baraja.ElementAt(i);
                    if (c.abierta == true)
                    {

                        if (nCard1 != -1 && nCard2 == -1 && i != terceraCarta) nCard2 = i;
                        if (nCard1 == -1 && nCard2 == -1 && i != terceraCarta) nCard1 = i;
                    }
                }
                Card card1 = baraja.ElementAt(nCard1);
                Card card2 = baraja.ElementAt(nCard2);
                myCanvas.Children.Remove(card1.carta);
                myCanvas.Children.Add(card1.cartaAtras);
                myCanvas.Children.Remove(card2.carta);
                myCanvas.Children.Add(card2.cartaAtras);
                baraja.ElementAt(nCard1).abierta = false;
                baraja.ElementAt(nCard2).abierta = false;
                cartasAbiertas -= 2;
            }

            if (cartasAbiertas == 2)
            {
                ComprobarPareja();
            }

            FinJuego();

        }




        private void ComprobarPareja()
        {
            int nCard1 = -1, nCard2 = -1;
            foreach (Card carta in baraja)
            {
                if (carta.abierta == true)
                {
                    if (nCard1 != -1 && nCard2 == -1) nCard2 = baraja.IndexOf(carta);
                    if (nCard1 == -1 && nCard2 == -1) nCard1 = baraja.IndexOf(carta);
                }
            }
            Card card1 = baraja.ElementAt(nCard1);
            Card card2 = baraja.ElementAt(nCard2);
            if (card1.nombreCarta.CompareTo(card2.nombreCarta) == 0)
            {
                myCanvas.Children.Remove(card1.carta);
                myCanvas.Children.Remove(card2.carta);
                baraja.Remove(card1);
                baraja.Remove(card2);
                cartasAbiertas -= 2;

            }

        }


        public void FinJuego()
        {
            if (baraja.Count == 0)
            {
                string msg = "¡Ya no quedan mas cartas! Has ganado! ¿Quieres jugar de nuevo?";
                string titulo = "¡Has ganado!";
                MessageBoxButton botones = MessageBoxButton.YesNo;
                MessageBoxImage icono = MessageBoxImage.Question;
                MessageBoxResult resultado;
                resultado = MessageBox.Show(msg, titulo, botones, icono);
                switch (resultado)
                {
                    case MessageBoxResult.Yes:
                        IniciarJuego();
                        break;
                    case MessageBoxResult.No:
                        myCanvas.Children.Clear();
                        myCanvas.Children.Add(logo);
                        replay.IsEnabled = false;
                        play.IsEnabled = true;
                        break;

                }
            }else if(baraja.Count == 1 || baraja.Count == 2 && baraja.ElementAt(0).nombreCarta.CompareTo(baraja.ElementAt(1).nombreCarta) != 0)
            {
                if(baraja.ElementAt(0).abierta == false)
                {
                    myCanvas.Children.Remove(baraja.ElementAt(0).cartaAtras);
                    myCanvas.Children.Add(baraja.ElementAt(0).carta);
                }
                if (baraja.Count == 2)
                {
                    if (baraja.ElementAt(1).abierta == false)
                    {
                        myCanvas.Children.Remove(baraja.ElementAt(1).cartaAtras);
                        myCanvas.Children.Add(baraja.ElementAt(1).carta);
                    }
                }
                string msg = "¡Has perdido! ¿Quieres jugar de nuevo?";
                string titulo = "GAME OVER";
                MessageBoxButton botones = MessageBoxButton.YesNo;
                MessageBoxImage icono = MessageBoxImage.Question;
                MessageBoxResult resultado;
                resultado = MessageBox.Show(msg, titulo, botones, icono);
                switch (resultado)
                {
                    case MessageBoxResult.Yes:
                        IniciarJuego();
                        break;
                    case MessageBoxResult.No:
                        myCanvas.Children.Clear();
                        myCanvas.Children.Add(logo);
                        baraja.Clear();
                        replay.IsEnabled = false;
                        play.IsEnabled = true;
                        break;

                }

            }
        }

        private void OnTick_TimerCartas(object sender, EventArgs e)
        {

            foreach (Card c in baraja)
            {
                myCanvas.Children.Remove(c.carta);
                myCanvas.Children.Add(c.cartaAtras);
                c.abierta = false;
            }
            timer.Stop();
            if (preferencias != null)
            {
                preferencias.sliderTiempo.IsEnabled = true;
                preferencias.lista.IsEnabled = true;
            }
            replay.IsEnabled = true;
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            baraja.Clear();
            myCanvas.Children.Clear();
            Play_Click(sender, e);
        }

        private void OnTick_Timer(object sender, EventArgs e)
        {
            OnTextTimer();
        }

        private void Configuracion_Click(object sender, RoutedEventArgs e)
        {
            preferencias = new Preferencias(this, baraja);
            preferencias.NumCartas = numeroCartas;
            preferencias.CambioNum += CambioNumCartas;
            preferencias.CambioTemp += CambioTiempo;
            preferencias.EliminarCard += EliminarCarta;
            preferencias.Show();
        }

        private void CambioNumCartas(object sender, CambioNumCartasArgs e)
        {
            numeroCartas = e.NumCartas;
        }

        private void CambioTiempo(object sender, CambioTiempoArgs e)
        {
            tiempoVueltasCartas = e.Tiempo;
        }

        private void EliminarCarta(object sender, EliminarCartaArgs e)
        {
            Card c = e.carta;
            if (c.abierta == true)
            {
                myCanvas.Children.Remove(c.carta);
                if(tiempoTexto == 0) cartasAbiertas--;
            }else
                myCanvas.Children.Remove(c.cartaAtras);
            baraja.Remove(c);
            FinJuego();
        }

        private void Sound_Click(object sender, RoutedEventArgs e)
        {
            if (reproduciendo == true)
            {
                mediaPlayer.Pause();
                reproduciendo = false;
                Image icon1 = new Image();
                BitmapImage BIicon1 = new BitmapImage();
                BIicon1.BeginInit();
                BIicon1.UriSource = new Uri("/imagenes/silence.png", UriKind.Relative);
                BIicon1.EndInit();
                icon1.Stretch = Stretch.Uniform;
                icon1.Source = BIicon1;
                sound.Content = icon1;
            }
            else
            {
                mediaPlayer.Play();
                reproduciendo = true;
                Image icon1 = new Image();
                BitmapImage BIicon1 = new BitmapImage();
                BIicon1.BeginInit();
                BIicon1.UriSource = new Uri("/imagenes/music.png", UriKind.Relative);
                BIicon1.EndInit();
                icon1.Stretch = Stretch.Uniform;
                icon1.Source = BIicon1;
                sound.Content = icon1;
            }
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        private void Informacion_Click(object sender, RoutedEventArgs e)
        {
            string titulo = "Acerca de Gwent: Memory Card Game";
            string msg = "Hecho por Adrian Antonio Gonzalez Pazos.\n" +
                "Para la asignatura 'Interfaces Graficas de Usuario'.\n" +
                "Gwent y The Witcher son marcas registradas de CD PROJEKT RED, Netflix y Andrzej Sapkowski" +
                ", y su uso ha sido con fines educativos.";
            MessageBoxButton botones = MessageBoxButton.OK;
            MessageBoxImage icono = MessageBoxImage.Information;
            MessageBox.Show(msg, titulo, botones, icono);
        }

        private void OnTextTimer()
        {
            etiq.Background = Brushes.Gold;
            etiq.Content = tiempoTexto;
            if (tiempoTexto == 0)
            {
                etiq.Content = "";
                etiq.Background = Brushes.Transparent;
                timerTexto.Stop();
                foreach (Card carta in baraja)
                {
                    carta.carta.MouseDown += Card_MouseDown;
                    carta.cartaAtras.MouseDown += Card_MouseDown;
                }

            }

            tiempoTexto--;

        }
    }
}

