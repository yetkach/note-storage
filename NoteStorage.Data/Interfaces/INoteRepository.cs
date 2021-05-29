using NoteStorage.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteStorage.Data.Interfaces
{
    public interface INoteRepository
    {
        Task<List<Note>> GetAllAsync(int id);
        Task<Note> GetByIdAsync(int id, int userId);
        Task AddAsync(Note note);
        void Update(Note note);
        void Delete(Note note);
    }
}
