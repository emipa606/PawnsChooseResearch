using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace PawnsChooseResearch;

public class SetResearch
{
    public static void SetRandomResearch(Pawn pawn)
    {
        var list = new List<ResearchProjectDef>();
        for (var num = 3; num >= 0; num--)
        {
            if (DefDatabase<ResearchProjectDef>.AllDefsListForReading.Where(x => x.CanStartNow)
                    .TryRandomElement(out var result) && !list.Contains(result) && (num < 1 && list.Count == 0 ||
                    !ResearchCapability.IsIncapable(pawn, result) && !ResearchCapability.IsAbhorrent(pawn, result)))
            {
                list.Add(result);
            }
        }

        if (list.Count == 1)
        {
            ResearchRecord.SetResearchPlan(pawn, list[0]);
            return;
        }

        var researchProjectDef = new ResearchProjectDef();
        var num2 = -100f;
        foreach (var item in list)
        {
            var num3 = ResearchPreferences.GetPreferenceScore(pawn, item);
            if (!(num3 > num2) && researchProjectDef != null)
            {
                continue;
            }

            researchProjectDef = item;
            num2 = num3;
        }

        Startup.LogMessage($"{pawn.NameFullColored} is choosing {researchProjectDef.label}");
        ResearchRecord.SetResearchPlan(pawn, researchProjectDef);
    }

    public static void SetRandomGroupResearch()
    {
        var list = new List<ResearchProjectDef>();
        var list2 = new List<Pawn>();
        foreach (var map in Find.Maps)
        {
            if (!map.IsPlayerHome)
            {
                continue;
            }

            foreach (var item in map.mapPawns.FreeColonistsSpawned)
            {
                if (item.workSettings.WorkIsActive(WorkTypeDefOf.Research))
                {
                    list2.Add(item);
                }
            }
        }

        if (list2.Count == 0 && DefDatabase<ResearchProjectDef>.AllDefsListForReading.Where(x => x.CanStartNow)
                .TryRandomElement(out var result))
        {
            Find.ResearchManager.currentProj = result;
            return;
        }

        var num = 1 + list2.Count;
        for (var i = 0; i < num; i++)
        {
            if (DefDatabase<ResearchProjectDef>.AllDefsListForReading.Where(x => x.CanStartNow)
                    .TryRandomElement(out var result2) && !list.Contains(result2))
            {
                list.Add(result2);
            }
        }

        var researchProjectDef = new ResearchProjectDef();
        var num2 = -100f;
        var num3 = 0f;
        foreach (var item2 in list)
        {
            foreach (var item3 in list2)
            {
                num3 = !ResearchCapability.IsAbhorrent(item3, item2)
                    ? !ResearchCapability.IsIncapable(item3, item2)
                        ? num3 + ResearchPreferences.GetPreferenceScore(item3, item2)
                        : num3 - 1f
                    : num3 - 3f;
            }

            if (!(num3 > num2) && researchProjectDef != null)
            {
                continue;
            }

            researchProjectDef = item2;
            num2 = num3;
        }

        Find.ResearchManager.currentProj = researchProjectDef;
        ResearchRecord.groupProject = researchProjectDef;
    }
}