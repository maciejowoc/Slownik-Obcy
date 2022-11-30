using System;
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
using System.IO;

namespace SlownikObcy
{
    /// <summary>
    /// Interaction logic for PolskoObcy.xaml
    /// </summary>
    public partial class PolskoObcy : Page
    {
        public PolskoObcy()
        {
            InitializeComponent();
            ZaladujDane();
        }

        Plik Zaladowany = new Plik();

        private void Powrot_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Glowna.xaml", UriKind.Relative));
        }

        private void Resetuj_Click(object sender, RoutedEventArgs e)
        {
            SzukanaFraza.Text = "";
            WyborJezyka.SelectedIndex = -1;
            SzukanaFraza.IsEnabled = false;
            BlokWynikow.Text = null;
        }

        private void ZaladujDane()
        {
            WyborJezyka.Items.Add("Angielski");
            WyborJezyka.Items.Add("Niemiecki");
        }

        private void WyborJezyka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string NazwaPliku;
            SzukanaFraza.IsEnabled = true;
            switch (WyborJezyka.SelectedIndex)
            {
                case 0:
                    {
                        NazwaPliku = "Ang.txt";
                        break;
                    }
                case 1:
                    {
                        NazwaPliku = "Niem.txt";
                        break;
                    }
                default:
                    {
                        NazwaPliku = "Ang.txt";
                        break;
                    }
            }
            try
            {
                Plik Temp = new Plik(NazwaPliku);
                Zaladowany = Temp;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Nie można odnaleźć pliku " + NazwaPliku + "!", "Błąd");
                Resetuj_Click(null, e);
            }
        }

        private void SzukanaFraza_TextChanged(object sender, TextChangedEventArgs e)
        {
            Plik Znalezione = new Plik();
            BlokWynikow.Text = null;
            string Poszukiwacz = SzukanaFraza.Text;
            Znalezione.BazaNazw.Clear();
            //Poszukiwanie działa na podobnej zasadzie co w pozostałych przypadkach
            if (!string.IsNullOrWhiteSpace(SzukanaFraza.Text))                  //Wyświetla wyniki jeśli pole tekstowe nie jest puste
            {
                foreach (Plik.Dane wyniki in Zaladowany.BazaNazw)
                {
                    if (wyniki.polski.Length >= Poszukiwacz.Length)
                    {
                        if (wyniki.polski.Substring(0, Poszukiwacz.Length).ToUpper().Contains(Poszukiwacz.ToUpper()))
                        {
                            Znalezione.BazaNazw.Add(wyniki);
                        }
                    }

                }
                foreach (Plik.Dane znajdzki in Znalezione.BazaNazw)
                {
                    BlokWynikow.Text += (znajdzki.polski + " - " + znajdzki.obcy + "\n");
                }
            }
        }
    }
}
