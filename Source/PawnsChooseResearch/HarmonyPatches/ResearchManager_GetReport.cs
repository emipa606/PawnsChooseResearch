using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PawnsChooseResearch;

[HarmonyPatch(typeof(Alert_NeedResearchProject), nameof(Alert_NeedResearchProject.GetReport))]
public static class ResearchManager_GetReport
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