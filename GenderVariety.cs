using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
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
			// Reset any NPC Head textures that may have be swapped.
			for (int i = 0; i < TextureAssets.NpcHead.Length; i++) {
				TextureAssets.NpcHead[i] = Main.Assets.Request<Texture2D>("Images/NPC_Head_" + i, AssetRequestMode.ImmediateLoad);
			}
			townNPCList = null;
		}

		public static void SendDebugMessage(string message, Color color = default) {
			if (ModContent.GetInstance<GVConfig>().EnableDebugMode) 
				Main.NewText(message, color);
		}
	}
}
