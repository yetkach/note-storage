using Microsoft.EntityFrameworkCore;
using NoteStorage.Data.EF;
using NoteStorage.Data.Entities;
using NoteStorage.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteStorage.Data.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext db;

        public NoteRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(Note note)
        {
            await db.Notes.AddAsync(note);
        }

        public void Delete(Note note)
        {
            db.Notes.Remove(note);
        }

        public async Task<Note> GetByIdAsync(int id, int userId)
        {
            return await db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        }

        public async Task<List<Note>> GetAllAsync(int userId)
        {
            return await db.Notes.Where(n => n.UserId == userId).ToListAsync();
        }

        public void Update(Note note)
        {
            db.Entry(note).State = EntityState.Modified;
        }
    }
}
