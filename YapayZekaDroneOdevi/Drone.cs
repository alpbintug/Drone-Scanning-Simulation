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
        public int BestGenParent { get; set; }
        public int Score { get; set; }

        public Drone(string drawing, Color color)
        {
            Score = 0;
            MoveHistory = new List<Point>();
            this.drawing = drawing;
            this.Color = color;
        }
    }
}
