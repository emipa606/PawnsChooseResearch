using RimWorld;
using Verse;

namespace PawnsChooseResearch;

public class ResearchPreferences
{
    public static float GetPreferenceScore(Pawn pawn, ResearchProjectDef researchProject)
    {
        var preferenceValue = 1f + (researchProject.ProgressPercent / 2f);
        if (researchProject.HasModExtension<ResearchCategory>())
        {
            Startup.LogMessage(
                $"{pawn.NameFullColored} checking {researchProject.LabelCap}. \n{researchProject.GetModExtension<ResearchCategory>()}");
            if (Mod_PawnsChooseResearch.Instance.Settings.checkPassions)
            {
                preferenceValue += getPassionScore(pawn, researchProject);
            }

            if (Mod_PawnsChooseResearch.Instance.Settings.checkTraits)
            {
                preferenceValue += getTraitScore(pawn, researchProject);
            }

            preferenceValue += getSpecialTraitScore(pawn, researchProject);
            if (researchProject.GetModExtension<ResearchCategory>().coreTech > 0)
            {
                preferenceValue += pawn.skills.GetSkill(SkillDefOf.Intellectual).Level * 0.05f;
            }
        }

        if (Mod_PawnsChooseResearch.Instance.Settings.preferSimple &&
            (int)researchProject.techLevel > (int)Faction.OfPlayer.def.techLevel)
        {
            preferenceValue -= (researchProject.techLevel - Faction.OfPlayerSilentFail.def.techLevel) /
                               (pawn.skills.GetSkill(SkillDefOf.Intellectual).Level + 1f);
        }

        return preferenceValue * Rand.Range(0.5f, 1f);
    }

    private static float getPassionScore(Pawn pawn, ResearchProjectDef researchProject)
    {
        var num = 0f;
        var foundType = false;
        if (researchProject.GetModExtension<ResearchCategory>().rangedTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Shooting).passion;
            foundType = true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().meleeTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Melee).passion;
            foundType = true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().constructionTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Construction).passion;
            foundType = true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().miningTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Mining).passion;
            foundType = true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().cookingTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Cooking).passion;
            foundType = true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().plantTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Plants).passion;
            foundType = true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().animalTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Animals).passion;
            foundType = true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().craftTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Crafting).passion;
            foundType = true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().artTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Artistic).passion;
            foundType = true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().medTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Medicine).passion;
            foundType = true;
        }

        if (researchProject.GetModExtension<ResearchCategory>().socialTech > 0)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Social).passion;
            foundType = true;
        }

        if (!foundType)
        {
            num += (int)pawn.skills.GetSkill(SkillDefOf.Intellectual).passion;
        }

        return num;
    }

    private static float getTraitScore(Pawn pawn, ResearchProjectDef researchProject)
    {
        var num = 0f;
        var allTraits = pawn.story.traits.allTraits;
        foreach (var trait in allTraits)
        {
            var def = trait.def;
            if (def == TraitDefOf.Industriousness && trait.Degree <= 0 ||
                def.GetModExtension<ResearchCategory>().progressTech > 0)
            {
                num += researchProject.ProgressPercent - 1f;
            }

            if (def.GetModExtension<ResearchCategory>().progressTech < 0)
            {
                num += 1f - researchProject.ProgressPercent;
            }

            if (def == TraitDefOf.Industriousness && trait.Degree > 0 ||
                def == Startup.Nerves && trait.Degree > 0 ||
                def.GetModExtension<ResearchCategory>().complexTech < 0)
            {
                num -= researchProject.baseCost / (int)researchProject.techLevel / 1000f;
            }

            if (def.GetModExtension<ResearchCategory>().complexTech > 0)
            {
                num += researchProject.baseCost / (int)researchProject.techLevel / 1000f;
            }

            if (researchProject.GetModExtension<ResearchCategory>().rangedTech > 0)
            {
                if (def == Startup.NaturalMood && trait.Degree < 0 ||
                    def.GetModExtension<ResearchCategory>().rangedTech > 0)
                {
                    num += 1f;
                }
                else if (trait.def == Startup.NaturalMood && trait.Degree > 0 ||
                         trait.def.GetModExtension<ResearchCategory>().rangedTech < 0)
                {
                    num -= 1f;
                }
            }

            if (researchProject.GetModExtension<ResearchCategory>().meleeTech > 0)
            {
                if (def == Startup.NaturalMood && trait.Degree < 0 ||
                    def.GetModExtension<ResearchCategory>().meleeTech > 0)
                {
                    num += 1f;
                }
                else if (trait.def == Startup.NaturalMood && trait.Degree > 0 ||
                         trait.def.GetModExtension<ResearchCategory>().meleeTech < 0)
                {
                    num -= 1f;
                }
            }

            if (researchProject.GetModExtension<ResearchCategory>().constructionTech > 0)
            {
                if (def == TraitDefOf.Industriousness && trait.Degree > 0 ||
                    def.GetModExtension<ResearchCategory>().constructionTech > 0)
                {
                    num += 1f;
                }
                else if (trait.def.GetModExtension<ResearchCategory>().constructionTech < 0)
                {
                    num -= 1f;
                }
            }

            if (researchProject.GetModExtension<ResearchCategory>().miningTech > 0)
            {
                if (def == TraitDefOf.Industriousness && trait.Degree > 0 ||
                    def.GetModExtension<ResearchCategory>().miningTech > 0)
                {
                    num += 1f;
                }
                else if (trait.def.GetModExtension<ResearchCategory>().miningTech < 0)
                {
                    num -= 1f;
                }
            }

            if (researchProject.GetModExtension<ResearchCategory>().cookingTech > 0)
            {
                if (def.GetModExtension<ResearchCategory>().cookingTech > 0)
                {
                    num += 1f;
                }
                else if (trait.def.GetModExtension<ResearchCategory>().cookingTech < 0)
                {
                    num -= 1f;
                }
            }

            if (researchProject.GetModExtension<ResearchCategory>().plantTech > 0)
            {
                if (def.GetModExtension<ResearchCategory>().plantTech > 0)
                {
                    num += 1f;
                }
                else if (trait.def.GetModExtension<ResearchCategory>().plantTech < 0)
                {
                    num -= 1f;
                }
            }

            if (researchProject.GetModExtension<ResearchCategory>().animalTech > 0)
            {
                if (def.GetModExtension<ResearchCategory>().animalTech > 0)
                {
                    num += 1f;
                }
                else if (trait.def.GetModExtension<ResearchCategory>().animalTech < 0)
                {
                    num -= 1f;
                }
            }

            if (researchProject.GetModExtension<ResearchCategory>().craftTech > 0)
            {
                if (def.GetModExtension<ResearchCategory>().craftTech > 0)
                {
                    num += 1f;
                }
                else if (trait.def.GetModExtension<ResearchCategory>().craftTech < 0)
                {
                    num -= 1f;
                }
            }

            if (researchProject.GetModExtension<ResearchCategory>().artTech > 0)
            {
                if (def.GetModExtension<ResearchCategory>().artTech > 0)
                {
                    num += 1f;
                }
                else if (trait.def.GetModExtension<ResearchCategory>().artTech < 0)
                {
                    num -= 1f;
                }
            }

            if (researchProject.GetModExtension<ResearchCategory>().medTech > 0)
            {
                if (def == Startup.NaturalMood && trait.Degree < 0 ||
                    def == TraitDef.Named("Immunity") && trait.Degree < 0 ||
                    def.GetModExtension<ResearchCategory>().medTech > 0)
                {
                    num += 1f;
                }
                else if (trait.def == TraitDef.Named("Immunity") && trait.Degree > 0 ||
                         trait.def.GetModExtension<ResearchCategory>().medTech < 0)
                {
                    num -= 1f;
                }
            }

            if (researchProject.GetModExtension<ResearchCategory>().socialTech <= 0)
            {
                continue;
            }

            if (def.GetModExtension<ResearchCategory>().socialTech > 0)
            {
                num += 1f;
            }
            else if (trait.def.GetModExtension<ResearchCategory>().socialTech < 0)
            {
                num -= 1f;
            }
        }

        return num;
    }

    private static float getSpecialTraitScore(Pawn pawn, ResearchProjectDef researchProject)
    {
        var num = 0f;
        var allTraits = pawn.story.traits.allTraits;
        foreach (var trait in allTraits)
        {
            var def = trait.def;
            if (def == TraitDefOf.Transhumanist)
            {
                if (researchProject.GetModExtension<ResearchCategory>().cyberTech > 0 ||
                    researchProject.defName == "Machining")
                {
                    num += 3f;
                }
                else if (researchProject.prerequisites != null)
                {
                    foreach (var prerequisite in researchProject.prerequisites)
                    {
                        if (prerequisite.GetModExtension<ResearchCategory>().cyberTech <= 0)
                        {
                            continue;
                        }

                        num += 3f;
                        break;
                    }
                }
            }

            if (def == TraitDefOf.DrugDesire && trait.Degree > 0 &&
                researchProject.GetModExtension<ResearchCategory>().drugTech > 0)
            {
                num += 3f;
            }

            if (def == TraitDef.Named("Ascetic") && researchProject == ResearchProjectDef.Named("NutrientPaste"))
            {
                num += 2f;
            }

            if (def == TraitDefOf.Pyromaniac && researchProject.defName is "Electricity"
                    or "Batteries" or "IEDs"
                    or "Mortars" or "BiofuelRefining" or "SmokepopBelt" or "ChargedShot" or "ShipReactor")
            {
                num += 1f;
            }

            if (def == TraitDefOf.Nudist)
            {
                var item = ResearchProjectDef.Named("ComplexClothing");
                if (researchProject.defName == "ComplexClothing" || researchProject.prerequisites != null &&
                    researchProject.prerequisites.Contains(item))
                {
                    num -= 1f;
                }
            }

            if (Mod_PawnsChooseResearch.Instance.Settings.vanillaTraitsActivated)
            {
                num += VTE_Integration.GetSpecialTraitScore_VTE(researchProject, def);
            }
        }

        return num;
    }
}