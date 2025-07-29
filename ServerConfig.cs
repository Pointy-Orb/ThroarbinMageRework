

using System.ComponentModel;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Terraria.ModLoader.Config;

namespace ThroarbinMageRework.Configs;

public enum AccessoryBalance
{
    Tweaked,
    Faithful
}

public class ServerConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DefaultValue(AccessoryBalance.Tweaked)]
    [ReloadRequired]
    public AccessoryBalance AccessoryBalance { get; set; }
}
