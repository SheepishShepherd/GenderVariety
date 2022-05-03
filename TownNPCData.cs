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

		internal TownNPCInfo(int type, int headIndex, Gender originalGender, string defaultPath) {
			this.type = type;
			this.headIndex = headIndex;
			this.originalGender = originalGender;

			// Define vanilla texture paths
			this.defaultPath = ValidateVanillaPath(defaultPath);
			partyPath = ValidateVanillaPath(defaultPath, "_Party");
			transformedPath = ValidateVanillaPath(defaultPath, "_Transformed");
			creditsPath = ValidateVanillaPath(defaultPath, "_Credits");

			// Define alternate texture paths
			defaultPath_Alt = ValidateModdedPath("GenderVariety/Resources/NPC/" + type + "_Default");
			if (!string.IsNullOrEmpty(partyPath)) {
				partyPath_Alt = "GenderVariety/Resources/NPC/" + type + "_Party";
			}
			if (!string.IsNullOrEmpty(transformedPath)) {
				transformedPath_Alt = "GenderVariety/Resources/NPC/" + type + "_Transformed";
			}
			if (!string.IsNullOrEmpty(creditsPath)) {
				creditsPath_Alt = "GenderVariety/Resources/NPC/" + type + "_Credits";
			}
		}

		// TODO: TownNPCInfo for modded town npcs?
		/*
		internal TownNPCInfo(int type, Gender originalGender, string defaultPath, string altPath) {
			this.type = type;
			this.headIndex = NPCHeadLoader.GetHeadSlot(ModContent.GetModNPC(type).HeadTexture);
			this.originalGender = originalGender;
		}
		*/


		// Make sure the path provided has an existing texture associated with it
		// The result will be using Main.Assets.Requst<Texture2D>(), so remove the 'Terraria/' bit
		string ValidateVanillaPath(string texturePath, string extension = "") {
			if (ModContent.HasAsset("Terraria/Images/" + texturePath + extension)) {
				return "Images/" + texturePath + extension;
			}
			return null;
		}

		string ValidateModdedPath(string texturePath) => ModContent.HasAsset(texturePath) ? texturePath : null;
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
				// Ordered by NPCHeadID (Mostly in order by NPCID, with the exception of the Guide being first)
				new TownNPCInfo(NPCID.Guide, NPCHeadID.Guide, Gender.Male, "NPC_22"),
				new TownNPCInfo(NPCID.Merchant, NPCHeadID.Merchant, Gender.Male, "TownNPCs/Merchant_Default"),
				new TownNPCInfo(NPCID.Nurse, NPCHeadID.Nurse, Gender.Female, "TownNPCs/Nurse_Default"),
				new TownNPCInfo(NPCID.Demolitionist, NPCHeadID.Demolitionist, Gender.Male, "TownNPCs/Demolitionist_Default"),
				new TownNPCInfo(NPCID.Dryad, NPCHeadID.Dryad, Gender.Female, "NPC_20"),
				new TownNPCInfo(NPCID.ArmsDealer, NPCHeadID.ArmsDealer, Gender.Male, "NPC_19"),
				new TownNPCInfo(NPCID.Clothier, NPCHeadID.Clothier, Gender.Male, "TownNPCs/Clothier_Default"),
				new TownNPCInfo(NPCID.Mechanic, NPCHeadID.Mechanic, Gender.Female, "TownNPCs/Mechanic_Default"),
				new TownNPCInfo(NPCID.GoblinTinkerer, NPCHeadID.GoblinTinkerer, Gender.Male, "NPC_107"),
				new TownNPCInfo(NPCID.Wizard, NPCHeadID.Wizard, Gender.Male, "TownNPCs/Wizard_Default"),
				new TownNPCInfo(NPCID.SantaClaus, NPCHeadID.SantaClaus, Gender.Male, "TownNPCs/Santa_Default"),
				new TownNPCInfo(NPCID.Truffle, NPCHeadID.Truffle, Gender.Male, "NPC_160"),
				new TownNPCInfo(NPCID.Steampunker, NPCHeadID.Steampunker, Gender.Female, "TownNPCs/Steampunker_Default"),
				new TownNPCInfo(NPCID.DyeTrader, NPCHeadID.DyeTrader, Gender.Male, "TownNPCs/DyeTrader_Default"),
				new TownNPCInfo(NPCID.PartyGirl, NPCHeadID.PartyGirl, Gender.Female, "NPC_208"),
				new TownNPCInfo(NPCID.Cyborg, NPCHeadID.Cyborg, Gender.Male, "TownNPCs/Cyborg_Default"),
				new TownNPCInfo(NPCID.Painter, NPCHeadID.Painter, Gender.Male, "TownNPCs/Painter_Default"),
				new TownNPCInfo(NPCID.WitchDoctor, NPCHeadID.WitchDoctor, Gender.Male, "NPC_228"),
				new TownNPCInfo(NPCID.Pirate, NPCHeadID.Pirate, Gender.Male, "TownNPCs/Pirate_Default"),
				new TownNPCInfo(NPCID.Stylist, NPCHeadID.Stylist, Gender.Female, "TownNPCs/Stylist_Default"),
				// There is an additional condition where the Traveling merchant can despawn
				// Until I figure out how I can easily approach this, the traveling merchant will not be affected
				//new TownNPCInfo(NPCID.TravellingMerchant, NPCHeadID.TravellingMerchant, Gender.Male, "TownNPCs/TravelingMerchant_Default"),
				new TownNPCInfo(NPCID.Angler, NPCHeadID.Angler, Gender.Male, "TownNPCs/Angler_Default"),
				new TownNPCInfo(NPCID.TaxCollector, NPCHeadID.TaxCollector, Gender.Male, "TownNPCs/TaxCollector_Default"),
				new TownNPCInfo(NPCID.DD2Bartender, NPCHeadID.DD2Bartender, Gender.Male, "NPC_550"),
				new TownNPCInfo(NPCID.Golfer, NPCHeadID.Golfer, Gender.Male, "TownNPCs/Golfer_Default"),
				new TownNPCInfo(NPCID.BestiaryGirl, NPCHeadID.BestiaryGirl, Gender.Female, "TownNPCs/BestiaryGirl_Default"),
				new TownNPCInfo(NPCID.Princess, NPCHeadID.Princess, Gender.Female, "TownNPCs/Princess_Default"),
			};
		}

		internal int GetNPCIndex(int npcType) => townNPCs.FindIndex(x => x.type == npcType);

		internal TownNPCInfo GetNPCInfo(int npcType) => townNPCs[GetNPCIndex(npcType)];

		internal bool IsAltGender(int npcType) {
			// If the index is invalid, the gender can't be the alternate
			if (TownNPCWorld.SavedGenders is null || GenderVariety.townNPCList.GetNPCIndex(npcType) == -1) {
				return false;
			}

			if (TownNPCWorld.SavedGenders.TryGetValue(new NPCDefinition(npcType), out Gender savedGender)) {
				Gender originalGender = GenderVariety.townNPCList.GetNPCInfo(npcType).originalGender;
				return savedGender != Gender.Unassigned && savedGender != originalGender;
			}

			return false;
		}
	}
}
