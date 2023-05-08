using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BulletHellGameJam
{
    internal class Player
    {
        public float x = 120, y = 200;
        public int width = 16, height = 16;

        public float currentXP = 0;

        public float currentHP = 30;
        public float maxHP = 30;

        float xa = 0, ya = 0;
        float speedX = 3, speedY = 1.5f;

        int shotCooldown = 5;
        int elapsedShotCooldown = 0;

        Boolean keyLeft = false;
        Boolean keyRight = false;
        Boolean keyUp = false;
        Boolean keyDown = false;
        Boolean keyShot = false;

        public float bulletDamage = 2.5f;

        public World world = null;

        public Player()
        {

        }

        public void Update()
        {
            if (elapsedShotCooldown < shotCooldown)
            {
                elapsedShotCooldown++;
            }

            if (keyLeft)
            {
                xa -= speedX;
            }
            if (keyRight)
            {
                xa += speedX;
            }
            if (keyUp)
            {
                ya -= speedY;
            }
            if (keyDown)
            {
                ya += speedY;
            }
            if (keyShot)
            {
                Shot();
            }

            Move();
        }

        public void Move()
        {

            if (xa > 0)
            {
                xa -= speedX;

                if (x < 256 - width - 8)
                    x += speedX;
            }
            if (xa < 0)
            {
                xa += speedX;

                if (x > 8)
                    x -= speedX;
            }
            if (ya > 0)
            {
                ya -= speedY;

                if (y < 240 - height - 8)
                    y += speedY;
            }
            if (ya < 0)
            {
                ya += speedY;

                if (y > 100)
                    y -= speedY;
            }
        }

        public void Render(Graphics g)
        {
            g.FillRectangle(Brushes.LightGreen, x, y, width, height);
        }

        public void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                keyLeft = true;
            }
            if (e.KeyCode == Keys.D)
            {
                keyRight = true;
            }
            if (e.KeyCode == Keys.W)
            {
                keyUp = true;
            }
            if (e.KeyCode == Keys.S)
            {
                keyDown = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                keyShot = true;
            }
        }
        public void KeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                keyLeft = false;
            }
            if (e.KeyCode == Keys.D)
            {
                keyRight = false;
            }
            if (e.KeyCode == Keys.W)
            {
                keyUp = false;
            }
            if (e.KeyCode == Keys.S)
            {
                keyDown = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                keyShot = false;
            }
        }

        public void Shot()
        {
            if (world == null) return;
            if (elapsedShotCooldown == shotCooldown)
            {
                Bullet bullet1 = new Bullet(BulletSource.Player, (int)x + 2, (int)y);
                Bullet bullet2 = new Bullet(BulletSource.Player, (int)x + 11, (int)y);

                bullet1.damage = bulletDamage + (0.3f * GetCurrentLevel());
                bullet2.damage = bulletDamage + (0.3f * GetCurrentLevel());

                world.AddBullet(BulletSource.Player, (int)x + 2, (int)y);
                world.AddBullet(BulletSource.Player, (int)x + 11, (int)y);
                elapsedShotCooldown = 0;
            }
        }
        int oldLevel = 0;
        int currentLevel = 0;
        public Rectangle getRect()
        {
            return new Rectangle((int)x, (int)y, width, height);
        }
        public void GetXP(float amount)
        {
            currentXP += amount;
            oldLevel = currentLevel;
            currentLevel = GetCurrentLevel();
            if(currentLevel != oldLevel)
            {
                maxHP *= 1.1f;
                Repair(maxHP / 5);
            }

        }
        public int GetCurrentLevel()
        {
            return (int)Math.Floor(0.7f * (Math.Pow(currentXP, 0.32f)));
        }
        public void Repair(float amount)
        {
            currentHP += amount;
            if(currentHP>maxHP)
            {
                currentHP = maxHP;
            }
        }

    }
}
