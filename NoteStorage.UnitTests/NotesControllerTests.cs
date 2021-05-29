using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NoteStorage.Data.EF;
using NoteStorage.Data.Entities;
using NoteStorage.Data.Repositories;
using NoteStorage.Logics.Interfaces;
using NoteStorage.Logics.Services;
using NoteStorage.Web.Controllers;
using NoteStorage.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NoteStorage.UnitTests
{
    public class NotesControllerTests : IDisposable
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        public NotesControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase("NotesDb").Options;
            context = new ApplicationDbContext(options);

            mapper = new MapperConfiguration(config =>
              config.AddProfile(typeof(TestingMapper))
          ).CreateMapper();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkObjectResultWithListOfUserModels()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            var notes = new List<Note>
            {
                new Note {Text = "test note 1", UserId = 1},
                new Note {Text  = "test note 2", UserId = 1}
            };

            await context.AddRangeAsync(notes);
            await context.SaveChangesAsync();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var notesService = new NoteService(unitOfWork, mapper);

            var userIdGenerator = new Mock<IUserIdProvider>();
            userIdGenerator.SetupGet(u => u.UserId).Returns(1);

            var controller = new NotesController(notesService, mapper, userIdGenerator.Object);

            //Act
            var result = await controller.GetAllAsync();

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);

            var noteModels = (result as OkObjectResult).Value as List<NoteModel>;
            Assert.NotNull(noteModels);
            Assert.Equal(notes.Count, noteModels.Count);
            Assert.All(noteModels, noteModel => Assert.NotNull(noteModel.Id));
            Assert.All(noteModels, noteModel => Assert.NotNull(noteModel.Text));
        }

        [Fact]
        public async Task GetAllAsync_ReturnsKeyNotFoundException()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            var notes = new List<Note>
            {
                new Note {Text = "test note 1", UserId = 1},
                new Note {Text  = "test note 2", UserId = 1}
            };

            await context.AddRangeAsync(notes);
            await context.SaveChangesAsync();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var notesService = new NoteService(unitOfWork, mapper);

            var userIdGenerator = new Mock<IUserIdProvider>();
            userIdGenerator.SetupGet(u => u.UserId).Returns(70);

            var controller = new NotesController(notesService, mapper, userIdGenerator.Object);

            //Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => controller.GetAllAsync());
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNoContent()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            var notes = new List<Note>
            {
                new Note {Text = "test note 1", UserId = 1, TimeOfCreation = DateTime.UtcNow},
                new Note {Text  = "test note 2", UserId = 1, TimeOfCreation = DateTime.UtcNow}
            };

            await context.AddRangeAsync(notes);
            await context.SaveChangesAsync();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var notesService = new NoteService(unitOfWork, mapper);

            var userIdGenerator = new Mock<IUserIdProvider>();
            userIdGenerator.SetupGet(u => u.UserId).Returns(1);

            var controller = new NotesController(notesService, mapper, userIdGenerator.Object);
            int noteId = 1;

            //Act
            var result = await controller.DeleteAsync(noteId);

            //Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsKeyNotFoundException()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            var notes = new List<Note>
            {
                new Note {Text = "test note 1", UserId = 1, TimeOfCreation = DateTime.UtcNow},
                new Note {Text  = "test note 2", UserId = 1, TimeOfCreation = DateTime.UtcNow}
            };

            await context.AddRangeAsync(notes);
            await context.SaveChangesAsync();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var notesService = new NoteService(unitOfWork, mapper);

            var userIdGenerator = new Mock<IUserIdProvider>();
            userIdGenerator.SetupGet(u => u.UserId).Returns(1);

            var controller = new NotesController(notesService, mapper, userIdGenerator.Object);
            int noteId = 1000;

            //Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => controller.DeleteAsync(noteId));
        }

        [Fact]
        public async Task DeleteAsync_ThrowsInvalidOperationException()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            var notes = new List<Note>
            {
                new Note {Text = "test note 1", UserId = 1},
                new Note {Text  = "test note 2", UserId = 1}
            };

            await context.AddRangeAsync(notes);
            await context.SaveChangesAsync();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var notesService = new NoteService(unitOfWork, mapper);

            var userIdGenerator = new Mock<IUserIdProvider>();
            userIdGenerator.SetupGet(u => u.UserId).Returns(1);

            var controller = new NotesController(notesService, mapper, userIdGenerator.Object);
            int noteId = 1;

            //Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => controller.DeleteAsync(noteId));
        }

        [Fact] 
        public async Task CreateAsync_ReturnsOkObjectResultWithNewCreatedNote()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var notesService = new NoteService(unitOfWork, mapper);

            var userIdGenerator = new Mock<IUserIdProvider>();
            userIdGenerator.SetupGet(u => u.UserId).Returns(1);

            var noteModel = new NoteModel { Text = "new test note" };
            var controller = new NotesController(notesService, mapper, userIdGenerator.Object);

            //Act
            var result = await controller.CreateAsync(noteModel);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var createdNoteModel = (result as OkObjectResult).Value as NoteModel;
            Assert.NotNull(createdNoteModel);
            Assert.Equal(noteModel.Text, createdNoteModel.Text);
        }

        [Fact]
        public async Task EditAsync_ReturnsOkObjectResultWithUpdatedNote()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            var notes = new List<Note>
            {
                new Note {Text = "test note 1", UserId = 1, TimeOfCreation = DateTime.UtcNow},
                new Note {Text  = "test note 2", UserId = 1,  TimeOfCreation = DateTime.UtcNow}
            };
            await context.AddRangeAsync(notes);
            await context.SaveChangesAsync();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var notesService = new NoteService(unitOfWork, mapper);

            var userIdGenerator = new Mock<IUserIdProvider>();
            userIdGenerator.SetupGet(u => u.UserId).Returns(1);

            var idOfUpdatebleNote = 1;
            var noteModel = new NoteModel { Text = "updated test note" };
            var controller = new NotesController(notesService, mapper, userIdGenerator.Object);

            //Act
            var result = await controller.EditAsync(idOfUpdatebleNote, noteModel);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var updatedNoteModel = (result as OkObjectResult).Value as NoteModel;
            Assert.NotNull(updatedNoteModel);
            Assert.Equal(noteModel.Text, updatedNoteModel.Text);
            Assert.Equal(idOfUpdatebleNote, updatedNoteModel.Id);
        }

        [Fact]
        public async Task EditAsync_ThrowsKeyNotFoundException()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            var notes = new List<Note>
            {
                new Note {Text = "test note 1", UserId = 1, TimeOfCreation = DateTime.UtcNow},
                new Note {Text  = "test note 2", UserId = 1,  TimeOfCreation = DateTime.UtcNow}
            };
            await context.AddRangeAsync(notes);
            await context.SaveChangesAsync();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var notesService = new NoteService(unitOfWork, mapper);

            var userIdGenerator = new Mock<IUserIdProvider>();
            userIdGenerator.SetupGet(u => u.UserId).Returns(1);

            var idOfUpdatebleNote = 1000;
            var noteModel = new NoteModel { Text = "updated test note" };
            var controller = new NotesController(notesService, mapper, userIdGenerator.Object);

            //Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => controller.EditAsync(idOfUpdatebleNote, noteModel));
        }

        [Fact]
        public async Task EditAsync_ThrowsInvalidOperationException()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            var notes = new List<Note>
            {
                new Note {Text = "test note 1", UserId = 1},
                new Note {Text  = "test note 2", UserId = 1}
            };
            await context.AddRangeAsync(notes);
            await context.SaveChangesAsync();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var notesService = new NoteService(unitOfWork, mapper);

            var userIdGenerator = new Mock<IUserIdProvider>();
            userIdGenerator.SetupGet(u => u.UserId).Returns(1);

            var idOfUpdatebleNote = 1;
            var noteModel = new NoteModel { Text = "updated test note" };
            var controller = new NotesController(notesService, mapper, userIdGenerator.Object);

            //Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => controller.EditAsync(idOfUpdatebleNote, noteModel));
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
