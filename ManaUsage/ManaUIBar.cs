using Terraria;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using ReLogic.Content;

namespace ThroarbinMageRework.ManaUsage;

public class ManaUIBar : UIElement
{
    public static Asset<Texture2D> texture;

    public readonly Rectangle frameRect = new(0, 0, 58, 22);
    public readonly Rectangle barRect = new(20, 48, 34, 22);
    public readonly Rectangle backRect = new(0, 24, 58, 22);
    public readonly Rectangle tipRect = new(60, 0, 4, 22);

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!ModContent.GetInstance<Configs.ClientConfig>().ShowMiniManaBar)
        {
            return;
        }
        if (Main.LocalPlayer.statMana >= Main.LocalPlayer.statManaMax2)
        {
            return;
        }
        if (Main.LocalPlayer.dead)
        {
            return;
        }
        var position = Utils.ToScreenPosition(Main.LocalPlayer.Top) / Main.UIScale;
        position.Y += (float)(15 * Main.GameZoomTarget / Main.UIScale) * ((float)Main.screenWidth / 2348f);
        position.X += 95f * ((float)Main.screenWidth / 2348f);
        spriteBatch.Draw(texture.Value, position, backRect, Color.White, 0f, new Vector2(backRect.Width / 2, backRect.Height / 2), 1f, SpriteEffects.None, 0f);
        var barPosition = position;
        barPosition.X += 8;
        var barFill = barRect;
        barFill.Width = (int)Utils.Remap((float)Main.LocalPlayer.statMana, 0, (float)Main.LocalPlayer.statManaMax2, 0, (float)barRect.Width);
        spriteBatch.Draw(texture.Value, barPosition, barFill, Color.White, 0f, new Vector2(barRect.Width / 2, barRect.Height / 2), 1f, SpriteEffects.None, 0f);
        if (Main.LocalPlayer.statMana > 0)
        {
            spriteBatch.Draw(texture.Value, new Vector2(position.X + barFill.Width - 10, barPosition.Y), tipRect, Color.White, 0f, new Vector2(tipRect.Width / 2, tipRect.Height / 2), 1f, SpriteEffects.None, 0f);
        }
        spriteBatch.Draw(texture.Value, position, frameRect, Color.White, 0f, new Vector2(frameRect.Width / 2, frameRect.Height / 2), 1f, SpriteEffects.None, 0f);
    }
}

public class ManaDisplay : UIState
{
    public ManaUIBar bar;

    public override void OnInitialize()
    {
        bar = new();

        Append(bar);
    }
}

[Autoload(Side = ModSide.Client)]
public class ManaUIBarSystem : ModSystem
{
    internal ManaDisplay manaDisplay;

    private UserInterface _manaDisplay;

    public override void Load()
    {
        ManaUIBar.texture = ModContent.Request<Texture2D>("ThroarbinMageRework/ManaUsage/MiniManaBar");
        manaDisplay = new();
        _manaDisplay = new();
        _manaDisplay.SetState(manaDisplay);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _manaDisplay?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "ThroarbinMageRework: Mini Mana Bar",
                delegate
                {
                    _manaDisplay.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}
