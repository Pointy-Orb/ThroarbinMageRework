using Terraria;
using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;

namespace ThroarbinMageRework.Progression;

public class DemonScytheChest : ModSystem
{
    public override void Load()
    {
        WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Reset"], ReplaceFlowerDetour);
    }

    void ReplaceFlowerDetour(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
    {
        orig(self, progress, configuration);
        for (int i = 0; i < GenVars.hellChestItem.Length; i++)
        {
            if (GenVars.hellChestItem[i] == ItemID.FlowerofFire)
            {
                GenVars.hellChestItem[i] = ItemID.DemonScythe;
            }
            if (GenVars.hellChestItem[i] == ItemID.UnholyTrident)
            {
                GenVars.hellChestItem[i] = ItemID.DemonScythe;
            }
        }
    }
}

public class DropFireFlower : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        foreach (IItemDropRule rule in npcLoot.Get())
        {
            if (rule is CommonDrop drop && drop.itemId == ItemID.DemonScythe)
            {
                var remixDrop = ItemDropRule.ByCondition(new Terraria.GameContent.ItemDropRules.Conditions.RemixSeed(), ItemID.UnholyTrident, drop.chanceDenominator, drop.amountDroppedMinimum, drop.amountDroppedMaximum, drop.chanceNumerator);
                var normalDrop = ItemDropRule.ByCondition(new Terraria.GameContent.ItemDropRules.Conditions.NotRemixSeed(), ItemID.FlowerofFire, drop.chanceDenominator, drop.amountDroppedMinimum, drop.amountDroppedMaximum, drop.chanceNumerator);
                npcLoot.Add(remixDrop);
                npcLoot.Add(normalDrop);
                npcLoot.Remove(drop);
            }
        }
    }
}

public class NerfFireFlower : GlobalItem
{
    public override void SetDefaults(Item entity)
    {
        FlowerDefaults(entity);
        TridentDefaults(entity);
    }

    private void TridentDefaults(Item entity)
    {
        if (entity.type != ItemID.UnholyTrident)
        {
            return;
        }
        if (!Main.remixWorld)
        {
            return;
        }
        entity.damage = 24;
        entity.useTime = 24;
        entity.useAnimation = 24;
    }

    private void FlowerDefaults(Item entity)
    {
        if (entity.type != ItemID.FlowerofFire)
        {
            return;
        }
        if (Main.remixWorld)
        {
            return;
        }
        entity.damage = 28;
        entity.useTime = 24;
        entity.useAnimation = 24;
    }
}
