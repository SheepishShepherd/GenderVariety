using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.GameContent;

namespace GenderVariety
{
	public class TownNPCWorld : ModSystem
	{
		internal static List<TownNPCData> SavedData;

		public override void OnWorldLoad()
		{
			SavedData = new List<TownNPCData>();
			for (int i = 0; i < GenderVariety.townNPCList.townNPCs.Count; i++) {
				TownNPCInfo list = GenderVariety.townNPCList.townNPCs[i];
				SavedData.Add(new TownNPCData(list.type, (int)Gender.Unassigned, "", ""));
			}
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag["SavedTownNPCData"] = SavedData;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			List<TownNPCData> temp = tag.Get<List<TownNPCData>>("SavedTownNPCData");
			foreach (TownNPCData data in temp) {
				int index = SavedData.FindIndex(x => x.type == data.type);
				if (index == -1) {
					SavedData.Add(data);
					continue;
				}
				SavedData[index] = data;
			}

			// Any unassigned gender's should be randomized for the next NPCs
			// TODO: Apply Forced Male/Female spawns here
			foreach (TownNPCData data in SavedData) {
				if (data.savedGender == (int)Gender.Unassigned) {
					data.savedGender = Main.rand.NextBool() ? (int)Gender.Male : (int)Gender.Female;
				}
			}

			// Apply the proper Head Textures on world load. They will be reset when the mod is unloaded.
			for (int i = 0; i < SavedData.Count; i++) {
				TownNPCData.SwapHeadTexture(SavedData[i].type);
			}
		}
	}

	public class TownNPCs : GlobalNPC
	{
		public override void SetTownNPCProfile(NPC npc, Dictionary<int, ITownNPCProfile> database) {
			if (GenderVariety.townNPCList.GetNPCIndex(npc.type) != -1) {
				database[npc.type] = new GenderProfile();
			}
		}

		public override void ModifyNPCNameList(NPC npc, List<string> nameList) {
			if (GenderVariety.townNPCList.IsAltGender(npc.type)) {
				if (TownNPCData.AltNames.TryGetValue(npc.type, out List<string> altNames)) {
					nameList = altNames;
				}
			}
		}

		public override void ModifyTypeName(NPC npc, ref string typeName) { 
			if (GenderVariety.townNPCList.IsAltGender(npc.type)) {
				if (npc.type == NPCID.PartyGirl) {
					typeName = "Party Boy"; // TODO: Translations
				}
				else if (npc.type == NPCID.SantaClaus) {
					typeName = "Mrs. Claus";
				}
			}
		}

		public override void OnKill(NPC npc) {
			int index = GenderVariety.townNPCList.GetNPCIndex(npc.type);
			if (index == -1) {
				return;
			}

			// When a Town NPC dies, we will determine the gender of the next one.
			Gender NewGender = Main.rand.NextBool() ? Gender.Male : Gender.Female;
			if (ModContent.GetInstance<GVConfig>().ForcedFemale.FindIndex(x => x.Type == npc.type) != -1) {
				NewGender = Gender.Female;
			}
			else if (ModContent.GetInstance<GVConfig>().ForcedMale.FindIndex(x => x.Type == npc.type) != -1) {
				NewGender = Gender.Male;
			}
			TownNPCWorld.SavedData[index] = new TownNPCData(npc.type, (int)NewGender, "", "");
		}

		// Swapped genders go to respective statues
		public override bool? CanGoToStatue(NPC npc, bool toKingStatue)	
		{
			int index = GenderVariety.townNPCList.GetNPCIndex(npc.type);
			if (index == -1 || npc.type == NPCID.SantaClaus) {
				// Santa doesnt teleport in vanilla
				return null;
			}
			else if (GenderVariety.townNPCList.IsAltGender(npc.type)) {
				TownNPCData npcData = TownNPCWorld.SavedData[index];
				return toKingStatue ? npcData.savedGender == (int)Gender.Female : npcData.savedGender == (int)Gender.Male;
			}
			return null;
		}

		// TODO: Change some chat texts
		//public override void GetChat(NPC npc, ref string chat)
	}
}
