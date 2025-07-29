using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Progression.GemHoods;

public class StaffDebuffs : GlobalProjectile
{
    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (projectile.owner < 0)
        {
            return;
        }
        var player = Main.player[projectile.owner];
        var hoodPlayer = player.GetModPlayer<GemHoodPlayer>();
        if (hoodPlayer.gemSet < 0)
        {
            return;
        }
        if (!Mod.TryFind<ModItem>(ItemID.Search.GetName(hoodPlayer.gemSet) + "Hood", out var hoodItem))
        {
            return;
        }
        if (!(hoodItem is GemHood hood))
        {
            return;
        }
        var staff = new Item();
        staff.SetDefaults(hood.staffId);
        if (projectile.type != staff.shoot)
        {
            return;
        }
        target.AddBuff(hood.stats.setBonusDebuff, Main.rand.Next(60, 240));
    }
}
