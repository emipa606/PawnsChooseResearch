//using DInterests;
//using RimWorld;
//using Verse;

//namespace PawnsChooseResearch;

//public class Interests_Integration
//{
//	public static float GetInterestScore(Pawn pawn, ResearchProjectDef researchProject)
//	{
//		float num = 0f;
//		bool flag = false;
//		if (researchProject.GetModExtension<ResearchCategory>().rangedTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Shooting).passion) / 100f;
//			flag = true;
//		}
//		if (researchProject.GetModExtension<ResearchCategory>().meleeTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Melee).passion) / 100f;
//			flag = true;
//		}
//		if (researchProject.GetModExtension<ResearchCategory>().constructionTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Construction).passion) / 100f;
//			flag = true;
//		}
//		if (researchProject.GetModExtension<ResearchCategory>().miningTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Mining).passion) / 100f;
//			flag = true;
//		}
//		if (researchProject.GetModExtension<ResearchCategory>().cookingTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Cooking).passion) / 100f;
//			flag = true;
//		}
//		if (researchProject.GetModExtension<ResearchCategory>().plantTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Plants).passion) / 100f;
//			flag = true;
//		}
//		if (researchProject.GetModExtension<ResearchCategory>().animalTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Animals).passion) / 100f;
//			flag = true;
//		}
//		if (researchProject.GetModExtension<ResearchCategory>().craftTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Crafting).passion) / 100f;
//			flag = true;
//		}
//		if (researchProject.GetModExtension<ResearchCategory>().artTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Artistic).passion) / 100f;
//			flag = true;
//		}
//		if (researchProject.GetModExtension<ResearchCategory>().medTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Medicine).passion) / 100f;
//			flag = true;
//		}
//		if (researchProject.GetModExtension<ResearchCategory>().socialTech > 0)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Social).passion) / 100f;
//			flag = true;
//		}
//		if (!flag)
//		{
//			num += InterestBase.GetValue((int)pawn.skills.GetSkill(SkillDefOf.Intellectual).passion) / 100f;
//		}
//		return num;
//	}
//}

