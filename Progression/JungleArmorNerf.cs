using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Progression;

public class JungleArmorNerf : ModSystem
{
    public override void PostAddRecipes()
    {
        //Quantity of tissue samples/shadow scales taken from Obsidian Armor recipe
        foreach (Recipe recipe in Main.recipe)
        {
            if (recipe.HasIngredient(ItemID.ShadowScale) || recipe.HasIngredient(ItemID.TissueSample))
            {
                continue;
            }
            if (recipe.HasResult(ItemID.JungleHat) || recipe.HasResult(ItemID.JunglePants))
            {
                var crimRecipe = recipe.Clone();
                recipe.AddIngredient(ItemID.ShadowScale, 5);
                crimRecipe.AddIngredient(ItemID.TissueSample, 5);
                crimRecipe.SortAfter(recipe);
                crimRecipe.Register();
            }
            if (recipe.HasResult(ItemID.JungleShirt))
            {
                var crimRecipe = recipe.Clone();
                recipe.AddIngredient(ItemID.ShadowScale, 10);
                crimRecipe.AddIngredient(ItemID.TissueSample, 10);
                crimRecipe.SortAfter(recipe);
                crimRecipe.Register();
            }
        }
    }
}
