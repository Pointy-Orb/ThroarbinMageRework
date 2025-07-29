using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Accessories;

public class ManaKnuckles : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 38;
        Item.height = 30;
        Item.value = Item.sellPrice(gold: 9);
        Item.rare = ItemRarityID.Pink;
        Item.accessory = true;
        Item.defense = 8;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FleshKnuckles)
            .AddIngredient(ItemID.ManaFlower)
            .AddTile(TileID.TinkerersWorkbench)
            .SortBeforeFirstRecipesOf(ItemID.ManaFlower)
            .Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.manaFlower = true;
        player.manaCost -= 0.08f;
        player.aggro += 400;
    }
}
