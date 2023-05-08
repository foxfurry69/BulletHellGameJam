using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellGameJam
{
    internal class World
    {
        public List<Bullet> bullets = new List<Bullet>();
        public List<Enemy> enemies = new List<Enemy>();
        public Player player = new Player();

        int maxEnemies = 10;
        int spawnInterval = 50;
        int elapsedSpawnInterval = 0;

        public Boolean isGameOver = false;

        public World() {
            player.world = this;
        }
        public void AddBullet(Bullet bullet)
        {
            bullet.world = this;
            bullets.Add(bullet);
        }
        public void RemoveBullet(Bullet bullet) { bullets.Remove(bullet); }
        public void AddBullet(BulletSource source, int x, int y)
        {
            Bullet bullet = new Bullet(source, x, y);
            bullet.world = this;
            bullets.Add(bullet);
        }
        public void Update()
        {
            player.Update();
            if (elapsedSpawnInterval < spawnInterval)
            {
                elapsedSpawnInterval++;
            }
            if(elapsedSpawnInterval >= spawnInterval)
            {
                if(enemies.Count < maxEnemies)
                {
Enemy enemy = new Enemy();
                enemy.world = this;
                enemies.Add(enemy);
                }
                
            }
            foreach (Bullet bullet in bullets.ToList())
            {
                bullet.Update();
            }
            foreach (Enemy enemy in enemies.ToList())
            {
                enemy.Update();
                if(enemy.outOfreach)
                {
                    enemies.Remove(enemy);
                }
            }
            CheckCollisions();
        }
        public void Render(Graphics g)
        {

 foreach (Bullet bullet in bullets)
            {
                bullet.Render(g);
            }
            foreach (Enemy enemy in enemies.ToList())
            {
                enemy.Render(g);
            }
            player.Render(g);

            g.DrawString("Level: " + player.GetCurrentLevel(), SystemFonts.DefaultFont, Brushes.LightGreen, 8, 8);

            g.DrawString(String.Format("HP: {0:00.00}/{1:00.00}" ,player.currentHP, player.maxHP), SystemFonts.DefaultFont, Brushes.LightGreen, 8, 24);

           
        }
        public void CheckCollisions()
        {
            // check collision of player with bullets
            foreach (Bullet bullet in bullets.ToList())
            {
                if(bullet.source == BulletSource.Enemy)
                {
                    if(bullet.GetRectangle().IntersectsWith(player.getRect()))
                    {
                        player.currentHP -= bullet.damage;
                        if(player.currentHP < 0)
                        {
                            isGameOver = true;
                            Debug.Print("Game Over!");
                        }
                    }
                }
            }

            // check collision of enemies with bullets
            foreach (Bullet bullet in bullets.ToList()) { 
                if(bullet.source == BulletSource.Player)
                {
                    foreach (Enemy enemy in enemies.ToList()) {
                        if(!enemy.hasCloak && enemy.GetRectangle().IntersectsWith(bullet.GetRectangle()))
                        {
                            enemy.currentHP -= bullet.damage;
                            player.currentXP += bullet.damage / 2;
                            if(enemy.currentHP <= 0 )
                            {
                                 player.GetXP(enemy.maxHP / 2)  ;
                                player.Repair(enemy.maxHP / 6);
                                enemies.Remove(enemy);
                        
                                Debug.WriteLine("Enemy ship destroyed!");
                            }
                            bullets.Remove(bullet);
                        }
                    }
                }
            }
        }
    }
}
