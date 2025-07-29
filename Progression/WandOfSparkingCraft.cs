using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Progression;

public class WandOfSparkingCraft : ModSystem
{
    public override void AddRecipes()
    {
        Recipe.Create(ItemID.WandofSparking)
            .AddRecipeGroup(RecipeGroupID.Wood, 8)
            .AddIngredient(ItemID.Torch, 99)
            .SortBeforeFirstRecipesOf(ItemID.WandofFrosting)
            .Register();
    }

    public override void PostWorldGen()
    {
        for (int i = 0; i < Main.chest.Length; i++)
        {
            if (Main.chest[i] == null)
            {
                continue;
            }
            var chest = Main.chest[i];
            foreach (Item item in chest.item)
            {
                if (item.type == ItemID.WandofSparking)
                {
                    item.SetDefaults(ItemID.Aglet);
                    item.Prefix(-1);
                }
            }
        }
    }
}
