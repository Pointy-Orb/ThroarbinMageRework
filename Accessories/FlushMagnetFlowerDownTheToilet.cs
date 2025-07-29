using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Accessories;

public class FlushMagnetFlowerDownTheToilet : ModSystem
{
    public override void PostAddRecipes()
    {
        foreach (Recipe recipe in Main.recipe)
        {
            if (recipe.HasResult(ItemID.MagnetFlower))
            {
                recipe.DisableRecipe();
            }
        }
    }
}
