using Terraria;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using System;
using Terraria.ModLoader;
using Terraria.ID;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using static Mono.Cecil.Cil.OpCodes;

namespace ThroarbinMageRework.Accessories;

[AutoloadEquip(EquipType.HandsOn)]
public class StarBand : ModItem
{
    public override void Load()
    {
        IL_NPC.NPCLoot_DropCommonLifeAndMana += NoNormalStars;
    }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 20;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 3, silver: 80);
        Item.accessory = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.BandofStarpower)
            .AddIngredient(ItemID.FallenStar, 5)
            .AddIngredient(ItemID.DemoniteBar, 5)
            .AddIngredient(ItemID.ShadowScale, 3)
            .AddTile(TileID.Anvils)
            .SortAfterFirstRecipesOf(ItemID.ManaRegenerationBand)
            .Register();

        CreateRecipe()
            .AddIngredient(ItemID.BandofStarpower)
            .AddIngredient(ItemID.FallenStar, 5)
            .AddIngredient(ItemID.CrimtaneBar, 5)
            .AddIngredient(ItemID.TissueSample, 3)
            .SortAfterFirstRecipesOf(ItemID.ManaRegenerationBand)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statManaMax2 += 20;
        player.GetModPlayer<StarBandPlayer>().starBand = true;
    }

    private static void NoNormalStars(ILContext il)
    {
        try
        {
            var c = new ILCursor(il);
            var firstLabel = il.DefineLabel();
            var secondLabel = il.DefineLabel();

            var playerArg = 1;
            c.GotoNext(i => i.MatchLdfld(typeof(Player).GetField(nameof(Player.statManaMax2))));
            c.GotoPrev(i => i.MatchLdarg(out playerArg));
            c.GotoNext(MoveType.After, i => i.MatchBge(out firstLabel));
            c.Emit(Ldarg, playerArg);
            c.EmitDelegate<Func<Player, bool>>((player) =>
            {
                return player.GetModPlayer<StarBandPlayer>().starBand;
            });
            c.Emit(Brtrue_S, firstLabel);

            c.GotoNext(i => i.MatchLdfld(typeof(Player).GetField(nameof(Player.statManaMax2))));
            c.GotoNext(MoveType.After, i => i.MatchBge(out secondLabel));
            c.Emit(Ldarg, playerArg);
            c.EmitDelegate<Func<Player, bool>>((player) =>
            {
                return player.GetModPlayer<StarBandPlayer>().starBand;
            });
            c.Emit(Brtrue_S, secondLabel);
        }
        catch
        {
            MonoModHooks.DumpIL(ModContent.GetInstance<ThroarbinMageRework>(), il);
        }
    }
}

public class StarBandPlayer : ModPlayer
{
    public bool starBand = false;

    public override void ResetEffects()
    {
        starBand = false;
    }

    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (target.immortal)
        {
            return;
        }
        if (!starBand)
        {
            return;
        }
        if (proj.DamageType != DamageClass.Magic)
        {
            return;
        }
        if (target.GetGlobalNPC<DispenseStars>().starDropCooldown > 0)
        {
            return;
        }
        if (Player.RollLuck(2) != 0)
        {
            return;
        }
        target.GetGlobalNPC<DispenseStars>().starDropCooldown = 300;
        Item.NewItem(target.GetSource_Loot(), target.position, target.width, target.height, ModContent.ItemType<StarBandStar>());
        for (int i = 0; i < target.width / 3; i++)
        {
            int manaDust = Dust.NewDust(target.position, target.width, target.height, DustID.GemDiamond, 0, 0, 50);
            Main.dust[manaDust].noGravity = true;
            Main.dust[manaDust].velocity = target.velocity;
        }
    }
}

public class StarBandStar : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Star);
        Item.width = 14;
        Item.height = 14;
        Item.alpha = 55;
    }

    public override void PostUpdate()
    {
        float num2 = (float)Main.rand.Next(90, 111) * 0.01f;
        num2 *= Main.essScale * 0.5f;
        Lighting.AddLight((int)((Item.position.X + (float)(Item.width / 2)) / 16f), (int)((Item.position.Y + (float)(Item.height / 2)) / 16f), 0.1f * num2, 0.1f * num2, 0.5f * num2);
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        var texture = TextureAssets.Item[Type];
        Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
        Vector2 drawOrigin = itemFrame.Size() / 2f;
        Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, drawOrigin.Y);
        scale = Main.essScale * 0.25f + 0.75f;
        spriteBatch.Draw(texture.Value, drawPosition, itemFrame, Color.White * ((float)(255 - Item.alpha) / 255), rotation, drawOrigin, scale, SpriteEffects.None, 0);
        return false;
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.ItemsThatShouldNotBeInInventory[Type] = true;
        ItemID.Sets.IgnoresEncumberingStone[Type] = true;
        ItemID.Sets.IsAPickup[Type] = true;
        ItemID.Sets.ItemSpawnDecaySpeed[Type] = 4;
    }

    public override bool CanPickup(Player player)
    {
        return base.CanPickup(player);
    }

    public override bool OnPickup(Player player)
    {
        SoundEngine.PlaySound(SoundID.Grab, player.position);
        player.statMana += 40;
        player.ManaEffect(40);
        if (player.statMana > player.statManaMax2)
        {
            player.statMana = player.statManaMax2;
        }
        return false;
    }

    public override void GrabRange(Player player, ref int grabRange)
    {
        if (player.manaMagnet)
        {
            grabRange += Item.manaGrabRange;
        }
    }

    public override bool GrabStyle(Player player)
    {
        if (player.manaMagnet)
        {
            PullMiniStar(player, 12f, 5);
            return true;
        }
        return false;
    }

    private void PullMiniStar(Player player, float speed, int acc)
    {
        Vector2 vector = new Vector2(Item.position.X + (float)(Item.width / 2), Item.position.Y + (float)(Item.height / 2));
        float num = player.Center.X - vector.X;
        float num2 = player.Center.Y - vector.Y;
        float num3 = (float)Math.Sqrt(num * num + num2 * num2);
        num3 = speed / num3;
        num *= num3;
        num2 *= num3;
        Item.velocity.X = (Item.velocity.X * (float)(acc - 1) + num) / (float)acc;
        Item.velocity.Y = (Item.velocity.Y * (float)(acc - 1) + num2) / (float)acc;
    }
}

public class DispenseStars : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public int starDropCooldown = 0;

    public override void PostAI(NPC npc)
    {
        if (starDropCooldown > 0)
        {
            starDropCooldown--;
        }
    }
}
