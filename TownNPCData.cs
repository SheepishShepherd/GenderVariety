using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace GenderVariety
{
	internal class TownNPCInfo
	{
		internal int type;
		internal int headIndex;
		internal Gender originalGender;

		internal string defaultPath;
		internal string partyPath;
		internal string transformedPath;
		internal string creditsPath;

		internal string defaultPath_Alt;
		internal string partyPath_Alt;
		internal string transformedPath_Alt;
		internal string creditsPath_Alt;

		internal TownNPCInfo(int type, int headIndex, Gender originalGender, string defaultPath, string partyPath = "noPath") {
			this.type = type;
			this.headIndex = headIndex;
			this.originalGender = originalGender;

			this.defaultPath = ModContent.HasAsset($"Terraria/Images/" + defaultPath) ? "Images/" + defaultPath : null;
			this.partyPath = ModContent.HasAsset("Terraria/Images/" + partyPath) ? "Images/" + partyPath : null;
			transformedPath = type == NPCID.BestiaryGirl ? "Images/TownNPCs/BestiaryGirl_Default_Transformed" : null;
			creditsPath = type == NPCID.BestiaryGirl ? "Images/TownNPCs/BestiaryGirl_Default_Credits" : null;

			defaultPath_Alt = "GenderVariety/Resources/NPC/" + type + "_Default";
			if (!string.IsNullOrEmpty(this.partyPath)) {
				defaultPath_Alt = "GenderVariety/Resources/NPC/" + type + "_Party";
			}
			if (!string.IsNullOrEmpty(transformedPath)) {
				defaultPath_Alt = "GenderVariety/Resources/NPC/" + type + "_Transformed";
			}
			if (!string.IsNullOrEmpty(creditsPath)) {
				defaultPath_Alt = "GenderVariety/Resources/NPC/" + type + "_Credits";
			}
		}
	}

	internal class TownNPCSetup
	{
		public TownNPCSetup() {
			GenderVariety.townNPCList = this;
			InitializeTownNPCList();
		}

		internal List<TownNPCInfo> townNPCs;

		private void InitializeTownNPCList() {
			townNPCs = new List<TownNPCInfo>() {
				// Ordered by wiki
				new TownNPCInfo(NPCID.Guide, NPCHeadID.Guide, Gender.Male, "NPC_22"),
				new TownNPCInfo(NPCID.Merchant, NPCHeadID.Merchant, Gender.Male, "TownNPCs/Merchant_Default", "TownNPCs/Merchant_Default_Party"),
				new TownNPCInfo(NPCID.Nurse, NPCHeadID.Nurse, Gender.Female, "TownNPCs/Nurse_Default", "TownNPCs/Nurse_Default_Party"),
				new TownNPCInfo(NPCID.Demolitionist, NPCHeadID.Demolitionist, Gender.Male, "TownNPCs/Demolitionist_Default", "TownNPCs/Demolitionist_Default_Party"),
				new TownNPCInfo(NPCID.DyeTrader, NPCHeadID.DyeTrader, Gender.Male, "TownNPCs/DyeTrader_Default", "TownNPCs/DyeTrader_Default_Party"),
				new TownNPCInfo(NPCID.Angler, NPCHeadID.Angler, Gender.Male, "TownNPCs/Angler_Default", "TownNPCs/Angler_Default_Party"),
				new TownNPCInfo(NPCID.BestiaryGirl, NPCHeadID.BestiaryGirl, Gender.Female, "TownNPCs/BestiaryGirl_Default", "TownNPCs/BestiaryGirl_Default_Party"),
				new TownNPCInfo(NPCID.Dryad, NPCHeadID.Dryad, Gender.Female, "NPC_20"),
				new TownNPCInfo(NPCID.Painter, NPCHeadID.Painter, Gender.Male, "TownNPCs/Painter_Default", "TownNPCs/Painter_Default_Party"),
				new TownNPCInfo(NPCID.Golfer, NPCHeadID.Golfer, Gender.Male, "TownNPCs/Golfer_Default", "TownNPCs/Golfer_Default_Party"),
				new TownNPCInfo(NPCID.ArmsDealer, NPCHeadID.ArmsDealer, Gender.Male, "NPC_19"),
				//TownNPCInfo.AddTownNPC(NPCID.DD2Bartender, 24, true),
				new TownNPCInfo(NPCID.Stylist, NPCHeadID.Stylist, Gender.Female, "TownNPCs/Stylist_Default", "TownNPCs/Stylist_Default_Party"),
				new TownNPCInfo(NPCID.GoblinTinkerer, NPCHeadID.GoblinTinkerer, Gender.Male, "NPC_107"),
				//TownNPCInfo.AddTownNPC(NPCID.WitchDoctor, 18, true),
				new TownNPCInfo(NPCID.Clothier, NPCHeadID.Clothier, Gender.Male, "TownNPCs/Clothier_Default", "TownNPCs/Clothier_Default_Party"),
				new TownNPCInfo(NPCID.Mechanic, NPCHeadID.Mechanic, Gender.Female, "TownNPCs/Mechanic_Default", "TownNPCs/Mechanic_Default_Party"),
				new TownNPCInfo(NPCID.PartyGirl, NPCHeadID.PartyGirl, Gender.Female, "NPC_208"),

				new TownNPCInfo(NPCID.Wizard, NPCHeadID.Wizard, Gender.Male, "TownNPCs/Wizard_Default", "TownNPCs/Wizard_Default_Party"),
				//TownNPCInfo.AddTownNPC(NPCID.TaxCollector, 23, true),
				new TownNPCInfo(NPCID.Truffle, NPCHeadID.Truffle, Gender.Male, "NPC_160"),
				new TownNPCInfo(NPCID.Pirate, NPCHeadID.Pirate, Gender.Male, "TownNPCs/Pirate_Default", "TownNPCs/Pirate_Default_Party"),
				new TownNPCInfo(NPCID.Steampunker, NPCHeadID.Steampunker, Gender.Female, "TownNPCs/Steampunker_Default", "TownNPCs/Steampunker_Default_Party"),
				new TownNPCInfo(NPCID.Cyborg, NPCHeadID.Cyborg, Gender.Male, "TownNPCs/Cyborg_Default", "TownNPCs/Cyborg_Default_Party"),
				new TownNPCInfo(NPCID.SantaClaus, NPCHeadID.SantaClaus, Gender.Male, "TownNPCs/Santa_Default", "TownNPCs/Santa_Default_Party"),
				//new TownNPCInfo(NPCID.Princess, NPCHeadID.Princess, Gender.Female, "TownNPCs/Princess_Default", "TownNPCs/Princess_Default_Party"),

				//TownNPCInfo.AddTownNPC(NPCID.TravellingMerchant, 21, true),
			};
		}

		internal int GetNPCIndex(int npcType) => townNPCs.FindIndex(x => x.type == npcType);

		internal TownNPCInfo GetNPCInfo(int npcType) => townNPCs[GetNPCIndex(npcType)];

		internal bool IsAltGender(int npcType) {
			// If the index is invalid, the gender can't be the alternate.
			int index = GenderVariety.townNPCList.GetNPCIndex(npcType);
			if (index == -1) {
				return false;
			}
			if (TownNPCWorld.SavedGenders is null) {
				return false;
			}

			if (TownNPCWorld.SavedGenders.TryGetValue(new NPCDefinition(npcType), out Gender savedGender)) {
				Gender originalGender = GenderVariety.townNPCList.GetNPCInfo(npcType).originalGender;
				return savedGender == Gender.Unassigned ? false : savedGender != originalGender;
			}

			return false;
		}
	}
}
