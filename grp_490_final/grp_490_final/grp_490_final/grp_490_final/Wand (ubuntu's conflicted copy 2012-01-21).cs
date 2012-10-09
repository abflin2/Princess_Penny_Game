using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNACS1Lib;

namespace grp_490_final {
	class Wand {

		public struct shot {
			public XNACS1Circle bullet;
			public bool hit;
			public XNACS1ParticleEmitter.FireEmitter emitter;

			public shot(Vector2 center, bool hit) {
				bullet = new XNACS1Circle(center, 1f, "bullet");
				this.hit = hit;
				emitter = new XNACS1ParticleEmitter.FireEmitter(center, 30, 0.5f, "bullet", Color.OrangeRed, 20f, 3f, new Vector2(-1, 0));
			}
		}

		List<shot> list = new List<shot>();

		int hits = 0;

		public Vector2 Update(Enemy enemy) {
			Vector2 s = new Vector2(-1f, -1f);
			for (int i = 0; i < list.Count; i++) {
				shot temp = list[i];
				if (list[i].bullet.Collided(enemy) && !list[i].hit) {
					s = enemy.TurnEnemyToBunny();
				    temp.hit = true;
				    list[i] = temp;
				}
				list[i].emitter.Center = list[i].bullet.Center;
			}

			return s;
		}

		public void Shoot(XNACS1Rectangle hero) {
			shot s = new shot(hero.Center, false);
			s.bullet.ShouldTravel = true;
			s.bullet.Velocity = (XNACS1Base.GamePad.ThumbSticks.Right.X == 0 && XNACS1Base.GamePad.ThumbSticks.Right.Y == 0) ? new Vector2(hero.Texture == "heroRight" ? 1 : -1, 0) : XNACS1Base.GamePad.ThumbSticks.Right;
			list.Add(s);

			BoundCollideStatus status = XNACS1Base.World.CollideWorldBound(s.bullet);
			switch (status) {
				case BoundCollideStatus.CollideBottom:
				case BoundCollideStatus.CollideLeft:
				case BoundCollideStatus.CollideRight:
				case BoundCollideStatus.CollideTop:
					s.bullet.RemoveFromAutoDrawSet();
					list.Remove(s);
					s.bullet = null;
					break;
			}
		}
	}
}
