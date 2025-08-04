using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Accessories;

public class CuffKnuckles : ModItem
{
    public override void SetDefaults()
    {
        var knuckles = new Item();
        var cuffs = new Item();
        knuckles.SetDefaults(ItemID.FleshKnuckles);
        Item.value += knuckles.value;
        cuffs.SetDefaults(ItemID.CelestialCuffs);
        Item.value += cuffs.value;

        Item.accessory = true;
        Item.defense = knuckles.defense;
        Item.width = 32;
        Item.height = 34;
        Item.rare = ItemRarityID.LightPurple;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.CelestialCuffs)
            .AddIngredient(ItemID.FleshKnuckles)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.manaMagnet = true;
        player.statManaMax2 += 20;
        player.magicCuffs = true;
        player.aggro += 400;
    }
}
