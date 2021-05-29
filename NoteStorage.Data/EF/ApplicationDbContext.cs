using Microsoft.EntityFrameworkCore;
using NoteStorage.Data.Entities;
using System;
using System.Collections.Generic;

namespace NoteStorage.Data.EF
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var users = new List<User>()
            {
                new User {Id = 1, Login = "loginuser1", Password = "passworduser1"},
                new User {Id = 2, Login = "loginuser2", Password = "passworduser2"},
            };

            var notes = new List<Note>()
            {
                new Note {Id = 1, Text = "test note 1", UserId = 1, TimeOfCreation = new DateTime (2021, 5, 18, 12, 00, 00) },
                new Note {Id = 2, Text = "test note 2", UserId = 1, TimeOfCreation = new DateTime (2021, 5, 18, 12, 00, 00)},
                new Note {Id = 3, Text = "test note 3", UserId = 2, TimeOfCreation = new DateTime (2021, 5, 18, 12, 00, 00)},
                new Note {Id = 4, Text = "test note 4", UserId = 2, TimeOfCreation = new DateTime (2021, 5, 18, 12, 00, 00)}
            };

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Note>().HasData(notes);
        }
    }
}
