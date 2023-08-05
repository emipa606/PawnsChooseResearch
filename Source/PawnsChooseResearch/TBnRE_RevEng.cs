using System.Reflection;
using HarmonyLib;
using Verse;

namespace PawnsChooseResearch;

[HarmonyPatch]
internal class TBnRE_RevEng
{
    private static bool Prepare(MethodInfo original)
    {
        return ModLister.GetActiveModWithIdentifier("GwinnBleidd.ResearchTweaks") != null;
    }

    private static MethodBase TargetMethod()
    {
        return AccessTools.Method(GenTypes.GetTypeInAnyAssembly("TBnRE.Utility"), "BoostResearch");
    }

    private static void Postfix(ref ResearchProjectDef chosenResearchProject, Pawn usedBy)
    {
        if (!Mod_PawnsChooseResearch.instance.Settings.TBnRE_Activated)
        {
            return;
        }

        if (Mod_PawnsChooseResearch.instance.Settings.groupResearch)
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