using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace GenderVariety
{
	// TODO: Update NPC textures (and more) for Multiplayer

	public class GenderVariety : Mod
	{
		internal static TownNPCSetup townNPCList;

		public GenderVariety() {

		}

		public override void Load() {
			townNPCList = new TownNPCSetup();
		}

		public override void Unload() {
			// Reset the textures!
			for (int i = 0; i < townNPCList.townNPCs.Count; i++) {
				TownNPCInfo townNPC = townNPCList.townNPCs[i];
				Main.npcTexture[townNPC.type] = townNPC.npcTexture;
				Main.npcHeadTexture[townNPC.headIndex] = townNPC.npcTexture_Head;
			}
			townNPCList = null;
		}

		public static void SendDebugMessage(string message, Color color = default) {
			if (ModContent.GetInstance<GVConfig>().EnableDebugMode) Main.NewText(message, color);
		}
	}
}
