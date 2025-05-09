

using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AbsoluteZinema
{
    internal sealed class AbsoluteZinemaConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnChanged()
        {
            RenderSystem.ReloadRenderTargets();
        }

        [Slider]
        [Range(0.4f, 1.0f)]
        [DefaultValue(0.7f)]
        public float MinScale;
    }
}
