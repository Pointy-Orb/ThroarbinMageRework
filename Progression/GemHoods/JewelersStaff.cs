using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using Terraria.DataStructures;

namespace ThroarbinMageRework.Progression.GemHoods;

public class JewelersStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.staff[Type] = true;
    }

    public override LocalizedText Tooltip => this.GetLocalization(ModContent.GetInstance<Configs.ClientConfig>().ComplicatedTooltips ? "TooltipComplicated" : "TooltipSimple");

    public override void SetDefaults()
    {
        Item.width = 46;
        Item.height = 42;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 1);
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.autoReuse = true;
        Item.mana = 12;
        Item.damage = 19;
        Item.DamageType = DamageClass.Magic;
        Item.noMelee = true;
        Item.shoot = ModContent.ProjectileType<JewelersStaffProjectile>();
        Item.shootSpeed = 10f;
        Item.knockBack = 5f;
        Item.UseSound = SoundID.Item43;
        Item.useTime = 36;
        Item.ChangePlayerDirectionOnShoot = true;
        Item.useAnimation = 36;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TopazStaff)
            .AddIngredient(ItemID.EmeraldStaff)
            .AddIngredient(ItemID.DiamondStaff)
            .AddIngredient(ItemID.DemoniteBar, 6)
            .AddTile(TileID.Anvils)
            .Register();

        CreateRecipe()
            .AddIngredient(ItemID.TopazStaff)
            .AddIngredient(ItemID.EmeraldStaff)
            .AddIngredient(ItemID.DiamondStaff)
            .AddIngredient(ItemID.CrimtaneBar, 6)
            .AddTile(TileID.Anvils)
            .Register();
    }
}

public class JewelersStaffProjectile : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_126";

    public override void OnSpawn(IEntitySource source)
    {
        int onCursor = Main.rand.Next(3);
        var player = Main.player[Projectile.owner];
        var hoodPlayer = player.GetModPlayer<GemHoodPlayer>();
        if (hoodPlayer.gemSet >= 0)
        {
            if (Mod.TryFind<ModItem>(ItemID.Search.GetName(hoodPlayer.gemSet) + "Hood", out var hoodItem))
            {
                if (hoodItem is GemHood hood)
                {
                    switch (hood.gemId)
                    {
                        case ItemID.Topaz:
                            onCursor = 0;
                            break;
                        case ItemID.Emerald:
                            onCursor = 1;
                            break;
                        case ItemID.Diamond:
                            onCursor = 2;
                            break;
                    }
                }
            }
        }
        var topazVelocity = Projectile.velocity;
        var emeraldVelocity = Projectile.velocity;
        var diamondVelocity = Projectile.velocity;
        var flip = Main.rand.NextFloatDirection();
        if (onCursor == 0)
        {
            emeraldVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            emeraldVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
            diamondVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            diamondVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
        }
        if (onCursor == 1)
        {
            topazVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            topazVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
            diamondVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            diamondVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
        }
        if (onCursor == 2)
        {
            topazVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            topazVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
            emeraldVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            emeraldVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
        }
        Projectile.NewProjectile(source, Projectile.position, topazVelocity, ProjectileID.TopazBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
        Projectile.NewProjectile(source, Projectile.position, emeraldVelocity, ProjectileID.EmeraldBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
        Projectile.NewProjectile(source, Projectile.position, diamondVelocity, ProjectileID.DiamondBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
        Projectile.Kill();
    }
}
