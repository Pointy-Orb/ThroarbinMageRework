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
                    var newItem = ItemID.Aglet;
                    var rand = WorldGen.genRand.Next(9);
                    switch (rand)
                    {
                        case 0:
                            newItem = ItemID.Spear;
                            break;
                        case 1:
                            newItem = ItemID.Blowpipe;
                            break;
                        case 2:
                            newItem = ItemID.WoodenBoomerang;
                            break;
                        case 3:
                            newItem = ItemID.Aglet;
                            break;
                        case 4:
                            newItem = ItemID.ClimbingClaws;
                            break;
                        case 5:
                            newItem = ItemID.Umbrella;
                            break;
                        case 6:
                            newItem = ItemID.CordageGuide;
                            break;
                        case 7:
                            newItem = ItemID.Radar;
                            break;
                        case 8:
                            newItem = ItemID.PortableStool;
                            break;
                    }
                    item.SetDefaults(newItem);
                    item.Prefix(-1);
                }
            }
        }
    }
}
