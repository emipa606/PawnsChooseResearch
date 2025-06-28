using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PawnsChooseResearch;

[HarmonyPatch(typeof(ResearchManager), nameof(ResearchManager.ResearchPerformed))]
public class ResearchManager_ResearchPerformed
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

[HarmonyPatch(typeof(Alert_NeedResearchProject), nameof(Alert_NeedResearchProject.GetReport))]
public class ResearchManager_GetReport
{
    public static void Prefix()
    {
        if (Startup.CurrentProjField.GetValue(Find.ResearchManager) != null)
        {
            return;
        }

        if (Mod_PawnsChooseResearch.Instance.Settings.groupResearch)
        {
            SetResearch.SetRandomGroupResearch();
        }
        else if (Startup.PossibleResearchProjectDefs.Where(x => x.CanStartNow)
                 .TryRandomElement(out var result))
        {
            Startup.CurrentProjField.SetValue(Find.ResearchManager, result);
        }
    }
}