using System.Reflection;
using HarmonyLib;
using Verse;

namespace PawnsChooseResearch;

[HarmonyPatch]
internal static class TBnRE_RevEng
{
    public static bool Prepare(MethodInfo original)
    {
        return ModLister.GetActiveModWithIdentifier("GwinnBleidd.ResearchTweaks", true) != null;
    }

    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(GenTypes.GetTypeInAnyAssembly("TBnRE.Utility"), "BoostResearch");
    }

    public static void Postfix(ref ResearchProjectDef chosenResearchProject, Pawn usedBy)
    {
        if (!Mod_PawnsChooseResearch.Instance.Settings.TBnRE_Activated)
        {
            return;
        }

        if (Mod_PawnsChooseResearch.Instance.Settings.groupResearch)
        {
            ResearchRecord.groupProject = chosenResearchProject;
            return;
        }

        if (!ResearchCapability.IsIncapable(usedBy, chosenResearchProject) &&
            !ResearchCapability.IsAbhorrent(usedBy, chosenResearchProject))
        {
            ResearchRecord.SetResearchPlan(usedBy, chosenResearchProject);
        }
    }
}