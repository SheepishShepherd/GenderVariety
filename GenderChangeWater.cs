using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ModLoader.Config;

namespace GenderVariety
{
	public class GenderChangeWater : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Has the ability to swap an npc's gender on hit.");
		}

		public override void SetDefaults() {
			Item.consumable = true;
			Item.damage = 20;
			Item.knockBack = 3f;
			Item.height = 20;
			Item.maxStack = 999;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<GenderChangeWater_Proj>();
			Item.shootSpeed = 9f;
			Item.useAnimation = 15;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.value = 200;
			Item.width = 18;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			int index = tooltips.FindIndex(x => x.Name == "Damage" || x.Name == "Favorite");
			if (index != -1) {
				string tooltip = $"[i:{ItemID.MulticolorWrench}] [c/{Color.Wheat.Hex3()}:Debug Item] [i:{ItemID.MulticolorWrench}]";
				tooltips.Insert(index, new TooltipLine(Mod, "DebugItem", tooltip));
			}
		}
	}

	public class GenderChangeWater_Proj : ModProjectile
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
			// Debug chat text: To show which NPC was hit with Gender Change Water
			GenderVariety.SendDebugMessage($"{target.FullName}({target.type}) was hit with Gender Change Water", Color.Purple);

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
			SoundEngine.PlaySound(SoundID.Shatter, (int)Projectile.position.X, (int)Projectile.position.Y);

			// Create dust of broken glass
			for (int i = 0; i < 5; i++) {
				// Broken glass
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Glass);
			}

			// Create dust of the Gender Water liquid
			for (int j = 0; j < 30; j++) {
				// Water Content
				bool purple = j % 2 == 0 || j > 10;
				int newDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, j % 2 == 0 || j > 10 ? 98 : 101, 0f, -2f, 0, default, 1.1f);
				
				Main.dust[newDust].alpha = 100;
				ref float x = ref Main.dust[newDust].velocity.X;
				x *= 1.5f;
				Dust dust = Main.dust[newDust];
				dust.velocity *= 3f;
			}
		}
	}
}
