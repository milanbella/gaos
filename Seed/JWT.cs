using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Gaos.Dbo;

namespace Gaos.Seed
{
    public class JWT
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // seed Dbo.User
            modelBuilder.Entity<Gaos.Dbo.JWT>().HasData(
                new Gaos.Dbo.JWT { Id = 1, Token = "x", UserId = 1 },
                new Gaos.Dbo.JWT { Id = 2, Token = "x", UserId = 1 },
                new Gaos.Dbo.JWT { Id = 3, Token = "x", UserId = 2 },
                new Gaos.Dbo.JWT { Id = 4, Token = "x", UserId = 2 },
                new Gaos.Dbo.JWT { Id = 5, Token = "x", UserId = 3 },
                new Gaos.Dbo.JWT { Id = 6, Token = "x", UserId = 3 },
                new Gaos.Dbo.JWT { Id = 7, Token = "x", UserId = 3 }
            );
        }
    }
}
