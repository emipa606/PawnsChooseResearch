using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace PawnsChooseResearch;

[HarmonyPatch]
public class Patch_ResearchInspect
{
    [HarmonyPatch(typeof(JobDriver), "GetReport")]
    [HarmonyPostfix]
    public static void Postfix(ref string __result, Pawn ___pawn)
    {
        if (__result != JobDefOf.Research.reportString)
        {
            return;
        }

        var researchProjectDef = Find.ResearchManager.currentProj;
        if (!Mod_PawnsChooseResearch.instance.Settings.restoreControl)
        {
            researchProjectDef = ResearchRecord.CurrentProject(___pawn);
        }

        if (researchProjectDef is { IsFinished: false })
        {
            __result = "PCR_ReportString".Translate(researchProjectDef.label) + " " +
                       researchProjectDef.ProgressPercent.ToStringPercent();
        }
    }
}