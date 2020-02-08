using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;

namespace GenderVariety
{
	// TODO: Update NPC textures (and more) for Multiplayer

	public class GenderVariety : Mod
	{
		internal static TownNPCSetup townNPCList;

		public GenderVariety() {

		}

		public override void Load() {
			townNPCList = new TownNPCSetup();
			IL.Terraria.NPC.NewNPC += NPC_NewNPC;
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
					if (Main.npc[num].GetGlobalNPC<TownNPCs>().altGender) {
						SendDebugMessage($"setGender = {Main.npc[num].GetGlobalNPC<TownNPCs>().setGender} (altGender = {Main.npc[num].GetGlobalNPC<TownNPCs>().altGender})", Color.LightPink);
						SendDebugMessage($"Name decided BEFORE change: {Main.npc[num].GivenName}", Color.LightPink);
						Main.npc[num].GivenName = TownNPCs.GenerateAltName(Main.npc[num].type);
						SendDebugMessage($"Name decided AFTER change: {Main.npc[num].GivenName}", Color.LightPink);
					}
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
