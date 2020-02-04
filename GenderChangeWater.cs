﻿using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;

namespace GenderVariety
{
	public class GenderChangeWater : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Has the ability to swap genders on hit.");
		}

		public override void SetDefaults() {
			item.consumable = true;
			item.damage = 20;
			item.knockBack = 3f;
			item.height = 20;
			item.maxStack = 999;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.rare = 3;
			item.shoot = ModContent.ProjectileType<GenderChangeWater_Proj>();
			item.shootSpeed = 9f;
			item.useAnimation = 15;
			item.UseSound = SoundID.Item1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 15;
			item.value = 200;
			item.width = 18;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			int index = tooltips.FindIndex(x => x.Name == "Damage" || x.Name == "Favorite");
			if (index != -1) tooltips.Insert(index, new TooltipLine(ModContent.GetInstance<GenderVariety>(), "DebugItem", $"[i:{ItemID.MulticolorWrench}] [c/{Color.Wheat.Hex3()}:Debug Item] [i:{ItemID.MulticolorWrench}]"));
		}
	}

	public class GenderChangeWater_Proj : ModProjectile
	{
		public override void SetDefaults() {
			projectile.aiStyle = 2;
			projectile.friendly = true;
			projectile.height = 14;
			projectile.penetrate = 1;
			projectile.width = 14;
		}

		public override bool? CanHitNPC(NPC target) => GenderVariety.townNPCList.townNPCs.Any(x => x.type == target.type);

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			int index = GenderVariety.townNPCList.townNPCs.FindIndex(x => x.type == target.type);
			if (index == -1) return;
			GenderVariety.SendDebugMessage($"You hit {target.FullName}", Color.ForestGreen);
			TownNPCs townNPCs = target.GetGlobalNPC<TownNPCs>();
			TownNPCInfo npcInfo = GenderVariety.townNPCList.townNPCs[index];

			int newGender = 0;
			if (npcInfo.isMale) newGender = townNPCs.altGender ? TownNPCs.Male : TownNPCs.Female;
			else newGender = townNPCs.altGender ? TownNPCs.Female : TownNPCs.Male;
			townNPCs.setGender = townNPCs.AssignGender(target, newGender);
		}

		public override void Kill(int timeLeft) {
			Main.PlaySound(SoundID.Shatter, (int)projectile.position.X, (int)projectile.position.Y);
			for (int i = 0; i < 5; i++) {
				// Broken glass
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 13);
			}
			for (int j = 0; j < 30; j++) {
				// Water Content
				bool purple = j % 2 == 0 || j > 10;
				int newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, j % 2 == 0 || j > 10 ? 98 : 101, 0f, -2f, 0, default, 1.1f);
				
				Main.dust[newDust].alpha = 100;
				ref float x = ref Main.dust[newDust].velocity.X;
				x *= 1.5f;
				Dust dust = Main.dust[newDust];
				dust.velocity *= 3f;
			}
		}
	}
}
