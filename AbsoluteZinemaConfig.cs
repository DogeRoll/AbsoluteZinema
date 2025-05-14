

using System.ComponentModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AbsoluteZinema
{
    internal sealed class AbsoluteZinemaConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ClientSide;
        private static readonly AbsoluteZinema _mod = ModContent.GetInstance<AbsoluteZinema>();

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
            if (_mod.IsBetterZoomLoaded)
            {
                MinZoom = 100;
                SameAsZoom = false;
                return;
            }
            if (SameAsZoom)
                MinScale = MinZoom;
        }

        [Header("$Mod.AbsoluteZinema.ZoomHeader")]

        [Slider]
        [Range(rangeMin, 100)]
        [Label("$Mod.AbsoluteZinema.MinZoom.Label")]
        [DefaultValue(rangeDefault)]
        public int MinZoom { get; set; }

        [Slider]
        [Range(rangeMin, 100)]
        [Label("$Mod.AbsoluteZinema.MinScale.Label")]
        [DefaultValue(rangeDefault)]
        public int MinScale { get; set; }

        [Label("$Mod.AbsoluteZinema.SameAsZoom.Label")]
        [DefaultValue(true)]
        public bool SameAsZoom { get; set; }

        #endregion

    }
}
