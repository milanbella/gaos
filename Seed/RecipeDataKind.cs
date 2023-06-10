using Microsoft.EntityFrameworkCore;
namespace Gaos.Seed
{
    enum RecipeDataKindEnum
    {
        BasicRecipeObjects,
        ProcessedRecipeObjects,
        RefinedRecipeObjects,
        AssembledRecipeObjects,
    };

    public class RecipeDataKind
    {

        public static void Seed(ModelBuilder modelBuilder)
        {
            // seed Dbo.User
            modelBuilder.Entity<Gaos.Dbo.Model.RecipeDataKind>().HasData(
                new Gaos.Dbo.Model.RecipeDataKind { Id = 1, Name = RecipeDataKindEnum.BasicRecipeObjects.ToString() },
                new Gaos.Dbo.Model.RecipeDataKind { Id = 2, Name = RecipeDataKindEnum.ProcessedRecipeObjects.ToString() },
                new Gaos.Dbo.Model.RecipeDataKind { Id = 3, Name = RecipeDataKindEnum.RefinedRecipeObjects.ToString() },
                new Gaos.Dbo.Model.RecipeDataKind { Id = 4, Name = RecipeDataKindEnum.AssembledRecipeObjects.ToString() }
            );
        } 
    }
}
