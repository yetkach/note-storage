using NoteStorage.Logics.ModelsDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteStorage.Logics.Interfaces
{
    public interface INoteService
    {
        Task<List<NoteDto>> GetAllAsync(int userId);
        Task<NoteDto> CreateAsync(NoteDto note, int userId);
        Task<NoteDto> UpdateAsync(int noteId, NoteDto note, int userId);
        Task DeleteAsync(int id, int useId);
    }
}
