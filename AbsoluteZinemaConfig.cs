

using System.ComponentModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AbsoluteZinema
{
    internal sealed class AbsoluteZinemaConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnChanged()
        {
            PreConfigSave();
            RenderSystem.ReloadRenderTargets();
            ZoomSystem.ReloadZoom();
        }

        #region zoom
        private const int rangeMin = 40;
        private const int rangeDefault = 70;

        private void PreConfigSave()
        {
            if (SameAsZoom)
                MinScale = MinZoom;
        }

        [Slider]
        [Range(rangeMin, 100)]
        [Label("$Mod.AbsoluteZinema.Configs.AbsoluteZinemaConfig.MinZoom.Label")]
        [DefaultValue(rangeDefault)]
        public int MinZoom { get; set; }

        [Slider]
        [Range(rangeMin, 100)]
        [Label("$Mod.AbsoluteZinema.Configs.AbsoluteZinemaConfig.MinScale.Label")]
        [DefaultValue(rangeDefault)]
        public int MinScale { get; set; }

        [Label("$Mod.AbsoluteZinema.Configs.AbsoluteZinemaConfig.SameAsZoom.Label")]
        [DefaultValue(true)]
        public bool SameAsZoom { get; set; }

        #endregion

    }
}
