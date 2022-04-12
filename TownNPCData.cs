using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		internal int ogGender;
		internal Texture2D npcTexture;
		internal Texture2D npcTexture_Head;
		internal Texture2D npcAltTexture;
		internal Texture2D npcAltTexture_Head;
		internal Texture2D npcPartyTexture;
		internal Texture2D npcAltPartyTexture;

		TownNPCInfo(int type, int headIndex, int ogGender) {
			this.type = type;
			this.headIndex = headIndex;
			this.ogGender = ogGender;
			if (NPCID.Sets.ExtraTextureCount[type] != 0) {
				this.npcTexture = TextureAssets.Npc[type].Value;
				this.npcPartyTexture = Main.npcAltTextures[type][1];
				TownNPCProfiles.Instance.GetProfile(type, out ITownNPCProfile profile);
				this.npcPartyTexture = profile.GetTextureNPCShouldUse(Main.npc[0]).Value;
				this.npcAltPartyTexture = ModContent.GetTexture($"GenderVariety/Resources/NPC/NPC_{type}_Alt_1");
			}
			else {
				this.npcTexture = Main.npcTexture[type];
				this.npcPartyTexture = null;
				this.npcAltPartyTexture = null;
			}
			this.npcAltTexture = ModContent.GetTexture($"GenderVariety/Resources/NPC/NPC_{type}");
			this.npcTexture_Head = Main.npcHeadTexture[headIndex];
			this.npcAltTexture_Head = ModContent.GetTexture($"GenderVariety/Resources/NPCHead/NPC_Head_{headIndex}");
		}

		internal static TownNPCInfo AddTownNPC(int type, int headIndex, int originalGender) {
			return new TownNPCInfo(type, headIndex, originalGender);
		}
	}

	internal class TownNPCSetup
	{
		public const int Unassigned = 0;
		public const int Male = 1;
		public const int Female = 2;

		public TownNPCSetup() {
			GenderVariety.townNPCList = this;
			InitializeTownNPCList();
		}

		internal List<TownNPCInfo> townNPCs;
		internal bool[] npcIsAltGender;

		private void InitializeTownNPCList() {
			townNPCs = new List<TownNPCInfo>() {
				// Ordered by wiki
				TownNPCInfo.AddTownNPC(NPCID.Guide, NPCHeadID.Guide, Male),
				TownNPCInfo.AddTownNPC(NPCID.Merchant, NPCHeadID.Merchant, Male),
				TownNPCInfo.AddTownNPC(NPCID.Nurse, NPCHeadID.Nurse, Female),
				TownNPCInfo.AddTownNPC(NPCID.Demolitionist, NPCHeadID.Demolitionist, Male),
				TownNPCInfo.AddTownNPC(NPCID.DyeTrader, NPCHeadID.DyeTrader, Male),
				TownNPCInfo.AddTownNPC(NPCID.Angler, NPCHeadID.Angler, Male),
				TownNPCInfo.AddTownNPC(NPCID.BestiaryGirl, NPCHeadID.BestiaryGirl, Female),
				TownNPCInfo.AddTownNPC(NPCID.Dryad, NPCHeadID.Dryad, Female),
				TownNPCInfo.AddTownNPC(NPCID.Painter, NPCHeadID.Painter, Male),
				TownNPCInfo.AddTownNPC(NPCID.Golfer, NPCHeadID.Golfer, Male),
				TownNPCInfo.AddTownNPC(NPCID.ArmsDealer, NPCHeadID.ArmsDealer, Male),
				//TownNPCInfo.AddTownNPC(NPCID.DD2Bartender, 24, true),
				TownNPCInfo.AddTownNPC(NPCID.Stylist, NPCHeadID.Stylist, Female),
				TownNPCInfo.AddTownNPC(NPCID.GoblinTinkerer, NPCHeadID.GoblinTinkerer, Male),
				//TownNPCInfo.AddTownNPC(NPCID.WitchDoctor, 18, true),
				TownNPCInfo.AddTownNPC(NPCID.Clothier, NPCHeadID.Clothier, Male),
				TownNPCInfo.AddTownNPC(NPCID.Mechanic, NPCHeadID.Mechanic, Female),
				TownNPCInfo.AddTownNPC(NPCID.PartyGirl, NPCHeadID.PartyGirl, Female),

				TownNPCInfo.AddTownNPC(NPCID.Wizard, NPCHeadID.Wizard, Male),
				//TownNPCInfo.AddTownNPC(NPCID.TaxCollector, 23, true),
				TownNPCInfo.AddTownNPC(NPCID.Truffle, NPCHeadID.Truffle, Male),
				TownNPCInfo.AddTownNPC(NPCID.Pirate, NPCHeadID.Pirate, Male),
				TownNPCInfo.AddTownNPC(NPCID.Steampunker, NPCHeadID.Steampunker, Female),
				TownNPCInfo.AddTownNPC(NPCID.Cyborg, NPCHeadID.Cyborg, Male),
				TownNPCInfo.AddTownNPC(NPCID.SantaClaus, NPCHeadID.SantaClaus, Male),
				TownNPCInfo.AddTownNPC(NPCID.Princess, NPCHeadID.Princess, Female),

				//TownNPCInfo.AddTownNPC(NPCID.TravellingMerchant, 21, true),
			};

			npcIsAltGender = new bool[townNPCs.Count]; // All are set to default (false)
		}

		internal bool IsAltGender(int index) {
			int saved = TownNPCWorld.SavedData[index].savedGender;
			if (saved == Unassigned) return false;
			else return saved != townNPCs[index].ogGender;
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
		internal static void AssignGender(NPC npc, int setGender = 0) {
			int index = GenderVariety.GetNPCIndex(npc.type);
			if (index == -1) {
				GenderVariety.SendDebugMessage($"{npc.TypeName}({npc.type}) is not a valid NPC for gender changing.", Color.IndianRed);
				return;
			}

			TownNPCData npcData = TownNPCWorld.SavedData[index];
			// If we aren't setting a gender manually, go through the OnSpawn process
			if (setGender == TownNPCSetup.Unassigned) {
				if (npcData.savedGender != TownNPCSetup.Unassigned) setGender = npcData.savedGender;
				else {
					if (ModContent.GetInstance<GVConfig>().ForcedMale.Any(x => x.Type == npc.type)) setGender = TownNPCSetup.Male;
					else if (ModContent.GetInstance<GVConfig>().ForcedFemale.Any(x => x.Type == npc.type)) setGender = TownNPCSetup.Female;
					else setGender = Main.rand.NextBool() ? TownNPCSetup.Male : TownNPCSetup.Female;
				}
			}

			// Debug Message
			string oldGender = npcData.savedGender == 0 ? "Unassigned" : npcData.savedGender == 1 ? "Male" : "Female";
			string newGender = setGender == 0 ? "Unassigned" : setGender == 1 ? "Male" : "Female";
			GenderVariety.SendDebugMessage($"The {npc.TypeName}({npc.type}) is now a {newGender} (previously {oldGender})", Color.MediumPurple);

			// Update name (texture changes update in PreAI)
			npcData.savedGender = setGender;
			GenderVariety.townNPCList.npcIsAltGender[index] = !GenderVariety.townNPCList.npcIsAltGender[index];
			SwapTextures(index, npc.type);
			npc.GivenName = GenerateAltName(index, npc);
		}

		internal static void SwapTextures(int index, int npcType) {
			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			if (GenderVariety.townNPCList.npcIsAltGender[index]) {
				if (NPCID.Sets.ExtraTextureCount[npcType] != 0) {
					Main.npcAltTextures[npcType][0] = townNPC.npcAltTexture;
					Main.npcAltTextures[npcType][1] = townNPC.npcAltPartyTexture;
				}
				else Main.npcTexture[npcType] = townNPC.npcAltTexture;
				Main.npcHeadTexture[townNPC.headIndex] = townNPC.npcAltTexture_Head;
			}
			else {
				if (NPCID.Sets.ExtraTextureCount[npcType] != 0) {
					Main.npcAltTextures[npcType][0] = townNPC.npcTexture;
					Main.npcAltTextures[npcType][1] = townNPC.npcPartyTexture;
				}
				else Main.npcTexture[npcType] = townNPC.npcTexture;
				Main.npcHeadTexture[townNPC.headIndex] = townNPC.npcTexture_Head;
			}
		}

		internal static string GenerateAltName(int index, NPC npc) {
			// Location should NEVER be -1 since this method can only be called after a check
			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			TownNPCData npcData = TownNPCWorld.SavedData[index];

			// Check saved names first. Saved names are reset on NPC death.
			if (!GenderVariety.townNPCList.npcIsAltGender[index]) {
				if (npcData.name != "") return npcData.name;
				else return NPC.getNewNPCName(npc.type);
			}
			else {
				if (npcData.altName != "") return npcData.altName;
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
