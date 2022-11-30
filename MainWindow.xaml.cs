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

namespace SlownikObcy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)     //Przy wyjściu z programu wyświetli okno z zapytaniem o wyjście z programu
        {
            MessageBoxResult messageBox =  MessageBox.Show("Czy na pewno chcesz wyjść z programu?", "Wyjście", MessageBoxButton.YesNo);          
            if(messageBox == MessageBoxResult.No)
            {
                e.Cancel = true;                        //Jeśli wciśnięty zostanie przycisk "Nie", program porzuci akcję zamykania programu
            }
        }

        //========================PASEK NARZĘDZI===================================

        private void WyjsciePasekNarzedzi_Click(object sender, RoutedEventArgs e)       //Wywołanie opeacji zamykania programu z poziomu paska narzędzi
        {
            this.Close();
        }

        private void InformacjeOProgramie_Click(object sender, RoutedEventArgs e)       //Wyświetlenie informacji o wersji
        {
            MessageBox.Show("Słownik języków obcych\nWersja 0.8\n\nMaciej Owoc\nNr indeksu: 100004");
        }

        private void Edytor_Click(object sender, RoutedEventArgs e)                     //Przejście na stronę edytowania haseł - EdytorHasel
        {
            MainFrame.Navigate(new Uri("EdytorHasel.xaml", UriKind.Relative));
        }
    }
}
