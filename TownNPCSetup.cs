using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.Reflection;

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
		}
	}

	public class TownNPCs : GlobalNPC
	{
		public static bool IsAltGender(NPC npc) {
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (index == -1) return false;
			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			TownNPCData npcData = TownNPCWorld.SavedData[index];
			return (townNPC.isMale && npcData.savedGender == GenderVariety.Female) || (!townNPC.isMale && npcData.savedGender == GenderVariety.Male);
		}

		public override void NPCLoot(NPC npc) {
			// Reset saved data here
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (index == -1) return;
			TownNPCWorld.SavedData[index] = new TownNPCData(npc.type, GenderVariety.Unassigned, "", "");
		}

		// Swapped genders go to respective statues
		public override bool? CanGoToStatue(NPC npc, bool toKingStatue) {
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (IsAltGender(npc) && index != -1 && npc.type != NPCID.SantaClaus) { // Santa doesnt teleport in vanilla
				TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
				return toKingStatue ? !townNPC.isMale : townNPC.isMale; // Since we check for altGender, we do the opposite
			}
			return null;
		}

		public override bool PreAI(NPC npc) {
			// Textures are updated in preAI to update when needed, since theres only two states
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (index == -1) return true;

			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			if (IsAltGender(npc)) {
				Main.npcTexture[npc.type] = townNPC.npcAltTexture;
				Main.npcHeadTexture[townNPC.headIndex] = townNPC.npcAltTexture_Head;
			}
			else {
				Main.npcTexture[npc.type] = townNPC.npcTexture;
				Main.npcHeadTexture[townNPC.headIndex] = townNPC.npcTexture_Head;
			}
			return true;
		}

		// TODO: Change some chat texts
		public override void GetChat(NPC npc, ref string chat) {
			base.GetChat(npc, ref chat);
		}
	}
}
