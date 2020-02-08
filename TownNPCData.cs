using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

	internal class TownNPCData : TagSerializable
	{
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

		internal static TownNPCData AddTownNPC(int type, int gender, string name, string altName) {
			return new TownNPCData(type, gender, name, altName);
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
