using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YapayZekaDroneOdevi
{
    static class Program
    {
        private static double mutationRate = 0.1;
        private static int totalMoves = 0;
        private static int maxMoves = 40;
        private static Point startingPoint;
        private static int droneCount = -1;
        private static int generationCount = 1;
        private static List<Drone> bestGen = null;
        private static int bestGenScore = int.MinValue;
        private static int bestGenArea = 0;
        private static int maxGenCount = 100;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            List<Tile[,]> mapHistory = new List<Tile[,]>();
     
            Anamenu anamenu = new Anamenu();
            Random random = new Random();

            List<Drone> lastGen=null;
            startingPoint = new Point(-1,-1);
            Tile[,] tiles = new Tile[9, 9];
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    tiles[i, j] = new Tile(new Rectangle(anamenu.panelCizim.Width * i / 9, anamenu.panelCizim.Height * j / 9, anamenu.panelCizim.Width / 9, anamenu.panelCizim.Height / 9));
                }
            }

            Font droneFont = new Font("Tahoma", 12, FontStyle.Bold);
            Bitmap bm = new Bitmap(anamenu.panelCizim.Width, anamenu.panelCizim.Height);

            anamenu.cmbBoxDroneCount.SelectedIndex = 0;
            anamenu.timerRefresh.Tick += (sender,e) => updateDrones(anamenu,  bm, tiles, droneFont,lastGen);
            anamenu.panelCizim.Click += (sender, e) => selectTile(sender, e as MouseEventArgs, anamenu, bm, tiles, droneFont);
            anamenu.buttonReset.Click += (sender, e) => reset(anamenu, bm, tiles, droneFont);
            anamenu.timerRefresh.Interval = 100;
            //anamenu.buttonToggleTimer.Click += (sender, e) => calculateMoves(tiles, mapHistory);
            anamenu.buttonToggleTimer.Click += (sender, e) => toggleTimer(sender, e as MouseEventArgs, anamenu.timerRefresh);
            resetDrawing(anamenu,bm,tiles,droneFont);
            
            Application.Run(anamenu);
        }
        #region VISUALIZATION
        static void reset(Anamenu anamenu, Bitmap bm, Tile[,] tiles, Font droneFont)
        {

            anamenu.labelDroneLocation.Text = "Select Drone Location";
            anamenu.cmbBoxDroneCount.SelectedIndex = 0;
            anamenu.cmbBoxDroneCount.Enabled = true;
            startingPoint.X = -1;
            startingPoint.Y = -1;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tiles[i, j] = new Tile(new Rectangle(anamenu.panelCizim.Width * i / 9, anamenu.panelCizim.Height * j / 9, anamenu.panelCizim.Width / 9, anamenu.panelCizim.Height / 9));
                }
            }
            resetDrawing(anamenu, bm, tiles, droneFont);
            if(anamenu.buttonToggleTimer.Text=="Stop")
                toggleTimer(anamenu.buttonToggleTimer,null,anamenu.timerRefresh);
        }
        static void toggleTimer(object sender,MouseEventArgs e, Timer timer)
        {
            if((sender as Button).Text == "Stop")
            {
                (sender as Button).Text = "Start";
                timer.Stop();
            }
            else
            {
                (sender as Button).Text = "Stop";
                timer.Start();
            }
        }
        static void selectTile(object sender,MouseEventArgs e, Anamenu anamenu, Bitmap bm, Tile[,] tiles, Font droneFont)
        {
            Random random = new Random();
            string[] droneStrings = { "|=|", "O-O", "UwU", "OwO", "+-+", "O+O", @"/|\", "UoU", "IvI", "<->", "<o>", "<|>" };

            if (startingPoint.X == -1&&e!=null)
            {
                startingPoint.X = e.X / 100;
                startingPoint.Y = e.Y / 100;
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tiles[i, j] = new Tile(new Rectangle(anamenu.panelCizim.Width * i / 9, anamenu.panelCizim.Height * j / 9, anamenu.panelCizim.Width / 9, anamenu.panelCizim.Height / 9));
                }
            }
            if (anamenu.cmbBoxDroneCount.SelectedIndex>=0)
                anamenu.labelDroneLocation.Text = "You selected: " + (startingPoint.X+ 1).ToString() + "-" + (startingPoint.Y+ 1).ToString();
            Color[] droneColors = { Color.Red, Color.Gold, Color.OrangeRed, Color.DarkGreen, Color.Magenta, Color.Turquoise, Color.BlueViolet, Color.Gray};
            if(droneCount==-1)
                droneCount = anamenu.cmbBoxDroneCount.SelectedIndex+1;
            for (int i = 0; i < droneCount; i++)
            {
                tiles[startingPoint.X, startingPoint.Y].Drones.Add(new Drone(droneStrings[random.Next(0, droneStrings.Length)], droneColors[i]));
                tiles[startingPoint.X, startingPoint.Y].Drones[i].BestGenParent = i;
            }
            resetDrawing(anamenu, bm, tiles, droneFont);
            anamenu.cmbBoxDroneCount.SelectedIndex = -1;
            anamenu.cmbBoxDroneCount.Enabled = false;
        }

        static void resetDrawing(Anamenu anamenu, Bitmap bm, Tile[,] tiles, Font droneFont, List<Drone> lastGen = null)
        {
            Graphics g = Graphics.FromImage(bm);
            Brush brush = new SolidBrush(Color.Black);
            g.FillRectangle(brush, 0, 0, anamenu.panelCizim.Width, anamenu.panelCizim.Height);
            Pen linePen = new Pen(Color.FromArgb(235,91,52));
            int lastGenArea = 0;


            float linePos;
            for(int i = 0; i < 9; i++)
            {
                linePos = (float)anamenu.panelCizim.Width * (i+1)  / 9;
                g.DrawLine(linePen, 0, linePos, anamenu.panelCizim.Width, linePos);
                g.DrawLine(linePen, linePos,0, linePos, anamenu.panelCizim.Height);
            }
            for (int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (tiles[i, j].Visited)
                        lastGenArea++;
                    for(int k=0;k<tiles[i,j].Drones.Count;k++)
                    {
                        float stringLength = tiles[i, j].Drones[k].drawing.Length * (droneFont.Size+3);
                        g.DrawString(tiles[i, j].Drones[k].drawing, droneFont, new SolidBrush(tiles[i, j].Drones[k].Color),tiles[i,j].Rectangle.X+(k%2)*50,tiles[i,j].Rectangle.Y+k/2*droneFont.Height);
                    }
                }
            }
            System.Text.StringBuilder infoText = new System.Text.StringBuilder();
            if (lastGen != null)
            {

                infoText.Append( "Best gen info:\nScore:" + bestGenScore.ToString() + "\nArea: " + bestGenArea.ToString()+"\n");
                foreach (Drone drone1 in bestGen)
                {
                    infoText.Append( drone1.Score.ToString() + " " + drone1.Color.ToString() + "\n");
                }

                infoText.Append( "\nLast gen info:\nScore: " + (lastGen.Sum(g => g.Score) + lastGenArea * 125*droneCount).ToString() + "\nArea: " + lastGenArea.ToString() + "\nIndividual Scores:\n");

                foreach (Drone drone in lastGen)
                {
                    infoText.Append( drone.Score.ToString() + " " +drone.Color.ToString() + "\n");
                    Pen dronePen = new Pen(drone.Color);
                    Point last = startingPoint;
                    Point next = startingPoint;
                    foreach(var move in drone.MoveHistory)
                    {
                        next.Offset(move.X, move.Y);
                        g.DrawLine(dronePen, last.X*100+50,last.Y*100+50, next.X*100+50,next.Y*100+50);
                        last = next;
                    }
                }
                anamenu.labelLastGenInfo.Text = infoText.ToString();
            }

            g.Dispose();
            brush.Dispose();
            linePen.Dispose();

            anamenu.panelCizim.Refresh();
            anamenu.panelCizim.Update();
            anamenu.panelCizim.BackgroundImage = bm;

        }

        #endregion

        #region CALCULATING MOVES
        static Point move()
        {
            Random random = new Random();
            int x = random.Next(-1, 2), y = random.Next(-1, 2);
            return new Point(x, y);
        }
        static void updateDrones(Anamenu anamenu, Bitmap bm, Tile[,] tiles, Font droneFont, List<Drone> lastGen = null)
        {
            #region SETTING UP VARIABLES
            anamenu.labelGenCount.Text = "Current gen: " + generationCount.ToString();
            anamenu.labelGenCount.Visible = true;
            int i, j, visitedCount = 0;
            Random random = new Random();
            double chance;
            Point moveTo;
            #endregion
            #region CALCULATING NEXT MOVES FOR EACH DRONE
            for(totalMoves = 0; totalMoves<maxMoves; totalMoves++)
            {

                for (i = 0; i < 9; i++)
                {
                    for (j = 0; j < 9; j++)
                    {
                        if (tiles[i, j].Drones.Count > 0)
                        {
                            tiles[i, j].Visited = true;
                        }
                        if (totalMoves > 0 && (startingPoint.X == i && startingPoint.Y == j))
                            continue;
                        int k = 0;
                        while (k < tiles[i, j].Drones.Count)
                        {

                            if (tiles[i, j].Drones[k].MoveHistory.Count <= totalMoves + 1)
                            {
                                do
                                {
                                    chance = random.NextDouble();
                                    if (bestGen == null)
                                        chance = 0;
                                    moveTo = move();
                                    if (chance > mutationRate && bestGen[tiles[i, j].Drones[k].BestGenParent].MoveHistory.Count > totalMoves)
                                    {
                                        moveTo = bestGen[tiles[i, j].Drones[k].BestGenParent].MoveHistory[totalMoves];
                                    }

                                } while ((moveTo.X == 0 && moveTo.Y == 0) || i + moveTo.X < 0 || i + moveTo.X > 8 || j + moveTo.Y < 0 || j + moveTo.Y > 8);

                                Point target = new Point(i + moveTo.X, j + moveTo.Y);
                                if (target.Equals(startingPoint))
                                    tiles[i, j].Drones[k].Score += 500;
                                tiles[i, j].Drones[k].Score -= 25;
                                if (tiles[i, j].Drones[k].MoveHistory.Count > 2)
                                {
                                    Point last = tiles[i, j].Drones[k].MoveHistory[tiles[i, j].Drones[k].MoveHistory.Count - 2];
                                    tiles[i, j].Drones[k].Score -= 45 * (Math.Abs(2 * i - last.X - target.X) + Math.Abs(2 * j - last.Y - target.Y));
                                }
                                if (tiles[i, j].Visited)
                                    tiles[i, j].Drones[k].Score -= 100;
                                tiles[i, j].Drones[k].MoveHistory.Add(moveTo);
                                tiles[i + moveTo.X, j + moveTo.Y].Drones.Add(tiles[i, j].Drones[k]);
                                tiles[i, j].Drones.RemoveAt(k);
                            }
                            else
                                k++;
                        }
                    }
                }
            }
            #endregion
            #region CURRENT GEN IS OVER
            if (totalMoves >= maxMoves)
            {
                generationCount++;
                totalMoves = 0;
                toggleTimer(anamenu.buttonToggleTimer, null, anamenu.timerRefresh);
                lastGen = new List<Drone>();
                for(i = 0; i < 9; i++)
                {
                    for (j = 0; j < 9; j++)
                    {
                        if (tiles[i, j].Drones.Count > 0)
                        {
                            
                            foreach (var item in tiles[i, j].Drones)
                            {
                                lastGen.Add(item);
                            }
                        }
                        if(tiles[i,j].Visited)
                            visitedCount++;
                    }
                }
                lastGen = lastGen.OrderByDescending(g => g.Score).ToList();
                int lastGenScore = lastGen.Sum(g => g.Score) + visitedCount * 125*droneCount;
                if (lastGenScore > bestGenScore&&visitedCount>=bestGenArea)
                {
                    bestGen = lastGen;
                    bestGenScore = lastGenScore;
                    bestGenArea = visitedCount;
                }
                resetDrawing(anamenu, bm, tiles, droneFont, lastGen);
                System.Threading.Thread.Sleep(2500);
                selectTile(null, null, anamenu, bm, tiles, droneFont);
                if(generationCount!=maxGenCount)
                toggleTimer(anamenu.buttonToggleTimer, null, anamenu.timerRefresh);
            }
            #endregion
        }
        #endregion
    }
}
