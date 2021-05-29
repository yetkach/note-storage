using AutoMapper;
using NoteStorage.Data.Entities;
using NoteStorage.Logics.ModelsDto;
using NoteStorage.Web.Models;

namespace NoteStorage.UnitTests
{
    public class TestingMapper : Profile
    {
        public TestingMapper()
        {
            CreateMap<Note, NoteDto>().ReverseMap();
            CreateMap<NoteDto, NoteModel>().ReverseMap();
        }
    }
}
