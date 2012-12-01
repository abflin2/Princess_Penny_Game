using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNACS1Lib;

namespace grp_490_final {
	class Wizard : XNACS1Rectangle {

		public int healthRemaining;
		int coolDown;
		int shieldCoolDown;
		bool shielded;
		//string leftTex;  //left texture
		//string rightTex;
		enum wizardStatus { isAlive, isDead };
		wizardStatus current;
		public bool heroHit;
		public HealthBar health;
		public int initialLives;
		public struct shot {
			public XNACS1Circle bullet;
			public bool hit;
			public XNACS1ParticleEmitter.FireEmitter emitter;
			public int shotLife;

			public shot(Vector2 center, bool hit) {
				bullet = new XNACS1Circle(center, .75f, "wizardShot");
				bullet.Speed = 1f;
				this.hit = hit;
				shotLife = 40;
				emitter = new XNACS1ParticleEmitter.FireEmitter(center, 10, 0.3f, "wizardShot", Color.Green, 1f, 3f, new Vector2(-1, 0));
			}
		}

		List<shot> list = new List<shot>();

		public Wizard(Vector2 center) {
			healthRemaining = 10;
			//this.Center = new Vector2(325, 240);
			Center = center;
			current = wizardStatus.isAlive;
			Width = Height = 6;
			Texture = "wizardRight";
			coolDown = 5;
			shieldCoolDown = 100;
			shielded = false;
			heroHit = false;
			//leftTex = texOther;
			//rightTex = tex;
			initialLives = 10;
			health = new HealthBar(this.Center);
		}
		

		public bool Update(Hero hero, Wand w) {
			heroHit = false;
			health.Update(new Vector2(this.Center.X, this.Center.Y+1), healthRemaining, initialLives);

			if (current == wizardStatus.isAlive) {
				if (shieldCoolDown < 0 && !shielded) {
					shielded = true;
					shieldCoolDown = 100;
				}
				else
					shieldCoolDown--;

				if (shieldCoolDown < 0 && shielded) {
					shielded = false;
					shieldCoolDown = 100;
				}


				if (shielded) {
					Texture = "wizardShield";

				}
				else {
					Texture = "wizardRight";
					if (coolDown < 0) {
						coolDown = 40;
						shoot(hero.Center);
					}
					else
						coolDown--;
				}

				for (int i = 0; i < list.Count; i++) {
					shot temp = list[i];
					if (list[i].shotLife == 0) {
						list[i].bullet.RemoveFromAutoDrawSet();
						list[i].emitter.RemoveFromAutoDrawSet();
						list.Remove(list[i]);
					}
					else {
						temp.shotLife--;
						if (hero.Collided(temp.bullet) && !temp.hit) {
							hero.Caught();
							hero.Center = hero.HeroInitialPos;
							temp.hit = true;
							heroHit = true;
						}
						list[i] = temp;
						list[i].emitter.Center = list[i].bullet.Center;
					}
				}
				for (int i = 0; i < w.list.Count; i++)
					if (w.list[i].bullet.Collided(this)) {
						if (!shielded)
							healthRemaining--;
						else {
							Random r = new Random();
							if (r.Next(0, 100) < 10)
								healthRemaining--;
						}
					}

				if (healthRemaining <= 0) {
					current = wizardStatus.isDead;
					Texture = "Newteddybear";
                    XNACS1Base.PlayACue("Bear-Sound");

					return true;
				}

				if (this.Center.X - hero.Center.X > 0)
					Texture = shielded ? "wizardShieldLeft" : "wizardLeft";
				else
					Texture = shielded ? "wizardShield" : "wizardRight";
			}

			if (current == wizardStatus.isDead) {
				for (int i = 0; i < list.Count; i++) {
					list[i].bullet.RemoveFromAutoDrawSet();
					list[i].emitter.RemoveFromAutoDrawSet();
					list.Remove(list[i]);
				}
			}
			return false;
		}

		private void shoot(Vector2 target) {
			shot s = new shot(this.Center, false);
			s.bullet.ShouldTravel = true;
			s.bullet.VelocityDirection = target - Center;
			list.Add(s);
		}


	}
}
