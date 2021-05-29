using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteStorage.Logics.Interfaces;
using NoteStorage.Logics.ModelsDto;
using NoteStorage.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteStorage.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService noteService;
        private readonly IMapper mapper;
        private readonly IUserIdProvider userIdProvider;

        public NotesController(INoteService noteService, IMapper mapper,
            IUserIdProvider userIdProvider)
        {
            this.noteService = noteService;
            this.mapper = mapper;
            this.userIdProvider = userIdProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var noteModels = await noteService.GetAllAsync(userIdProvider.UserId);
            var notes = mapper.Map<List<NoteModel>>(noteModels);

            return Ok(notes);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await noteService.DeleteAsync(id, userIdProvider.UserId);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] NoteModel noteModel)
        {
            var noteDto = mapper.Map<NoteDto>(noteModel);
            var newNoteDto = await noteService.CreateAsync(noteDto, userIdProvider.UserId);
            var newNote = mapper.Map<NoteModel>(newNoteDto);

            return Ok(newNote);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] NoteModel noteModel)
        {
            var noteDto = mapper.Map<NoteDto>(noteModel);
            var updatedNoteDto = await noteService.UpdateAsync(id, noteDto, userIdProvider.UserId);
            var updatedNote = mapper.Map<NoteModel>(updatedNoteDto);

            return Ok(updatedNote);
        }
    }
}
