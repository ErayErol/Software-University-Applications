namespace MiniFootball.Test.Mocks
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class DatabaseMock
    {
        public static MessiFinderDbContext Instance
        {
            get
            {
                var options = new DbContextOptionsBuilder<MessiFinderDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new MessiFinderDbContext(options);
            }
        }

    }
}
