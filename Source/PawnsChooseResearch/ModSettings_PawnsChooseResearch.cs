using Verse;

namespace PawnsChooseResearch;

public class ModSettings_PawnsChooseResearch : ModSettings
{
    public bool checkPassions = true;

    public bool checkTraits = true;
    public bool groupResearch;

    public bool interestsActivated;

    public bool mustHaveSkill = true;

    public bool preferSimple = true;

    public bool restoreControl;

    public bool TBnRE_Activated = true;

    public bool vanillaTraitsActivated;

    public bool verboseLogging;

    public void CheckMods()
    {
        interestsActivated = false;
        vanillaTraitsActivated =
            ModLister.GetActiveModWithIdentifier("VanillaExpanded.VanillaTraitsExpanded") != null;
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref groupResearch, "groupResearch");
        Scribe_Values.Look(ref restoreControl, "restoreControl");
        Scribe_Values.Look(ref mustHaveSkill, "musthaveSkill", true);
        Scribe_Values.Look(ref checkPassions, "checkPassions", true);
        Scribe_Values.Look(ref checkTraits, "checkTraits", true);
        Scribe_Values.Look(ref preferSimple, "preferSimple", true);
        Scribe_Values.Look(ref TBnRE_Activated, "TBnRE_Activated", true);
        Scribe_Values.Look(ref verboseLogging, "verboseLogging");
        base.ExposeData();
    }
}