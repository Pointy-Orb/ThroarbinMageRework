using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThroarbinMageRework.Progression.GemHoods;

[AutoloadEquip(EquipType.Head)]
public class GemHood : ModItem
{
    public class Stats
    {
        public required int defense;
        public required int maxManaIncrease;
        public required float manaReducePercent;

        public required int setBonusManaIncrease;
        public required float setBonusCritChance;
        public int setBonusDebuff = -1;
    }

    public readonly Stats stats;

    public int robeId => ItemID.Search.GetId(ItemID.Search.GetName(gemId) + "Robe");
    public int staffId => ItemID.Search.GetId(ItemID.Search.GetName(gemId) + "Staff");

    public readonly int gemId;

    public override string Name => ItemID.Search.GetName(gemId) + "Hood";
    public override LocalizedText Tooltip
    {
        get
        {
            if (stats.maxManaIncrease > 0)
            {
                return Language.GetText("Mods.ThroarbinMageRework.Items.GemHood.Tooltip").WithFormatArgs(stats.maxManaIncrease, stats.manaReducePercent * 100);
            }
            else
            {
                return Language.GetText("CommonItemTooltip.PercentReducedManaCost").WithFormatArgs(stats.manaReducePercent * 100);
            }
        }
    }
    public override LocalizedText DisplayName => Language.GetText("Mods.ThroarbinMageRework.Items.GemHood.DisplayName").WithFormatArgs(Language.GetTextValue($"ItemName.{ItemID.Search.GetName(gemId)}"));

    protected override bool CloneNewInstances => true;

    public LocalizedText SetBonusText { get; private set; }

    public GemHood(int gemId, Stats stats)
    {
        this.stats = stats;
        this.gemId = gemId;
        if (stats.setBonusDebuff > -1)
        {
            SetBonusText = Language.GetText("Mods.ThroarbinMageRework.Items.GemHood.SetBonus").WithFormatArgs(
                stats.setBonusManaIncrease,
                stats.setBonusCritChance * 100,
                Language.GetTextValue($"ItemName.{ItemID.Search.GetName(gemId) + "Staff"}"),
                Language.GetTextValue($"BuffName.{BuffID.Search.GetName(stats.setBonusDebuff)}")
            );
        }
        else
        {
            SetBonusText = Language.GetText("Mods.ThroarbinMageRework.Items.GemHood.SetBonusNoDebuff").WithFormatArgs(
                stats.setBonusManaIncrease,
                stats.setBonusCritChance * 100
            );
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 12)
            .AddIngredient(gemId, 6)
            .AddTile(TileID.Loom)
            .SortBeforeFirstRecipesOf(robeId)
            .Register();
    }

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 20;

        Item.defense = stats.defense;

        Item gem = new();
        gem.SetDefaults(gemId);
        Item silk = new();
        silk.SetDefaults(ItemID.Silk);
        Item.value = (gem.value * 6) + (silk.value * 3);

        Item robe = new();
        robe.SetDefaults(robeId);
        Item.rare = robe.rare;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == robeId;
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = SetBonusText.Value;
        player.statManaMax2 += stats.setBonusManaIncrease;
        player.GetModPlayer<GemHoodPlayer>().gemSet = gemId;
    }

    public override void UpdateEquip(Player player)
    {
        player.statManaMax2 += stats.maxManaIncrease;
        player.manaCost -= stats.manaReducePercent;
    }

}

public class GemHoodPlayer : ModPlayer
{
    public bool GemHoodSetOn => gemSet != -1;

    public int gemSet = -1;

    public override void ResetEffects()
    {
        gemSet = -1;
    }

    public override void ModifyWeaponCrit(Item item, ref float crit)
    {
        if (!GemHoodSetOn)
        {
            return;
        }
        if (!(Player.armor[0].ModItem is GemHood hood))
        {
            return;
        }
        if (item.DamageType != DamageClass.Magic)
        {
            return;
        }
        crit += (hood.stats.setBonusCritChance * 100);
    }
}

public class GemHoodLoader : ILoadable
{
    public void Load(Mod mod)
    {
        mod.AddContent(new GemHood(ItemID.Amethyst, new GemHood.Stats
        {
            defense = 0,
            setBonusCritChance = 0.02f,
            setBonusManaIncrease = 20,
            manaReducePercent = 0.03f,
            maxManaIncrease = 0,
            setBonusDebuff = BuffID.OnFire
        }));
        mod.AddContent(new GemHood(ItemID.Topaz, new GemHood.Stats
        {
            defense = 0,
            setBonusCritChance = 0.02f,
            setBonusManaIncrease = 20,
            manaReducePercent = 0.05f,
            maxManaIncrease = 0,
            setBonusDebuff = BuffID.OnFire
        }));
        mod.AddContent(new GemHood(ItemID.Sapphire, new GemHood.Stats
        {
            defense = 1,
            setBonusCritChance = 0.03f,
            setBonusManaIncrease = 20,
            manaReducePercent = 0.05f,
            maxManaIncrease = 20,
            setBonusDebuff = BuffID.Poisoned
        }));
        mod.AddContent(new GemHood(ItemID.Emerald, new GemHood.Stats
        {
            defense = 1,
            setBonusCritChance = 0.04f,
            setBonusManaIncrease = 20,
            manaReducePercent = 0.07f,
            maxManaIncrease = 20,
            setBonusDebuff = BuffID.Poisoned
        }));
        mod.AddContent(new GemHood(ItemID.Ruby, new GemHood.Stats
        {
            defense = 2,
            setBonusCritChance = 0.04f,
            setBonusManaIncrease = 40,
            manaReducePercent = 0.09f,
            maxManaIncrease = 20,
            setBonusDebuff = BuffID.Frostburn
        }));
        mod.AddContent(new GemHood(ItemID.Diamond, new GemHood.Stats
        {
            defense = 3,
            setBonusCritChance = 0.05f,
            setBonusManaIncrease = 40,
            manaReducePercent = 0.13f,
            maxManaIncrease = 20,
            setBonusDebuff = BuffID.Frostburn
        }));
        mod.AddContent(new GemHood(ItemID.Amber, new GemHood.Stats
        {
            defense = 2,
            setBonusCritChance = 0.04f,
            setBonusManaIncrease = 40,
            manaReducePercent = 0.11f,
            maxManaIncrease = 20,
            setBonusDebuff = BuffID.Frostburn
        }));
    }

    public void Unload()
    {
    }
}
