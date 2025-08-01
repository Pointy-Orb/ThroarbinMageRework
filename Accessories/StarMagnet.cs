using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Accessories;

public class StarMagnet : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 28;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 3, silver: 80);
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statManaMax2 += 20;
        player.GetModPlayer<StarBandPlayer>().starBand = true;
        player.manaMagnet = true;
    }
}
