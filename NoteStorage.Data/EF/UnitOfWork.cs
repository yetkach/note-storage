using NoteStorage.Data.Interfaces;
using System.Threading.Tasks;

namespace NoteStorage.Data.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;
        public INoteRepository Notes { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(ApplicationDbContext db, IUserRepository usersRepository,
            INoteRepository notesRepository)
        {
            Notes = notesRepository;
            Users = usersRepository;
            this.db = db;
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
