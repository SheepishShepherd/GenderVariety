using Terraria.ID;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using Terraria;
using Terraria.ModLoader;

namespace GenderVariety
{
	// TODO: Update NPC textures (and more) for Multiplayer
	public class GenderVariety : Mod
	{
		public const int Unassigned = 0;
		public const int Male = 1;
		public const int Female = 2;

		internal static TownNPCSetup townNPCList;

		public GenderVariety() {

		}

		public override void Load() {
			townNPCList = new TownNPCSetup();
			IL.Terraria.NPC.NewNPC += NPC_NewNPC;
			IL_NPC_TypeName += ChangeNPCTypeName;
		}

		private void ChangeNPCTypeName(ILContext il) {
			ILCursor c = new ILCursor(il);
			if (!c.TryGotoNext(i => i.MatchRet())) return;
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<Func<string, NPC, string>>(
				delegate (string ogValue, NPC npc) {
					int index = townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
					if (index == -1) return ogValue;
					
					TownNPCData npcData = TownNPCWorld.SavedData[index];
					if (TownNPCs.IsAltGender(npc)) {
						if (npc.type == NPCID.PartyGirl) return "Party Boy";
						else if (npc.type == NPCID.SantaClaus) return "Mrs. Claus";
					}
					return ogValue;
				});
		}

		public static event ILContext.Manipulator IL_NPC_TypeName {
			add {
				HookEndpointManager.Modify(typeof(NPC).GetProperty(nameof(NPC.TypeName)).GetGetMethod(), value);
			}
			remove {
				HookEndpointManager.Unmodify(typeof(NPC).GetProperty(nameof(NPC.TypeName)).GetGetMethod(), value);
			}
		}

		private void NPC_NewNPC(ILContext il) {
			//create the il cursor
			ILCursor c = new ILCursor(il);

			//go to line 0
			c.Goto(0);

			//Hold a local varible with the il label
			ILLabel wherestdve = null;

			// find the if block that compares the values
			if (c.TryGotoNext(i => i.MatchBlt(out _) && (i.Previous?.Match(OpCodes.Ldc_I4_0) == true) && (i.Previous?.Previous?.Match(OpCodes.Ldloc_0) == true)) &&
				c.TryGotoNext(i => i.MatchBneUn(out wherestdve))) {
				c.GotoLabel(wherestdve); // go to where the if block ends

				c.Index += 8; //insert above it somehow works??? EDIT: Idk where it is but +8 seems to work

				//Get the num param
				c.Emit(OpCodes.Ldloc_0);

				//Emit delegate action code
				c.EmitDelegate<Action<int>>(delegate (int num) {
					//Code to insert/inject
					NPC npc = Main.npc[num];
					int index = townNPCList.townNPCs.FindIndex(x => x.type == npc.type);
					if (index != -1) TownNPCData.AssignGender(npc);
				});
			}
			else Logger.Error("IL Error Fail"); //Log the error
		}

		public override void Unload() {
			// Reset the textures!
			for (int i = 0; i < townNPCList.townNPCs.Count; i++) {
				TownNPCInfo townNPC = townNPCList.townNPCs[i];
				Main.npcTexture[townNPC.type] = townNPC.npcTexture;
				Main.npcHeadTexture[townNPC.headIndex] = townNPC.npcTexture_Head;
			}
			townNPCList = null;
		}

		public static void SendDebugMessage(string message, Color color = default) {
			if (ModContent.GetInstance<GVConfig>().EnableDebugMode) Main.NewText(message, color);
		}
	}
}
