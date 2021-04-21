using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace YapayZekaDroneOdevi
{
    class Drone
    {
        public string drawing { get; set; }
        public Color Color { get; set; }
        public List<Point> MoveHistory { get; set; }

        public Drone(string drawing, Color color)
        {
            MoveHistory = new List<Point>();
            this.drawing = drawing;
            this.Color = color;
        }
    }
}
