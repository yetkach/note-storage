using NoteStorage.Data.Entities;
using System.Threading.Tasks;

namespace NoteStorage.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string login, string password);
    }
}
