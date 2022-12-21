using RimWorld;
using Verse;

namespace PawnsChooseResearch;

public class VTE_Integration
{
    public static float GetSpecialTraitScore_VTE(ResearchProjectDef researchProject, TraitDef curTrait)
    {
        var num = 0f;
        if (curTrait == TraitDef.Named("VTE_Clumsy"))
        {
            if (researchProject == ResearchProjectDef.Named("Autodoors"))
            {
                num += 1f;
            }
        }
        else if (curTrait == TraitDef.Named("VTE_ColdInclined"))
        {
            if (researchProject == ResearchProjectDef.Named("PassiveCooler") ||
                researchProject == ResearchProjectDef.Named("AirConditioning"))
            {
                num += 3f;
            }
        }
        else if (curTrait == TraitDef.Named("VTE_Neat"))
        {
            if (researchProject == ResearchProjectDef.Named("SterileMaterials"))
            {
                num += 3f;
            }
        }
        else if (curTrait == TraitDef.Named("VTE_CouchPotato"))
        {
            if (researchProject == ResearchProjectDef.Named("TubeTelevision") ||
                researchProject == ResearchProjectDef.Named("FlatscreenTelevision"))
            {
                num += 3f;
            }
        }
        else if (curTrait == TraitDef.Named("VTE_ChildOfSea"))
        {
            if (researchProject == ResearchProjectDef.Named("WatermillGenerator"))
            {
                num += 3f;
            }
        }
        else if (curTrait == TraitDef.Named("VTE_ChildOfMountain"))
        {
            if (researchProject == ResearchProjectDef.Named("GeothermalPower") ||
                researchProject == ResearchProjectDef.Named("DeepDrilling") ||
                researchProject == ResearchProjectDef.Named("GroundPenetratingScanner"))
            {
                num += 3f;
            }
        }
        else if (curTrait == TraitDef.Named("VTE_DrunkenMaster") || curTrait == TraitDef.Named("VTE_Lush"))
        {
            if (researchProject == ResearchProjectDef.Named("Brewing") || researchProject.prerequisites != null &&
                researchProject.prerequisites.Contains(ResearchProjectDef.Named("Brewing")))
            {
                num += 3f;
            }
        }
        else if (curTrait == TraitDef.Named("VTE_Ecologist"))
        {
            if (researchProject == ResearchProjectDef.Named("SolarPanels") ||
                researchProject == ResearchProjectDef.Named("WatermillGenerator") ||
                researchProject == ResearchProjectDef.Named("GeothermalPower"))
            {
                num += 3f;
            }
        }
        else if (curTrait == TraitDef.Named("VTE_Wanderlust"))
        {
            if (researchProject == ResearchProjectDef.Named("PackagedSurvivalMeal") ||
                researchProject == ResearchProjectDef.Named("TransportPod"))
            {
                num += 3f;
            }
        }
        else if (curTrait == TraitDef.Named("VTE_Technophobe"))
        {
            num -= (int)(researchProject.techLevel - 2);
        }

        return num;
    }
}
