using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace GenderVariety
{
	public class GenderVariety : Mod
	{
		internal static TownNPCSetup townNPCList;

		public GenderVariety() {

		}

		public override void Load() {
			townNPCList = new TownNPCSetup();
		}

		public override void Unload() {
			for (int i = 0; i < townNPCList.townNPCs.Count; i++) {
				TownNPCData.SwapHeadTexture(townNPCList.townNPCs[i].type, true);
			}
			townNPCList = null;
		}

		public static void SendDebugMessage(string message, Color color = default) {
			if (ModContent.GetInstance<GVConfig>().EnableDebugMode) 
				Main.NewText(message, color);
		}
	}
}
