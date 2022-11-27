using CarService.Data.Local;
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
using System.Windows.Shapes;

namespace CarService.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for ClientVisitInfoWindow.xaml
    /// </summary>
    public partial class ClientVisitInfoWindow : Window
    {
        public ClientVisitInfoWindow(Client inputClient)
        {
            InitializeComponent();

            var itemSoruce = new CarServiceModel().ClientServices
                .Where(clientService => clientService.ClientID == inputClient.ID)
                .ToList();

            listViewClientVisit.ItemsSource = itemSoruce;

            listViewClientVisit.Visibility =
                itemSoruce.Count == 0 ? Visibility.Hidden : Visibility.Visible;
        }
    }
}
