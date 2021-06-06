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
using OOOPolomka.PageClients;

namespace OOOPolomka.PageClients
{
    /// <summary>
    /// Логика взаимодействия для PageClientPhotoList.xaml
    /// </summary>
    public partial class PageClientPhotoList : Page
    {
        public PageClientPhotoList()
        {
            InitializeComponent();
            List<VwClients> Items = new List<VwClients>();
            Items = context.VwClients.ToList();
            LVClientPhotoList.ItemsSource = Items;
        }
        private void BtCancle_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.SelectedFrame.Navigate(new PageListClients());
        }

    }
}
