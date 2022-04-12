using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace GenderVariety
{
	public class GenderChangeWater : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Has the ability to swap genders on hit.");
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
			if (index != -1) tooltips.Insert(index, new TooltipLine(ModContent.GetInstance<GenderVariety>(), "DebugItem", $"[i:{ItemID.MulticolorWrench}] [c/{Color.Wheat.Hex3()}:Debug Item] [i:{ItemID.MulticolorWrench}]"));
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

		public override bool? CanHitNPC(NPC target) => GenderVariety.GetNPCIndex(target.type) >= 0;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			int index = GenderVariety.GetNPCIndex(target.type);
			if (index == -1) return;
			GenderVariety.SendDebugMessage($"{target.FullName}({target.type}) was hit with Gender Change Water", Color.ForestGreen);

			TownNPCInfo townNPC = GenderVariety.townNPCList.townNPCs[index];
			TownNPCData npcData = TownNPCWorld.SavedData[index];

			int newGender = 0;
			if (npcData.savedGender == TownNPCSetup.Unassigned) {
				if (townNPC.ogGender == TownNPCSetup.Male) newGender = TownNPCSetup.Female;
				else newGender = TownNPCSetup.Male;
			}
			else {
				if (npcData.savedGender == TownNPCSetup.Male) newGender = TownNPCSetup.Female;
				if (npcData.savedGender == TownNPCSetup.Female) newGender = TownNPCSetup.Male;
			}
			TownNPCData.AssignGender(target, newGender);
		}

		public override void Kill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Shatter, (int)Projectile.position.X, (int)Projectile.position.Y);
			for (int i = 0; i < 5; i++) {
				// Broken glass
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 13);
			}
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
