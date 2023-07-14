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
                "Anderson",
                "Bennett",
                "Carter",
                "Davis",
                "Evans",
                "Foster",
                "Garcia",
                "Harrison",
                "Johnson",
                "King",
                "Lawson",
                "Mitchell",
                "Nelson",
                "Parker",
                "Ramirez",
                "Sanchez",
                "Thompson",
                "Walker",
                "Young",
                "Zimmerman",
                "Adams",
                "Baker",
                "Campbell",
                "Douglas",
                "Edwards",
                "Franklin",
                "Gonzalez",
                "Hernandez",
                "Jackson",
                "Kelly",
                "Lewis",
                "Martinez",
                "Owens",
                "Patterson",
                "Richardson",
                "Scott",
                "Turner",
                "Williams",
                "Wilson",
                "Younger",
                "Abbott",
                "Bishop",
                "Chambers",
                "Donovan",
                "Ellis",
                "Fitzgerald",
                "Gibson",
                "Hawkins",
                "Ingram",
                "Jenkins",
                "Knight",
                "Lawrence",
                "Morrison",
                "Owens",
                "Palmer",
                "Reynolds",
                "Saunders",
                "Thornton",
                "Wallace",
                "Yates",
                "Anderson",
                "Baker",
                "Carlson",
                "Dixon",
                "Evans",
                "Foster",
                "Grayson",
                "Hughes",
                "Ingram",
                "Jenkins",
                "Knight",
                "Lawson",
                "Mitchell",
                "Nelson",
                "Olson",
                "Peterson",
                "Reynolds",
                "Sullivan",
                "Thompson",
                "Watson",
                "Adams",
                "Bennett",
                "Collins",
                "Davidson",
                "Ellis",
                "Fisher",
                "Gonzalez",
                "Hawkins",
                "Ingram",
                "Jameson",
                "Kelly",
                "Lawson",
                "Morgan",
                "Owens",
                "Parker",
                "Richardson",
                "Simmons",
                "Turner",
                "Walker",
                "Young",
                };
        }
    }
}
