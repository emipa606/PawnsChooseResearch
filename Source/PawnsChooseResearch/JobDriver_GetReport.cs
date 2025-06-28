using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace PawnsChooseResearch;

[HarmonyPatch(typeof(JobDriver), nameof(JobDriver.GetReport))]
public class JobDriver_GetReport
{
    public static void Postfix(ref string __result, Pawn ___pawn)
    {
        if (__result != JobDefOf.Research.reportString)
        {
            return;
        }

        var researchProjectDef = (ResearchProjectDef)Startup.CurrentProjField.GetValue(Find.ResearchManager);
        if (!Mod_PawnsChooseResearch.Instance.Settings.restoreControl)
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