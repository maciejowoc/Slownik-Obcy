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
    /// Interaction logic for Glowna.xaml
    /// </summary>
    public partial class Glowna : Page
    {
        public Glowna()
        {
            InitializeComponent();
        }

        private void Wyszukiwanie_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Tlumaczenie.xaml", UriKind.Relative));
        }

        private void Polsko_Obcy_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("PolskoObcy.xaml", UriKind.Relative));
        }

        private void Wyjscie_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(Wyszukiwanie).Close();
        }
    }
}
