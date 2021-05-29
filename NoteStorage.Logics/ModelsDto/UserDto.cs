using System.Collections.Generic;

namespace NoteStorage.Logics.ModelsDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<NoteDto> Notes { get; set; }
    }
}
