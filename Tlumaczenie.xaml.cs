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
    /// Interaction logic for Tlumaczenie.xaml
    /// </summary>
    public partial class Tlumaczenie : Page
    {
        public Tlumaczenie()
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
            catch(FileNotFoundException)
            {
                MessageBox.Show("Nie można odnaleźć pliku " + NazwaPliku + "!", "Błąd");
                Resetuj_Click(null, e);
            }
        }
        void SzukanaFraza_TextChanged(object sender, TextChangedEventArgs e)        //Funkcja uruchamia się za każdą zmianą treści pola szukanej frazy
            {
            Plik Znalezione = new Plik();
                BlokWynikow.Text = null;                                            //Wyczyszczenie bloku wyników
                string Poszukiwacz = SzukanaFraza.Text;                             //Poszukiwana fraza
                Znalezione.BazaNazw.Clear();                                        //Profilaktyczne wyczyszczenie bazy potencjalnych haseł
                if (!string.IsNullOrWhiteSpace(SzukanaFraza.Text))                  //Wyświetla wyniki jeśli pole tekstowe nie jest puste
                {
                    foreach (Plik.Dane wyniki in Zaladowany.BazaNazw)
                    {
                        if (wyniki.obcy.Length >= Poszukiwacz.Length)
                        {
                            if (wyniki.obcy.Substring(0, Poszukiwacz.Length).ToUpper().Contains(Poszukiwacz.ToUpper()))
                            {
                                Znalezione.BazaNazw.Add(wyniki);
                            }
                        }

                    }
                    foreach (Plik.Dane znajdzki in Znalezione.BazaNazw)                     //Wyświetlenie wszystkich pasujących haseł
                    {
                        BlokWynikow.Text += (znajdzki.obcy + " - " + znajdzki.polski + "\n");
                    }
                }
            }
        }
    }