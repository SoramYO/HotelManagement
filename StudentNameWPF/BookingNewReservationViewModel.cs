using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;



namespace StudentNameWPF
{
    public class BookingNewReservationViewModel : ViewModelBase
    {
        private readonly BookingReservationService _bookingReservationService;
        private readonly RoomInfomationService _roomInfomationService;
        private readonly BookingService _bookingService;
        private readonly RoomTypeService _roomTypeService;
        private DateOnly _checkInDate = DateOnly.FromDateTime(DateTime.Today);
        private DateOnly _checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        private RoomType _selectedRoomType;
        private int _roomCount;
        private decimal _totalPrice;
        private RoomInformation _selectedRoom;
        private readonly Customer _customer;

        public DateOnly CheckInDate
        {
            get { return _checkInDate; }
            set
            {
                _checkInDate = value;
                OnPropertyChanged(nameof(CheckInDate));
                CalculateTotalPrice();
            }
        }

        public DateOnly CheckOutDate
        {
            get { return _checkOutDate; }
            set
            {
                _checkOutDate = value;
                OnPropertyChanged(nameof(CheckOutDate));
                CalculateTotalPrice();
            }
        }

        public RoomType SelectedRoomType
        {
            get { return _selectedRoomType; }
            set
            {
                _selectedRoomType = value;
                OnPropertyChanged(nameof(SelectedRoomType));
                LoadAvailableRooms();
                CalculateTotalPrice();
            }
        }

        public int RoomCount
        {
            get { return _roomCount; }
            set
            {
                _roomCount = value;
                OnPropertyChanged(nameof(RoomCount));
                CalculateTotalPrice();
            }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            private set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public ObservableCollection<RoomType> RoomTypes { get; private set; }
        public ObservableCollection<RoomInformation> AvailableRooms { get; private set; } = new();
        public RoomInformation SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
            }
        }

        public ObservableCollection<BookingItem> SelectedRooms { get; private set; }

        public ICommand SelectRoomTypeCommand { get; private set; }
        public ICommand AddToBookingCommand { get; private set; }
        public ICommand RemoveFromBookingCommand { get; private set; }
        public ICommand ProceedToCheckoutCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public BookingNewReservationViewModel(Customer customer)
        {
            _customer = customer;
            _bookingReservationService = new();
            _roomInfomationService = new();
            _roomTypeService = new();
            _bookingService = new();
            // Initialize the collections
            RoomTypes = new ObservableCollection<RoomType>(_roomTypeService.GetAllRoomTypes());
            AvailableRooms = new ObservableCollection<RoomInformation>();
            SelectedRooms = new ObservableCollection<BookingItem>();

            // Initialize commands
            SelectRoomTypeCommand = new RelayCommand(SelectRoomType);
            AddToBookingCommand = new RelayCommand(param => AddToBooking());
            RemoveFromBookingCommand = new RelayCommand(param => RemoveFromBooking());
            ProceedToCheckoutCommand = new RelayCommand(param => ProceedToCheckout());
            CancelCommand = new RelayCommand(param => Cancel());
        }

        private void SelectRoomType(object parameter)
        {
            if (parameter is RoomType roomType)
            {
                SelectedRoomType = roomType;
                LoadAvailableRooms();
                CalculateTotalPrice();
            }
        }


        private void AddToBooking()
        {
            if (SelectedRoom == null)
            {
                MessageBox.Show("Please select a room first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SelectedRooms.Any(b => b.Room == SelectedRoom))
            {
                MessageBox.Show("This room is already in your booking list", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if dates are selected
            if (CheckInDate == default || CheckOutDate == default)
            {
                MessageBox.Show("Please select both check-in and check-out dates", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var today = DateOnly.FromDateTime(DateTime.Now);

            // Check if room is already added
            if (SelectedRooms.Any(b => b.Room == SelectedRoom))
            {
                MessageBox.Show("This room is already in your booking list", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Date validation checks
            if (CheckInDate < today)
            {
                MessageBox.Show("Check-in date cannot be in the past", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckOutDate <= CheckInDate)
            {
                MessageBox.Show("Check-out date must be after check-in date", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Optional: Add maximum stay duration check (e.g., 30 days)
            int stayDuration = CheckOutDate.DayNumber - CheckInDate.DayNumber;
            if (stayDuration > 30)
            {
                MessageBox.Show("Maximum stay duration is 30 days", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }




            // If all checks pass, add the room to booking
            SelectedRooms.Add(new BookingItem
            {
                Room = SelectedRoom,
                RoomType = SelectedRoomType,
                CheckInDate = CheckInDate.ToDateTime(TimeOnly.MinValue),
                CheckOutDate = CheckOutDate.ToDateTime(TimeOnly.MinValue)
            });

            CalculateTotalPrice();
        }

        private void RemoveFromBooking()
        {
            if (SelectedRoom != null && SelectedRooms.Any(b => b.Room == SelectedRoom))
            {
                // Remove the room from the selected rooms
                var bookingItem = SelectedRooms.FirstOrDefault(b => b.Room == SelectedRoom);
                if (bookingItem != null)
                {
                    SelectedRooms.Remove(bookingItem);
                    CalculateTotalPrice();
                }
            }
        }

        private void ProceedToCheckout()
        {
            try
            {
                if (!ValidateBooking())
                {
                    return;
                }
                if (!SelectedRooms.Any())
                {
                    MessageBox.Show("Please select at least one room before proceeding to checkout.",
                        "No Rooms Selected",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Create confirmation message
                string confirmMessage = $"Confirm booking for:\n";
                foreach (var item in SelectedRooms)
                {
                    confirmMessage += $"\nRoom {item.Room.RoomNumber}" +
                                    $"\nCheck-in: {item.CheckInDate:dd/MM/yyyy}" +
                                    $"\nCheck-out: {item.CheckOutDate:dd/MM/yyyy}\n";
                }
                confirmMessage += $"\nTotal Price: {TotalPrice:C}";

                // Show confirmation dialog
                var result = MessageBox.Show(confirmMessage,
                    "Confirm Booking",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Create booking for each room
                    foreach (var bookingItem in SelectedRooms)
                    {
                        // Create booking reservation
                        var bookingReservation = new BookingReservation
                        {
                            BookingDate = DateOnly.FromDateTime(DateTime.Now),
                            TotalPrice = TotalPrice,
                            CustomerId = _customer.CustomerId,
                            BookingStatus = 1 // Active status
                        };

                        // Create booking detail
                        var bookingDetail = new BookingDetail
                        {
                            RoomId = bookingItem.Room.RoomId,
                            StartDate = DateOnly.FromDateTime(bookingItem.CheckInDate),
                            EndDate = DateOnly.FromDateTime(bookingItem.CheckOutDate),
                            ActualPrice = bookingItem.Room.RoomPricePerDay *
                                         (DateOnly.FromDateTime(bookingItem.CheckOutDate).DayNumber -
                                          DateOnly.FromDateTime(bookingItem.CheckInDate).DayNumber)
                        };

                        // Create the booking using the service
                        _bookingService.CreateBooking(bookingReservation, bookingDetail, TotalPrice);
                    }

                    MessageBox.Show("Booking completed successfully!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    // Clear the selected rooms
                    SelectedRooms.Clear();
                    CalculateTotalPrice();

                    // Close the window if needed
                    var window = Application.Current.Windows
                        .OfType<Window>()
                        .SingleOrDefault(w => w.DataContext == this);
                    window?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during checkout: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private bool ValidateBooking()
        {
            if (!SelectedRooms.Any())
            {
                MessageBox.Show("Please select at least one room before proceeding to checkout.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            foreach (var booking in SelectedRooms)
            {
                if (DateOnly.FromDateTime(booking.CheckInDate) < today)
                {
                    MessageBox.Show("Check-in date cannot be in the past.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return false;
                }

                if (booking.CheckOutDate <= booking.CheckInDate)
                {
                    MessageBox.Show("Check-out date must be after check-in date.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }

        private void Cancel()
        {
            // Logic to cancel the reservation process
            SelectedRooms.Clear();
            CalculateTotalPrice();
        }

        private void LoadAvailableRooms()
        {
            if (SelectedRoomType != null)
            {
                AvailableRooms.Clear();
                var rooms = _roomInfomationService.GetAvailableRooms(
                    SelectedRoomType.RoomTypeId,
                    CheckInDate,
                    CheckOutDate);
                foreach (var room in rooms)
                {
                    AvailableRooms.Add(room);
                }
            }
        }

        private void CalculateTotalPrice()
        {
            if (SelectedRoomType != null && SelectedRooms.Any())
            {
                int nights = CheckOutDate.DayNumber - CheckInDate.DayNumber;
                TotalPrice = SelectedRooms.Sum(r => (decimal?)r.Room.RoomPricePerDay * nights) ?? 0;
            }
            else
            {
                TotalPrice = 0;
            }
        }

        public class BookingItem
        {
            public RoomInformation Room { get; set; }
            public RoomType RoomType { get; set; }

            public DateTime CheckInDate { get; set; }
            public DateTime CheckOutDate { get; set; }
        }
        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;

            public RelayCommand(Action<object> execute)
            {
                _execute = execute;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => _execute(parameter);
        }


    }
}
