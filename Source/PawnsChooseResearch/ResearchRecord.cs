using System.Collections.Generic;
using Verse;

namespace PawnsChooseResearch;

public class ResearchRecord : GameComponent
{
    public static List<ResearchProjectDef> currentProjects = new List<ResearchProjectDef>();

    private static List<Pawn> trackedPawns = new List<Pawn>();

    private static Dictionary<Pawn, ResearchProjectDef> researchPlan;

    public static ResearchProjectDef groupProject;

    public ResearchRecord(Game game)
    {
        researchPlan = new Dictionary<Pawn, ResearchProjectDef>();
    }

    public static ResearchProjectDef CurrentProject(Pawn trackedPawn, bool showGroupProj = true)
    {
        if (showGroupProj && Mod_PawnsChooseResearch.instance.Settings.groupResearch)
        {
            return groupProject;
        }

        if (researchPlan.TryGetValue(trackedPawn, out var value) && value is { IsFinished: false })
        {
            return researchPlan[trackedPawn];
        }

        return null;
    }

    public static void SetResearchPlan(Pawn trackedPawn, ResearchProjectDef myProject)
    {
        if (researchPlan.ContainsKey(trackedPawn))
        {
            researchPlan[trackedPawn] = myProject;
        }
        else
        {
            researchPlan.Add(trackedPawn, myProject);
        }
    }

    public static void UpdateResearchRecord()
    {
        if (Mod_PawnsChooseResearch.instance.Settings.groupResearch ||
            Mod_PawnsChooseResearch.instance.Settings.restoreControl)
        {
            return;
        }

        foreach (var key in researchPlan.Keys)
        {
            if (key.Dead || key.Destroyed)
            {
                researchPlan.Remove(key);
            }
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref researchPlan, "VarietyRecord", LookMode.Reference, LookMode.Def, ref trackedPawns,
            ref currentProjects);
        Scribe_Defs.Look(ref groupProject, "GroupProject");
    }

    public override void FinalizeInit()
    {
        if (researchPlan is { Count: > 0 })
        {
            UpdateResearchRecord();
            foreach (var key in researchPlan.Keys)
            {
                Startup.LogMessage($"{key.Name} is researching {CurrentProject(key)}");
            }
        }

        Mod_PawnsChooseResearch.instance.Settings.CheckMods();
        base.FinalizeInit();
    }
}