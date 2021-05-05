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
        private static bool __AREA__MODE__ = false;
        private static bool __SCORE__MODE__ = true;
        private static bool __RULET__MODE__ = __AREA__MODE__;
        private static int currentPop = 0;
        private static int populationCount = 1000;
        private static List<Drone>[] currentGen = new List<Drone>[populationCount];
        private static int[] currentGenAreas = new int[populationCount];
        private static int[] currentGenScores = new int[populationCount];
        private static double mutationRate = 0.01;
        private static int totalMoves = 0;
        private static int maxMoves = 80;
        private static Point startingPoint;
        private static int droneCount = -1;
        private static int generationCount = 0;
        private static List<Drone> bestGen = null;
        private static int bestGenScore = int.MinValue;
        private static int bestGenArea = 0;
        private static int maxGenCount = 100;
        private static int lastGenArea = 0;
        private static int lastGenScore = int.MinValue;
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
        static void resetTiles(Anamenu anamenu,Tile[,] tiles, MouseEventArgs e = null)
        {
            Random random = new Random();
            string[] droneStrings = { "|=|", "O-O", "UwU", "OwO", "+-+", "O+O", @"/|\", "UoU", "IvI", "<->", "<o>", "<|>" };

            if (startingPoint.X == -1 && e != null)
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
            if (anamenu.cmbBoxDroneCount.SelectedIndex >= 0)
                anamenu.labelDroneLocation.Text = "You selected: " + (startingPoint.X + 1).ToString() + "-" + (startingPoint.Y + 1).ToString();
            Color[] droneColors = { Color.Red, Color.Gold, Color.OrangeRed, Color.DarkGreen, Color.Magenta, Color.Turquoise, Color.BlueViolet, Color.Gray };
            if (droneCount == -1)
                droneCount = anamenu.cmbBoxDroneCount.SelectedIndex + 1;
            for (int i = 0; i < droneCount; i++)
            {
                tiles[startingPoint.X, startingPoint.Y].Drones.Add(new Drone(droneStrings[random.Next(0, droneStrings.Length)], droneColors[i]));
                tiles[startingPoint.X, startingPoint.Y].Drones[i].BestGenParent = random.Next(populationCount);
            }
            maxMoves = 80 / droneCount;
        }
        static void selectTile(object sender,MouseEventArgs e, Anamenu anamenu, Bitmap bm, Tile[,] tiles, Font droneFont)
        {
            resetTiles(anamenu, tiles,e);
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
                infoText.Append("Best gen info:\nScore:" + bestGenScore.ToString() + "\nArea:" + bestGenArea.ToString());
                infoText.Append( "\nLast gen info:\nScore: " + lastGenScore.ToString() + "\nArea: " + lastGenArea.ToString() + "\nIndividual Scores:\n");

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
        static void updateDrones(Anamenu anamenu, Bitmap bm, Tile[,] tiles, Font droneFont, List<Drone> drones = null)
        {
            #region SETTING UP VARIABLES
            anamenu.labelGenCount.Text = "Current gen: " + generationCount.ToString();
            anamenu.labelGenCount.Visible = true;
            int i, j, visitedCount = 0, sumScore = 0; ;
            Random random = new Random();
            double chance;
            Point moveTo;
            #endregion
            #region CALCULATING NEXT MOVES FOR EACH DRONE

            //POPÜLASYON BÜYÜKLÜÐÜ EKLE VE RULET TEKERÝ
            //+ CROSSOVER YAP

            for (currentPop = 0; currentPop < populationCount; currentPop++)
            {
                currentGenScores[currentPop] = 0;
                visitedCount = 0;
                sumScore = 0;
                for (totalMoves = 0; totalMoves < maxMoves; totalMoves++)
                {
                    for (i = 0; i < 9; i++)
                    {
                        for (j = 0; j < 9; j++)
                        {
                            if (tiles[i, j].Drones.Count > 0)
                            {
                                tiles[i, j].Visited = true;
                            }/*
                            if (totalMoves > 0 && (startingPoint.X == i && startingPoint.Y == j))
                                continue;*/
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
                                        if (chance > mutationRate && maxMoves > totalMoves)
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
                totalMoves = 0;
                //
                drones = new List<Drone>();
                for (i = 0; i < 9; i++)
                {
                    for (j = 0; j < 9; j++)
                    {
                        if (tiles[i, j].Drones.Count > 0)
                        {
                            foreach (var item in tiles[i, j].Drones)
                            {
                                drones.Add(item);
                                sumScore += item.Score;
                            }
                        }
                        if (tiles[i, j].Visited)
                            visitedCount++;
                    }
                }
            
                int vc = visitedCount, ss = sumScore;
                lastGenArea = visitedCount;
                lastGenScore = sumScore;
                if(lastGenArea>bestGenArea && __RULET__MODE__ == __AREA__MODE__)
                {
                    bestGenArea = lastGenArea;
                    bestGenScore = lastGenScore;
                }
                else if(lastGenScore > bestGenScore && __RULET__MODE__ == __SCORE__MODE__)
                {

                    bestGenArea = lastGenArea;
                    bestGenScore = lastGenScore;
                }
                currentGenAreas[currentPop] = vc;
                currentGen[currentPop] = drones;
                currentGenScores[currentPop] = ss;
                resetTiles(anamenu, tiles);
            }
            crossover();
            //toggleTimer(anamenu.buttonToggleTimer, null, anamenu.timerRefresh);
            resetDrawing(anamenu, bm, tiles, droneFont, drones);
            System.Threading.Thread.Sleep(2500);
            selectTile(null, null, anamenu, bm, tiles, droneFont);
            if(generationCount==maxGenCount)
            toggleTimer(anamenu.buttonToggleTimer, null, anamenu.timerRefresh);
            generationCount++;
            #endregion
        }
        #endregion
        #region GENETÝK SEÇÝLÝM
        private static void crossover()
        {
            List<Drone> drones = rulet(__RULET__MODE__);
            Random random = new Random();
            int cutpoint;

            for(int i = 0; i < populationCount / 2; i++)
            {
                cutpoint = random.Next(1, drones[i].MoveHistory.Count - 1);
                List<Point> drone1Gene = drones[i].MoveHistory.GetRange(0, cutpoint);
                List<Point> drone2Gene = drones[populationCount-i-1].MoveHistory.GetRange(0, cutpoint);
                for(int j = 0; j < cutpoint; j++)
                {
                    drones[i].MoveHistory[j] = drone2Gene[j];
                    drones[populationCount - i - 1].MoveHistory[j] = drone2Gene[j];
                }
            }
            bestGen = drones;


        }

        private static List<Drone> rulet(bool mode)
        {
            List<Drone> selectedDrones = new List<Drone>();
            List<int> selectedIndicies = new List<int>();

            Random random = new Random();
            if (mode) //SCORE MODE
            {
                int max = currentGenScores.Max();
                int mean = currentGenScores.Sum() / populationCount;
                for (int i = 0; i < populationCount; i++)
                {
                    if (random.NextDouble() <= (double)currentGenScores[i] / max && currentGenScores[i]>mean)
                    {
                        selectedIndicies.Add(i);
                    }
                }
                if (selectedIndicies.Count < populationCount/droneCount)
                {
                    for (int i = 0; i < populationCount / droneCount; i++)
                    {
                        if (!selectedIndicies.Contains(i))
                            selectedIndicies.Add(i);
                    }
                }
            }
            else //AREA MODE
            {
                int max = currentGenAreas.Max();
                int mean = currentGenAreas.Sum() / populationCount;
                for(int i = 0;i < populationCount; i++)
                {
                    if (random.NextDouble() < (double)currentGenAreas[i] / max && currentGenAreas[i] > mean)
                    {
                        selectedIndicies.Add(i);
                    }
                }
                if (selectedIndicies.Count < populationCount / droneCount)
                {
                    for (int i = 0; i < populationCount / droneCount; i++)
                    {
                        if (!selectedIndicies.Contains(i))
                            selectedIndicies.Add(i);
                    }
                }
            }

            //Generate new population
            while(selectedDrones.Count< populationCount)
            {
                int listIndex = selectedIndicies[random.Next(0, selectedIndicies.Count)];
                int droneIndex = 0;
                if (currentGen[listIndex].Count > 0)
                {
                    int maxVal = currentGen[listIndex].Max(g => g.Score);
                    for(int i = 0; i < currentGen[listIndex].Count; i++)
                    {
                        if (currentGen[listIndex][i].Score == maxVal)
                            droneIndex = i;
                    }
                    selectedDrones.Add(currentGen[listIndex][droneIndex]);
                    currentGen[listIndex].RemoveAt(droneIndex);
                }
            }
            return selectedDrones;
        }
        #endregion
    }
}
