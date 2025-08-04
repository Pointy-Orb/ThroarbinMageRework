using Terraria;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Accessories;

public class FlowerEmblem : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 32;
        Item.value = Item.sellPrice(gold: 4, silver: 20);
        Item.rare = ItemRarityID.LightPurple;
        Item.accessory = true;
    }

    private readonly List<short> manaFlowers = new() { ItemID.ManaFlower, ItemID.ArcaneFlower, ItemID.MagnetFlower };

    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(15);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.CelestialEmblem)
            .AddIngredient(ItemID.ManaFlower)
            .AddTile(TileID.TinkerersWorkbench)
            .SortBeforeFirstRecipesOf(ItemID.ManaFlower)
            .Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.manaFlower = true;
        player.manaMagnet = true;
        player.manaCost -= 0.08f;
        player.GetDamage(DamageClass.Magic) += 0.15f;
    }
}

