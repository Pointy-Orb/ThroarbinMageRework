using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework;

public class StaffRecipes : ModSystem
{
    public override void PostAddRecipes()
    {
        foreach (Recipe recipe in Main.recipe)
        {
            if (recipe.HasResult(ItemID.AmethystStaff) && !recipe.HasIngredient(ItemID.TinBar))
            {
                var altRecipe = recipe.Clone();
                altRecipe.RemoveIngredient(ItemID.CopperBar);
                altRecipe.AddIngredient(ItemID.TinBar, 10);
                altRecipe.SortAfterFirstRecipesOf(ItemID.AmethystStaff);
                altRecipe.Register();
            }
            if (recipe.HasResult(ItemID.TopazStaff) && !recipe.HasIngredient(ItemID.CopperBar))
            {
                var altRecipe = recipe.Clone();
                altRecipe.RemoveIngredient(ItemID.TinBar);
                altRecipe.AddIngredient(ItemID.CopperBar, 10);
                altRecipe.SortAfterFirstRecipesOf(ItemID.TopazStaff);
                altRecipe.Register();
            }
            if (recipe.HasResult(ItemID.SapphireStaff) && !recipe.HasIngredient(ItemID.TungstenBar))
            {
                var altRecipe = recipe.Clone();
                altRecipe.RemoveIngredient(ItemID.SilverBar);
                altRecipe.AddIngredient(ItemID.TungstenBar, 10);
                altRecipe.SortAfterFirstRecipesOf(ItemID.SapphireStaff);
                altRecipe.Register();
            }
            if (recipe.HasResult(ItemID.EmeraldStaff) && !recipe.HasIngredient(ItemID.SilverBar))
            {
                var altRecipe = recipe.Clone();
                altRecipe.RemoveIngredient(ItemID.TungstenBar);
                altRecipe.AddIngredient(ItemID.SilverBar, 10);
                altRecipe.SortAfterFirstRecipesOf(ItemID.EmeraldStaff);
                altRecipe.Register();
            }
            if (recipe.HasResult(ItemID.RubyStaff) && !recipe.HasIngredient(ItemID.PlatinumBar))
            {
                var altRecipe = recipe.Clone();
                altRecipe.RemoveIngredient(ItemID.GoldBar);
                altRecipe.AddIngredient(ItemID.PlatinumBar, 10);
                altRecipe.SortAfterFirstRecipesOf(ItemID.RubyStaff);
                altRecipe.Register();
            }
            if (recipe.HasResult(ItemID.DiamondStaff) && !recipe.HasIngredient(ItemID.GoldBar))
            {
                var altRecipe = recipe.Clone();
                altRecipe.RemoveIngredient(ItemID.PlatinumBar);
                altRecipe.AddIngredient(ItemID.GoldBar, 10);
                altRecipe.SortAfterFirstRecipesOf(ItemID.DiamondStaff);
                altRecipe.Register();
            }
        }
    }
}
