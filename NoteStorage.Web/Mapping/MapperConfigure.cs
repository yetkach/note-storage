using AutoMapper;
using NoteStorage.Logics.ModelsDto;
using NoteStorage.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteStorage.Web.Mapping
{
    public class MapperConfigure : Profile
    {
        public MapperConfigure()
        {
            CreateMap<UserModel, UserDto>();
            CreateMap<NoteDto, NoteModel>().ReverseMap();
        }
    }
}
