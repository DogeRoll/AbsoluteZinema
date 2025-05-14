using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameInput;
using Terraria.Initializers;
using Terraria.ModLoader;

namespace AbsoluteZinema
{
    internal class ZoomSystem : ModSystem
    {
        private static readonly AbsoluteZinemaConfig _config = ModContent.GetInstance<AbsoluteZinemaConfig>();
        private static readonly AbsoluteZinema _mod = ModContent.GetInstance<AbsoluteZinema>();

        public override void Load()
        {
            if (!_mod.IsBetterZoomLoaded)
            {
                On_Main.UpdateViewZoomKeys += On_Main_UpdateViewZoomKeys;
                IL_IngameOptions.Draw += IL_IngameOptions_Draw;
                IL_UILinksInitializer.HandleOptionsSpecials += IL_UILinkInitializer_HandleOptionSpecials;
                IL_Main.DoDraw += IL_Main_DoDraw;
            }
        }

        public static void ReloadZoom()
        {
            Main.QueueMainThreadAction(() =>
            {
                try
                {
                    if (Main.GameZoomTarget < MinZoom)
                        Main.GameZoomTarget = MinZoom;
                }
                catch (NullReferenceException ex)
                {
                }
            });
        }

        public static float MinZoom => (float)_config.MinZoom / 100f;

        private void IL_Main_DoDraw(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            c.GotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>("GameViewMatrix"),
                i => i.MatchLdsfld<Main>("ForcedMinimumZoom"),
                i => i.MatchLdsfld<Main>("GameZoomTarget"));
            c.Remove();
            c.Emit(OpCodes.Call, typeof(ZoomSystem).GetProperty("MinZoom").GetGetMethod());
        }

        private void IL_UILinkInitializer_HandleOptionSpecials(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            c.GotoNext(MoveType.Before,
                i => i.MatchLdsfld<Main>("GameZoomTarget"),
                i => i.MatchLdcR4(out _),
                i => i.MatchLdcR4(out _),
                i => i.MatchCall<PlayerInput>("get_CurrentProfile"));
            c.Index++;
            c.Remove();
            c.Emit(OpCodes.Call, typeof(ZoomSystem).GetProperty("MinZoom").GetGetMethod());
        }

        private static float Normalize(float val) => (val - MinZoom) / (2f - MinZoom);

        private static float Denormalize(float val) => val * (2f - MinZoom) + MinZoom;

        private void IL_IngameOptions_Draw(ILContext il)
        {
            ILCursor c = new ILCursor(il);


            /*
                - float num14 = DrawValueBar(sb, scale, Main.GameZoomTarget - 1f);
                + float num14 = DrawValueBar(sb, scale, Normalize(Main.GameZoomTarget));
            */
            c.GotoNext(MoveType.Before,
                i => i.MatchLdsfld<Main>("GameZoomTarget"),
                i => i.MatchLdcR4(out _),
                i => i.MatchSub(),
                i => i.MatchLdcI4(out _),
                i => i.MatchLdnull(),
                i => i.MatchCall(out _));

            c.Remove();
            c.Remove();
            c.Remove();
            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField("GameZoomTarget"));
            c.Emit(OpCodes.Call, typeof(ZoomSystem).GetMethod("Normalize", BindingFlags.NonPublic | BindingFlags.Static));

            /*
                - Main.GameZoomTarget = num14 + 1;
                + Main.GameZoomTarget = Denormalize(num14);
            */

            int newGameZoomTarget = -1;
            c.GotoNext(MoveType.Before,
                i => i.MatchLdloc(out newGameZoomTarget),
                i => i.MatchLdcR4(out _),
                i => i.MatchAdd(),
                i => i.MatchStsfld<Main>("GameZoomTarget"));
            c.Remove();
            c.Remove();
            c.Remove();
            c.Emit(OpCodes.Ldloc_S, (byte)newGameZoomTarget);
            c.Emit(OpCodes.Call, typeof(ZoomSystem).GetMethod("Denormalize", BindingFlags.NonPublic | BindingFlags.Static));
        }

        private void On_Main_UpdateViewZoomKeys(On_Main.orig_UpdateViewZoomKeys orig, Main self)
        {
            if (!Main.inFancyUI)
            {
                float num = 0.02f;
                if (PlayerInput.Triggers.Current.ViewZoomIn)
                    Main.GameZoomTarget = Utils.Clamp(Main.GameZoomTarget + num, _config.MinZoom, 2f); // MARKED

                if (PlayerInput.Triggers.Current.ViewZoomOut)
                    Main.GameZoomTarget = Utils.Clamp(Main.GameZoomTarget - num, _config.MinZoom, 2f); // MARKED
            }
        }
    }
}
