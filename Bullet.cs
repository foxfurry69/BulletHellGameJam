using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellGameJam
{
    internal class Bullet
    {
        public int x, y, w = 4, h = 4;
        public float damage = 1f;
        public BulletSource source = BulletSource.Enemy;

        int maxFlightTime = 60;
        int elapsedFlightTime = 0;

        int xa, ya;

        public World world = null;

        public Bullet() { }
        public Bullet(BulletSource source, int x, int y)
        {
            this.x = x;
            this.y = y;
            this.source = source;
            if (source == BulletSource.Player)
            {
                w = 2;
                h = 7;
            }
            
        }

        public void Update()
        {
            if (elapsedFlightTime < maxFlightTime)
            {
                elapsedFlightTime++;
            }
            if (elapsedFlightTime >= maxFlightTime)
            {
                if (world != null)
                {
                    world.RemoveBullet(this);
                }
            }

            if (source == BulletSource.Player)
            {
                y -= 6;
            }
            else
            {
                y += 6;
            }
        }
        public void Render(Graphics g)
        {
            if (source == BulletSource.Player)
            {
                g.FillRectangle(Brushes.LightGreen, GetRectangle());
            }
            else
            {
                g.FillRectangle(Brushes.Red, GetRectangle());
            }
        }
        public Rectangle GetRectangle()
        {
            return new Rectangle(x, y, w, h);
        }
    }
}
