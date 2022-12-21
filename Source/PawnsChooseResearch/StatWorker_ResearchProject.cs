using RimWorld;
using Verse;

namespace PawnsChooseResearch;

public class StatWorker_ResearchProject : StatWorker
{
    public override bool ShouldShowFor(StatRequest req)
    {
        if (req.Thing is not Pawn pawn)
        {
            return false;
        }

        return ResearchRecord.CurrentProject(pawn, false) != null &&
               !pawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled;
    }

    public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
    {
        if (req.Thing is not Pawn trackedPawn)
        {
            return 0f;
        }

        return ResearchRecord.CurrentProject(trackedPawn, false)?.ProgressPercent ?? 0f;
    }

    public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
    {
        return req.Thing is not Pawn trackedPawn
            ? string.Empty
            : GenText.ToTitleCaseSmart(ResearchRecord.CurrentProject(trackedPawn, false)?.label ?? string.Empty);
    }

    public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
    {
        if (req.Thing is not Pawn trackedPawn)
        {
            return string.Empty;
        }

        return ResearchRecord.CurrentProject(trackedPawn)?.description ??
               "PCR_StatDef_ResearchProjectNoProject".Translate();
    }
}
