using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace GenderVariety
{
	public class TownNPCWorld : ModWorld
	{
		internal static List<TownNPCData> SavedData;

		public override void Initialize() {
			SavedData = new List<TownNPCData>();

			for (int i = 0; i < GenderVariety.townNPCList.townNPCs.Count; i++) {
				TownNPCInfo list = GenderVariety.townNPCList.townNPCs[i];
				SavedData.Add(new TownNPCData(list.type, TownNPCs.Unassigned, "", ""));
			}
		}

		public override TagCompound Save() {
			TagCompound savedTag = new TagCompound {
				{"SavedTownNPCData", SavedData}
			};
			return savedTag;
		}

		public override void Load(TagCompound tag) {
			SavedData = tag.Get<List<TownNPCData>>("SavedTownNPCData");
		}
	}

	public class TownNPCs : GlobalNPC
	{
		public const int Unassigned = 0;
		public const int Male = 1;
		public const int Female = 2;

		public override bool InstancePerEntity => true;
		public int setGender = Unassigned;
		public bool altGender = false;
		public bool genderChanged = false;

		public override void SetDefaults(NPC npc) {
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (index != -1 && !Main.gameMenu) {
				setGender = AssignGender(npc);
				Main.NewText(setGender, Color.LightPink);
				if (npc.type == NPCID.PartyGirl || npc.type == NPCID.SantaClaus) ChangeTypeName(npc, altGender);
			}
		}

		public override void SpawnNPC(int npc, int tileX, int tileY) {
			// Only works for modded npcs :(
		}
		
		// Leave 0 to choose at random/config. Using 1 or 2 sets it to that gender
		public int AssignGender(NPC npc, int setGender = 0) {
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (index == -1) {
				GenderVariety.SendDebugMessage($"NPC is not listed ({npc.type})", Color.LightYellow);
				return 0;
			}

			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			TownNPCData npcData = TownNPCWorld.SavedData[index];
			string savedData = "";
			// If we aren't setting a gender manually, go through the OnSpawn process
			if (setGender == Unassigned) {
				if (npcData.savedGender != Unassigned) {
					setGender = npcData.savedGender;
					if (ModContent.GetInstance<GVConfig>().EnableDebugMode) savedData = $" (saved)";
				}
				else {
					if (ModContent.GetInstance<GVConfig>().ForcedMale.Any(x => x.Type == npc.type)) setGender = Male;
					else if (ModContent.GetInstance<GVConfig>().ForcedFemale.Any(x => x.Type == npc.type)) setGender = Female;
					else setGender = Main.rand.NextBool() ? Male : Female;
				}
			}

			string oldGender = this.setGender == 0 ? "Unassigned" : this.setGender == 1 ? "Male" : "Female";
			string newGender = setGender == 0 ? "Unassigned" : setGender == 1 ? "Male" : "Female";

			GenderVariety.SendDebugMessage($"Changed gender for {npc.type} from {oldGender} to {newGender}{savedData}", Color.MediumPurple);
			
			altGender = (townNPC.isMale && setGender == Female) || (!townNPC.isMale && setGender == Male); // Determine if alternate gender
			SwapNPCTextures(index, npc.type);

			TownNPCWorld.SavedData[index].savedGender = setGender; // Update world save
			return setGender;
		}

		public void ChangeTypeName(NPC npc, bool isAlt) {
			// TODO: this doesnt work, fix it
			string value = Lang.GetNPCNameValue(npc.netID);
			if (npc.type == NPCID.PartyGirl) value = "Party Boy";
			else if (npc.type == NPCID.SantaClaus) value = "Mrs. Claus";
			PropertyInfo property = typeof(NPC).GetProperty("TypeName", BindingFlags.Public);
			if (property != null) property.SetValue(npc, value);
		}

		public override void NPCLoot(NPC npc) {
			// Reset saved data here
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (index == -1) return;
			TownNPCWorld.SavedData[index] = new TownNPCData(npc.type, Unassigned, "", "");
		}

		// Swapped genders go to respective statues
		public override bool? CanGoToStatue(NPC npc, bool toKingStatue) {
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (altGender && index != -1 && npc.type != NPCID.SantaClaus) { // Santa doesnt teleport in vanilla
				TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
				return toKingStatue ? !townNPC.isMale : townNPC.isMale; // Since we check for altGender, we do the opposite
			}
			return null;
		}

		public override void GetChat(NPC npc, ref string chat) {
			base.GetChat(npc, ref chat);
		}

		public void SwapNPCTextures(int index, int npcType) {
			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			GenderVariety.SendDebugMessage("Changing gender textures...", Color.ForestGreen);
			if (altGender) {
				Main.npcTexture[npcType] = townNPC.npcAltTexture;
				Main.npcHeadTexture[townNPC.headIndex] = townNPC.npcAltTexture_Head;
			}
			else {
				Main.npcTexture[npcType] = townNPC.npcTexture;
				Main.npcHeadTexture[townNPC.headIndex] = townNPC.npcTexture_Head;
			}
		}

		// TODO: New appropriate names for our genderswapped friends
		public static string GenerateAltName(int type) {
			switch (type) {
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
		
		public override bool PreAI(NPC npc) {
			// Find the list location of the NPC
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
			if (index != -1) {
				//if (setGender == Unassigned) setGender = AssignGender(npc);
				if (altGender) {
					if (TownNPCWorld.SavedData[index].altName == "") {
						TownNPCWorld.SavedData[index].altName = npc.GivenName;
					}
				}
				else {
					if (TownNPCWorld.SavedData[index].name == "") {
						TownNPCWorld.SavedData[index].name = npc.GivenName;
					}
				}
			}
			return true;
		}
	}
}
