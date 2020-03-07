using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace GenderVariety
{
	public class GVConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Label("Choose which NPCs will always spawn as MALE")]
		public List<NPCDefinition> ForcedMale { get; set; } = new List<NPCDefinition>();

		[Label("Choose which NPCs will always spawn as FEMALE")]
		public List<NPCDefinition> ForcedFemale { get; set; } = new List<NPCDefinition>();

		[DefaultValue(false)]
		[Label("Enable Debug Mode")]
		public bool EnableDebugMode { get; set; }
	}
}
