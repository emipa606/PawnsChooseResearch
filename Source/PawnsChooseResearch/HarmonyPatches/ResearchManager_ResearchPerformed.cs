using HarmonyLib;
using RimWorld;
using Verse;

namespace PawnsChooseResearch;

[HarmonyPatch(typeof(ResearchManager), nameof(ResearchManager.ResearchPerformed))]
public static class ResearchManager_ResearchPerformed
{
    public static void Prefix(Pawn researcher)
    {
        if (Mod_PawnsChooseResearch.Instance.Settings.restoreControl || researcher?.CurJobDef != JobDefOf.Research)
        {
            return;
        }

        var researchProjectDef = ResearchRecord.CurrentProject(researcher);
        if (researchProjectDef is not { CanStartNow: true })
        {
            if (Mod_PawnsChooseResearch.Instance.Settings.groupResearch)
            {
                SetResearch.SetRandomGroupResearch();
            }
            else
            {
                SetResearch.SetRandomResearch(researcher);
            }
        }

        Startup.CurrentProjField.SetValue(Find.ResearchManager, ResearchRecord.CurrentProject(researcher));
    }
}