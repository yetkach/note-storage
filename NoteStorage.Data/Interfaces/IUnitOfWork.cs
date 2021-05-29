using System.Threading.Tasks;

namespace NoteStorage.Data.Interfaces
{
    public interface IUnitOfWork
    {
        INoteRepository Notes { get; }
        IUserRepository Users { get; }
        Task CompleteAsync();
    }
}
