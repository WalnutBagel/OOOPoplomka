using Microsoft.Win32;
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
    /// Логика взаимодействия для PageEditClients.xaml
    /// </summary>
    public partial class PageEditClients : Page
    {

        bool Edit = false;
        string imagePath = "";
        Client originClient;


        public PageEditClients()
        {
            InitializeComponent();
            if (idClient != 0)
            {
                Edit = true;
            }
            else Edit = false;

            if (Edit == false)
            {

            }
            else
            {
                originClient = context.Client.Where(i => i.ID.Equals(idClient)).FirstOrDefault();
                Client client = originClient;
                if (client != null)
                {
                    TbID.Text = client.ID.ToString();
                    TbFirstName.Text = client.FirstName.ToString();
                    TbLastName.Text = client.LastName.ToString();
                    TbMiddleName.Text = client.Patronymic.ToString();
                    TbEmail.Text = client.Email.ToString();
                    TbPhone.Text = client.Phone.ToString();
                    DpDateBirth.SelectedDate = client.Birthday;
                    Image ClientPhotobuff = new Image();
                    ClientPhoto.Source = new BitmapImage(new Uri(client.PhotoPath, UriKind.Relative));
                    switch (client.GenderCode)
                    {
                        case "м":
                            {
                                CbGender.SelectedIndex = 0;
                                break;
                            }
                        case "ж":
                            {
                                CbGender.SelectedIndex = 1;
                                break;
                            }
                        default:
                            {
                                MessageBox.Show("Ошибка базы данных!");
                                break;
                            }
                    }
                    //SetPhoto(client.PhotoPath);
                    //var clientTags = AppConnect.modelOdb.ClientTag.Where(a => a.IdClient == client.IdClient).ToList();

                }
                else MessageBox.Show("The selected client did not have any visits", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtCancle_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.SelectedFrame.Navigate(new PageListClients());
        }

        private void BtEditClient_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.SelectedFrame.Navigate(new PageListClients());
        }

        private void EditPhoto_Click(object sender, RoutedEventArgs e)
        {
            var Picturedialog = new OpenFileDialog();
            Picturedialog.Filter = "(*.bmp, *.jpg)|*.bmp;*.jpg|Все файлы (*.*)|*.*";
            if (Picturedialog.ShowDialog() == true)
            {
                imagePath = Picturedialog.FileName;
            }

            if (imagePath != null)
            {
                Uri pathImage = new Uri(imagePath);
                BitmapImage image = new BitmapImage(pathImage);
                ClientPhoto.Source = image;
            }
        }
    }
}
