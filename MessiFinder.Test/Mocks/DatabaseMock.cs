namespace MessiFinder.Test.Mocks
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
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Give a Unique name to the DB
                    .Options;

                return new MessiFinderDbContext(options);
            }
        }

    }
}
