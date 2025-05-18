
using Terraria;
using Terraria.ModLoader;

namespace AbsoluteZinema
{
	public class AbsoluteZinema : Mod
	{
        private static readonly AbsoluteZinemaConfig _config = ModContent.GetInstance<AbsoluteZinemaConfig>();

        public bool IsBetterZoomLoaded => ModLoader.HasMod("BetterZoom");

        public override void Load()
        {
            if (IsBetterZoomLoaded)
                throw new System.Exception("[AbsoluteZinema]: Mod is incompatible with better zoom now");
        }
    }
}
