using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Windows;
using System.Windows.Controls;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for RoomDetail.xaml
    /// </summary>
    public partial class RoomDetail : Window
    {
        private RoomTypeService _roomTypeService = new();

        private RoomInfomationService _roomInfomationService = new();
        public RoomInformation RoomInformation { get; set; }
        public RoomDetail()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            if (RoomInformation != null)
            {
                RoomNumberTextBox.Text = RoomInformation.RoomNumber;
                RoomDetailTextBox.Text = RoomInformation.RoomDetailDescription;
                RoomMaxCapacityTextBox.Text = RoomInformation.RoomMaxCapacity.ToString();

                // Set the correct selected value for RoomTypeComboBox based on RoomTypeId
                RoomTypeComboBox.SelectedValue = RoomInformation.RoomTypeId;

                RoomPriceTextBox.Text = RoomInformation.RoomPricePerDay.ToString();
                RoomStatusComboBox.SelectedIndex = RoomInformation.RoomStatus.HasValue ? Convert.ToInt32(RoomInformation.RoomStatus.Value) : 0;
            }
        }


        private void LoadData()
        {
            RoomTypeComboBox.ItemsSource = _roomTypeService.GetAllRoomTypes();
            RoomTypeComboBox.SelectedValuePath = "RoomTypeId";
            RoomTypeComboBox.DisplayMemberPath = "RoomTypeName";
            RoomTypeComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateInput(sender, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None));

            if (RoomInformation == null)
            {
                RoomInformation room = new RoomInformation();
                room.RoomNumber = RoomNumberTextBox.Text;
                room.RoomDetailDescription = RoomDetailTextBox.Text;
                room.RoomMaxCapacity = int.Parse(RoomMaxCapacityTextBox.Text);

                // Get the selected RoomTypeId from the ComboBox
                room.RoomTypeId = (int)RoomTypeComboBox.SelectedValue;

                room.RoomPricePerDay = decimal.Parse(RoomPriceTextBox.Text);
                room.RoomStatus = (byte)RoomStatusComboBox.SelectedIndex;
                _roomInfomationService.AddRoom(room);
                MessageBox.Show("Room added successfully");
                Close();

            }
            else
            {
                RoomInformation.RoomNumber = RoomNumberTextBox.Text;
                RoomInformation.RoomDetailDescription = RoomDetailTextBox.Text;
                RoomInformation.RoomMaxCapacity = int.Parse(RoomMaxCapacityTextBox.Text);

                // Update RoomTypeId correctly
                RoomInformation.RoomTypeId = (int)RoomTypeComboBox.SelectedValue;

                RoomInformation.RoomPricePerDay = decimal.Parse(RoomPriceTextBox.Text);
                RoomInformation.RoomStatus = (byte)RoomStatusComboBox.SelectedIndex;
                _roomInfomationService.EditRoom(RoomInformation);
                MessageBox.Show("Room updated successfully");

                Close();

            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ValidateInput(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(RoomNumberTextBox.Text) || string.IsNullOrEmpty(RoomDetailTextBox.Text) || string.IsNullOrEmpty(RoomMaxCapacityTextBox.Text) || string.IsNullOrEmpty(RoomPriceTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }


            if (!int.TryParse(RoomMaxCapacityTextBox.Text, out _))
            {
                MessageBox.Show("Room Max Capacity must be a number");
                RoomMaxCapacityTextBox.Text = "";
            }
            if (!decimal.TryParse(RoomPriceTextBox.Text, out _))
            {
                MessageBox.Show("Room Price must be a number");
                RoomPriceTextBox.Text = "";
            }
        }
    }
}
