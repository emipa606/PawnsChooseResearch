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

    public static readonly FieldInfo currentProjField;
    public static readonly TraitDef Nerves = TraitDef.Named("Nerves");
    public static readonly TraitDef NaturalMood = TraitDef.Named("NaturalMood");

    static Startup()
    {
        currentProjField = AccessTools.Field(typeof(ResearchManager), "currentProj");
        var harmony = new Harmony("rimworld.pawnschooseresearch");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        HarmonyUnpatching();
        DefDatabase<StatDef>.Add(MakeProjectStatDef());

        foreach (var projectDef in DefDatabase<ResearchProjectDef>.AllDefs)
        {
            if (projectDef == null)
            {
                continue;
            }

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
        try
        {
            var modExtension = projectDef.GetModExtension<ResearchCategory>();
            if (modExtension == null)
            {
                if (projectDef.modExtensions == null)
                {
                    projectDef.modExtensions = [];
                }

                projectDef.modExtensions?.Add(new ResearchCategory());
            }

            modExtension = projectDef.GetModExtension<ResearchCategory>();
            if (modExtension == null)
            {
                LogMessage($"{projectDef.LabelCap} does not have a mod-extension. This shouldnt happen", true);
                return;
            }

            if (projectDef.UnlockedDefs == null || !projectDef.UnlockedDefs.Any())
            {
                LogMessage($"{projectDef.LabelCap} does not unlock anything, counts as progress");
                modExtension.progressTech = 1;
                changedProjects++;
                return;
            }

            var researchValueDictionary = new Dictionary<ResearchType, int>();
            foreach (var def in projectDef.UnlockedDefs)
            {
                LogMessage($"Evaluating {def.LabelCap}");
                var thingEnum = EvaluateThing(def);
                if (thingEnum == ResearchType.None)
                {
                    continue;
                }

                if (!researchValueDictionary.TryAdd(thingEnum, 1))
                {
                    researchValueDictionary[thingEnum]++;
                }
            }

            if (!researchValueDictionary.Any())
            {
                LogMessage($"{projectDef.LabelCap} could not be figured out");
                return;
            }

            var bestValue = researchValueDictionary.OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key)
                .First()
                .Key;
            var changed = true;
            switch (bestValue)
            {
                case ResearchType.Animals:
                    modExtension.animalTech = 1;
                    break;
                case ResearchType.Art:
                    modExtension.artTech = 1;
                    break;
                case ResearchType.ComplexTech:
                    modExtension.complexTech = 1;
                    break;
                case ResearchType.Construction:
                    modExtension.constructionTech = 1;
                    break;
                case ResearchType.Cooking:
                    modExtension.cookingTech = 1;
                    break;
                case ResearchType.Core:
                    modExtension.coreTech = 1;
                    break;
                case ResearchType.Crafting:
                    modExtension.craftTech = 1;
                    break;
                case ResearchType.Cyber:
                    modExtension.cyberTech = 1;
                    break;
                case ResearchType.Drug:
                    modExtension.drugTech = 1;
                    break;
                case ResearchType.Medical:
                    modExtension.medTech = 1;
                    break;
                case ResearchType.Melee:
                    modExtension.meleeTech = 1;
                    break;
                case ResearchType.Mining:
                    modExtension.miningTech = 1;
                    break;
                case ResearchType.Never:
                    modExtension.neverTech = 1;
                    break;
                case ResearchType.Plants:
                    modExtension.plantTech = 1;
                    break;
                case ResearchType.Progress:
                    modExtension.progressTech = 1;
                    break;
                case ResearchType.Ranged:
                    modExtension.rangedTech = 1;
                    break;
                case ResearchType.Social:
                    modExtension.socialTech = 1;
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
        catch (Exception exception)
        {
            LogMessage($"{projectDef.LabelCap} failed to figure out: {exception}");
        }
    }

    private static ResearchType EvaluateThing(Def def)
    {
        if (def == null)
        {
            return ResearchType.None;
        }

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
                return thingDef.costList?.Any(resource => resource.thingDef?.defName.Contains("Component") == true) ==
                       true
                    ? ResearchType.Cyber
                    : ResearchType.Crafting;
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

            if (!thingDef.IsBuildingArtificial)
            {
                return ResearchType.None;
            }

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

            if (thingDef.StatBaseDefined(StatDefOf.Beauty) &&
                thingDef.GetStatValueAbstract(StatDefOf.Beauty) > 25 ||
                thingDef.StatBaseDefined(StatDefOf.BeautyOutdoors) &&
                thingDef.GetStatValueAbstract(StatDefOf.BeautyOutdoors) > 25)
            {
                return ResearchType.Art;
            }

            return ResearchType.Construction;
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