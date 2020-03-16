using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace GenderVariety
{
	public class TownNPCWorld : ModWorld
	{
		internal static List<TownNPCData> SavedData;

		public override void Initialize() {
			SavedData = new List<TownNPCData>();
			for (int i = 0; i < GenderVariety.townNPCList.townNPCs.Count; i++) {
				TownNPCInfo list = GenderVariety.townNPCList.townNPCs[i];
				SavedData.Add(new TownNPCData(list.type, TownNPCSetup.Unassigned, "", ""));
			}
		}

		public override TagCompound Save() {
			TagCompound savedTag = new TagCompound {
				{"SavedTownNPCData", SavedData}
			};
			return savedTag;
		}

		public override void Load(TagCompound tag) {
			SavedData = tag.Get<List<TownNPCData>>("SavedTownNPCData");
		}
	}

	public class TownNPCs : GlobalNPC
	{
		public override void NPCLoot(NPC npc) { // Reset saved data here
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (index == -1) return;
			TownNPCWorld.SavedData[index] = new TownNPCData(npc.type, TownNPCSetup.Unassigned, "", "");
		}

		// Swapped genders go to respective statues
		public override bool? CanGoToStatue(NPC npc, bool toKingStatue) {
			int index = GenderVariety.GetNPCIndex(npc.type);
			if (index != -1 && npc.type != NPCID.SantaClaus && GenderVariety.townNPCList.npcIsAltGender[index]) {
				TownNPCData npcData = TownNPCWorld.SavedData[index]; // Santa doesnt teleport in vanilla
				return toKingStatue ? npcData.savedGender == TownNPCSetup.Female : npcData.savedGender == TownNPCSetup.Male; // Since we check for altGender, we do the opposite
			}
			return null;
		}

		// TODO: Change some chat texts
		//public override void GetChat(NPC npc, ref string chat)
	}
	
	internal class TownNPCPlayer : ModPlayer
	{
		public override void OnEnterWorld(Player player) {
			// Setup textures when data is loaded
			for (int i = 0; i < TownNPCWorld.SavedData.Count; i++) {
				if (TownNPCWorld.SavedData[i].savedGender == TownNPCSetup.Unassigned) GenderVariety.townNPCList.npcIsAltGender[i] = false;
				else GenderVariety.townNPCList.npcIsAltGender[i] = TownNPCWorld.SavedData[i].savedGender != GenderVariety.townNPCList.townNPCs[i].ogGender;
				TownNPCData.SwapTextures(i, TownNPCWorld.SavedData[i].type);
			}
		}
	}
}
