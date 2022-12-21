using System.Collections.Generic;
using System.Linq;
using Verse;

namespace PawnsChooseResearch;

public class ResearchCategory : DefModExtension
{
    public int animalTech = 0;

    public int artTech = 0;

    public int complexTech = 0;

    public int constructionTech = 0;

    public int cookingTech = 0;
    public int coreTech = 0;

    public int craftTech = 0;

    public int cyberTech = 0;

    public int drugTech = 0;

    public int medTech = 0;

    public int meleeTech = 0;

    public int miningTech = 0;

    public int neverTech = 0;

    public int plantTech = 0;

    public int progressTech = 0;

    public int rangedTech = 0;

    public int socialTech = 0;

    public override string ToString()
    {
        return
            $"animalTech: {animalTech}, artTech: {artTech}, complexTech: {complexTech}, constructionTech: {constructionTech}, cookingTech: {cookingTech}, coreTech: {coreTech}, craftTech: {craftTech}, rangedTech: {rangedTech}, " +
            $"cyberTech: {cyberTech}, drugTech: {drugTech}, medTech: {medTech}, meleeTech: {meleeTech}, miningTech: {miningTech}, neverTech: {neverTech}, plantTech: {plantTech}, progressTech: {progressTech}, socialTech: {socialTech}";
    }

    public static void CategoryCheck()
    {
        Startup.LogMessage("Running Test");
        foreach (var item in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
        {
            if (!item.HasModExtension<ResearchCategory>())
            {
                Log.Warning($"{item.label} is missing research category def mod extension.");
            }

            if (item.GetModExtension<ResearchCategory>().coreTech > 0)
            {
                Startup.LogMessage($"{item.label} is a core tech.");
            }

            if (item.GetModExtension<ResearchCategory>().neverTech > 0)
            {
                Startup.LogMessage($"{item.label} will  be researched last.");
            }

            if (item.GetModExtension<ResearchCategory>().cyberTech > 0)
            {
                Startup.LogMessage($"{item.label} is abhorred by body purists.");
            }

            if (item.GetModExtension<ResearchCategory>().coreTech == 0 &&
                item.GetModExtension<ResearchCategory>().progressTech == 0 &&
                item.GetModExtension<ResearchCategory>().complexTech == 0 &&
                item.GetModExtension<ResearchCategory>().rangedTech == 0 &&
                item.GetModExtension<ResearchCategory>().meleeTech == 0 &&
                item.GetModExtension<ResearchCategory>().constructionTech == 0 &&
                item.GetModExtension<ResearchCategory>().miningTech == 0 &&
                item.GetModExtension<ResearchCategory>().cookingTech == 0 &&
                item.GetModExtension<ResearchCategory>().plantTech == 0 &&
                item.GetModExtension<ResearchCategory>().animalTech == 0 &&
                item.GetModExtension<ResearchCategory>().craftTech == 0 &&
                item.GetModExtension<ResearchCategory>().artTech == 0 &&
                item.GetModExtension<ResearchCategory>().medTech == 0 &&
                item.GetModExtension<ResearchCategory>().socialTech == 0 &&
                item.GetModExtension<ResearchCategory>().neverTech == 0)
            {
                Startup.LogMessage($"{item.label} is not categorized.");
            }
        }
    }

    public bool IsSetToSomething()
    {
        var intType = GetType().GetFields();

        return intType.Any(info => info.FieldType == typeof(int) && (int)info.GetValue(this) != 0);
    }

    public static void TestProjectCheck()
    {
        var researchProjectDef = ResearchProjectDef.Named("Epona_Research_SteamGatling");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s coreTech score is {researchProjectDef.GetModExtension<ResearchCategory>().coreTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s progressTech score is {researchProjectDef.GetModExtension<ResearchCategory>().progressTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s complexTech score is {researchProjectDef.GetModExtension<ResearchCategory>().complexTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s rangedTech score is {researchProjectDef.GetModExtension<ResearchCategory>().rangedTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s meleeTech score is {researchProjectDef.GetModExtension<ResearchCategory>().meleeTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s constructionTech score is {researchProjectDef.GetModExtension<ResearchCategory>().constructionTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s miningTech score is {researchProjectDef.GetModExtension<ResearchCategory>().miningTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s cookingTech score is {researchProjectDef.GetModExtension<ResearchCategory>().cookingTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s plantTech score is {researchProjectDef.GetModExtension<ResearchCategory>().plantTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s animalTech score is {researchProjectDef.GetModExtension<ResearchCategory>().animalTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s craftTech score is {researchProjectDef.GetModExtension<ResearchCategory>().craftTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s artTech score is {researchProjectDef.GetModExtension<ResearchCategory>().artTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s medTech score is {researchProjectDef.GetModExtension<ResearchCategory>().medTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s socialTech score is {researchProjectDef.GetModExtension<ResearchCategory>().socialTech}");
        Startup.LogMessage(
            $"{researchProjectDef.label}'s cyberTech score is {researchProjectDef.GetModExtension<ResearchCategory>().cyberTech}");
    }

    private static void CECheck()
    {
        var list = new List<ResearchProjectDef>
        {
            ResearchProjectDef.Named("CE_TurretHeavyWeapons"),
            ResearchProjectDef.Named("CE_ChargeTurret"),
            ResearchProjectDef.Named("CE_HeavyTurret"),
            ResearchProjectDef.Named("CE_Launchers"),
            ResearchProjectDef.Named("CE_AdvancedLaunchers"),
            ResearchProjectDef.Named("CE_AdvancedAmmo"),
            ResearchProjectDef.Named("VFES_Artillery_Debug")
        };
        for (var i = 0; i < list.Count; i++)
        {
            Startup.LogMessage(
                $"{list[i].label} ranged tech is {list[i].GetModExtension<ResearchCategory>().rangedTech}");
            Startup.LogMessage(
                $"{list[i].label} never tech is {list[i].GetModExtension<ResearchCategory>().neverTech}");
        }
    }
}