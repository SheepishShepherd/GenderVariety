using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ModLoader.Config;

namespace GenderVariety.Items
{
	public class GenderChangeElixir : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Has the ability to swap the gender of townsfolk on hit.");
		}

		public override void SetDefaults() {
			Item.damage = 1;
			Item.knockBack = 2.5f;
			Item.height = 20;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Orange;
			Item.reuseDelay = 64;
			Item.shoot = ModContent.ProjectileType<GenderChangeElixir_Proj>();
			Item.shootSpeed = 9f;
			Item.useAnimation = 19;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 64;
			Item.width = 18;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			// Add a DEBUG ITEM tooltip
			int index = tooltips.FindIndex(x => x.Name == "Damage" || x.Name == "Favorite");
			if (index != -1) {
				string wrench = $"[i:{ItemID.MulticolorWrench}]";
				string tooltip = $"{wrench} [c/{Color.Wheat.Hex3()}:Debug Item] {wrench}]";
				tooltips.Insert(index, new TooltipLine(Mod, "DebugItem", tooltip));
			}

			// Remove the damage and crit chance tooltips
			tooltips.RemoveAll(x => x.Name == "Damage");
			tooltips.RemoveAll(x => x.Name == "CritChance");
		}
	}

	public class GenderChangeElixir_Proj : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.aiStyle = 2;
			Projectile.friendly = true;
			Projectile.height = 14;
			Projectile.penetrate = 1;
			Projectile.width = 14;
		}

		// This projectile should only hit NPCs that can have their gender changed
		public override bool? CanHitNPC(NPC target) => GenderVariety.townNPCList.GetNPCIndex(target.type) != -1;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			// Debug chat text: To show which NPC was hit with Gender Change Elixir
			string npc = $"{target.FullName} [{target.type}]";
			GenderVariety.SendDebugMessage($"{npc} was hit with Gender Change Elixir", Color.Purple);

			// Index is indirectly checked for a -1 value by CanHitNPC. NPCs who aren't on the list won't ever be hit.
			TownNPCInfo info = GenderVariety.townNPCList.GetNPCInfo(target.type);

			// Determine the new gender based on what is saved.
			// If unassigned, set the gender to the opposite of the original (default) gender.
			// Otherwise, set the gender opposite of what it is currently saved as.
			Gender savedGender = TownNPCWorld.SavedGenders[new NPCDefinition(target.type)];
			Gender newGender = savedGender switch {
				Gender.Male => Gender.Female,
				Gender.Female => Gender.Male,
				_ => info.originalGender == Gender.Male ? Gender.Female : Gender.Male,
			};

			// Assign the gender for the hit target.
			TownNPCs.AssignGender(target, newGender);
		}

		public override void Kill(int timeLeft) {
			// Play a glass shatter sound
			SoundEngine.PlaySound(SoundID.Shatter, Projectile.position);

			// Create dust of broken glass
			for (int i = 0; i < 5; i++) {
				// Broken glass
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Glass);
			}

			// Create dust of the Gender Elixir liquid
			for (int j = 0; j < 30; j++) {
				// Water Content
				short purple = j % 2 == 0 || j > 10 ? DustID.Water_Corruption : DustID.Water_Snow;
				int newDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, purple, 0f, -2f);

				Main.dust[newDust].alpha = 100;
				ref float x = ref Main.dust[newDust].velocity.X;
				x *= 1.5f;
				Main.dust[newDust].velocity *= 3f;
			}
		}
	}
}
