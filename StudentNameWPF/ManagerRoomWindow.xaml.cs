using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Windows;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for ManagerRoomWindow.xaml
    /// </summary>
    public partial class ManagerRoomWindow : Window
    {
        private RoomInfomationService _roomInfomationService = new();
        public ManagerRoomWindow()
        {
            InitializeComponent();
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            RoomDetail roomDetail = new();
            roomDetail.ShowDialog();
            LoadData();

        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            var room = RoomDataGrid.SelectedItem as RoomInformation;
            if (room != null)
            {
                RoomDetail roomDetail = new();
                roomDetail.RoomInformation = room;
                roomDetail.ShowDialog();

                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a customer to edit");
            }

        }

        private void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            var room = RoomDataGrid.SelectedItem as RoomInformation;
            if (room != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete this room?", "Delete Customer", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    return;
                }
                _roomInfomationService.RemoveRoom(room);
                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a customer to delete");
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            RoomDataGrid.ItemsSource = null;
            RoomDataGrid.ItemsSource = _roomInfomationService.GetAllRoomInformation();
        }
    }
}
