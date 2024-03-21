using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PawnsChooseResearch;

[HarmonyPatch]
public class Patch_Research
{
    [HarmonyPatch(typeof(ResearchManager), "ResearchPerformed")]
    [HarmonyPrefix]
    public static void Prefix_ResearchPerformed(Pawn researcher)
    {
        if (Mod_PawnsChooseResearch.instance.Settings.restoreControl || researcher?.CurJobDef != JobDefOf.Research)
        {
            return;
        }

        var researchProjectDef = ResearchRecord.CurrentProject(researcher);
        if (researchProjectDef is not { CanStartNow: true })
        {
            if (Mod_PawnsChooseResearch.instance.Settings.groupResearch)
            {
                SetResearch.SetRandomGroupResearch();
            }
            else
            {
                SetResearch.SetRandomResearch(researcher);
            }
        }

        Startup.currentProjField.SetValue(Find.ResearchManager, ResearchRecord.CurrentProject(researcher));
    }

    [HarmonyPatch(typeof(Alert_NeedResearchProject), "GetReport")]
    [HarmonyPrefix]
    public static void Prefix_SetResearch()
    {
        if (Startup.currentProjField.GetValue(Find.ResearchManager) != null)
        {
            return;
        }

        if (Mod_PawnsChooseResearch.instance.Settings.groupResearch)
        {
            SetResearch.SetRandomGroupResearch();
        }
        else if (DefDatabase<ResearchProjectDef>.AllDefsListForReading.Where(x => x.CanStartNow)
                 .TryRandomElement(out var result))
        {
            Startup.currentProjField.SetValue(Find.ResearchManager, result);
        }
    }
}