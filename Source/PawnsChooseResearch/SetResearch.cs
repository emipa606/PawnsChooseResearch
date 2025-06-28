using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace PawnsChooseResearch;

public class SetResearch
{
    public static void SetRandomResearch(Pawn pawn)
    {
        var possibleProjects = Startup.PossibleResearchProjectDefs.Where(projectDef =>
            projectDef.CanStartNow && !ResearchCapability.IsIncapable(pawn, projectDef) &&
            !ResearchCapability.IsAbhorrent(pawn, projectDef)).ToArray();

        if (!possibleProjects.Any())
        {
            return;
        }

        if (possibleProjects.Count() == 1)
        {
            ResearchRecord.SetResearchPlan(pawn, possibleProjects.First());
            return;
        }

        var researchProjectDef = new ResearchProjectDef();
        var highestScore = -100f;
        foreach (var researchProject in possibleProjects.InRandomOrder().Take(Math.Min(possibleProjects.Count(), 10)))
        {
            var currentScore = ResearchPreferences.GetPreferenceScore(pawn, researchProject);
            if (currentScore <= highestScore && researchProjectDef != null)
            {
                continue;
            }

            researchProjectDef = researchProject;
            highestScore = currentScore;
        }

        Startup.LogMessage($"{pawn.NameFullColored} is choosing {researchProjectDef.label}");
        ResearchRecord.SetResearchPlan(pawn, researchProjectDef);
    }

    public static void SetRandomGroupResearch()
    {
        var pawns = new List<Pawn>();
        foreach (var map in Find.Maps)
        {
            if (!map.IsPlayerHome)
            {
                continue;
            }

            foreach (var pawn in map.mapPawns.FreeColonistsSpawned)
            {
                if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Research))
                {
                    pawns.Add(pawn);
                }
            }
        }

        var possibleProjects =
            Startup.PossibleResearchProjectDefs.Where(projectDef => projectDef.CanStartNow).ToArray();

        if (!possibleProjects.Any())
        {
            return;
        }

        if (possibleProjects.Count() == 1)
        {
            Startup.CurrentProjField.SetValue(Find.ResearchManager, possibleProjects.First());
            return;
        }

        if (pawns.Count == 0 && possibleProjects.TryRandomElement(out var result))
        {
            Startup.CurrentProjField.SetValue(Find.ResearchManager, result);
            return;
        }


        var pawnsCount = 1 + pawns.Count;

        var researchProjectDefs =
            possibleProjects.InRandomOrder().Take(Math.Min(possibleProjects.Count(), 20 + pawnsCount));

        var researchProjectDef = new ResearchProjectDef();
        var highestScore = -100f;
        foreach (var projectDef in researchProjectDefs)
        {
            var currentScore = 0f;
            foreach (var pawn in pawns)
            {
                currentScore = !ResearchCapability.IsAbhorrent(pawn, projectDef)
                    ? !ResearchCapability.IsIncapable(pawn, projectDef)
                        ? currentScore + ResearchPreferences.GetPreferenceScore(pawn, projectDef)
                        : currentScore - 1f
                    : currentScore - 3f;
            }

            if (currentScore <= highestScore && researchProjectDef != null)
            {
                continue;
            }

            researchProjectDef = projectDef;
            highestScore = currentScore;
        }

        Startup.CurrentProjField.SetValue(Find.ResearchManager, researchProjectDef);
        ResearchRecord.groupProject = researchProjectDef;
    }
}