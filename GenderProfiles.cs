using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace GenderVariety
{
	public class GenderProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;

		public string GetNameForVariant(NPC npc) {
			// If the NPC has the alternate gender, use the new name list
			if (GenderVariety.townNPCList.IsAltGender(npc.type) && TownNPCData.AltNames.TryGetValue(npc.type, out List<string> names)) {
				return names[Main.rand.Next(names.Count - 1)];
			}
			return npc.getNewNPCName();
		}

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) {
			TownNPCInfo info = GenderVariety.townNPCList.townNPCs[GenderVariety.townNPCList.GetNPCIndex(npc.type)];
			if (GenderVariety.townNPCList.IsAltGender(npc.type)) {
				if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
					return ModContent.Request<Texture2D>($"GenderVariety/Resources/NPC/NPC_{npc.type}_Default", AssetRequestMode.ImmediateLoad);

				if (npc.altTexture == 1 && !string.IsNullOrEmpty(info.partyPath))
					return ModContent.Request<Texture2D>($"GenderVariety/Resources/NPC/NPC_{npc.type}_Party", AssetRequestMode.ImmediateLoad);

				if (npc.altTexture == 2 && !string.IsNullOrEmpty(info.transformedPath))
					return ModContent.Request<Texture2D>($"GenderVariety/Resources/NPC/NPC_{npc.type}_Transformed", AssetRequestMode.ImmediateLoad);

				if (npc.altTexture == 3 && !string.IsNullOrEmpty(info.creditsPath))
					return ModContent.Request<Texture2D>($"GenderVariety/Resources/NPC/NPC_{npc.type}_Credits", AssetRequestMode.ImmediateLoad);

				return ModContent.Request<Texture2D>($"GenderVariety/Resources/NPC/NPC_{npc.type}_Default", AssetRequestMode.ImmediateLoad);
			}
			else {
				if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
					return Main.Assets.Request<Texture2D>($"Images/{info.defaultPath}", AssetRequestMode.ImmediateLoad);

				if (npc.altTexture == 1 && !string.IsNullOrEmpty(info.partyPath))
						return Main.Assets.Request<Texture2D>($"Images/{info.partyPath}", AssetRequestMode.ImmediateLoad);

				if (npc.altTexture == 2 && !string.IsNullOrEmpty(info.transformedPath))
					return Main.Assets.Request<Texture2D>($"Images/{info.transformedPath}", AssetRequestMode.ImmediateLoad);

				if (npc.altTexture == 3 && !string.IsNullOrEmpty(info.creditsPath))
					return Main.Assets.Request<Texture2D>($"Images/{info.creditsPath}", AssetRequestMode.ImmediateLoad);

				return Main.Assets.Request<Texture2D>($"Images/{info.defaultPath}", AssetRequestMode.ImmediateLoad);
			}
		}

		public int GetHeadTextureIndex(NPC npc) {
			TownNPCSetup setup = GenderVariety.townNPCList;
			return setup.townNPCs[setup.GetNPCIndex(npc.type)].headIndex;
		}
	}
}
