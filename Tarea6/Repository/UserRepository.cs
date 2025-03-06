using Microsoft.EntityFrameworkCore;
using Tarea6.Contex;
using Tarea6.Models;

namespace Tarea6.Repository
{
    public class UserRepository
    {
        private readonly UserContex _contex;

        public UserRepository(UserContex contex)
        {
            _contex = contex;
        }


        public async Task Addasync(User user)
        {
            await _contex.AddAsync(user);
            await _contex.SaveChangesAsync();

        }

        public async Task Deleteasync(User user)
        {
            _contex.Set<User>().Remove(user);
            await _contex.SaveChangesAsync();

        }
        public async Task Updateasync(User user)
        {
            _contex.Entry(user).State = EntityState.Modified;
            await _contex.SaveChangesAsync();

        }

        public async Task<List<User>> Getallasync()
        {

            return await _contex.Set<User>().ToListAsync();

        }
        public async Task<User?> GetBYId(int id)
        {
            return await _contex.Set<User>().FindAsync(id);
        }

      

    }
}

    

