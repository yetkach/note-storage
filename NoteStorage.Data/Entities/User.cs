using System.Collections.Generic;

namespace NoteStorage.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
