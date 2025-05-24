using System.ComponentModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AbsoluteZinema
{
    internal class AbsoluteZinemaCompatibilityConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        #region compatibility options

        public enum CompatibilityOptionsValue
        {
            [Label("Off")]
            Off = 0,

            [Label("On")]
            On = 1,

            [Label("Modified")]
            Modified = 2
        }

        [Label("$Mod.AbsoluteZinema.TilePreDrawingMode.Label")]
        [DrawTicks]
        [DefaultValue(CompatibilityOptionsValue.Modified)]
        public CompatibilityOptionsValue TileSpecialDrawingMode { get; set; }


        [Label("$Mod.AbsoluteZinema.Configs.AbsoluteZinemaCompatibilityConfig.TilePreDrawingMode.Label")]
        [DrawTicks]
        [DefaultValue(CompatibilityOptionsValue.Modified)]
        public CompatibilityOptionsValue TilePreDrawingMode { get; set; }


        [Label("$Mod.AbsoluteZinema.Configs.AbsoluteZinemaCompatibilityConfig.TilePostDrawingMode.Label")]
        [DrawTicks]
        [DefaultValue(CompatibilityOptionsValue.Modified)]
        public CompatibilityOptionsValue TilePostDrawingMode { get; set; }


        [Label("$Mod.AbsoluteZinema.Configs.AbsoluteZinemaCompatibilityConfig.WallPreDrawingMode.Label")]
        [DrawTicks]
        [DefaultValue(CompatibilityOptionsValue.Modified)]
        public CompatibilityOptionsValue WallPreDrawingMode { get; set; }


        [Label("$Mod.AbsoluteZinema.Configs.AbsoluteZinemaCompatibilityConfig.WallPostDrawingMode.Label")]
        [DrawTicks]
        [DefaultValue(CompatibilityOptionsValue.Modified)]
        public CompatibilityOptionsValue WallPostDrawingMode { get; set; }

        #endregion
    }
}
