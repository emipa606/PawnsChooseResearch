using Mlie;
using UnityEngine;
using Verse;

namespace PawnsChooseResearch;

internal class Mod_PawnsChooseResearch : Mod
{
    private static string currentVersion;
    public static Mod_PawnsChooseResearch instance;

    public ModSettings_PawnsChooseResearch settings;

    public Mod_PawnsChooseResearch(ModContentPack content)
        : base(content)
    {
        instance = this;
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal ModSettings_PawnsChooseResearch Settings
    {
        get
        {
            if (settings == null)
            {
                settings = GetSettings<ModSettings_PawnsChooseResearch>();
            }

            return settings;
        }
        set => settings = value;
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var rect = new Rect(260f, 50f, inRect.width * 0.4f, inRect.height);
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.CheckboxLabeled("PCR_PlayerControl".Translate(),
            ref Settings.restoreControl);
        if (!Settings.restoreControl)
        {
            listing_Standard.CheckboxLabeled("PCR_PawnsCoordinateResearch".Translate(),
                ref Settings.groupResearch);
            if (!Settings.groupResearch)
            {
                listing_Standard.CheckboxLabeled("PCR_MustHaveSkillCapability".Translate(),
                    ref Settings.mustHaveSkill);
            }

            listing_Standard.CheckboxLabeled("PCR_TraitsAffectResearch".Translate(),
                ref Settings.checkTraits);
            listing_Standard.CheckboxLabeled("PCR_PassionsAffectResearch".Translate(),
                ref Settings.checkPassions);

            listing_Standard.CheckboxLabeled("PCR_AvoidTooAdvancedResearch".Translate(),
                ref Settings.preferSimple);
            if (ModLister.GetActiveModWithIdentifier("GwinnBleidd.ResearchTweaks") != null)
            {
                listing_Standard.CheckboxLabeled("PCR_ReverseEngineeringAssignsResearch".Translate(),
                    ref Settings.TBnRE_Activated);
            }
        }

        listing_Standard.CheckboxLabeled("PCR_VerboseLogging".Translate(),
            ref Settings.verboseLogging);

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("PCR_CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Pawns Choose Research";
    }
}