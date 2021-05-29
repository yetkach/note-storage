using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoteStorage.Data.EF;
using NoteStorage.Data.Entities;
using NoteStorage.Data.Repositories;
using NoteStorage.Jwt;
using NoteStorage.Jwt.Services;
using NoteStorage.Logics.Mapping;
using NoteStorage.Logics.Services;
using NoteStorage.Web.Controllers;
using NoteStorage.Web.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NoteStorage.UnitTests
{
    public class LoginControllerTests : IDisposable
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        public LoginControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase("UserDb").Options;
            context = new ApplicationDbContext(options);

            mapper = new MapperConfiguration(config =>
               config.AddProfile(typeof(MapperProfile))
           ).CreateMapper();
        }


        [Fact]
        public async Task AuthenticateAsync_ReturnsStringTokenWhenValidUserModel()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            context.Add(user);
            context.SaveChanges();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var userService = new UserService(unitOfWork, mapper);

            var authOptions = Options.Create<AuthOptions>(new AuthOptions());
            authOptions.Value.Issuer = "authServer";
            authOptions.Value.Secret = "secretKeygndkroelghh";
            authOptions.Value.Audience = "resourceServer";
            authOptions.Value.TokenLifeTime = 3600;
            var jwtTokenManager = new JwtTokenService(authOptions);

            var controller = new LoginController(userService, jwtTokenManager);
            var userModel = new UserModel { Login = "loginuser", Password = "passworduser" };

            //Act
            var actionResult = await controller.AuthenticateAsync(userModel);

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
            var token = (actionResult as OkObjectResult).Value as string;
            Assert.NotNull(token);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsUnauthorizedResultWhenInValidUserModel()
        {
            //Arrange
            var user = new User { Login = "loginuser", Password = "passworduser" };
            context.Add(user);
            context.SaveChanges();

            var usersRepository = new UserRepository(context);
            var notesRepository = new NoteRepository(context);
            var unitOfWork = new UnitOfWork(context, usersRepository, notesRepository);
            var userService = new UserService(unitOfWork, mapper);

            var authOptions = Options.Create<AuthOptions>(new AuthOptions());
            authOptions.Value.Issuer = "authServer";
            authOptions.Value.Secret = "secretKeygndkroelghh";
            authOptions.Value.Audience = "resourceServer";
            authOptions.Value.TokenLifeTime = 3600;
            var jwtTokenManager = new JwtTokenService(authOptions);

            var controller = new LoginController(userService, jwtTokenManager);
            var userModel = new UserModel { Login = "login", Password = "passwor" };

            //Act
            var actionResult = await controller.AuthenticateAsync(userModel);

            //Assert
            Assert.IsType<UnauthorizedResult>(actionResult);
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
