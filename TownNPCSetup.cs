using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace GenderVariety
{
	public class TownNPCWorld : ModSystem
	{
		internal static Dictionary<NPCDefinition, Gender> SavedGenders;
		internal static List<NPC> AltGenderWorldGen = new List<NPC>();

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {
			// Randomizing the gender of Town NPCs on World Generating tasks will be needed.
			// Some seeds add other npcs or replace the guide, so we want to be sure to account for all of them.
			int GuideIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Guide"));
			if (GuideIndex != -1) {
				tasks.Insert(GuideIndex + 1, new GuideGenderModifier("Assigning town npc gender", 1f));
			}
		}

		public override void OnWorldLoad() {
			// Initialize SavedGenders, defaulting all genders to unassigned
			SavedGenders = new Dictionary<NPCDefinition, Gender>();
			for (int i = 0; i < GenderVariety.townNPCList.townNPCs.Count; i++) {
				TownNPCInfo list = GenderVariety.townNPCList.townNPCs[i];
				SavedGenders.Add(new NPCDefinition(list.type), Gender.Unassigned);
			}

			// Loot through all NPCs that have been flagged for an alternate gender during world gen
			// Assign the genders for them after SavedGenders is initialized
			foreach (NPC npc in AltGenderWorldGen) {
				Gender originalGender = GenderVariety.townNPCList.GetNPCInfo(npc.type).originalGender;
				Gender newGender = originalGender == Gender.Male ? Gender.Female : Gender.Male;
				TownNPCs.AssignGender(npc, newGender);
			}
		}

		public override void SaveWorldData(TagCompound tag) {
			TagCompound TempGenders = new TagCompound();
			if (SavedGenders is not null) {
				foreach (KeyValuePair<NPCDefinition, Gender> entry in SavedGenders) {
					NPCDefinition npc = new NPCDefinition(entry.Key.Type);
					TempGenders.Add(npc.Mod + " " + npc.Name, (int)entry.Value);
				}
				tag["SavedGenders"] = TempGenders;
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			TagCompound SavedData = tag.Get<TagCompound>("SavedGenders");
			foreach (KeyValuePair<string, object> entry in SavedData) {
				// Get the NPCDefinition key, first string being Mod and second being NPC Name
				string[] splitKey = entry.Key.Split(" ", 2, StringSplitOptions.None);
				NPCDefinition npcDef = new NPCDefinition(splitKey[0], splitKey[1]);

				// Attempt to grab and set any saved genders
				if (SavedGenders.TryGetValue(npcDef, out Gender setGender)) {
					// Skip over any NPCs that spawned during World Gen
					if (AltGenderWorldGen.FindIndex(npc => npc.type == npcDef.Type) != -1) {
						continue;
					}
					// If gender was unassigned, assign it now for the next npc spawn
					// Otherwise, load the saved gender
					if ((Gender)entry.Value == Gender.Unassigned) {
						SavedGenders[npcDef] = TownNPCs.RandomizeSpawnGender(npcDef.Type);
					}
					else {
						SavedGenders[npcDef] = (Gender)entry.Value;
					}
				}
				else {
					// If the Saved Genders does not contain our NPC, Add a new entry
					Gender randGender = (Gender)entry.Value;
					if (randGender == Gender.Unassigned) {
						randGender = Main.rand.NextBool() ? Gender.Male : Gender.Female;
					}
					SavedGenders.Add(npcDef, randGender);
				}
			}

			// Apply the proper Head Textures on world load. They will be reset when the mod is unloaded.
			foreach (NPCDefinition npc in SavedGenders.Keys) {
				TownNPCs.SwapHeadTexture(npc.Type);
			}

			// If World Gen had taken place, clear the list
			AltGenderWorldGen.Clear();
		}
	}

	public class GuideGenderModifier : GenPass
	{
		public GuideGenderModifier(string name, float loadWeight) : base(name, loadWeight) {
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
			progress.Message = "Assigning town npc genders";
			
			// Loop through the NPCs and apply genders to all that can have genders.
			// Main.rand.NextBool gives us a 50% chance to apply the alternate gender.
			// If successful, we add the npc to a list to Assign the gender when we have the Saved Genders loaded.
			for (int i = 0; i < Main.maxNPCs; i++) {
				int id = Main.npc[i].type;
				if (GenderVariety.townNPCList.GetNPCIndex(id) == -1)
					continue;

				if (GenderVariety.townNPCList.GetNPCInfo(id).originalGender != TownNPCs.RandomizeSpawnGender(id)) {
					TownNPCWorld.AltGenderWorldGen.Add(Main.npc[i]);
				}
			}
		}
	}

	public class TownNPCs : GlobalNPC
	{
		public override ITownNPCProfile ModifyTownNPCProfile(NPC npc) {
			return GenderVariety.townNPCList.GetNPCIndex(npc.type) != -1 ? new GenderProfile() : null;
		}

		public override void ModifyNPCNameList(NPC npc, List<string> nameList) {
			if (GenderVariety.townNPCList.IsAltGender(npc.type) && AltNames.TryGetValue(npc.type, out List<string> altNames)) {
				nameList = altNames;
			}
		}

		public override void ModifyTypeName(NPC npc, ref string typeName) {
			if (GenderVariety.townNPCList.IsAltGender(npc.type)) {
				if (npc.type == NPCID.PartyGirl) {
					typeName = "Party Boy"; // TODO: Translations
				}
				else if (npc.type == NPCID.SantaClaus) {
					typeName = "Mrs. Claus";
				}
				else if (npc.type == NPCID.Princess) {
					typeName = "Prince";
				}
			}
		}

		public override void OnKill(NPC npc) {
			// When a Town NPC dies, we will randomize the gender for the next spawn
			if (TownNPCWorld.SavedGenders.TryGetValue(new NPCDefinition(npc.type), out Gender gender)) {
				gender = RandomizeSpawnGender(npc.type);
			}
		}

		// Swapped genders go to respective statues
		public override bool? CanGoToStatue(NPC npc, bool toKingStatue)	{
			int index = GenderVariety.townNPCList.GetNPCIndex(npc.type);
			if (index == -1 || npc.type == NPCID.SantaClaus) {
				// Santa doesnt teleport in vanilla
				return null;
			}
			else if (GenderVariety.townNPCList.IsAltGender(npc.type)) {
				if (TownNPCWorld.SavedGenders.TryGetValue(new NPCDefinition(npc.type), out Gender gender)) {
					return toKingStatue ? gender == Gender.Female : gender == Gender.Male;
				}
			}
			return null;
		}

		// TODO: Change some chat texts?
		//public override void GetChat(NPC npc, ref string chat)

		internal static Gender RandomizeSpawnGender(int npcType) {
			// Consider any npcs who have a configured gender
			GVConfig config = ModContent.GetInstance<GVConfig>();
			if (config.ForcedMale.FindIndex(npc => npc.Type == npcType) != -1) {
				return Gender.Male;
			}
			else if (config.ForcedFemale.FindIndex(npc => npc.Type == npcType) != -1) {
				return Gender.Female;
			}

			// Otherwise, randomize the gender
			return Main.rand.NextBool() ? Gender.Male : Gender.Female;
		}

		internal static void AssignGender(NPC npc, Gender setGender = Gender.Unassigned) {
			int index = GenderVariety.townNPCList.GetNPCIndex(npc.type);
			string npcIdentity = $"{npc.FullName} [{npc.type}]";
			if (index == -1) {
				GenderVariety.SendDebugMessage($"{npcIdentity} is not a valid NPC for gender changing.", Color.IndianRed);
				return;
			}

			NPCDefinition npcDef = new NPCDefinition(npc.type);
			Gender savedGender = TownNPCWorld.SavedGenders[npcDef];
			// If we aren't setting a gender manually, go through the OnSpawn process
			if (setGender == Gender.Unassigned) {
				setGender = savedGender != Gender.Unassigned ? savedGender : RandomizeSpawnGender(npc.type);
			}

			// Debug Message
			string oldGender = (int)savedGender == 0 ? "Unassigned" : (int)savedGender == 1 ? "Male" : "Female";
			string newGender = setGender == Gender.Male ? "Male" : "Female";
			GenderVariety.SendDebugMessage($"{npcIdentity} changed from {oldGender} to {newGender}", Color.MediumPurple);

			// Update name (texture changes update in PreAI)
			TownNPCWorld.SavedGenders[npcDef] = setGender;
			SwapHeadTexture(npc.type);
		}

		internal static void SwapHeadTexture(int npcType) {
			TownNPCInfo townNPC = GenderVariety.townNPCList.GetNPCInfo(npcType);

			Asset<Texture2D> texture;
			if (GenderVariety.townNPCList.IsAltGender(npcType)) {
				string path = "GenderVariety/Resources/Head/" + townNPC.headIndex;
				texture = ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad);
			}
			else {
				string path = "Images/NPC_Head_" + townNPC.headIndex;
				texture = Main.Assets.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad);
			}
			TextureAssets.NpcHead[townNPC.headIndex] = texture;
		}

		// Original names: https://terraria.gamepedia.com/NPC_names
		// Preferably 12 names for each NPC
		public readonly static Dictionary<int, List<string>> AltNames = new Dictionary<int, List<string>>() {
			{
				NPCID.Guide,
				new List<string>() {
					"Anne", "Becky", "Connie", "Chloe", "Jill", "Kim",
					"Nichole", "Sarah", "Taylor",
				}
			},
			{
				NPCID.Merchant,
				new List<string>() {
					"Deborah", "Gilda", "Gloria", "Harriet", "Ingrid", "Margaret",
					"Mavis", "Wilma",
				}
			},
			{
				NPCID.Nurse,
				new List<string>() {
					"Justin", "Kaleb", "Keith", "Kenneth", "Owen", "Xander",
				}
			},
			{
				NPCID.Demolitionist,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.DyeTrader,
				new List<string>() {
					"Delma", "Hijeb"
				}
			},
			{
				NPCID.Angler,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.BestiaryGirl,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.Dryad,
				new List<string>() {
					"Basil"
				}
			},
			{
				NPCID.Painter,
				new List<string>() {
					"Agnese", "Beatrice", "Carla", "Gabriella", "Lilica", "Lucia",
					"Noemie", "Rachele", "Ruth", "Vittoria", "Violet", "Vivia",
				}
			},
			{
				NPCID.Golfer,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.ArmsDealer,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.DD2Bartender,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.Stylist,
				new List<string>() {
					"Rogue"
				}
			},
			{
				NPCID.GoblinTinkerer,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.WitchDoctor,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.Clothier,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.Mechanic,
				new List<string>() {
					"Butch"
				}
			},
			{
				NPCID.PartyGirl,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.Wizard,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.TaxCollector,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.Truffle,
				new List<string>() {
					"Matsutake", "Russula", "Lepiota", "Calocybe", "Bolete",
				}
			},
			{
				NPCID.Pirate,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.Steampunker,
				new List<string>() {
					"Braiden", "Delroi", "Johnathan", "Ronspierre", "Smith"
				}
			},
			{
				NPCID.Cyborg,
				new List<string>() {
					"AltNameTest",
				}
			},
			{
				NPCID.SantaClaus,
				new List<string>() {
					"Mary Claus", "Carol Claus", "Jessica Claus"
				}
			},
			{
				NPCID.Princess,
				new List<string>() {
					"AltNameTest",
				}
			}
		};
	}
}
