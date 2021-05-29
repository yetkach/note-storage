using AutoMapper;
using NoteStorage.Data.Entities;
using NoteStorage.Logics.ModelsDto;

namespace NoteStorage.Logics.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Note, NoteDto>().ReverseMap();
        }
    }
}
