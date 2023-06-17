using Microsoft.EntityFrameworkCore;
namespace Gaos.Seed
{
    public class RecipeDataKind
    {

        public static void Seed(ModelBuilder modelBuilder)
        {
            // seed Dbo.User
            modelBuilder.Entity<Gaos.Dbo.Model.RecipeDataKind>().HasData(
                new Gaos.Dbo.Model.RecipeDataKind { Id = (int)Gaos.Dbo.Model.RecipeDataKindEnum.BasicRecipeObjects, Name = Gaos.Dbo.Model.RecipeDataKindEnum.BasicRecipeObjects.ToString() },
                new Gaos.Dbo.Model.RecipeDataKind { Id = (int)Gaos.Dbo.Model.RecipeDataKindEnum.ProcessedRecipeObjects, Name = Gaos.Dbo.Model.RecipeDataKindEnum.ProcessedRecipeObjects.ToString() },
                new Gaos.Dbo.Model.RecipeDataKind { Id = (int)Gaos.Dbo.Model.RecipeDataKindEnum.EnhancedRecipeObjects, Name = Gaos.Dbo.Model.RecipeDataKindEnum.EnhancedRecipeObjects.ToString() },
                new Gaos.Dbo.Model.RecipeDataKind { Id = (int)Gaos.Dbo.Model.RecipeDataKindEnum.AssembledRecipeObjects, Name = Gaos.Dbo.Model.RecipeDataKindEnum.AssembledRecipeObjects.ToString() }
            );
        } 
    }
}
