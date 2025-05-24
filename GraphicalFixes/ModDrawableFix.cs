using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;

namespace AbsoluteZinema.GraphicalFixes
{
    internal class ModDrawableFix : IGraphicalFix
    {

        private static readonly AbsoluteZinemaConfig _config = ModContent.GetInstance<AbsoluteZinemaConfig>();
        private static readonly AbsoluteZinemaCompatibilityConfig _compat_config = ModContent.GetInstance<AbsoluteZinemaCompatibilityConfig>();

        private Hook? _tilesSpecialDrawHook;
        private Hook? _tilesPreDrawHook;
        private Hook? _tilesPostDrawHook;
        private Hook? _wallsPreDrawHook;
        private Hook? _wallsPostDrawHook;

        public bool ShouldBeApplied() { return true; }

        public void Apply()
        {
            InstallHooks();
        }

        public void Remove()
        {
            RemoveHooks();
        }

        private delegate void Orig_TileLoader_SpecialDraw(int type, int specialTileX, int specialTileY, SpriteBatch spriteBatch);
        private delegate bool Orig_TileLoader_PreDraw(int i, int j, int type, SpriteBatch spriteBatch);
        private delegate void Orig_TileLoader_PostDraw(int i, int j, int type, SpriteBatch spriteBatch);
        private delegate bool Orig_WallLoader_PreDraw(int i, int j, int type, SpriteBatch spriteBatch);
        private delegate void Orig_WallLoader_PostDraw(int i, int j, int type, SpriteBatch spriteBatch);


        private static void On_TileLoader_SpecialDraw(Orig_TileLoader_SpecialDraw orig, int type, int specialTileX, int specialTileY, SpriteBatch spriteBatch)
        {
            if (_compat_config.TilePreDrawingMode == AbsoluteZinemaCompatibilityConfig.CompatibilityOptionsValue.Off)
                return;
            if (_compat_config.TilePreDrawingMode == AbsoluteZinemaCompatibilityConfig.CompatibilityOptionsValue.Modified)
            {
                int sw = Main.screenWidth, sh = Main.screenHeight, osr = Main.offScreenRange;
                var sp = Main.screenPosition;
                Main.screenWidth = EvalScaledDim(sw);
                Main.screenHeight = EvalScaledDim(sh);
                Main.offScreenRange = RenderSystem.Offset;
                Main.screenPosition = EvalScaledScreenPosition(sw, sh);
                orig(type, specialTileX, specialTileY, spriteBatch);
                Main.screenWidth = sw;
                Main.screenHeight = sh;
                Main.offScreenRange = osr;
                Main.screenPosition = sp;
                return;
            }
            orig(type, specialTileX, specialTileY, spriteBatch);
        }

        private static bool On_TileLoader_PreDraw(Orig_TileLoader_PreDraw orig, int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (_compat_config.TilePreDrawingMode == AbsoluteZinemaCompatibilityConfig.CompatibilityOptionsValue.Off)
                return true;
            if (_compat_config.TilePreDrawingMode == AbsoluteZinemaCompatibilityConfig.CompatibilityOptionsValue.Modified)
            {
                int sw = Main.screenWidth, sh = Main.screenHeight, osr = Main.offScreenRange;
                var sp = Main.screenPosition;
                Main.screenWidth = EvalScaledDim(sw);
                Main.screenHeight = EvalScaledDim(sh);
                Main.offScreenRange = RenderSystem.Offset;
                Main.screenPosition = EvalScaledScreenPosition(sw, sh);
                var result = orig(i, j, type, spriteBatch);
                Main.screenWidth = sw;
                Main.screenHeight = sh;
                Main.offScreenRange = osr;
                Main.screenPosition = sp;
                return result;
            }
            return orig(i, j, type, spriteBatch);
        }

        private static void On_TileLoader_PostDraw(Orig_TileLoader_PostDraw orig, int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (_compat_config.TilePreDrawingMode == AbsoluteZinemaCompatibilityConfig.CompatibilityOptionsValue.Off)
                return;
            if (_compat_config.TilePreDrawingMode == AbsoluteZinemaCompatibilityConfig.CompatibilityOptionsValue.Modified)
            {
                int sw = Main.screenWidth, sh = Main.screenHeight, osr = Main.offScreenRange;
                var sp = Main.screenPosition;
                Main.screenWidth = EvalScaledDim(sw);
                Main.screenHeight = EvalScaledDim(sh);
                Main.offScreenRange = RenderSystem.Offset;
                Main.screenPosition = EvalScaledScreenPosition(sw, sh);
                orig(i, j, type, spriteBatch);
                Main.screenWidth = sw;
                Main.screenHeight = sh;
                Main.offScreenRange = osr;
                Main.screenPosition = sp;
                return;
            }
            orig(i, j, type, spriteBatch);
        }

        private static bool On_WallLoader_PreDraw(Orig_WallLoader_PreDraw orig, int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (_compat_config.TilePreDrawingMode == AbsoluteZinemaCompatibilityConfig.CompatibilityOptionsValue.Off)
                return true;
            if (_compat_config.TilePreDrawingMode == AbsoluteZinemaCompatibilityConfig.CompatibilityOptionsValue.Modified)
            {
                int sw = Main.screenWidth, sh = Main.screenHeight, osr = Main.offScreenRange;
                var sp = Main.screenPosition;
                Main.screenWidth = EvalScaledDim(sw);
                Main.screenHeight = EvalScaledDim(sh);
                Main.offScreenRange = RenderSystem.Offset;
                Main.screenPosition = EvalScaledScreenPosition(sw, sh);
                var result = orig(i, j, type, spriteBatch);
                Main.screenWidth = sw;
                Main.screenHeight = sh;
                Main.offScreenRange = osr;
                Main.screenPosition = sp;
                return result;
            }
            return orig(i, j, type, spriteBatch);
        }

        private static void On_WallLoader_PostDraw(Orig_WallLoader_PostDraw orig, int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (_compat_config.TilePreDrawingMode == AbsoluteZinemaCompatibilityConfig.CompatibilityOptionsValue.Off)
                return;
            if (_compat_config.TilePreDrawingMode == AbsoluteZinemaCompatibilityConfig.CompatibilityOptionsValue.Modified)
            {
                int sw = Main.screenWidth, sh = Main.screenHeight, osr = Main.offScreenRange;
                var sp = Main.screenPosition;
                Main.screenWidth = EvalScaledDim(sw);
                Main.screenHeight = EvalScaledDim(sh);
                Main.offScreenRange = RenderSystem.Offset;
                Main.screenPosition = EvalScaledScreenPosition(sw, sh);
                orig(i, j, type, spriteBatch);
                Main.screenWidth = sw;
                Main.screenHeight = sh;
                Main.offScreenRange = osr;
                Main.screenPosition = sp;
                return;
            }
            orig(i, j, type, spriteBatch);
        }

        private void InstallHooks()
        {
            var tilesSpecialDraw = typeof(TileLoader).GetMethod("SpecialDraw",
                BindingFlags.Static | BindingFlags.Public);
            var tilesPreDraw = typeof(TileLoader).GetMethod("PreDraw",
                BindingFlags.Static | BindingFlags.Public);
            var tilesPostDraw = typeof(TileLoader).GetMethod("PostDraw",
                BindingFlags.Static | BindingFlags.Public);
            var wallsPreDraw = typeof(WallLoader).GetMethod("PreDraw",
                BindingFlags.Static | BindingFlags.Public);
            var wallsPostDraw = typeof(WallLoader).GetMethod("PostDraw",
                BindingFlags.Static | BindingFlags.Public);

            _tilesSpecialDrawHook = new Hook(tilesSpecialDraw, new Action<Orig_TileLoader_SpecialDraw, int, int, int, SpriteBatch>(On_TileLoader_SpecialDraw));
            _tilesPreDrawHook = new Hook(tilesPreDraw, new Func<Orig_TileLoader_PreDraw, int, int, int, SpriteBatch, bool>(On_TileLoader_PreDraw));
            _tilesPostDrawHook = new Hook(tilesPostDraw, new Action<Orig_TileLoader_PostDraw, int, int, int, SpriteBatch>(On_TileLoader_PostDraw));
            _wallsPreDrawHook = new Hook(wallsPreDraw, new Func<Orig_WallLoader_PreDraw, int, int, int, SpriteBatch, bool>(On_WallLoader_PreDraw));
            _wallsPostDrawHook = new Hook(wallsPostDraw, new Action<Orig_WallLoader_PostDraw, int, int, int, SpriteBatch>(On_WallLoader_PostDraw));
        }

        private void RemoveHooks()
        {
            _tilesSpecialDrawHook?.Dispose();
            _tilesPreDrawHook?.Dispose();
            _tilesPostDrawHook?.Dispose();
            _wallsPreDrawHook?.Dispose();
            _wallsPostDrawHook?.Dispose();

            _tilesSpecialDrawHook = null;
            _tilesPreDrawHook = null;
            _tilesPostDrawHook = null;
            _wallsPreDrawHook = null;
            _wallsPostDrawHook = null;

        }

        private static int EvalScaledDim(int dim) => (int)(100f / _config.MinScale * dim);

        private static Vector2 EvalScaledScreenPosition(int w, int h)
        {
            float scale = 100f / _config.MinScale - 1;
            return Main.screenPosition - new Vector2(w / 2 * scale, h / 2 * scale);
        }

    }
}
