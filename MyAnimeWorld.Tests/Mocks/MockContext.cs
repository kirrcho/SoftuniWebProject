using Microsoft.EntityFrameworkCore;
using MyAnimeWorld.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Tests.Mocks
{
    public static class MockContext
    {
        public static AnimeWorldContext GetContext()
        {
            var options = new DbContextOptionsBuilder<AnimeWorldContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AnimeWorldContext(options);
        }
    }
}
