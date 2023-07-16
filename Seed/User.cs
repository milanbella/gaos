using Microsoft.EntityFrameworkCore;

namespace Gaos.Seed
{
    public class User
    {


        public static void Seed(ModelBuilder modelBuilder, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                // seed Dbo.User
                modelBuilder.Entity<Gaos.Dbo.Model.User>().HasData(
                    new Gaos.Dbo.Model.User { Id = 1, Name = "karol1", Email = "karol1@localhost", PasswordHash = "karol1" },
                    new Gaos.Dbo.Model.User { Id = 2, Name = "karol2", Email = "karol2@localhost", PasswordHash = "karol2" },
                    new Gaos.Dbo.Model.User { Id = 3, Name = "karol3", Email = "karol3@localhost", PasswordHash = "karol3" }
                );

                string[] randomNames = GetRandomLastNames();
                Gaos.Dbo.Model.User[] users = new Gaos.Dbo.Model.User[randomNames.Length];

                for (int i = 0; i < randomNames.Length; i++)
                {
                    users[i] = new Gaos.Dbo.Model.User
                    {
                        Id = i + 4,
                        Name = $"{randomNames[i]}",
                        Email = $"{randomNames[i]}",
                        PasswordHash = $"{randomNames[i]}",
                    };
                }
                modelBuilder.Entity<Gaos.Dbo.Model.User>().HasData(users);
            }
        } 

        public static string[] GetRandomLastNames()
        {

            return new string[] {
                "Abbott",
                "Adams",
                "Anderson",
                "Baker",
                "Bennett",
                "Bishop",
                "Campbell",
                "Carlson",
                "Carter",
                "Chambers",
                "Collins",
                "Davidson",
                "Davis",
                "Dixon",
                "Donovan",
                "Douglas",
                "Edwards",
                "Ellis",
                "Evans",
                "Fisher",
                "Fitzgerald",
                "Foster",
                "Franklin",
                "Garcia",
                "Gibson",
                "Gonzalez",
                "Grayson",
                "Harrison",
                "Hawkins",
                "Hernandez",
                "Hughes",
                "Ingram",
                "Jackson",
                "Jameson",
                "Jenkins",
                "Johnson",
                "Kelly",
                "King",
                "Knight",
                "Lawrence",
                "Lawson",
                "Lewis",
                "Martinez",
                "Mitchell",
                "Morgan",
                "Morrison",
                "Nelson",
                "Olson",
                "Owens",
                "Palmer",
                "Parker",
                "Peterson",
                "Ramirez",
                "Reynolds",
                "Richardson",
                "Sanchez",
                "Saunders",
                "Scott",
                "Simmons",
                "Sullivan",
                "Thornton",
                "Turner",
                "Walker",
                "Wallace",
                "Watson",
                "Williams",
                "Wilson",
                "Yates",
                "Young",
                "Younger",
                "Zimmerman",
                };
        }
    }
}
