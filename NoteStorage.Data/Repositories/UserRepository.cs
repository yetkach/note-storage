using Microsoft.EntityFrameworkCore;
using NoteStorage.Data.EF;
using NoteStorage.Data.Entities;
using NoteStorage.Data.Interfaces;
using System.Threading.Tasks;

namespace NoteStorage.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext db;

        public UserRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<User> GetAsync(string login, string password)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
        }
    }
}
