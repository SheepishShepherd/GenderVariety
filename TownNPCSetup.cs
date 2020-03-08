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
				SavedData.Add(new TownNPCData(list.type, GenderVariety.Unassigned, "", ""));
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
			// Setup textures when data is loaded
			for (int i = 0; i < SavedData.Count; i++) {
				TownNPCData.SwapTextures(i, SavedData[i].type);
			}
		}
	}

	public class TownNPCs : GlobalNPC
	{
		public override void NPCLoot(NPC npc) {
			// Reset saved data here
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (index == -1) return;
			TownNPCWorld.SavedData[index] = new TownNPCData(npc.type, GenderVariety.Unassigned, "", "");
		}

		// Swapped genders go to respective statues
		public override bool? CanGoToStatue(NPC npc, bool toKingStatue) {
			int index = GenderVariety.GetNPCIndex(npc.type);
			if (index != -1 && npc.type != NPCID.SantaClaus && TownNPCInfo.IsAltGender(npc)) { // Santa doesnt teleport in vanilla
				TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
				return toKingStatue ? !townNPC.isMale : townNPC.isMale; // Since we check for altGender, we do the opposite
			}
			return null;
		}

		public override bool PreAI(NPC npc) {
			// Textures are updated in preAI to update when needed, since theres only two states
			
			return true;
		}

		// TODO: Change some chat texts
		//public override void GetChat(NPC npc, ref string chat)
	}
}
