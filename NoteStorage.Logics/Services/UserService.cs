using AutoMapper;
using NoteStorage.Data.Interfaces;
using NoteStorage.Logics.Interfaces;
using NoteStorage.Logics.ModelsDto;
using System.Threading.Tasks;

namespace NoteStorage.Logics.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<UserDto> AuthenticateAsync(string login, string password)
        {
            var user = await unitOfWork.Users.GetAsync(login, password);
            var userDto = mapper.Map<UserDto>(user);

            return userDto;
        }
    }
}
