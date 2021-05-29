using NoteStorage.Logics.ModelsDto;
using System.Threading.Tasks;

namespace NoteStorage.Logics.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AuthenticateAsync(string login, string password);
    }
}
