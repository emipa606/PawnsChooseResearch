using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PawnsChooseResearch;

[StaticConstructorOnStartup]
public class Startup
{
    private static int changedProjects;

    static Startup()
    {
        var harmony = new Harmony("rimworld.pawnschooseresearch");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        HarmonyUnpatching();
        DefDatabase<StatDef>.Add(MakeProjectStatDef());

        foreach (var projectDef in DefDatabase<ResearchProjectDef>.AllDefs)
        {
            var category = projectDef.GetModExtension<ResearchCategory>();
            if (category != null && category.IsSetToSomething())
            {
                LogMessage($"{projectDef.LabelCap} is already fixed by patches");
                continue;
            }

            EvaluateResearchProject(projectDef);
        }

        if (changedProjects > 0)
        {
            LogMessage($"Generated research-category for {changedProjects} research projects", true);
        }
    }

    private static void EvaluateResearchProject(ResearchProjectDef projectDef)
    {
        if (projectDef.UnlockedDefs == null || !projectDef.UnlockedDefs.Any())
        {
            LogMessage($"{projectDef.LabelCap} does not unlock anything, counts as progress");
            projectDef.GetModExtension<ResearchCategory>().progressTech = 1;
            changedProjects++;
            return;
        }

        var researchValueDictionary = new Dictionary<ResearchType, int>();
        foreach (var def in projectDef.UnlockedDefs)
        {
            var thingEnum = EvaluateThing(def);
            if (thingEnum == ResearchType.None)
            {
                continue;
            }

            if (researchValueDictionary.ContainsKey(thingEnum))
            {
                researchValueDictionary[thingEnum]++;
                continue;
            }

            researchValueDictionary[thingEnum] = 1;
        }

        if (!researchValueDictionary.Any())
        {
            LogMessage($"{projectDef.LabelCap} could not be figured out");
            return;
        }

        var bestValue = researchValueDictionary.OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key).First()
            .Key;
        var changed = true;
        switch (bestValue)
        {
            case ResearchType.Animals:
                projectDef.GetModExtension<ResearchCategory>().animalTech = 1;
                break;
            case ResearchType.Art:
                projectDef.GetModExtension<ResearchCategory>().artTech = 1;
                break;
            case ResearchType.ComplexTech:
                projectDef.GetModExtension<ResearchCategory>().complexTech = 1;
                break;
            case ResearchType.Construction:
                projectDef.GetModExtension<ResearchCategory>().constructionTech = 1;
                break;
            case ResearchType.Cooking:
                projectDef.GetModExtension<ResearchCategory>().cookingTech = 1;
                break;
            case ResearchType.Core:
                projectDef.GetModExtension<ResearchCategory>().coreTech = 1;
                break;
            case ResearchType.Crafting:
                projectDef.GetModExtension<ResearchCategory>().craftTech = 1;
                break;
            case ResearchType.Cyber:
                projectDef.GetModExtension<ResearchCategory>().cyberTech = 1;
                break;
            case ResearchType.Drug:
                projectDef.GetModExtension<ResearchCategory>().drugTech = 1;
                break;
            case ResearchType.Medical:
                projectDef.GetModExtension<ResearchCategory>().medTech = 1;
                break;
            case ResearchType.Melee:
                projectDef.GetModExtension<ResearchCategory>().meleeTech = 1;
                break;
            case ResearchType.Mining:
                projectDef.GetModExtension<ResearchCategory>().miningTech = 1;
                break;
            case ResearchType.Never:
                projectDef.GetModExtension<ResearchCategory>().neverTech = 1;
                break;
            case ResearchType.Plants:
                projectDef.GetModExtension<ResearchCategory>().plantTech = 1;
                break;
            case ResearchType.Progress:
                projectDef.GetModExtension<ResearchCategory>().progressTech = 1;
                break;
            case ResearchType.Ranged:
                projectDef.GetModExtension<ResearchCategory>().rangedTech = 1;
                break;
            case ResearchType.Social:
                projectDef.GetModExtension<ResearchCategory>().socialTech = 1;
                break;
            default:
                changed = false;
                break;
        }

        if (changed)
        {
            changedProjects++;
        }

        LogMessage($"{projectDef.LabelCap} set to {bestValue}");
    }

    private static ResearchType EvaluateThing(Def def)
    {
        try
        {
            if (def is not ThingDef thingDef)
            {
                return ResearchType.None;
            }

            if (thingDef.IsArt)
            {
                return ResearchType.Art;
            }

            if (thingDef.isTechHediff || thingDef.IsMedicine)
            {
                return ResearchType.Medical;
            }

            if (thingDef.IsAnimalProduct)
            {
                return ResearchType.Animals;
            }

            if (thingDef.IsApparel)
            {
                if (thingDef.costList?.Any(resource => resource.thingDef.defName.Contains("Component")) == true)
                {
                    return ResearchType.Cyber;
                }

                return ResearchType.Crafting;
            }

            if (thingDef.IsRangedWeapon)
            {
                return ResearchType.Ranged;
            }

            if (thingDef.IsMeleeWeapon)
            {
                return ResearchType.Melee;
            }

            if (thingDef.IsDrug)
            {
                return ResearchType.Drug;
            }

            if (thingDef.designationCategory?.defName == "Ship")
            {
                return ResearchType.Progress;
            }

            if (thingDef.IsBuildingArtificial)
            {
                var buildingProps = thingDef.building;
                if (buildingProps.isSittable || thingDef.IsTable)
                {
                    return ResearchType.Construction;
                }

                if (buildingProps.joyKind != null)
                {
                    return ResearchType.Social;
                }

                if (buildingProps.IsTurret)
                {
                    return ResearchType.Ranged;
                }

                if (thingDef.recipes?.Any() != null)
                {
                    var normalRecipes = thingDef.recipes.Where(recipeDef => recipeDef.workSkill != null);
                    var mechRecipesCount = thingDef.recipes.Count(recipeDef => recipeDef.mechanitorOnlyRecipe);

                    if (!normalRecipes.Any())
                    {
                        if (mechRecipesCount > 0)
                        {
                            return ResearchType.ComplexTech;
                        }
                    }
                    else
                    {
                        var skillCount = normalRecipes.Select(recipeDef => recipeDef.workSkill)
                            .GroupBy(skillDef => skillDef)
                            .ToDictionary(skillDefs => skillDefs.Key, grouping => grouping.Count())
                            .MaxBy(pair => pair.Value);

                        if (mechRecipesCount > skillCount.Value)
                        {
                            return ResearchType.ComplexTech;
                        }

                        switch (skillCount.Key.defName)
                        {
                            case "Shooting":
                                return ResearchType.Ranged;
                            case "Melee":
                                return ResearchType.Melee;
                            case "Construction":
                                return ResearchType.Construction;
                            case "Mining":
                                return ResearchType.Mining;
                            case "Cooking":
                                return ResearchType.Cooking;
                            case "Plants":
                                return ResearchType.Plants;
                            case "Animals":
                                return ResearchType.Animals;
                            case "Crafting":
                                return ResearchType.Crafting;
                            case "Artistic":
                                return ResearchType.Art;
                            case "Medicine":
                                return ResearchType.Medical;
                            case "Social":
                                return ResearchType.Social;
                            case "Intellectual":
                                return ResearchType.ComplexTech;
                        }
                    }
                }

                if (thingDef.EverTransmitsPower)
                {
                    var powerCompProps = thingDef.GetCompProperties<CompProperties_Power>();
                    if (powerCompProps.compClass.IsInstanceOfType(typeof(CompPowerPlant)) ||
                        powerCompProps.compClass.IsSubclassOf(typeof(CompPowerPlant)) ||
                        powerCompProps.compClass.IsInstanceOfType(typeof(CompPowerBattery)) ||
                        powerCompProps.compClass.IsSubclassOf(typeof(CompPowerBattery)))
                    {
                        return ResearchType.Progress;
                    }
                }

                if (thingDef.defName.StartsWith("Gene"))
                {
                    return ResearchType.Medical;
                }

                return ResearchType.Construction;
            }

            return ResearchType.None;
        }
        catch (Exception exception)
        {
            Log.Warning($"Failed to figure out {def.LabelCap}: {exception}");
            return ResearchType.None;
        }
    }

    private static void HarmonyUnpatching()
    {
        var method = typeof(ResearchManager).GetMethod("FinishProject");
        var enumerable = Harmony.GetPatchInfo(method)?.Prefixes
            ?.Where(p => p.owner is "Fluffy.ResearchTree" or "rimworld.ResearchPal");
        if (enumerable == null || !enumerable.Any())
        {
            return;
        }

        var harmony = new Harmony("razor2.3.PawnsDiscloseResearch");
        foreach (var item in enumerable)
        {
            LogMessage($"PDR: Belt found suspenders ({item.owner}). Re-enabling vanilla notification . . . ", true);
            harmony.Unpatch(method, HarmonyPatchType.Prefix, item.owner);
        }
    }

    private static StatDef MakeProjectStatDef()
    {
        var statDef = new StatDef
        {
            defName = "ResearchProject",
            label = "PCR_StatDef_ResearchProject".Translate(),
            description = "PCR_StatDef_ResearchProjectDesc".Translate(),
            category = StatCategoryDefOf.BasicsPawn,
            workerClass = typeof(StatWorker_ResearchProject),
            toStringStyle = ToStringStyle.PercentZero,
            defaultBaseValue = 0f
        };
        return statDef;
    }

    public static void LogMessage(string message, bool force = false)
    {
        if (force || Mod_PawnsChooseResearch.instance.Settings.verboseLogging)
        {
            Log.Message($"[PawnsChooseResearch]: {message}");
        }
    }

    private enum ResearchType
    {
        Animals,
        Art,
        ComplexTech,
        Construction,
        Cooking,
        Core,
        Crafting,
        Cyber,
        Drug,
        Medical,
        Melee,
        Mining,
        Never,
        Plants,
        Progress,
        Ranged,
        Social,
        None
    }
}