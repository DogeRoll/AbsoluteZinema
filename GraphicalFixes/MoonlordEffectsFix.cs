using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.GameContent.Events;

namespace AbsoluteZinema.GraphicalFixes
{
    internal class MoonlordEffectsFix : IGraphicalFix
    {
        public void Apply()
        {
            IL_MoonlordDeathDrama.DrawWhite += IL_MoonlordDeathDrama_DrawWhite;
        }

        private void IL_MoonlordDeathDrama_DrawWhite(ILContext il)
        {
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
