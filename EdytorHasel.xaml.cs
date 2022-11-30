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
    /// Interaction logic for EdytorHasel.xaml
    /// </summary>
    public partial class EdytorHasel : Page
    {
        public EdytorHasel()
        {
            InitializeComponent();
            ZaladujDane();
        }
        private void ZaladujDane()
        {
            WyborJezyka.Items.Add("Angielski");
            WyborJezyka.Items.Add("Niemiecki");
        }
        Plik Zaladowany = new Plik();                       //Przechowuje wszystkie załadowane z pliku hasła
        string NazwaPliku;                                  //Nazwa pliku, nad którym będziemy pracowali
        private void Powrot_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Glowna.xaml", UriKind.Relative));
        }

        private void WyborJezyka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HasloInput.IsEnabled = true;
            TlumaczenieWynik.IsEnabled = true;                      //Odblokowanie pól do wprowadzania danych po wybraniu języka
            switch (WyborJezyka.SelectedIndex)                      //Wybór nazwy pliku na podstawie wybranego indeksu elementu ComboBox
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
                Plik Temp = new Plik(NazwaPliku);       //Obsługa wyjątku: próba załadowania pliku o nazwie podanej z pola ComboBox       
                Zaladowany = Temp;                      //Załadowanie danych do Bazy Zaladowany
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Nie można odnaleźć pliku " + NazwaPliku + "!", "Błąd");       //W przypadku nieodnalezienia danego pliku wyświetli komunikat
                Resetuj_Click(null, e);                         //"Sztuczne" Wywołanie funkcji Resetuj_Click;
            }
        }

        private void Resetuj_Click(object sender, RoutedEventArgs e)        //Funkcja przywraca wszystkie elementy strony do ustawień początkowych
        {
            WyborJezyka.SelectedIndex = -1;
            HasloInput.Text = "";
            HasloInput.IsEnabled = false;
            TlumaczenieWynik.Text = "";
            TlumaczenieWynik.IsEnabled = false;
            WynikWyszukiwania.Text = "-- Brak wyników --";
        }

        private void HasloInput_TextChanged(object sender, TextChangedEventArgs e)      //Funkcja uruchamia się za każdą zmianą treści pola szukanej frazy
        {
            Plik Znalezione = new Plik();
            WynikWyszukiwania.Text = null;                                    //Wyczyszczenie bloku wyników
            string Poszukiwacz = HasloInput.Text;                             //Poszukiwana fraza
            Znalezione.BazaNazw.Clear();                                      //Profilaktyczne wyczyszczenie bazy potencjalnych haseł
            if (!string.IsNullOrWhiteSpace(HasloInput.Text))                  //Wyświetla wyniki jeśli pole tekstowe nie jest puste
            {
                foreach (Plik.Dane wyniki in Zaladowany.BazaNazw)             //Dla każdego elementu bazy Zaladowany nastąpi porównywanie haseł do zawartości pola TextBox
                {
                    if (wyniki.obcy.Length >= Poszukiwacz.Length)             //Jeżeli szukana fraza jest krótsza od hasła w bazie (dynamiczne wyszukiwanie)
                    {
                        if (wyniki.obcy.Substring(0, Poszukiwacz.Length).ToUpper().Contains(Poszukiwacz.ToUpper()))        //Porównanie nie uwzględnia wielkości znaków; rozpoczyna się od początku wyrazu
                        {
                            Znalezione.BazaNazw.Add(wyniki);                  //Jeżeli takie hasło istnieje to zostaje dodane do bazy nazw znalezionych haseł
                        }
                    }
                }
                if(Znalezione.BazaNazw.Count>0)
                {
                    WynikWyszukiwania.Text += (Znalezione.BazaNazw[0].obcy + " - " + Znalezione.BazaNazw[0].polski);
                }
            }
            else
            {
                WynikWyszukiwania.Text = "-- Brak wyników --";      //Jeżeli nic nie znajdzie to wyświetli wiadomość -- Brak wyników --
            }
        }

        private void UsunHaslo_Click(object sender, RoutedEventArgs e)      //Usuwanie wybranego hasła
        {
            //Wykorzystano tutaj pole wyświetlające wynik wyszukiwania, jeśli istnieje dane hasło to można je usunąć, jeśli nie istnieje to wyświetlany jest komunikat:
            if (WynikWyszukiwania.Text == "")
            {
                MessageBox.Show("Nie można odnaleźć szukanej frazy:\n" + HasloInput.Text, "Brak szukanej frazy!");
                Resetuj_Click(null, e);
            }
            else
            {
                //Poszukiwanie działa na tej samej zasadzie co wcześniej
                Plik Znalezione = new Plik();
                string Poszukiwacz = HasloInput.Text;
                Znalezione.BazaNazw.Clear();
                foreach (Plik.Dane wynik in Zaladowany.BazaNazw)
                {
                    if (wynik.obcy.ToUpper().Contains(Poszukiwacz.ToUpper()))
                    {
                        Zaladowany.BazaNazw.Remove(wynik);
                        MessageBox.Show("Usunięto hasło:\n" + wynik.obcy + " - " + wynik.polski, "Sukces");
                        break;
                    }
                }
                Zaladowany.Zapisz(NazwaPliku);
            }
        }

        private void DodajHaslo_Click(object sender, RoutedEventArgs e)     //Dodanie nowego hasła słownikowego
        {
            if (HasloInput.Text != "" && TlumaczenieWynik.Text != "")       //Muszą być oba pola wypełnione: fraza oraz tłumaczenie
            {
                string PoszukiwaczObcy = HasloInput.Text;
                string PoszukiwaczPolski = TlumaczenieWynik.Text;
                Plik.Dane FrazaWynikowa;

                foreach (Plik.Dane wynik in Zaladowany.BazaNazw)    //Sprawdzanie czy już instnieje dane hasło
                {
                    if (wynik.obcy.ToUpper().Contains(PoszukiwaczObcy.ToUpper()) && wynik.polski.ToUpper().Contains(PoszukiwaczPolski.ToUpper()))
                    {
                        MessageBox.Show("Podane hasło:\n" + wynik.obcy + " - " + wynik.polski + "\njuż istnieje!", "Ostrzeżenie");
                        TlumaczenieWynik.Text = "";
                        HasloInput.Text = "";
                        return;
                    }
                }
                FrazaWynikowa.obcy = HasloInput.Text;
                FrazaWynikowa.polski = TlumaczenieWynik.Text;
                Zaladowany.BazaNazw.Add(FrazaWynikowa);
                Zaladowany.Zapisz(NazwaPliku);
                MessageBox.Show("Dodano hasło:\n" + FrazaWynikowa.obcy + " - " + FrazaWynikowa.polski, "Sukces");
            }
            else        
            {
                MessageBox.Show("Wypełnij wszystkie wymagane pola!" , "Błąd");
                TlumaczenieWynik.Text = "";
                HasloInput.Text = "";
            }
        }
    }
}
