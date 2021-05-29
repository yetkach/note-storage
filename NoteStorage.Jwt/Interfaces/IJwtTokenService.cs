using NoteStorage.Logics.ModelsDto;

namespace NoteStorage.Jwt.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserDto userModel);
    }
}
