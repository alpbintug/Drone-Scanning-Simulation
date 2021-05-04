using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace YapayZekaDroneOdevi
{
    class Tile
    {
        public List<Drone> Drones { get; set; }
        public Rectangle Rectangle { get; set; }
        public Boolean Visited { get; set; }

        public Tile(Rectangle Rectangle)
        {
            Visited = false;
            Drones = new List<Drone>();
            this.Rectangle = Rectangle;
        }
        public Tile(Rectangle Rectangle, List<Drone> Drones)
        {
            Visited = false;
            this.Drones = Drones;
            this.Rectangle = Rectangle;
        }
    }
}
