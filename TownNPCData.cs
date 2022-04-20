using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace GenderVariety
{
	internal class TownNPCInfo
	{
		internal int type;
		internal int headIndex;
		internal Gender originalGender;

		internal string defaultPath;
		internal string partyPath;
		internal string transformedPath;
		internal string creditsPath;

		internal TownNPCInfo(int type, int headIndex, Gender originalGender, string defaultPath, string partyPath = "noPath") {
			this.type = type;
			this.headIndex = headIndex;
			this.originalGender = originalGender;

			this.defaultPath = ModContent.HasAsset($"Terraria/Images/{defaultPath}") ? defaultPath : null;
			this.partyPath = ModContent.HasAsset($"Terraria/Images/{partyPath}") ? partyPath : null;
			transformedPath = type == NPCID.BestiaryGirl ? "TownNPCs/BestiaryGirl_Default_Transformed" : null;
			creditsPath = type == NPCID.BestiaryGirl ? "TownNPCs/BestiaryGirl_Default_Credits" : null;
		}

		internal static TownNPCInfo AddTownNPC(int type, int headIndex, Gender originalGender, string noAltPath, string partyPath = "") {
			return new TownNPCInfo(type, headIndex, originalGender, noAltPath, partyPath);
		}
	}

	internal class TownNPCSetup
	{
		public TownNPCSetup() {
			GenderVariety.townNPCList = this;
			InitializeTownNPCList();
		}

		internal List<TownNPCInfo> townNPCs;

		private void InitializeTownNPCList() {
			townNPCs = new List<TownNPCInfo>() {
				// Ordered by wiki
				new TownNPCInfo(NPCID.Guide, NPCHeadID.Guide, Gender.Male, $"NPC_{NPCID.Guide}"),
				new TownNPCInfo(NPCID.Merchant, NPCHeadID.Merchant, Gender.Male, "TownNPCs/Merchant_Default", "TownNPCs/Merchant_Default_Party"),
				new TownNPCInfo(NPCID.Nurse, NPCHeadID.Nurse, Gender.Female, "TownNPCs/Nurse_Default", "TownNPCs/Nurse_Default_Party"),
				new TownNPCInfo(NPCID.Demolitionist, NPCHeadID.Demolitionist, Gender.Male, "TownNPCs/Demolitionist_Default", "TownNPCs/Demolitionist_Default_Party"),
				new TownNPCInfo(NPCID.DyeTrader, NPCHeadID.DyeTrader, Gender.Male, "TownNPCs/DyeTrader_Default", "TownNPCs/DyeTrader_Default_Party"),
				new TownNPCInfo(NPCID.Angler, NPCHeadID.Angler, Gender.Male, "TownNPCs/Angler_Default", "TownNPCs/Angler_Default_Party"),
				new TownNPCInfo(NPCID.BestiaryGirl, NPCHeadID.BestiaryGirl, Gender.Female, "TownNPCs/BestiaryGirl_Default", "TownNPCs/BestiaryGirl_Default_Party"),
				new TownNPCInfo(NPCID.Dryad, NPCHeadID.Dryad, Gender.Female, $"NPC_{NPCID.Dryad}"),
				new TownNPCInfo(NPCID.Painter, NPCHeadID.Painter, Gender.Male, "TownNPCs/Painter_Default", "TownNPCs/Painter_Default_Party"),
				new TownNPCInfo(NPCID.Golfer, NPCHeadID.Golfer, Gender.Male, "TownNPCs/Golfer_Default", "TownNPCs/Golfer_Default_Party"),
				new TownNPCInfo(NPCID.ArmsDealer, NPCHeadID.ArmsDealer, Gender.Male, $"NPC_{NPCID.ArmsDealer}"),
				//TownNPCInfo.AddTownNPC(NPCID.DD2Bartender, 24, true),
				new TownNPCInfo(NPCID.Stylist, NPCHeadID.Stylist, Gender.Female, "TownNPCs/Stylist_Default", "TownNPCs/Stylist_Default_Party"),
				new TownNPCInfo(NPCID.GoblinTinkerer, NPCHeadID.GoblinTinkerer, Gender.Male, $"NPC_{NPCID.GoblinTinkerer}"),
				//TownNPCInfo.AddTownNPC(NPCID.WitchDoctor, 18, true),
				new TownNPCInfo(NPCID.Clothier, NPCHeadID.Clothier, Gender.Male, "TownNPCs/Clothier_Default", "TownNPCs/Clothier_Default_Party"),
				new TownNPCInfo(NPCID.Mechanic, NPCHeadID.Mechanic, Gender.Female, "TownNPCs/Mechanic_Default", "TownNPCs/Mechanic_Default_Party"),
				new TownNPCInfo(NPCID.PartyGirl, NPCHeadID.PartyGirl, Gender.Female, $"NPC_{NPCID.PartyGirl}"),

				new TownNPCInfo(NPCID.Wizard, NPCHeadID.Wizard, Gender.Male, "TownNPCs/Wizard_Default", "TownNPCs/Wizard_Default_Party"),
				//TownNPCInfo.AddTownNPC(NPCID.TaxCollector, 23, true),
				new TownNPCInfo(NPCID.Truffle, NPCHeadID.Truffle, Gender.Male, $"NPC_{NPCID.Truffle}"),
				new TownNPCInfo(NPCID.Pirate, NPCHeadID.Pirate, Gender.Male, "TownNPCs/Pirate_Default", "TownNPCs/Pirate_Default_Party"),
				new TownNPCInfo(NPCID.Steampunker, NPCHeadID.Steampunker, Gender.Female, "TownNPCs/Steampunker_Default", "TownNPCs/Steampunker_Default_Party"),
				new TownNPCInfo(NPCID.Cyborg, NPCHeadID.Cyborg, Gender.Male, "TownNPCs/Cyborg_Default", "TownNPCs/Cyborg_Default_Party"),
				new TownNPCInfo(NPCID.SantaClaus, NPCHeadID.SantaClaus, Gender.Male, "TownNPCs/Santa_Default", "TownNPCs/Santa_Default_Party"),
				//new TownNPCInfo(NPCID.Princess, NPCHeadID.Princess, Gender.Female, "TownNPCs/Princess_Default", "TownNPCs/Princess_Default_Party"),

				//TownNPCInfo.AddTownNPC(NPCID.TravellingMerchant, 21, true),
			};
		}

		internal int GetNPCIndex(int npcType) => townNPCs.FindIndex(x => x.type == npcType);

		internal bool IsAltGender(int npcType) {
			// If the index is invalid, the gender can't be the alternate.
			int index = GenderVariety.townNPCList.GetNPCIndex(npcType);
			if (index == -1) {
				return false;
			}

			int savedGender = TownNPCWorld.SavedData[index].savedGender;
			int originalGender = (int)GenderVariety.townNPCList.townNPCs[index].originalGender;
			return savedGender == (int)Gender.Unassigned ? false : savedGender != originalGender;
		}
	}

	internal class TownNPCData : TagSerializable {
		internal int type;
		internal int savedGender;
		internal string name;
		internal string altName;

		public static Func<TagCompound, TownNPCData> DESERIALIZER = tag => new TownNPCData(tag);

		public TownNPCData(int type, int gender, string name, string altName) {
			this.type = type;
			this.savedGender = gender;
			this.name = name;
			this.altName = altName;
		}

		private TownNPCData(TagCompound tag) {
			type = tag.Get<int>(nameof(type));
			savedGender = tag.Get<int>(nameof(savedGender));
			name = tag.Get<string>(nameof(name));
			altName = tag.Get<string>(nameof(altName));
		}

		public TagCompound SerializeData() {
			return new TagCompound {
				{ nameof(type), type },
				{ nameof(savedGender), savedGender },
				{ nameof(name), name },
				{ nameof(altName), altName },
			};
		}

		// Leave 0 to choose at random/config. Using 1 or 2 sets it to that gender
		internal static void AssignGender(NPC npc, Gender setGender = Gender.Unassigned) {
			int index = GenderVariety.townNPCList.GetNPCIndex(npc.type);
			if (index == -1) {
				GenderVariety.SendDebugMessage($"{npc.TypeName}({npc.type}) is not a valid NPC for gender changing.", Color.IndianRed);
				return;
			}

			TownNPCData npcData = TownNPCWorld.SavedData[index];
			// If we aren't setting a gender manually, go through the OnSpawn process
			if (setGender == Gender.Unassigned) {
				if ((Gender)npcData.savedGender != Gender.Unassigned) {
					setGender = (Gender)npcData.savedGender;
				}
				else {
					if (ModContent.GetInstance<GVConfig>().ForcedMale.Any(x => x.Type == npc.type)) {
						setGender = Gender.Male;
					}
					else if (ModContent.GetInstance<GVConfig>().ForcedFemale.Any(x => x.Type == npc.type)) {
						setGender = Gender.Female;
					}
					else {
						setGender = Main.rand.NextBool() ? Gender.Male : Gender.Female;
					}
				}
			}

			// Debug Message
			string oldGender = npcData.savedGender == 0 ? "Unassigned" : npcData.savedGender == 1 ? "Male" : "Female";
			string newGender = setGender == Gender.Unassigned ? "Unassigned" : setGender == Gender.Male ? "Male" : "Female";
			GenderVariety.SendDebugMessage($"The {npc.TypeName}({npc.type}) is now a {newGender} (previously {oldGender})", Color.MediumPurple);

			// Update name (texture changes update in PreAI)
			npcData.savedGender = (int)setGender;
			SwapHeadTexture(npc.type);
		}

		internal static void SwapHeadTexture(int npcType, bool resetTexture = false) {
			int index = GenderVariety.townNPCList.GetNPCIndex(npcType);
			if (index == -1) {
				return;
			}
			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			if (resetTexture) {
				TextureAssets.NpcHead[townNPC.headIndex] = Main.Assets.Request<Texture2D>($"Images/NPC_Head_{townNPC.headIndex}", AssetRequestMode.ImmediateLoad);
			}
			else {
				if (GenderVariety.townNPCList.IsAltGender(npcType)) {
					TextureAssets.NpcHead[townNPC.headIndex] = ModContent.Request<Texture2D>($"GenderVariety/Resources/NPCHead/NPC_Head_{townNPC.headIndex}", AssetRequestMode.ImmediateLoad);
				}
				else {
					TextureAssets.NpcHead[townNPC.headIndex] = Main.Assets.Request<Texture2D>($"Images/NPC_Head_{townNPC.headIndex}", AssetRequestMode.ImmediateLoad);
				}
			}
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
					"Cress", "Xelther", "Zedoary",
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
