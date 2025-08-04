using Terraria;
using System;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;

namespace ThroarbinMageRework.Accessories;

[AutoloadEquip(EquipType.Front, EquipType.Back)]
public class MagicCloak : ModItem
{
    public const float buffValue = 0.15f;
    public const float buffMinimum = 0.02f;

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 36;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 1);
        Item.accessory = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 12)
            .AddIngredient(ItemID.FallenStar, 15)
            .AddTile(TileID.Loom)
            .Register();

        Recipe.Create(ItemID.ManaCloak)
            .AddIngredient(this)
            .AddIngredient(ItemID.StarCloak)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override LocalizedText Tooltip
    {
        get
        {
            var faithful = ModContent.GetInstance<Configs.ServerConfig>().AccessoryBalance == Configs.AccessoryBalance.Faithful;
            var complicatedTooltips = ModContent.GetInstance<Configs.ClientConfig>().ComplicatedTooltips && faithful;
            var key = complicatedTooltips ? "TooltipComplicated" : "TooltipSimple";
            if (complicatedTooltips)
            {
                return this.GetLocalization(key);
            }
            return this.GetLocalization(key).WithFormatArgs((faithful ? 5 : MathF.Round(buffValue * 100f, 2)), (faithful ? 1 : buffMinimum * 100));
        }
    }

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        return incomingItem.type != ItemID.ManaCloak;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if ((player.GetManaCost(player.HeldItem) <= 0 && player.HeldItem.DamageType == DamageClass.Magic) || ItemID.Sets.IsSpaceGun[player.HeldItem.type])
        {
            return;
        }
        if (ModContent.GetInstance<Configs.ServerConfig>().AccessoryBalance == Configs.AccessoryBalance.Faithful)
        {
            if (player.statMana >= (int)((float)player.statManaMax2 * 0.75f))
            {
                player.GetDamage(DamageClass.Magic) += 0.05f;
            }
            else if (player.statMana >= (int)((float)player.statManaMax2 * 0.50f))
            {
                player.GetDamage(DamageClass.Magic) += 0.03f;
            }
            else
            {
                player.GetDamage(DamageClass.Magic) += 0.01f;
            }
        }
        else
        {
            var bonus = Utils.Remap(player.statMana, player.statManaMax2 / 2, player.statManaMax2, buffMinimum, buffValue + (ManaCloakRework.updatingCloakUpgrade ? ManaCloakRework.cloakUpgradeDamageBonus : 0f));
            if (player.statMana < player.statManaMax2 / 2)
            {
                bonus = buffMinimum;
            }
            player.GetDamage(DamageClass.Magic) += bonus;
        }
    }
}

public class ManaCloakRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        foreach (Recipe recipe in Main.recipe)
        {
            if (recipe.HasResult(ItemID.ManaCloak) && recipe.Mod == null)
            {
                recipe.DisableRecipe();
            }
        }
    }
}

public class ManaCloakRework : GlobalItem
{
    public static bool updatingCloakUpgrade = false;

    public const float cloakUpgradeDamageBonus = 0.05f;

    public override void Load()
    {
        On_Player.ApplyEquipFunctional += UndoManaFlowerCloak;
    }

    private static void UndoManaFlowerCloak(On_Player.orig_ApplyEquipFunctional orig, Player self, Item currentItem, bool hideVisual)
    {
        bool wasManaFlower = self.manaFlower;
        orig(self, currentItem, hideVisual);
        if (currentItem.type == ItemID.ManaCloak && self.manaFlower && !wasManaFlower)
        {
            self.manaFlower = false;
        }
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (item.type != ItemID.ManaCloak)
        {
            return;
        }
        bool addedOtherTooltip = false;
        int placementIndex = 0;
        for (int i = 0; i < tooltips.Count; i++)
        {
            var line = tooltips[i];
            if (Language.GetTextValue("ItemTooltip.ManaFlower").Contains(line.Text))
            {
                line.Hide();
                if (!addedOtherTooltip)
                {
                    placementIndex = i;
                    addedOtherTooltip = true;
                }
            }
        }
        tooltips.Insert(placementIndex, new(Mod, "MagicCloakTip", Language.GetTextValue("Mods.ThroarbinMageRework.Items.MagicCloak." + (ModContent.GetInstance<Configs.ClientConfig>().ComplicatedTooltips ? "TooltipComplicated" : "TooltipSimple"), (MagicCloak.buffValue + cloakUpgradeDamageBonus) * 100f, MagicCloak.buffMinimum * 100f)));
    }

    public override void UpdateAccessory(Item item, Player player, bool hideVisual)
    {
        if (item.type != ItemID.ManaCloak)
        {
            return;
        }
        updatingCloakUpgrade = true;
        ModContent.GetModItem(ModContent.ItemType<MagicCloak>()).UpdateAccessory(player, hideVisual);
        updatingCloakUpgrade = false;
    }
}
