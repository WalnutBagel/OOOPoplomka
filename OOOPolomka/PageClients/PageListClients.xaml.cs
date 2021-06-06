using OOOPolomka.ApplicationData;
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
using System.Windows.Threading;


namespace OOOPolomka.PageClients
{
    /// <summary>
    /// Логика взаимодействия для PageListClients.xaml
    /// </summary>
    public partial class PageListClients : Page
    {
        public List<VwClients> ClientList = new List<VwClients>(); // создаем список, в который будем выгружать наше представление

        public PageListClients()
        {
            InitializeComponent();
            ClientList = context.VwClients.ToList();
            UpdateTable(false);
            List<Gender> genders = context.Gender.ToList();
            //заполняем listGenderBox гендерами
            //оставляю это так только потому, что таким образом можно закидывать, например, роли
            //а так бы я просто заполнил значения в XAML
            genders.Insert(0, new Gender() { Name = "Все" }); // добавляем в список гендеров пункт "Все"
            ListGenderBox.ItemsSource = genders; //поместим в ListGenderBox список гендеров
            ListGenderBox.DisplayMemberPath = "Name"; // показвает какой столбец таблицы гендеров мы выводим
            ListGenderBox.SelectedIndex = 0;
            ListSortBox.SelectedIndex = 3;
        }
        public void UpdateTable(bool Filter)
        {
            if (Filter)
            {
                lvClients.ItemsSource = ClientList; // если были использованы фильтры - выводим отфильтрованный список
            }
            else
            {
                lvClients.ItemsSource = context.VwClients.ToList(); // иначе - загружаем новый
            }
        }

        public void MakeFilters()
        {
            // поиск по полям
            var list = context.VwClients.Where(i => i.FLP.Contains(TbSearchFIO.Text)) // загружаем во временный список представление
                                        .Where(i => i.Email.Contains(TbSearchEmail.Text)) // в котором мы проверяем то, что написанно
                                        .Where(i => i.Phone.Contains(TbSearchPhone.Text)) // в одном из 3 поисков
                                        .ToList();

            // сортировка по полу
            if (ListGenderBox != null) // если был изменен ListGenderBox
            {
                switch (ListGenderBox.SelectedIndex)
                {
                    case 1:
                        list = list.Where(x => x.Gender.Equals("ж")).ToList();
                        break;
                    case 2:
                        list = list.Where(x => x.Gender.Equals("м")).ToList();
                        break;
                }
            }

            // выборка людей с днем рождения в этом месяце
            if (CheckBirthDate != null)
            {
                if (CheckBirthDate.IsChecked == true)
                { // тут бывает беда с Month - почему собственно сделали так
                    list = list.Where(i => i.Birthday.HasValue && i.Birthday.Value.Month.Equals(DateTime.Now.Month)).ToList();
                }
            }

            // сортировка по ВСЕМУ ОСТАЛЬНОМУ
            if (ListSortBox != null)
            {
                switch (ListSortBox.SelectedIndex)
                {
                    case 0:
                        list = list.OrderBy(i => i.FLP).ToList();
                        break;
                    case 1:
                        list = list.OrderByDescending(i => i.Visit_count).ToList();
                        break;
                    case 2:
                        list = list.OrderByDescending(i => i.Date_of_last_visit).ToList();
                        break;
                }
            }


            ClientList = list; // в список с которого проиходит выгрузка в таблицу - забиваем новый измененный
            UpdateTable(true); // обновляем таблицу
        }

        private void SearchClearBtn_Click(object sender, RoutedEventArgs e)
        {
            TbSearchFIO.Clear(); // чистим поля с поиском
            TbSearchEmail.Clear();
            TbSearchPhone.Clear();
            ListGenderBox.SelectedIndex = 0; // чистим гендер
            CheckBirthDate.IsChecked = false; // убираем чек с дня рождения
            ListSortBox.SelectedIndex = 3; // убираем сортировку
            UpdateTable(false); // обновляем таблицу с False - без фильтрации
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (lvClients.SelectedItem is VwClients client)
            {
                var result = MessageBox.Show("Вы действительно хотите удалить пользователя?", "Удаление клиента",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (context.ClientService.Where(i => i.ClientID == client.ID).FirstOrDefault() != null)
                    {
                        MessageBox.Show("У клиента есть записи, удаление запрещено", "Уведомление", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        context.Client.Remove(context.Client.Where(i => i.ID.Equals(client.ID)).FirstOrDefault());
                        context.SaveChanges();
                        MessageBox.Show("Запись удалена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        MakeFilters();
                    }
                }
            }
            else 
            {
                MessageBox.Show("Выберите запись из списка", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            };
        }

        private void ListGenderBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MakeFilters();
        }

        private void TbSearchFIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            MakeFilters();
        }

        private void TbSearchEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            MakeFilters();
        }

        private void TbSearchPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            MakeFilters();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.SelectedFrame.Navigate(new PageEditClients());
        }

        private void ClientVisitList_Click(object sender, RoutedEventArgs e)
        {
            if (lvClients.SelectedItem is VwClients clients)
            {
                if (clients.Visit_count != 0)
                {
                    idClient = clients.ID;
                    AppFrame.SelectedFrame.Navigate(new PageListVisit());
                }
                else MessageBox.Show("The selected client did not have any visits", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Choose client in a list", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (lvClients.SelectedItem is VwClients clients)
            {
                idClient = clients.ID;
                AppFrame.SelectedFrame.Navigate(new PageEditClients());
            }
            else
            {
                MessageBox.Show("Choose client in a list", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CheckBirthDate_Checked(object sender, RoutedEventArgs e)
        {
            MakeFilters();
        }

        private void CheckBirthDate_Unchecked(object sender, RoutedEventArgs e)
        {
            MakeFilters();
        }

        private void ListSortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MakeFilters();
        }

        private void ClientPhotoList_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.SelectedFrame.Navigate(new PageClientPhotoList());
        }
    }
}
