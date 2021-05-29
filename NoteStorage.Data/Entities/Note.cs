using System;

namespace NoteStorage.Data.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime TimeOfCreation { get; set; }
    }
}
