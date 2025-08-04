using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace ThroarbinMageRework.Accessories;

public enum MagicGloveTier
{
    NoGlove,
    MagicGlove,
    WitchsGlove
}

[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
public class MagicGlove : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 28;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 4, silver: 25);
        Item.accessory = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FeralClaws)
            .AddIngredient(ItemID.FallenStar, 5)
            .AddIngredient(ItemID.HellstoneBar, 8)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<MagicGlovePlayer>().magicGlove = MagicGloveTier.MagicGlove;
    }

    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((ModContent.GetInstance<Configs.ServerConfig>().AccessoryBalance == Configs.AccessoryBalance.Faithful ? 10 : 25));
}

public class MagicGlovePlayer : ModPlayer
{
    public MagicGloveTier magicGlove = MagicGloveTier.NoGlove;

    public override void ResetEffects()
    {
        magicGlove = MagicGloveTier.NoGlove;
    }
}

public class MagicProjectileVelocity : GlobalProjectile
{
    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (projectile.DamageType != DamageClass.Magic)
        {
            return;
        }
        if (projectile.owner < 0)
        {
            return;
        }
        var player = Main.player[projectile.owner];
        if (player.GetModPlayer<MagicGlovePlayer>().magicGlove == MagicGloveTier.NoGlove)
        {
            return;
        }
        projectile.velocity *= ModContent.GetInstance<Configs.ServerConfig>().AccessoryBalance == Configs.AccessoryBalance.Faithful ? 1.1f : 1.25f;
        if (player.GetModPlayer<MagicGlovePlayer>().magicGlove == MagicGloveTier.WitchsGlove)
        {
            projectile.extraUpdates += 1;
        }
    }
}

[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
public class WitchsGlove : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 28;
        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(gold: 8, silver: 25);
        Item.accessory = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<MagicGlove>())
            .AddIngredient(ItemID.SpectreBar, 8)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        return incomingItem.type != ModContent.ItemType<MagicGlove>();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<MagicGlovePlayer>().magicGlove = MagicGloveTier.WitchsGlove;
    }
}
