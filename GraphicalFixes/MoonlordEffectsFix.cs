using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.GameContent.Events;

namespace AbsoluteZinema.GraphicalFixes
{
    internal class MoonlordEffectsFix : IGraphicalFix
    {
        public bool ShouldBeApplied() { return true; }
        public void Apply()
        {
            IL_MoonlordDeathDrama.DrawWhite += IL_MoonlordDeathDrama_DrawWhite;
        }

        private void IL_MoonlordDeathDrama_DrawWhite(ILContext il)
        {
            /*
                - spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle(0, 0, 1, 1), color);
                + spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-Main.offScreenRange, -Main.offScreenRange, Main.screenWidth + Main.offScreenRange * 2, Main.screenHeight + Main.offScreenRange * 2), new Rectangle(0, 0, 1, 1), color);
            */
            ILCursor c = new ILCursor(il);

            c.GotoNext(MoveType.Before,
                i => i.MatchLdcI4(out _),
                i => i.MatchLdcI4(out _),
                i => i.MatchLdsfld<Main>("screenWidth"));

            c.Remove();
            c.Remove();
            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField("offScreenRange"));
            c.Emit(OpCodes.Neg);
            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField("offScreenRange"));
            c.Emit(OpCodes.Neg);

            c.Index++;
            c.Remove();
            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField("offScreenRange"));
            c.Emit(OpCodes.Ldc_I4_2);
            c.Emit(OpCodes.Mul);

            c.GotoNext(MoveType.Before,
                i => i.MatchLdsfld<Main>("screenHeight"));

            c.Index++;
            c.Remove();
            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField("offScreenRange"));
            c.Emit(OpCodes.Ldc_I4_2);
            c.Emit(OpCodes.Mul);
        }
    }
}
