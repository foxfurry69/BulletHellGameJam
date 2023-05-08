using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellGameJam
{
    internal class Enemy
    {
        Random random = new Random();

        float x, y, xa, ya;

        float ringX = 0;
        float ringY = 0;

        int width = 16, height = 16;
        int speedX = 0, speedY = 0;

        int elapsedTicks = 0;

        public float maxHP = 10;
        public float currentHP = 10;

        float maxCloak = 80;
        float currentCloak = 0;

        int bulletCooldown = 30;
        int elapsedBulletCooldown = -10;

        float bulletDamage = 4;
        int bulletRounds = 5;

        int retreatAtY = 230;
        int minX = 10;
        int maxX = 246;
        int maxY = 140;

        public Boolean hasCloak = false;
        public Boolean outOfreach = false;

        EnemyTypes type = EnemyTypes.Fighter;

        Boolean doingRing = false;
        Boolean doingJumps = false;
        Boolean pursuingPlayer = false;
        Boolean spawningAllies = false;
        Boolean canCloak = false;

        Color color = Color.Red;

        public World world;

        public Enemy()
        {
            x = random.Next(8, 240);
            y = -20;
            Randomize();
        }

        public void Update()
        {
            elapsedTicks++;

            if (elapsedBulletCooldown < bulletCooldown)
            {
                elapsedBulletCooldown++;
            }

            Move();
            Shot();

            if (y > 280)
            {
                outOfreach = true;
            }
        }
        public void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(color), GetRectangle());
        }
        public void Move()
        {
            int playerX = (int)world.player.x;
            int playerY = (int)world.player.y;

            if (!pursuingPlayer)
            {
                ya += speedY;
            }


            if (pursuingPlayer)
            {
                if (x < playerX)
                {
                    xa += speedX;
                }
                if (x > playerX)
                {
                    xa -= speedX;
                }
                if (y < maxY)
                {
                    ya += speedY;
                }
                
            }

            if (doingJumps)
            {
                MoveInJumps();
            }
            if (doingRing)
            {
                MoveInRing(60, elapsedTicks * 3);
            }

            if (xa > 0)
            {
                xa -= speedX;

                x += speedX;
            }
            if (xa < 0)
            {
                xa += speedX;

                x -= speedX;
            }
            if (ya > 0)
            {
                ya -= speedY;

                y += speedY;
            }
            if (ya < 0)
            {
                ya += speedY;

                y -= speedY;
            }
            if (y > 290)
            {
                y = -40;
            }
            if (x < minX)
            {
                x = minX;
            }
            if (x > maxX)
            {
                x = maxX;
            }

        }
        public void Shot()
        {
            if (elapsedBulletCooldown < bulletCooldown) return;
            for (int i = 0; i < bulletRounds; i++)
            {
                Bullet bullet = new Bullet(BulletSource.Enemy, (int)(x + ringX + width / 2 + i * (width / bulletRounds)), (int)(y + height + ringY));
                bullet.world = world;
                bullet.damage = bulletDamage;
                switch (type)
                {
                    case EnemyTypes.Fighter:
                        bullet.w = 2;
                        bullet.h = 4;
                        break;
                    case EnemyTypes.Orbit:
                        bullet.w = 1;
                        bullet.h = 7;
                        break;
                }

                world.AddBullet(bullet);
            }
            elapsedBulletCooldown = 0;
        }
        public void MoveInRing(float radius, float angle)
        {
            ringX = (float)(radius * Math.Cos(DegreesToRadians(angle)));
            ringY = (float)(radius * Math.Sin(DegreesToRadians(angle)));
        }
        public void MoveInJumps()
        {
            if (x + speedX < maxX)
            {
                xa += speedX;
            }
            else
            {
                xa -= speedX;
            }
            if (y + speedY < retreatAtY)
            {
                ya -= speedY;
            }
        }
        public double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
        public Rectangle GetRectangle()
        {
            return new Rectangle((int)(x + ringX), (int)(y + ringY), width, height);
        }
        public void SetUpFighterCraft()
        {
            type = EnemyTypes.Fighter;
            color = Color.Red;

            doingRing = false;
            doingJumps = false;

            width = 10;
            height = 20;

            speedX = 3 + random.Next(-1, 2);
            speedY = 1;

            maxHP = 22;
            currentHP = maxHP;

            bulletRounds = 1;
            bulletDamage = 0.01f;

            bulletCooldown = 3;
        }
        public void SetUpOrbitCraft()
        {
            type = EnemyTypes.Orbit;
            color = Color.Orange;

            doingRing = true;
            canCloak = true;

            width = 14;
            height = 14;

            speedX = 3;
            speedY = 1;

            maxHP = 11;
            currentHP = maxHP;

            bulletDamage = 0.3f;
            bulletRounds = 2;

            bulletCooldown = 15;
        }

        public void Randomize()
        {
            int newBehavior = random.Next(0, 5);
            switch (newBehavior)
            {
                case 0:
                    SetUpFighterCraft();

                    break;
                case 1:
                    SetUpOrbitCraft();

                    break;
                case 2:
                    this.color = Color.Yellow;
                    pursuingPlayer = true;
                    spawningAllies = true;
                    bulletDamage = 0.6f;
                    bulletRounds = 3;
                    speedX = 2;
                    speedY = 3;


                    break;
                default:
                    SetUpFighterCraft();
                    break;

            }
        }
    }
    public enum EnemyTypes
    {
        Fighter,
        Orbit,
        Cloaker,
        JumpShip,
        Cannon
    }
}
