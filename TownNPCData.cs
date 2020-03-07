using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace GenderVariety
{
	internal class TownNPCInfo
	{
		internal int type;
		internal int headIndex;
		internal bool isMale;
		internal Texture2D npcTexture;
		internal Texture2D npcTexture_Head;
		internal Texture2D npcAltTexture;
		internal Texture2D npcAltTexture_Head;

		//internal Texture2D npcPartyTexture;
		//internal Texture2D npcAltPartyTexture;

		TownNPCInfo(int type, int headIndex, bool isMale) {
			this.type = type;
			this.headIndex = headIndex;
			this.isMale = isMale;
			this.npcTexture = Main.npcTexture[type];
			this.npcTexture_Head = Main.npcHeadTexture[headIndex];
			this.npcAltTexture = ModContent.GetTexture($"GenderVariety/Resources/NPC/NPC_{type}");
			this.npcAltTexture_Head = ModContent.GetTexture($"GenderVariety/Resources/NPCHead/NPC_Head_{headIndex}");

			//TODO: Party Hat textures arrays? needed too
			//this.npcPartyTexture = Main.npcAltTextures[type][0];
			//this.npcAltPartyTexture = ModContent.GetTexture($"GenderVariety/Resources/NPC_Head_{headIndex}");
		}

		internal static TownNPCInfo AddTownNPC(int type, int headIndex, bool isMale) {
			return new TownNPCInfo(type, headIndex, isMale);
		}
		
		public static bool IsAltGender(NPC npc) {
			int index = GenderVariety.GetNPCIndex(npc);
			if (index == -1) return false;
			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			TownNPCData npcData = TownNPCWorld.SavedData[index];
			return (townNPC.isMale && npcData.savedGender == GenderVariety.Female) || (!townNPC.isMale && npcData.savedGender == GenderVariety.Male);
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
			townNPCs = new List<TownNPCInfo>()
			{
				TownNPCInfo.AddTownNPC(NPCID.Guide, 1, true),
				TownNPCInfo.AddTownNPC(NPCID.Merchant, 2, true),
				TownNPCInfo.AddTownNPC(NPCID.Nurse, 3, false),
				TownNPCInfo.AddTownNPC(NPCID.Demolitionist, 4, true),
				TownNPCInfo.AddTownNPC(NPCID.Dryad, 5, false),
				TownNPCInfo.AddTownNPC(NPCID.ArmsDealer, 6, true),
				TownNPCInfo.AddTownNPC(NPCID.Clothier, 7, true),
				TownNPCInfo.AddTownNPC(NPCID.Mechanic, 8, false),
				TownNPCInfo.AddTownNPC(NPCID.GoblinTinkerer, 9, true),
				TownNPCInfo.AddTownNPC(NPCID.Wizard, 10, true),
				TownNPCInfo.AddTownNPC(NPCID.SantaClaus, 11, true),
				TownNPCInfo.AddTownNPC(NPCID.Truffle, 12, true),
				TownNPCInfo.AddTownNPC(NPCID.Steampunker, 13, false),
				TownNPCInfo.AddTownNPC(NPCID.DyeTrader, 14, true),
				TownNPCInfo.AddTownNPC(NPCID.PartyGirl, 15, false),
				TownNPCInfo.AddTownNPC(NPCID.Cyborg, 16, true),
				TownNPCInfo.AddTownNPC(NPCID.Painter, 17, true),
				//TownNPCInfo.AddTownNPC(NPCID.WitchDoctor, 18, true),
				TownNPCInfo.AddTownNPC(NPCID.Pirate, 19, true),
				TownNPCInfo.AddTownNPC(NPCID.Stylist, 20, false),
				//TownNPCInfo.AddTownNPC(NPCID.TravellingMerchant, 21, true),
				TownNPCInfo.AddTownNPC(NPCID.Angler, 22, true),
				//TownNPCInfo.AddTownNPC(NPCID.TaxCollector, 23, true),
				//TownNPCInfo.AddTownNPC(NPCID.DD2Bartender, 24, true),
			};
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
			int index = GenderVariety.GetNPCIndex(npc);
			if (index == -1) {
				GenderVariety.SendDebugMessage($"{npc.TypeName}({npc.type}) is not a valid NPC for gender changing.", Color.IndianRed);
				return;
			}

			TownNPCData npcData = TownNPCWorld.SavedData[index];
			// If we aren't setting a gender manually, go through the OnSpawn process
			if (setGender == GenderVariety.Unassigned) {
				if (npcData.savedGender != GenderVariety.Unassigned) setGender = npcData.savedGender;
				else {
					if (ModContent.GetInstance<GVConfig>().ForcedMale.Any(x => x.Type == npc.type)) setGender = GenderVariety.Male;
					else if (ModContent.GetInstance<GVConfig>().ForcedFemale.Any(x => x.Type == npc.type)) setGender = GenderVariety.Female;
					else setGender = Main.rand.NextBool() ? GenderVariety.Male : GenderVariety.Female;
				}
			}

			// Debug Message
			string oldGender = npcData.savedGender == 0 ? "Unassigned" : npcData.savedGender == 1 ? "Male" : "Female";
			string newGender = setGender == 0 ? "Unassigned" : setGender == 1 ? "Male" : "Female";
			GenderVariety.SendDebugMessage($"The {npc.TypeName}({npc.type}) is now a {newGender} (previously {oldGender})", Color.MediumPurple);

			// Update name (texture changes update in PreAI)
			npcData.savedGender = setGender;
			SwapTextures(index, npc);
			npc.GivenName = GenerateAltName(index, npc);
		}

		internal static void SwapTextures(int index, NPC npc){
			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			if (TownNPCInfo.IsAltGender(npc)) {
				if (NPCID.Sets.ExtraTextureCount[npc.type] != 0) Main.npcAltTextures[npc.type][0] = townNPC.npcAltTexture;
				else Main.npcTexture[npc.type] = townNPC.npcAltTexture;
				Main.npcHeadTexture[townNPC.headIndex] = townNPC.npcAltTexture_Head;
			}
			else {
				if (NPCID.Sets.ExtraTextureCount[npc.type] != 0) Main.npcAltTextures[npc.type][0] = townNPC.npcTexture;
				else Main.npcTexture[npc.type] = townNPC.npcTexture;
				Main.npcHeadTexture[townNPC.headIndex] = townNPC.npcTexture_Head;
			}
		}

		internal static string GenerateAltName(int index, NPC npc) {
			// Location should NEVER be -1 since this method can only be called after a check
			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			TownNPCData npcData = TownNPCWorld.SavedData[index];

			// Check saved names first. Saved names are reset on NPC death.
			if (!TownNPCInfo.IsAltGender(npc)) {
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
