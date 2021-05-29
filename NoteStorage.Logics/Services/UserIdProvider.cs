using Microsoft.AspNetCore.Http;
using NoteStorage.Logics.Interfaces;
using System;
using System.Security.Claims;

namespace NoteStorage.Logics.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int UserId => Int32.Parse(httpContextAccessor
            .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
}
