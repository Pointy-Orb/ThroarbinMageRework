using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Accessories;

[AutoloadEquip(EquipType.HandsOn)]
public class CelestialBand : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 20;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 7);
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statManaMax2 += 20;
        player.GetModPlayer<StarBandPlayer>().starBand = true;
        player.manaMagnet = true;
        player.GetDamage(DamageClass.Magic) += 0.15f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<StarBand>())
            .AddIngredient(ItemID.CelestialEmblem)
            .AddTile(TileID.TinkerersWorkbench)
            .SortAfterFirstRecipesOf(ItemID.CelestialEmblem)
            .Register();
    }
}
