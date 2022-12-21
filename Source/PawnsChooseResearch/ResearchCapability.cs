using RimWorld;
using Verse;

namespace PawnsChooseResearch;

public class ResearchCapability
{
    public static bool IsIncapable(Pawn pawn, ResearchProjectDef researchProject)
    {
        if (!researchProject.HasModExtension<ResearchCategory>() ||
            !Mod_PawnsChooseResearch.instance.Settings.mustHaveSkill)
        {
            return false;
        }

        if (researchProject.GetModExtension<ResearchCategory>().rangedTech > 0 &&
            pawn.skills.GetSkill(SkillDefOf.Shooting).TotallyDisabled)
        {
            return true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().meleeTech > 0 &&
            pawn.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled)
        {
            return true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().constructionTech > 0 &&
            pawn.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled)
        {
            return true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().miningTech > 0 &&
            pawn.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled)
        {
            return true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().cookingTech > 0 &&
            pawn.skills.GetSkill(SkillDefOf.Cooking).TotallyDisabled)
        {
            return true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().plantTech > 0 &&
            pawn.skills.GetSkill(SkillDefOf.Plants).TotallyDisabled)
        {
            return true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().animalTech > 0 &&
            pawn.skills.GetSkill(SkillDefOf.Animals).TotallyDisabled)
        {
            return true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().craftTech > 0 &&
            pawn.skills.GetSkill(SkillDefOf.Crafting).TotallyDisabled)
        {
            return true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().artTech > 0 &&
            pawn.skills.GetSkill(SkillDefOf.Artistic).TotallyDisabled)
        {
            return true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().medTech > 0 &&
            pawn.skills.GetSkill(SkillDefOf.Medicine).TotallyDisabled)
        {
            return true;
        }

        return researchProject.GetModExtension<ResearchCategory>().socialTech > 0 &&
               pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
    }

    public static bool IsAbhorrent(Pawn pawn, ResearchProjectDef researchProject)
    {
        if (researchProject.HasModExtension<ResearchCategory>() &&
            researchProject.GetModExtension<ResearchCategory>().neverTech > 0 || researchProject.baseCost > 90000f)
        {
            return true;
        }

        var allTraits = pawn.story.traits.allTraits;
        foreach (var trait in allTraits)
        {
            if (trait.def == TraitDefOf.Pyromaniac)
            {
                if (researchProject.defName == "Firefoam")
                {
                    return true;
                }

                if (researchProject.prerequisites != null &&
                    researchProject.prerequisites.Contains(ResearchProjectDef.Named("Firefoam")))
                {
                    return true;
                }
            }

            if (trait.def == TraitDefOf.BodyPurist)
            {
                if (researchProject.GetModExtension<ResearchCategory>().cyberTech > 0)
                {
                    return true;
                }

                if (researchProject.prerequisites != null)
                {
                    foreach (var researchProjectDef in researchProject.prerequisites)
                    {
                        if (researchProjectDef.GetModExtension<ResearchCategory>().cyberTech > 0)
                        {
                            return true;
                        }
                    }
                }
            }

            if (trait.def == TraitDefOf.DrugDesire && trait.Degree < 0 &&
                researchProject.GetModExtension<ResearchCategory>().drugTech > 1)
            {
                return true;
            }

            if (trait.def == TraitDefOf.Ascetic &&
                researchProject.defName is "TubeTelevision" or "FlatscreenTelevision")
            {
                return true;
            }

            if (Mod_PawnsChooseResearch.instance.Settings.vanillaTraitsActivated &&
                (trait.def == TraitDef.Named("VTE_RefinedPalate") ||
                 trait.def == TraitDef.Named("VTE_Gastronomist")) &&
                researchProject.defName is "NutrientPaste" or "PackagedSurvivalMeal")
            {
                return true;
            }
        }

        return false;
    }
}