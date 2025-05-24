using System;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using AbsoluteZinema.GraphicalFixes;
using Terraria.ID;

namespace AbsoluteZinema
{

    internal class RenderSystem : ModSystem
    {
        private static readonly AbsoluteZinemaConfig _config = ModContent.GetInstance<AbsoluteZinemaConfig>();

        private static readonly int _offset = 192;

        public static int Offset => _offset;

        public override void Load()
        {
            /* NOTE: In future this IL's should be applied as graphical fix */
            On_Main.GetScreenOverdrawOffset += On_Main_GetScreenOverdrawOffset;
            IL_Main.InitTargets_int_int += IL_Main_InitTargets;
            IL_Main.DrawBlack += IL_Main_DrawBlack;
            GraphicalFixManager.ApplyAllFixes();
            ReloadRenderTargets();
        }

        public override void Unload()
        {
            On_Main.GetScreenOverdrawOffset -= On_Main_GetScreenOverdrawOffset;
            IL_Main.InitTargets_int_int -= IL_Main_InitTargets;
            IL_Main.DrawBlack -= IL_Main_DrawBlack;
            GraphicalFixManager.Clear();
            ReloadRenderTargets();
        }

        public static void ReloadRenderTargets()
        {
            /* SetResolution() call InitTargets() function, that sets off screen drawing area and renderers */
            Main.QueueMainThreadAction(() =>
            {
                var initTargets = typeof(Main).GetMethod("InitTargets", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { }, null);
                var isBusy = typeof(Main).GetField("_isResizingAndRemakingTargets", BindingFlags.Static | BindingFlags.NonPublic);
                try
                {
                    if (!(bool)isBusy.GetValue(null))
                    {
                        isBusy.SetValue(null, true);
                        initTargets.Invoke(Main.instance, null);
                        isBusy.SetValue(null, false);
                    }
                }
                catch (NullReferenceException ex)
                {
                    isBusy.SetValue(null, false);
                    Console.WriteLine($"{ex.ToString()}: This should only happen on server initialization.");
                }
            });
        }

        private void IL_Main_DrawBlack(ILContext il)
        {
            ILCursor c = new ILCursor(il);


            /* 
                - num3 = point.X;
                + num3 = 0;
            */
            int idx = -1;
            c.GotoNext(MoveType.Before,
                i => i.MatchLdloc(out _),
                i => i.MatchLdfld(typeof(Point).GetField("X")),
                i => i.MatchStloc(out idx));


            c.Remove();
            c.Remove();
            c.Remove();
            c.Emit(OpCodes.Ldc_I4_0);
            c.Emit(OpCodes.Stloc_S, (byte)idx);

            /* 
                - num4 = Main.maxTilesX - point.X;
                + num4 = Main.maxTilesX;
            */
            c.GotoNext(MoveType.Before,
                i => i.MatchLdsfld(out _),
                i => i.MatchLdloc(out _),
                i => i.MatchLdfld(typeof(Point).GetField("X")),
                i => i.MatchSub(),
                i => i.MatchStloc(out _));
            c.Index++;
            c.Remove();
            c.Remove();
            c.Remove();

            /* 
                - num5 = point.Y;
                + num5 = 0;
            */
            c.GotoNext(MoveType.Before,
                i => i.MatchLdloc(out _),
                i => i.MatchLdfld(typeof(Point).GetField("Y")),
                i => i.MatchStloc(out idx));

            c.Remove();
            c.Remove();
            c.Remove();
            c.Emit(OpCodes.Ldc_I4_0);
            c.Emit(OpCodes.Stloc_S, (byte)idx);

            /* 
                - num6 = Main.maxTilesY - point.Y;
                + num6 = Main.maxTilesY;
            */
            c.GotoNext(MoveType.Before,
                i => i.MatchLdsfld(out _),
                i => i.MatchLdloc(out _),
                i => i.MatchLdfld(typeof(Point).GetField("Y")),
                i => i.MatchSub(),
                i => i.MatchStloc(out _));
            c.Index++;
            c.Remove();
            c.Remove();
            c.Remove();

        }

        private static int EvalOffset(int dim) => (int)((float)dim * (1.0f / ((float)_config.MinScale / 100f) - 1.0f) / 2);

        private Point On_Main_GetScreenOverdrawOffset(On_Main.orig_GetScreenOverdrawOffset orig)
        {
            return new Point(0, 0);
        }

        private void IL_Main_InitTargets(ILContext il)
        {
            /* 

                ReleaseTargets();
                offScreenRange = 192 

                + _renderTargetMaxSize = maxScreenW * 3 + 400 * Main.maxScreenW / 1920;
		        + offScreenRange = 192 + EvalOffset;

                if (width + offScreenRange * 2 > _renderTargetMaxSize)
			        offScreenRange = (_renderTargetMaxSize - width) / 2;
            
             */

            ILCursor c = new ILCursor(il);

            c.GotoNext(MoveType.After,
                i => i.MatchStsfld<Main>("offScreenRange"));


            MethodInfo evalOffset = typeof(RenderSystem).GetMethod("EvalOffset", BindingFlags.NonPublic | BindingFlags.Static);
            // maxScreenW * 2
            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField("maxScreenW"));
            c.Emit(OpCodes.Ldc_I4_3);
            c.Emit(OpCodes.Mul);
            // + ((400 * maxScreenW) / 1920)
            c.Emit(OpCodes.Ldc_I4, 400);
            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField("maxScreenW"));
            c.Emit(OpCodes.Mul);
            c.Emit(OpCodes.Ldc_I4, 1920);
            c.Emit(OpCodes.Div);
            c.Emit(OpCodes.Add);
            // _renderTargetMaxSize = result
            c.Emit(OpCodes.Stsfld, typeof(Main).GetField("_renderTargetMaxSize", BindingFlags.NonPublic | BindingFlags.Static));
            // offScreenRange = 192 + evalOffset
            c.Emit(OpCodes.Ldc_I4, _offset);
            c.Emit(OpCodes.Ldarg_1);
            c.Emit(OpCodes.Call, evalOffset);
            c.Emit(OpCodes.Add);
            c.Emit(OpCodes.Stsfld, typeof(Main).GetField("offScreenRange"));

        }

    }

}
