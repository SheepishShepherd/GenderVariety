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
		internal bool HasPartyTexture;

		internal Asset<Texture2D> npcTexture_Male;
		internal Asset<Texture2D> npcTexture_Male_Party;
		internal Asset<Texture2D> npcTexture_Male_Head;

		internal Asset<Texture2D> npcTexture_Female;
		internal Asset<Texture2D> npcTexture_Female_Party;
		internal Asset<Texture2D> npcTexture_Female_Head;

		TownNPCInfo(int type, int headIndex, Gender originalGender, string noAltPath, string partyPath = "") {
			this.type = type;
			this.headIndex = headIndex;
			this.originalGender = originalGender;
			if (originalGender == Gender.Male) {
				npcTexture_Male = RequestTextureSafely($"Terraria/Images/{noAltPath}");
				npcTexture_Male_Party = RequestTextureSafely($"Terraria/Images/{partyPath}");
				npcTexture_Male_Head = TextureAssets.NpcHead[headIndex];

				npcTexture_Female = RequestTextureSafely($"GenderVariety/Resources/NPC/NPC_{type}");
				npcTexture_Female_Party = RequestTextureSafely($"GenderVariety/Resources/NPC/NPC_{type}");
				npcTexture_Female_Head = RequestTextureSafely($"GenderVariety/Resources/NPCHead/NPC_Head_{headIndex}");
			}
			else {
				npcTexture_Female = RequestTextureSafely($"Terraria/Images/{noAltPath}");
				npcTexture_Female_Party = RequestTextureSafely($"Terraria/Images/{partyPath}");
				npcTexture_Female_Head = TextureAssets.NpcHead[headIndex];

				npcTexture_Male = RequestTextureSafely($"GenderVariety/Resources/NPC/NPC_{type}");
				npcTexture_Male_Party = RequestTextureSafely($"GenderVariety/Resources/NPC/NPC_{type}");
				npcTexture_Male_Head = RequestTextureSafely($"GenderVariety/Resources/NPCHead/NPC_Head_{headIndex}");
			}

			HasPartyTexture = npcTexture_Male_Party == null || npcTexture_Female_Party == null ? false : true;
		}

		internal Asset<Texture2D> GetOriginalNPCTexture() => originalGender == Gender.Male ? npcTexture_Male : npcTexture_Female;

		internal Asset<Texture2D> GetAlternateNPCTexture() => originalGender == Gender.Male ? npcTexture_Female : npcTexture_Male;

		internal Asset<Texture2D> GetOriginalNPCHeadTexture() => originalGender == Gender.Male ? npcTexture_Male_Head : npcTexture_Female_Head;

		internal Asset<Texture2D> GetAlternateNPCHeadTexture() => originalGender == Gender.Male ? npcTexture_Female_Head : npcTexture_Male_Head;

		Asset<Texture2D> RequestTextureSafely(string path) {
			if (ModContent.HasAsset(path)) {
				return ModContent.Request<Texture2D>(path);
			}
			return null;
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
				TownNPCInfo.AddTownNPC(NPCID.Guide, NPCHeadID.Guide, Gender.Male, $"NPC_{NPCID.Guide}"),
				TownNPCInfo.AddTownNPC(NPCID.Merchant, NPCHeadID.Merchant, Gender.Male, "TownNPCs/Merchantr_Default", "TownNPCs/Merchantr_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.Nurse, NPCHeadID.Nurse, Gender.Female, "TownNPCs/Nurse_Default", "TownNPCs/Nurse_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.Demolitionist, NPCHeadID.Demolitionist, Gender.Male, "TownNPCs/Demolitionist_Default", "TownNPCs/Demolitionist_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.DyeTrader, NPCHeadID.DyeTrader, Gender.Male, "TownNPCs/DyeTrader_Default", "TownNPCs/DyeTrader_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.Angler, NPCHeadID.Angler, Gender.Male, "TownNPCs/Angler_Default", "TownNPCs/Angler_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.BestiaryGirl, NPCHeadID.BestiaryGirl, Gender.Female, "TownNPCs/BestiaryGirl_Default", "TownNPCs/BestiaryGirl_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.Dryad, NPCHeadID.Dryad, Gender.Female, $"NPC_{NPCID.Dryad}"),
				TownNPCInfo.AddTownNPC(NPCID.Painter, NPCHeadID.Painter, Gender.Male, "TownNPCs/Painter_Default", "TownNPCs/Painter_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.Golfer, NPCHeadID.Golfer, Gender.Male, "TownNPCs/Golfer_Default", "TownNPCs/Golfer_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.ArmsDealer, NPCHeadID.ArmsDealer, Gender.Male, $"NPC_{NPCID.ArmsDealer}"),
				//TownNPCInfo.AddTownNPC(NPCID.DD2Bartender, 24, true),
				TownNPCInfo.AddTownNPC(NPCID.Stylist, NPCHeadID.Stylist, Gender.Female, "TownNPCs/Stylist_Default", "TownNPCs/Stylist_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.GoblinTinkerer, NPCHeadID.GoblinTinkerer, Gender.Male, $"NPC_{NPCID.GoblinTinkerer}"),
				//TownNPCInfo.AddTownNPC(NPCID.WitchDoctor, 18, true),
				TownNPCInfo.AddTownNPC(NPCID.Clothier, NPCHeadID.Clothier, Gender.Male, "TownNPCs/Clothier_Default", "TownNPCs/Clothier_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.Mechanic, NPCHeadID.Mechanic, Gender.Female, "TownNPCs/Mechanic_Default", "TownNPCs/Mechanic_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.PartyGirl, NPCHeadID.PartyGirl, Gender.Female, $"NPC_{NPCID.PartyGirl}"),

				TownNPCInfo.AddTownNPC(NPCID.Wizard, NPCHeadID.Wizard, Gender.Male, "TownNPCs/Wizard_Default", "TownNPCs/Wizard_Default_Party"),
				//TownNPCInfo.AddTownNPC(NPCID.TaxCollector, 23, true),
				TownNPCInfo.AddTownNPC(NPCID.Truffle, NPCHeadID.Truffle, Gender.Male, $"NPC_{NPCID.Truffle}"),
				TownNPCInfo.AddTownNPC(NPCID.Pirate, NPCHeadID.Pirate, Gender.Male, "TownNPCs/Pirate_Default", "TownNPCs/Pirate_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.Steampunker, NPCHeadID.Steampunker, Gender.Female, "TownNPCs/Steampunker_Default", "TownNPCs/Steampunker_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.Cyborg, NPCHeadID.Cyborg, Gender.Male, "TownNPCs/Cyborg_Default", "TownNPCs/Cyborg_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.SantaClaus, NPCHeadID.SantaClaus, Gender.Male, "TownNPCs/Santa_Default", "TownNPCs/Santa_Default_Party"),
				TownNPCInfo.AddTownNPC(NPCID.Princess, NPCHeadID.Princess, Gender.Female, "TownNPCs/Princess_Default", "TownNPCs/Princess_Default_Party"),

				//TownNPCInfo.AddTownNPC(NPCID.TravellingMerchant, 21, true),
			};
		}

		internal int GetNPCIndex(int npcType) => townNPCs.FindIndex(x => x.type == npcType);

		internal bool IsAltGender(int npc) {
			int savedIndex = TownNPCWorld.SavedData.FindIndex(x => x.type == npc);
			int infoIndex = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc);

			if (savedIndex == -1 || infoIndex == -1) {
				return false;
			}

			int savedGender = TownNPCWorld.SavedData[savedIndex].savedGender;
			int originalGender = (int)GenderVariety.townNPCList.townNPCs[infoIndex].originalGender;
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
			SwapTextures(index, npc.type);
			npc.GivenName = GenerateAltName(index, npc);
		}

		internal static void SwapTextures(int index, int npcType) {
			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			if (GenderVariety.townNPCList.IsAltGender(npcType)) {
				// TODO: This will probably need to be IL Edited into a ITownNPCProfile.GetTextureNPCShouldUse. Not sure.
				TextureAssets.Npc[npcType] = townNPC.GetAlternateNPCTexture();
				TextureAssets.NpcHead[townNPC.headIndex] = townNPC.GetAlternateNPCHeadTexture();
			}
			else {
				TextureAssets.Npc[npcType] = townNPC.GetOriginalNPCTexture();
				TextureAssets.NpcHead[townNPC.headIndex] = townNPC.GetOriginalNPCHeadTexture();
			}
		}

		internal static string GenerateAltName(int index, NPC npc) {
			// Location should NEVER be -1 since this method can only be called after a check
			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			TownNPCData npcData = TownNPCWorld.SavedData[index];

			// Check saved names first. Saved names are reset on NPC death.
			if (!GenderVariety.townNPCList.IsAltGender(npc.type)) {
				return npcData.name != "" ? npcData.name : NPC.getNewNPCName(npc.type);
			}
			else {
				if (npcData.altName != "") {
					return npcData.altName;
				}
				else {
					switch (townNPC.type) {
						case NPCID.Guide: return TownNPCNames.Guide[Main.rand.Next(TownNPCNames.Guide.Count)];
						case NPCID.Merchant: return TownNPCNames.Merchant[Main.rand.Next(TownNPCNames.Merchant.Count)];
						case NPCID.Nurse: return TownNPCNames.Nurse[Main.rand.Next(TownNPCNames.Nurse.Count)];
						case NPCID.Demolitionist: return TownNPCNames.Demolitionist[Main.rand.Next(TownNPCNames.Demolitionist.Count)];
						case NPCID.DyeTrader: return TownNPCNames.DyeTrader[Main.rand.Next(TownNPCNames.DyeTrader.Count)];
						case NPCID.Dryad: return TownNPCNames.Merchant[Main.rand.Next(TownNPCNames.Dryad.Count)];
						case NPCID.ArmsDealer: return TownNPCNames.ArmsDealer[Main.rand.Next(TownNPCNames.ArmsDealer.Count)];
						case NPCID.Stylist: return TownNPCNames.Stylist[Main.rand.Next(TownNPCNames.Stylist.Count)];
						case NPCID.Painter: return TownNPCNames.Painter[Main.rand.Next(TownNPCNames.Painter.Count)];
						case NPCID.Angler: return TownNPCNames.Angler[Main.rand.Next(TownNPCNames.Angler.Count)];
						case NPCID.GoblinTinkerer: return TownNPCNames.GoblinTinkerer[Main.rand.Next(TownNPCNames.GoblinTinkerer.Count)];
						case NPCID.Clothier: return TownNPCNames.Clothier[Main.rand.Next(TownNPCNames.Clothier.Count)];
						case NPCID.Mechanic: return TownNPCNames.Mechanic[Main.rand.Next(TownNPCNames.Mechanic.Count)];
						case NPCID.PartyGirl: return TownNPCNames.PartyGirl[Main.rand.Next(TownNPCNames.PartyGirl.Count)];
						case NPCID.Wizard: return TownNPCNames.Wizard[Main.rand.Next(TownNPCNames.Wizard.Count)];
						case NPCID.Truffle: return TownNPCNames.Truffle[Main.rand.Next(TownNPCNames.Truffle.Count)];
						case NPCID.Pirate: return TownNPCNames.Pirate[Main.rand.Next(TownNPCNames.Pirate.Count)];
						case NPCID.Steampunker: return TownNPCNames.Steampunker[Main.rand.Next(TownNPCNames.Steampunker.Count)];
						case NPCID.Cyborg: return TownNPCNames.Cyborg[Main.rand.Next(TownNPCNames.Cyborg.Count)];
						default: return "";
					}
				}
			}
		}
	}

	internal class TownNPCNames
	{
		// Original names: https://terraria.gamepedia.com/NPC_names
		// Preferably 12 names for each NPC

		public readonly static List<string> Guide = new List<string>() {
			"Anne", "Becky", "Connie", "Chloe", "Jill", "Kim",
			"Nichole", "Sarah", "Taylor",
		};
		public readonly static List<string> Merchant = new List<string>() {
			"Deborah", "Gilda", "Gloria", "Harriet", "Ingrid", "Margaret",
			"Mavis", "Wilma",
		};
		public readonly static List<string> Nurse = new List<string>() {
			"Justin", "Kaleb", "Keith", "Kenneth", "Owen", "Xander",
		};
		public readonly static List<string> Painter = new List<string>() {
			"Agnese", "Beatrice", "Carla", "Gabriella", "Lilica", "Lucia",
			"Noemie", "Rachele", "Ruth", "Vittoria", "Violet", "Vivia",
		};
		public readonly static List<string> DyeTrader = new List<string>() {
			"Delma", "Hijeb", ""
		};
		public readonly static List<string> Demolitionist = new List<string>() {
			"Alt"
		};
		public readonly static List<string> Dryad = new List<string>() {
			"Basil",
		};
		public readonly static List<string> ArmsDealer = new List<string>() {
			"Alt"
		};
		public readonly static List<string> PartyGirl = new List<string>() {
			"Alt"
		};
		public readonly static List<string> Stylist = new List<string>() {
			"Rogue",
		};
		public readonly static List<string> Angler = new List<string>() {
			"Alt"
		};
		public readonly static List<string> GoblinTinkerer = new List<string>() {
			"Alt"
		};
		public readonly static List<string> Clothier = new List<string>() {
			"Alt"
		};
		public readonly static List<string> Mechanic = new List<string>() {
			"Butch", 
		};
		public readonly static List<string> Pirate = new List<string>() {
			"Alt"
		};
		public readonly static List<string> Truffle = new List<string>() {
			"Cress", "Xelther", "Zedoary",
		};
		public readonly static List<string> Wizard = new List<string>() {
			"Alt"
		};
		public readonly static List<string> Steampunker = new List<string>() {
			"Braiden", "Delroi", "Johnathan", "Ronspierre", "Smith"
		};
		public readonly static List<string> Cyborg = new List<string>() {
			"Alt"
		};
		public readonly static List<string> Claus = new List<string>() {
			"Mary Claus", "Carol Claus", "Jessica Claus"
		};
	}
}
