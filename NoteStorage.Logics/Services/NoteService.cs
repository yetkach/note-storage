using AutoMapper;
using NoteStorage.Data.Entities;
using NoteStorage.Data.Interfaces;
using NoteStorage.Logics.Interfaces;
using NoteStorage.Logics.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteStorage.Logics.Services
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public NoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<NoteDto> CreateAsync(NoteDto noteDto, int userId)
        {
            var note = mapper.Map<Note>(noteDto);
            note.UserId = userId;
            note.TimeOfCreation = DateTime.UtcNow;

            await unitOfWork.Notes.AddAsync(note);
            await unitOfWork.CompleteAsync();

            var newNoteDto = mapper.Map<NoteDto>(note);
            return newNoteDto;
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var note = await unitOfWork.Notes.GetByIdAsync(id, userId);

            if (note == null)
            {
                throw new KeyNotFoundException("User does not have a note with this id");
            }

            if (note.TimeOfCreation.AddMinutes(15) < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Time to delete this note is over");
            }

            unitOfWork.Notes.Delete(note);
            await unitOfWork.CompleteAsync();
        }

        public async Task<List<NoteDto>> GetAllAsync(int userId)
        {
            var notes = await unitOfWork.Notes.GetAllAsync(userId);

            if (!notes.Any())
            {
                throw new KeyNotFoundException("User has no saved notes");
            }

            var notesDto = mapper.Map<List<NoteDto>>(notes);
            return notesDto;
        }

        public async Task<NoteDto> UpdateAsync(int noteId, NoteDto noteDto, int userId)
        {
            var updatableNote = await unitOfWork.Notes.GetByIdAsync(noteId, userId);

            if (updatableNote == null)
            {
                throw new KeyNotFoundException("User does not have a note with this id");
            }

            if (updatableNote.TimeOfCreation.AddMinutes(15) < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Time to update this note is over");
            }

            updatableNote.Text = noteDto.Text;
            var note = mapper.Map<Note>(updatableNote);

            unitOfWork.Notes.Update(note);
            await unitOfWork.CompleteAsync();

            var updatedNote = mapper.Map<NoteDto>(note);
            return updatedNote;
        }
    }
}
