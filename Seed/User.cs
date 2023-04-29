﻿using Microsoft.EntityFrameworkCore;

namespace gaos.Seed
{
    public class User
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // seed Dbo.User
            modelBuilder.Entity<gaos.Dbo.User>().HasData(
                new gaos.Dbo.User { Id = 1, Name = "karol1", Email = "karol1@localhost", PasswordHash = "karol1" },
                new gaos.Dbo.User { Id = 2, Name = "karol2", Email = "karol2@localhost", PasswordHash = "karol2" },
                new gaos.Dbo.User { Id = 3, Name = "karol3", Email = "karol3@localhost", PasswordHash = "karol3" }
            );
        } 
    }
}
