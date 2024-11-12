using FUMiniHotelDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelDAL
{
    public class RoomInformationRepository
    {
        private FuminiHotelManagementContext _context;

        public List<RoomInformation> GetAll()
        {
            _context = new();
            return _context.RoomInformations.Include(r => r.RoomType).ToList();
        }

        public void Update(RoomInformation roomInformation)
        {
            _context = new();
            _context.RoomInformations.Update(roomInformation);
            _context.SaveChanges();
        }
    }
}
