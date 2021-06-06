using OOOPolomka.SystemAppClass;
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
using static OOOPolomka.ApplicationData.AppConnect;

namespace OOOPolomka.PageClients
{
    /// <summary>
    /// Логика взаимодействия для PageListVisit.xaml
    /// </summary>
    public partial class PageListVisit : Page
    {
        public PageListVisit()
        {
            InitializeComponent();
            LVClientVisit.ItemsSource = context.ViewClientService.Where(i => i.ClientID.Equals(idClient)).ToList();
        }

        private void BtCancle_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.SelectedFrame.Navigate(new PageListClients());
        }
    }
}
