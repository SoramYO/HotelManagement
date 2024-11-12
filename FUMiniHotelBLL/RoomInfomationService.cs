using FUMiniHotelDAL;
using FUMiniHotelDAL.Models;

namespace FUMiniHotelBLL
{
    public class RoomInfomationService
    {
        private Repository<RoomInformation> _roomInformationRepository = new();
        private RoomInformationRepository _roomRepository = new();
        public IEnumerable<RoomInformation> GetAllRoomInformation()
        {
            return _roomRepository.GetAll();
        }

        public List<RoomInformation> GetAvailableRooms(int roomTypeId, DateOnly checkInDate, DateOnly checkOutDate)
        {
            return _roomInformationRepository.GetAll().Where(r => r.RoomTypeId == roomTypeId && r.RoomStatus == 1).ToList();
        }

        public void AddRoom(RoomInformation room)
        {
            _roomInformationRepository.Add(room);
        }
        public RoomInformation GetRoomById(int roomId)
        {
            return _roomInformationRepository.GetById(roomId);
        }
        public void EditRoom(RoomInformation room)
        {
            var roomInDb = _roomInformationRepository.GetById(room.RoomId);
            roomInDb.RoomNumber = room.RoomNumber;
            roomInDb.RoomDetailDescription = room.RoomDetailDescription;
            roomInDb.RoomMaxCapacity = room.RoomMaxCapacity;
            roomInDb.RoomTypeId = room.RoomTypeId;
            roomInDb.RoomPricePerDay = room.RoomPricePerDay;
            roomInDb.RoomStatus = room.RoomStatus;
            _roomRepository.Update(roomInDb);
        }
        public void RemoveRoom(RoomInformation room)
        {
            _roomInformationRepository.Delete(room);
        }
    }
}
