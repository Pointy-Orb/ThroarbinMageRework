using Terraria;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Accessories;

public class ManaField : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 50;
        Item.height = 40;
        Item.value = Item.sellPrice(gold: 6);
        Item.rare = ItemRarityID.LightRed;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<ManaFieldPlayer>().manaFieldEquipped = true;
    }

    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((ModContent.GetInstance<Configs.ServerConfig>().AccessoryBalance == Configs.AccessoryBalance.Faithful ? 10 : 25));
}

public class ManaFieldPlayer : ModPlayer
{
    public bool manaFieldEquipped = false;

    public int oldStatMana { get; private set; } = 0;

    public override void ResetEffects()
    {
        manaFieldEquipped = false;
    }

    public override bool CanUseItem(Item item)
    {
        if (item.mana > 0)
        {
            oldStatMana = Player.statMana;
        }
        return true;
    }
}

public class ManaFieldDrop : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type != NPCID.EnchantedSword)
        {
            return;
        }
        npcLoot.Add(ItemDropRule.ExpertGetsRerolls(ModContent.ItemType<ManaField>(), 50, 1));
    }
}


public class ManaFieldEffect : GlobalProjectile
{
    public float backPercent => ModContent.GetInstance<Configs.ServerConfig>().AccessoryBalance == Configs.AccessoryBalance.Faithful ? 0.1f : 0.25f;

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (target.immortal)
        {
            return;
        }
        if (hit.DamageType != DamageClass.Magic)
        {
            return;
        }
        if (projectile.owner < 0)
        {
            return;
        }
        var player = Main.player[projectile.owner];
        var manaFieldPlayer = player.GetModPlayer<ManaFieldPlayer>();
        if (!manaFieldPlayer.manaFieldEquipped)
        {
            return;
        }
        if (player.Center.Distance(target.Center) > 320)
        {
            return;
        }
        var manaUsed = manaFieldPlayer.oldStatMana - player.statMana;
        if (manaUsed <= 0)
        {
            return;
        }
        var giveBackPercent = Int32.Clamp((int)((float)manaUsed * backPercent), 1, Int32.MaxValue);
        player.statMana += giveBackPercent;
        if (projectile.owner == Main.myPlayer)
        {
            player.ManaEffect(giveBackPercent);
        }
        for (int i = 0; i < target.width / 6; i++)
        {
            int manaDust = Dust.NewDust(target.position, target.width, target.height, DustID.GemSapphire, 0, 0, 50);
            Main.dust[manaDust].noGravity = true;
            Main.dust[manaDust].velocity = target.velocity;
        }
    }
}
