using System;
using System.Numerics;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace AbsoluteZinema.GraphicalFixes
{
    internal class RendererResolutionFix : IGraphicalFix
    {
        private static readonly AbsoluteZinemaConfig _config = ModContent.GetInstance<AbsoluteZinemaConfig>();

        public bool ShouldBeApplied() { return true; }

        public void Apply()
        {
            IL_HitTile.DrawFreshAnimations += IL_HitTile_DrawFreshAnimations;
            IL_Main.DrawSmartInteract += IL_Main_DrawSmartInteract;
            IL_Main.DrawSmartCursor += IL_Main_DrawSmartCursor;
            IL_Main.DrawInterface_6_TileGridOption += IL_Main_TileGridOption;

            IL_Main.DoDraw += IL_Main_DoDraw;
            IL_Main.DrawTileCracks += IL_Main_DrawTileCracks;
            IL_Main.DrawBlack += IL_Main_DrawBlack;
            IL_Main.oldDrawWater += IL_Main_oldDrawWater;
            IL_Main.DrawLiquid += IL_Main_DrawLiquids;
            IL_Main.DrawWaters += IL_Main_DrawWaters;
            IL_Main.OldDrawBackground += IL_Main_OldDrawBackground;
            IL_Main.DrawBackground += IL_Main_DrawBackground;
            IL_ScreenShaderData.Apply += IL_ScreenShaderData_Apply;
            IL_WaterShaderData.Apply += IL_WaterShaderData_Apply;
            IL_WaterShaderData.DrawWaves += IL_WaterShaderData_DrawWaves;
            IL_WaterShaderData.StepLiquids += IL_WaterShaderData_StepLiquids;
            IL_TileDrawing.DrawLiquidBehindTiles += IL_TileDrawing_DrawLiquidsBehindTiles;
            IL_TileDrawing.Draw += IL_TileDrawing_Draw;
            IL_Main.DrawCapture += IL_Main_DrawCapture;
            IL_WallDrawing.DrawWalls += IL_WallDrawing_DrawWalls;
            IL_TileDrawing.EnsureWindGridSize += IL_TileDrawing_EnsureWindGridSize;
            IL_TileDrawing.PreparePaintForTilesOnScreen += IL_TileDrawing_PreparePaintForTileOnScreen;
            IL_Main.InitTargets_int_int += IL_Main_InitTargets;
        }

        public void Remove()
        {
            IL_HitTile.DrawFreshAnimations -= IL_HitTile_DrawFreshAnimations;
            IL_Main.DrawSmartInteract -= IL_Main_DrawSmartInteract;
            IL_Main.DrawSmartCursor -= IL_Main_DrawSmartCursor;
            IL_Main.DrawInterface_6_TileGridOption -= IL_Main_TileGridOption;

            IL_Main.DoDraw -= IL_Main_DoDraw;
            IL_Main.DrawTileCracks -= IL_Main_DrawTileCracks;
            IL_Main.DrawBlack -= IL_Main_DrawBlack;
            IL_Main.oldDrawWater -= IL_Main_oldDrawWater;
            IL_Main.DrawLiquid -= IL_Main_DrawLiquids;
            IL_Main.DrawWaters -= IL_Main_DrawWaters;
            IL_Main.OldDrawBackground -= IL_Main_OldDrawBackground;
            IL_Main.DrawBackground -= IL_Main_DrawBackground;
            IL_ScreenShaderData.Apply -= IL_ScreenShaderData_Apply;
            IL_WaterShaderData.Apply -= IL_WaterShaderData_Apply;
            IL_WaterShaderData.DrawWaves -= IL_WaterShaderData_DrawWaves;
            IL_WaterShaderData.StepLiquids -= IL_WaterShaderData_StepLiquids;
            IL_TileDrawing.DrawLiquidBehindTiles -= IL_TileDrawing_DrawLiquidsBehindTiles;
            IL_TileDrawing.Draw -= IL_TileDrawing_Draw;
            IL_Main.DrawCapture -= IL_Main_DrawCapture;
            IL_WallDrawing.DrawWalls -= IL_WallDrawing_DrawWalls;
            IL_TileDrawing.EnsureWindGridSize -= IL_TileDrawing_EnsureWindGridSize;
            IL_TileDrawing.PreparePaintForTilesOnScreen -= IL_TileDrawing_PreparePaintForTileOnScreen;
            IL_Main.InitTargets_int_int -= IL_Main_InitTargets;
        }

        private void IL_HitTile_DrawFreshAnimations(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            FieldInfo offScreenRange = typeof(Main).GetField("offScreenRange", BindingFlags.Public | BindingFlags.Static);
            ConstructorInfo vector2Ctor = typeof(Vector2).GetConstructor(new[] { typeof(float), typeof(float) });
            MethodInfo evalOffsetSub = typeof(RendererResolutionFix).GetMethod("EvalOffsetSubstraction", BindingFlags.Static | BindingFlags.NonPublic);

            c.GotoNext(MoveType.After,
                i => i.MatchLdsfld(offScreenRange),
                i => i.MatchConvR4(),
                i => i.MatchCall(out _));

            c.Index--;
            c.Remove();
            c.Emit(OpCodes.Ldsfld, offScreenRange);
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Sub);
            c.Emit(OpCodes.Conv_R4);
            c.Emit(OpCodes.Call, vector2Ctor);
        }

        private void IL_Main_DrawSmartInteract(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_Main_DrawSmartCursor(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_Main_TileGridOption(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_Main_DoDraw(ILContext il)
        {
            var c = new ILCursor(il);
            for (int i = 0; i < 15; i++)
            {
                PatchDoDrawNext(c);
            }

            var c_ = new ILCursor(il);
            for (int i = 0; i < 5; i++)
                ConditionReplacementCommon(c_);

            c.GotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchLdcI4(out _),
                i => i.MatchMul(),
                i => i.MatchConvR4(),
                i => i.MatchAdd(),
                i => i.MatchLdsfld<Main>("Camera"),
                i => i.MatchCallvirt(out _),
                i => i.MatchLdfld(out _),
                i => i.MatchLdsfld<Main>("offScreenRange"));
            MethodInfo evalOffsetSub = typeof(RendererResolutionFix).GetMethod("EvalOffsetSubstraction", BindingFlags.Static | BindingFlags.NonPublic);
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Sub);
        }

        private void IL_Main_DrawTileCracks(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_Main_DrawBlack(ILContext il)
        {
            VectorReplacerCommon(il);
            var c = new ILCursor(il);
            c.GotoNext(MoveType.Before,
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchNeg(),
                i => i.MatchLdcI4(out _),
                i => i.MatchDiv());
            MethodInfo evalOffsetSub = typeof(RendererResolutionFix).GetMethod("EvalOffsetSubstraction", BindingFlags.Static | BindingFlags.NonPublic);
            c.Index += 2;
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Add);
        }

        private void IL_Main_oldDrawWater(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_Main_DrawLiquids(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_Main_DrawWaters(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_Main_OldDrawBackground(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_Main_DrawBackground(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_ScreenShaderData_Apply(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_WaterShaderData_Apply(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchConvR4(),
                i => i.MatchLdsfld<Main>("offScreenRange"));
            MethodInfo evalOffsetSub = typeof(RendererResolutionFix).GetMethod("EvalOffsetSubstraction", BindingFlags.Static | BindingFlags.NonPublic);
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Sub);
            c.GotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchConvR4(),
                i => i.MatchLdsfld<Main>("offScreenRange"));
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Sub);
        }

        private void IL_WaterShaderData_DrawWaves(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_WaterShaderData_StepLiquids(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_TileDrawing_DrawLiquidsBehindTiles(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_TileDrawing_Draw(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_Main_DrawCapture(ILContext il)
        {
            VectorReplacerCommon(il);
            var c = new ILCursor(il);
            c.GotoNext(MoveType.Before,
                i => i.MatchLdfld(out _),
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchLdcI4(out _),
                i => i.MatchMul(),
                i => i.MatchConvR4(),
                i => i.MatchAdd(),
                i => i.MatchNewobj(out _));
            MethodInfo evalOffsetSub = typeof(RendererResolutionFix).GetMethod("EvalOffsetSubstraction", BindingFlags.Static | BindingFlags.NonPublic);
            c.Index += 2;
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Sub);
        }

        private void IL_WallDrawing_DrawWalls(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext(MoveType.After,
                i => i.MatchLdloc1(),
                i => i.MatchConvR4(),
                i => i.MatchLdloc1());
            MethodInfo evalOffsetSub = typeof(RendererResolutionFix).GetMethod("EvalOffsetSubstraction", BindingFlags.Static | BindingFlags.NonPublic);
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Sub);

            c.GotoNext(MoveType.After,
                i => i.MatchLdloc1(),
                i => i.MatchLdcI4(out _),
                i => i.MatchDiv(),
                i => i.MatchStloc(out _),
                i => i.MatchLdloc1(),
                i => i.MatchLdcI4(out _),
                i => i.MatchDiv(), 
                i => i.MatchStloc(out _));
            c.Index -= 3;
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Sub);
        }

        private void IL_TileDrawing_EnsureWindGridSize(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_TileDrawing_PreparePaintForTileOnScreen(ILContext il)
        {
            VectorReplacerCommon(il);
        }

        private void IL_Main_InitTargets(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            c.GotoNext(MoveType.After,
                i => i.MatchLdarg2(),
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchLdcI4(out _),
                i => i.MatchMul(),
                i => i.MatchAdd(),
                i => i.MatchStarg(out _));

            MethodInfo evalOffsetSub = typeof(RendererResolutionFix).GetMethod("EvalOffsetSubstraction", BindingFlags.Static | BindingFlags.NonPublic);
            c.Index--;
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Ldc_I4_2);
            c.Emit(OpCodes.Mul);
            c.Emit(OpCodes.Sub);
        }

        FieldInfo yField = typeof(Vector2).GetField("Y", BindingFlags.Public | BindingFlags.Instance);
        private static void PatchDoDrawNext(ILCursor c)
        {
            c.GotoNext(MoveType.After,
                i => i.MatchLdsflda<Main>("screenPosition"),
                i => i.MatchLdfld(out _),
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchConvR4(),
                i => i.MatchSub(),
                i => i.MatchStfld(out _),
                i => i.MatchLdsflda(out _),
                i => i.MatchLdsflda<Main>("screenPosition"),
                i => i.MatchLdfld(out _),
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchConvR4(),
                i => i.MatchSub(),
                i => i.MatchStfld(out _));

            MethodInfo evalOffsetSub = typeof(RendererResolutionFix).GetMethod("EvalOffsetSubstraction", BindingFlags.Static | BindingFlags.NonPublic);
            c.Index--;
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Conv_R4);
            c.Emit(OpCodes.Add);
        }

        
        /// <summary>
        /// There are few places where IL blocks has similar signatures
        /// so this function replaces vector2(offset, offset) to 
        /// vector2(offset, offset-substract)
        /// </summary>
        /// <param name="il"></param>
        /// <returns></returns>
        private static void VectorReplacerCommon(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchConvR4(),
                i => i.MatchLdsfld<Main>("offScreenRange"));
            MethodInfo evalOffsetSub = typeof(RendererResolutionFix).GetMethod("EvalOffsetSubstraction", BindingFlags.Static | BindingFlags.NonPublic);
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Sub);
        }

        private static void ConditionReplacementCommon(ILCursor c)
        {
            c.GotoNext(MoveType.Before,
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchConvR4(),
                i => i.MatchSub(),
                i => i.MatchSub(),
                i => i.MatchCall(out _),
                i => i.MatchLdsfld<Main>("offScreenRange"),
                i => i.MatchConvR4(),
                i => i.MatchBleUn(out _),
                i => i.MatchLdarg(out _),
                i => i.MatchCall(out _));
            MethodInfo evalOffsetSub = typeof(RendererResolutionFix).GetMethod("EvalOffsetSubstraction", BindingFlags.Static | BindingFlags.NonPublic);
            c.Index++;
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Sub);
            c.Index += 5;
            c.Emit(OpCodes.Call, evalOffsetSub);
            c.Emit(OpCodes.Sub);
        }

        private static int EvalOffset(int dim) => (int)((float)dim * (100f / (float)_config.MinScale - 1.0f) / 2);

        /// <summary>
        ///  Calcs how many pixels should be substracted from scaled
        ///  WIDTHxWIDTH square to fit scaled WIDTHxHEIGHT rectangle.
        /// </summary>
        /// <returns>Count of pixels to substract</returns>
        private static int EvalOffsetSubstraction()
        {
            return EvalOffset(Main.screenWidth - Main.screenHeight);
        } 
    }
}
