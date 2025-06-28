using Mlie;
using UnityEngine;
using Verse;

namespace PawnsChooseResearch;

internal class Mod_PawnsChooseResearch : Mod
{
    private static string currentVersion;
    public static Mod_PawnsChooseResearch Instance;

    private ModSettings_PawnsChooseResearch settings;

    public Mod_PawnsChooseResearch(ModContentPack content)
        : base(content)
    {
        Instance = this;
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal ModSettings_PawnsChooseResearch Settings
    {
        get
        {
            settings ??= GetSettings<ModSettings_PawnsChooseResearch>();

            return settings;
        }
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var rect = new Rect(260f, 50f, inRect.width * 0.4f, inRect.height);
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.CheckboxLabeled("PCR_PlayerControl".Translate(),
            ref Settings.restoreControl);
        if (!Settings.restoreControl)
        {
            listingStandard.CheckboxLabeled("PCR_PawnsCoordinateResearch".Translate(),
                ref Settings.groupResearch);
            if (!Settings.groupResearch)
            {
                listingStandard.CheckboxLabeled("PCR_MustHaveSkillCapability".Translate(),
                    ref Settings.mustHaveSkill);
            }

            listingStandard.CheckboxLabeled("PCR_TraitsAffectResearch".Translate(),
                ref Settings.checkTraits);
            listingStandard.CheckboxLabeled("PCR_PassionsAffectResearch".Translate(),
                ref Settings.checkPassions);

            listingStandard.CheckboxLabeled("PCR_AvoidTooAdvancedResearch".Translate(),
                ref Settings.preferSimple);
            if (ModLister.GetActiveModWithIdentifier("GwinnBleidd.ResearchTweaks", true) != null)
            {
                listingStandard.CheckboxLabeled("PCR_ReverseEngineeringAssignsResearch".Translate(),
                    ref Settings.TBnRE_Activated);
            }
        }

        listingStandard.CheckboxLabeled("PCR_VerboseLogging".Translate(),
            ref Settings.verboseLogging);

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("PCR_CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Pawns Choose Research";
    }
}