namespace MiniFootball.Test.Mocks
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class DatabaseMock
    {
        public static MiniFootballDbContext Instance
        {
            get
            {
                var options = new DbContextOptionsBuilder<MiniFootballDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new MiniFootballDbContext(options);
            }
        }

    }
}
