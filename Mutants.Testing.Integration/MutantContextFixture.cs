using System;
using Microsoft.EntityFrameworkCore;
using Mutants.Data;

namespace Mutants.Testing.Integration
{
    public class MutantContextFixture : IDisposable
    {
        public MutantContextFixture()
        {
            var builder = new DbContextOptionsBuilder<MutantContext>();
            builder.UseSqlServer(GetConnectionString());
            var options = builder.Options;
            Context = new MutantContext(options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        private static string GetConnectionString() =>
            //"Server=(local),1434;Database=mutantes;Trusted_Connection=False;User=SA;Password=34B1dKdXzoUB";
            "Server=tcp:azeballessqlserver.database.windows.net,1433;Initial Catalog=mutantes;Persist Security Info=False;User ID=azeballes;Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public void Dispose()
        {
            Context.Dispose();
        }

        public MutantContext Context { get; }
    }
}
