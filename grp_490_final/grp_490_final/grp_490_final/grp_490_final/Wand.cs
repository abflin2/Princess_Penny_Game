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
			public int shotLife;

			public shot(Vector2 center, bool hit) {
				bullet = new XNACS1Circle(center, .75f, "violetlight");
				bullet.Speed = 1f;
				this.hit = hit;
				shotLife = 15;
				emitter = new XNACS1ParticleEmitter.FireEmitter(center, 10, 0.3f, "violetlight", Color.Pink, 1f, 3f, new Vector2(-1, 0));
			}
		}

		public List<shot> list = new List<shot>();
		int hits = 0;
		int level;

		public void UpdateWand(int level) { this.level = level; }

		public Vector2 Update(Enemy[] enemy, WallSet walls) {
			Vector2 s = new Vector2(-1f, -1f);
			for (int i = 0; i < list.Count; i++) {
				shot temp = list[i];
				if (list[i].shotLife == 0) {
					list[i].bullet.RemoveFromAutoDrawSet();
					list[i].emitter.RemoveFromAutoDrawSet();
					list.Remove(list[i]);
				}
				else {
					temp.shotLife--;
					for (int j = 0; j < enemy.Length; j++) {
						list[i] = temp;
						if (list[i].bullet.Collided(enemy[j]) && !list[i].hit) {
							enemy[j].Caught();
							if (enemy[j].NumLivesLeft <= 0) {
								s = enemy[j].TurnEnemyToBunny();
							}

							temp.bullet.RemoveFromAutoDrawSet();
							temp.emitter.RemoveFromAutoDrawSet();
							list.Remove(list[i]);
							temp.hit = true;
							break;
						}

				
					}
					if (!temp.hit) {
						list[i] = temp;

						list[i].emitter.Center = list[i].bullet.Center;

						for (int k = 0; k < walls.RoomWalls.Length; k++) {
							if (temp.bullet.Collided(walls.RoomWalls[k])) {
								temp.bullet.RemoveFromAutoDrawSet();
								temp.emitter.RemoveFromAutoDrawSet();
								list.Remove(list[i]);
							}
						}
					}
				}
			}

			return s;
		}
		public void Shoot(XNACS1Rectangle hero) {
			Vector2 velDir;
			XNACS1Base.PlayACue("spell");
			switch (level) {
				case 0:
					shot s = new shot(hero.Center, false);
					s.bullet.ShouldTravel = true;
					s.bullet.VelocityDirection =
						(XNACS1Base.GamePad.ThumbSticks.Right.X == 0 && XNACS1Base.GamePad.ThumbSticks.Right.Y == 0)
						? new Vector2(hero.Texture == "newPrincessRight" ? 1 : -1, 0) : XNACS1Base.GamePad.ThumbSticks.Right;
					list.Add(s);
					break;

				case 1:
					shot s2 = new shot(hero.Center, false);
					shot s3 = new shot(hero.Center, false);

					velDir =
						(XNACS1Base.GamePad.ThumbSticks.Right.X == 0 && XNACS1Base.GamePad.ThumbSticks.Right.Y == 0)
						? new Vector2(hero.Texture == "newPrincessRight" ? 1 : -1, 0) : XNACS1Base.GamePad.ThumbSticks.Right;

					

					if (velDir.X < 0f && velDir.Y < 0f) {
					    s2.bullet.VelocityDirection = new Vector2(velDir.X + .1f, velDir.Y - .1f);
					    s3.bullet.VelocityDirection = new Vector2(velDir.X - .1f, velDir.Y + .1f);
					}

					else if (velDir.Y < 0f && velDir.X > 0f) {
					    s2.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y - .1f);
					    s3.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y + .1f);
					
					}

					else if (velDir.Y > 0f && velDir.X < 0f) {
					    s2.bullet.VelocityDirection = new Vector2(velDir.X -.1f , velDir.Y);
					    s3.bullet.VelocityDirection = new Vector2(velDir.X + .1f, velDir.Y);
					}

					else {
						s2.bullet.VelocityDirection = new Vector2(velDir.X + .1f, velDir.Y - .1f);
						s3.bullet.VelocityDirection = new Vector2(velDir.X - .1f, velDir.Y + .1f);
					}

					s2.bullet.ShouldTravel = true;
					s3.bullet.ShouldTravel = true;
					list.Add(s2);
					list.Add(s3);

					break;
					
				case 2:
					shot s4 = new shot(hero.Center, false);
					shot s5 = new shot(hero.Center, false);
					shot s6 = new shot(hero.Center, false);

					s4.bullet.ShouldTravel = true;
					s5.bullet.ShouldTravel = true;
					s6.bullet.ShouldTravel = true;


					velDir =
						(XNACS1Base.GamePad.ThumbSticks.Right.X == 0 && XNACS1Base.GamePad.ThumbSticks.Right.Y == 0)
						? new Vector2(hero.Texture == "newPrincessRight" ? 1 : -1, 0) : XNACS1Base.GamePad.ThumbSticks.Right;


					if (velDir.X < 0f && velDir.Y < 0f) {
						s4.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y);
						s5.bullet.VelocityDirection = new Vector2(velDir.X + .1f, velDir.Y - .1f);
						s6.bullet.VelocityDirection = new Vector2(velDir.X - .1f, velDir.Y + .1f);
					}

					else if (velDir.Y < 0f && velDir.X > 0f) {
						s4.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y);
						s5.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y - .1f);
						s6.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y + .1f);

					}

					else if (velDir.Y > 0f && velDir.X < 0f) {
						s4.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y);
						s5.bullet.VelocityDirection = new Vector2(velDir.X - .1f, velDir.Y);
						s6.bullet.VelocityDirection = new Vector2(velDir.X + .1f, velDir.Y);
					}

					else {
						s4.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y);
						s5.bullet.VelocityDirection = new Vector2(velDir.X + .1f, velDir.Y - .1f);
						s6.bullet.VelocityDirection = new Vector2(velDir.X - .1f, velDir.Y + .1f);
					}
					
					list.Add(s4);
					list.Add(s5);
					list.Add(s6);

					break;

				default:

					shot s7 = new shot(hero.Center, false);
					shot s8 = new shot(hero.Center, false);
					shot s9 = new shot(hero.Center, false);
					shot s10 = new shot(hero.Center, false);

					s7.bullet.ShouldTravel = true;
					s8.bullet.ShouldTravel = true;
					s9.bullet.ShouldTravel = true;
					s10.bullet.ShouldTravel = true;

					velDir =
						(XNACS1Base.GamePad.ThumbSticks.Right.X == 0 && XNACS1Base.GamePad.ThumbSticks.Right.Y == 0)
						? new Vector2(hero.Texture == "newPrincessRight" ? 1 : -1, 0) : XNACS1Base.GamePad.ThumbSticks.Right;


					if (velDir.X < 0f && velDir.Y < 0f) {
						s7.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y);
						s8.bullet.VelocityDirection = new Vector2(velDir.X + .1f, velDir.Y - .1f);
						s9.bullet.VelocityDirection = new Vector2(velDir.X - .1f, velDir.Y + .1f);
						s10.bullet.VelocityDirection = new Vector2(velDir.X * -1f, velDir.Y * -1f);
					}

					else if (velDir.Y < 0f && velDir.X > 0f) {
						s7.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y);
						s8.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y - .1f);
						s9.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y + .1f);
						s10.bullet.VelocityDirection = new Vector2(velDir.X * -1f, velDir.Y * -1f);
					}

					else if (velDir.Y > 0f && velDir.X < 0f) {
						s7.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y);
						s8.bullet.VelocityDirection = new Vector2(velDir.X - .1f, velDir.Y);
						s9.bullet.VelocityDirection = new Vector2(velDir.X + .1f, velDir.Y);
						s10.bullet.VelocityDirection = new Vector2(velDir.X * -1f, velDir.Y * -1f);
					}

					else {
						s7.bullet.VelocityDirection = new Vector2(velDir.X, velDir.Y);
						s8.bullet.VelocityDirection = new Vector2(velDir.X + .1f, velDir.Y - .1f);
						s9.bullet.VelocityDirection = new Vector2(velDir.X - .1f, velDir.Y + .1f);
						s10.bullet.VelocityDirection = new Vector2(velDir.X * -1f, velDir.Y * -1f);
					}
					

					list.Add(s7);
					list.Add(s8);
					list.Add(s9);
					list.Add(s10);
					break;


			}
		}
	}
}
