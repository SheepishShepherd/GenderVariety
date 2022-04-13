using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

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
		}
	}

	public class TownNPCs : GlobalNPC
	{
		public override void OnKill(NPC npc)
		{
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (index == -1) {
				return;
			}
			TownNPCWorld.SavedData[index] = new TownNPCData(npc.type, (int)Gender.Unassigned, "", "");
		}

		// Swapped genders go to respective statues
		public override bool? CanGoToStatue(NPC npc, bool toKingStatue)	
		{
			int index = GenderVariety.townNPCList.GetNPCIndex(npc.type);
			if (index != -1 && npc.type != NPCID.SantaClaus && GenderVariety.townNPCList.IsAltGender(npc.type)) {
				TownNPCData npcData = TownNPCWorld.SavedData[index]; // Santa doesnt teleport in vanilla
				return toKingStatue ? npcData.savedGender == (int)Gender.Female : npcData.savedGender == (int)Gender.Male;
			}
			return null;
		}

		// TODO: Change some chat texts
		//public override void GetChat(NPC npc, ref string chat)
	}
	
	internal class TownNPCPlayer : ModPlayer
	{
		public override void OnEnterWorld(Player player)
		{
			// Setup textures when data is loaded
			for (int i = 0; i < TownNPCWorld.SavedData.Count; i++) {
				TownNPCData.SwapTextures(i, TownNPCWorld.SavedData[i].type);
			}
		}
	}
}
