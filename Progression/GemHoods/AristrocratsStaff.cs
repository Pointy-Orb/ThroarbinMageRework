using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using Terraria.DataStructures;

namespace ThroarbinMageRework.Progression.GemHoods;

public class AristrocratsStaff : ModItem
{
    public override LocalizedText Tooltip => this.GetLocalization(ModContent.GetInstance<Configs.ClientConfig>().ComplicatedTooltips ? "TooltipComplicated" : "TooltipSimple");

    public override void SetStaticDefaults()
    {
        Item.staff[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 46;
        Item.height = 42;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 1);
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.autoReuse = true;
        Item.mana = 9;
        Item.damage = 16;
        Item.DamageType = DamageClass.Magic;
        Item.noMelee = true;
        Item.shoot = ModContent.ProjectileType<AristrocratsStaffProjectile>();
        Item.shootSpeed = 9f;
        Item.knockBack = 4.5f;
        Item.UseSound = SoundID.Item43;
        Item.useTime = 32;
        Item.ChangePlayerDirectionOnShoot = true;
        Item.useAnimation = 32;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.AmethystStaff)
            .AddIngredient(ItemID.SapphireStaff)
            .AddIngredient(ItemID.RubyStaff)
            .AddIngredient(ItemID.DemoniteBar, 6)
            .SortAfterFirstRecipesOf(ItemID.RubyStaff)
            .AddTile(TileID.Anvils)
            .Register();

        CreateRecipe()
            .AddIngredient(ItemID.AmethystStaff)
            .AddIngredient(ItemID.SapphireStaff)
            .AddIngredient(ItemID.RubyStaff)
            .AddIngredient(ItemID.CrimtaneBar, 6)
            .SortAfterFirstRecipesOf(ItemID.RubyStaff)
            .AddTile(TileID.Anvils)
            .Register();
    }
}

public class AristrocratsStaffProjectile : ModProjectile
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
                        case ItemID.Amethyst:
                            onCursor = 0;
                            break;
                        case ItemID.Sapphire:
                            onCursor = 1;
                            break;
                        case ItemID.Ruby:
                            onCursor = 2;
                            break;
                    }
                }
            }
        }
        var amethystVelocity = Projectile.velocity;
        var sapphireVelocity = Projectile.velocity;
        var rubyVelocity = Projectile.velocity;
        var flip = Main.rand.NextFloatDirection();
        if (onCursor == 0)
        {
            sapphireVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            sapphireVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
            rubyVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            rubyVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
        }
        if (onCursor == 1)
        {
            amethystVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            amethystVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
            rubyVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            rubyVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
        }
        if (onCursor == 2)
        {
            amethystVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            amethystVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
            sapphireVelocity.X += Main.rand.NextFloat(-0.8f, 0.8f);
            sapphireVelocity.Y += Main.rand.NextFloat(-0.8f, 0.8f);
        }
        Projectile.NewProjectile(source, Projectile.position, amethystVelocity, ProjectileID.AmethystBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
        Projectile.NewProjectile(source, Projectile.position, sapphireVelocity, ProjectileID.SapphireBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
        Projectile.NewProjectile(source, Projectile.position, rubyVelocity, ProjectileID.RubyBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
        Projectile.Kill();
    }
}
