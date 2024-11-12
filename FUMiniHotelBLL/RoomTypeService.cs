using FUMiniHotelDAL;
using FUMiniHotelDAL.Models;

namespace FUMiniHotelBLL
{
    public class RoomTypeService
    {
        private Repository<RoomType> _roomTypeRepository = new();

        public List<RoomType> GetAllRoomTypes()
        {
            return _roomTypeRepository.GetAll().ToList();
        }
    }
}
