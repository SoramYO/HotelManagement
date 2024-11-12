using FUMiniHotelDAL.Models;

namespace FUMiniHotelDAL
{
    public class Repository<T> where T : class
    {
        private readonly FuminiHotelManagementContext _context;

        public Repository()
        {
            _context = new FuminiHotelManagementContext();
        }

        // Add a new entity
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        // Delete an entity
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        // Update an entity
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        // Get all entities
        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        // Get a single entity by ID
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
    }
}
