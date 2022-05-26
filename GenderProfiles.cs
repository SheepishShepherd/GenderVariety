using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace GenderVariety
{
	public class GenderProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;

		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public int GetHeadTextureIndex(NPC npc) => GenderVariety.townNPCList.GetNPCInfo(npc.type).headIndex;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) {
			TownNPCInfo info = GenderVariety.townNPCList.GetNPCInfo(npc.type);
			if (GenderVariety.townNPCList.IsAltGender(npc.type)) {
				if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
					return ModContent.Request<Texture2D>(info.defaultPath_Alt);

				if (npc.altTexture == 1 && !string.IsNullOrEmpty(info.partyPath_Alt))
					return ModContent.Request<Texture2D>(info.partyPath_Alt);

				if (npc.altTexture == 2 && !string.IsNullOrEmpty(info.transformedPath_Alt))
					return ModContent.Request<Texture2D>(info.transformedPath_Alt);

				if (npc.altTexture == 3 && !string.IsNullOrEmpty(info.creditsPath_Alt))
					return ModContent.Request<Texture2D>(info.creditsPath_Alt);

				return ModContent.Request<Texture2D>(info.defaultPath_Alt);
			}
			else {
				if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
					return Main.Assets.Request<Texture2D>(info.defaultPath);

				if (npc.altTexture == 1 && !string.IsNullOrEmpty(info.partyPath))
					return Main.Assets.Request<Texture2D>(info.partyPath);

				if (npc.altTexture == 2 && !string.IsNullOrEmpty(info.transformedPath))
					return Main.Assets.Request<Texture2D>(info.transformedPath);

				if (npc.altTexture == 3 && !string.IsNullOrEmpty(info.creditsPath))
					return Main.Assets.Request<Texture2D>(info.creditsPath);

				return Main.Assets.Request<Texture2D>(info.defaultPath);
			}
		}
	}
}
