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
        private static int totalMoves = 0;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

     
            Anamenu anamenu = new Anamenu();
            Random random = new Random();

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
            anamenu.timerRefresh.Tick += (sender,e) => updateDrones(sender,e, anamenu, bm, tiles, droneFont);
            anamenu.panelCizim.Click += (sender, e) => selectTile(sender, e as MouseEventArgs, anamenu, bm, tiles, droneFont);

            anamenu.timerRefresh.Interval = 100;
            anamenu.buttonToggleTimer.Click += (sender, e) => toggleTimer(sender, e as MouseEventArgs, anamenu.timerRefresh);
            resetDrawing(anamenu,bm,tiles,droneFont);
            
            Application.Run(anamenu);
        }
        static void toggleTimer(object sender,MouseEventArgs e, Timer timer)
        {
            if((sender as Button).Text == "Start")
            {
                (sender as Button).Text = "Stop";
                timer.Start();
            }
            else
            {
                (sender as Button).Text = "Start";
                timer.Stop();
            }
        }
        static void selectTile(object sender,MouseEventArgs e, Anamenu anamenu, Bitmap bm, Tile[,] tiles, Font droneFont)
        {
            Random random = new Random();
            string[] droneStrings = { "|=|", "O-O", "UwU", "OwO", "+-+", "O+O", @"/|\", "UoU", "IvI", "<->", "<o>", "<|>" };
            int x, y;

            x = e.X / 100;
            y = e.Y / 100;
            if(anamenu.cmbBoxDroneCount.SelectedIndex>=0)
                anamenu.labelDroneLocation.Text = "You selected: " + (x+1).ToString() + "-" + (y+1).ToString();
            Color[] droneColors = { Color.DarkRed, Color.Gold, Color.OrangeRed, Color.Silver, Color.Magenta, Color.Turquoise, Color.AliceBlue, Color.LightSeaGreen, Color.BlueViolet, Color.Gray, Color.White, Color.LightGoldenrodYellow };
            int droneCount = anamenu.cmbBoxDroneCount.SelectedIndex+1;
            for (int i = 0; i < droneCount; i++)
            {
                tiles[x, y].Drones.Add(new Drone(droneStrings[random.Next(0, droneStrings.Length)], droneColors[random.Next(0, droneColors.Length)]));
            }
            resetDrawing(anamenu, bm, tiles, droneFont);
            anamenu.cmbBoxDroneCount.SelectedIndex = -1;
            anamenu.cmbBoxDroneCount.Enabled = false;
        }

        static void resetDrawing(Anamenu anamenu, Bitmap bm, Tile[,] tiles, Font droneFont)
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
                g.DrawString("annen", SystemFonts.DefaultFont, new SolidBrush(Color.Gold), 0 + linePos, anamenu.panelCizim.Width + linePos);
            }
            for (int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    for(int k=0;k<tiles[i,j].Drones.Count;k++)
                    {
                        float stringLength = tiles[i, j].Drones[k].drawing.Length * (droneFont.Size+3);
                        g.DrawString(tiles[i, j].Drones[k].drawing, droneFont, new SolidBrush(tiles[i, j].Drones[k].Color),tiles[i,j].Rectangle.X+(k%2)*50,tiles[i,j].Rectangle.Y+k/2*droneFont.Height);
                    }
                }
            }

            g.Dispose();
            brush.Dispose();
            linePen.Dispose();

            anamenu.panelCizim.Refresh();
            anamenu.panelCizim.Update();
            anamenu.panelCizim.BackgroundImage = bm;
        }
        static void updateDrones(object sender, EventArgs e, Anamenu anamenu, Bitmap bm, Tile[,] tiles, Font droneFont)
        {
            int x=0, y=0;
            Random random = new Random();
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    for(int k = 0; k < tiles[i, j].Drones.Count; k++)
                    {
                        if (tiles[i,j].Drones[k].MoveHistory.Count<=totalMoves) 
                        {
                            do
                            {
                                x = random.Next(-1, 2);
                                y = random.Next(-1, 2);
                            } while ((x == 0 && y == 0) || i + x < 0 || i + x > 8 || j + y < 0 || j + y > 8);
                            tiles[i, j].Drones[k].MoveHistory.Add(new Point(i, j));
                            tiles[i + x, j + y].Drones.Add(tiles[i, j].Drones[k]);
                            tiles[i, j].Drones.RemoveAt(k);
                        }
                        
                    }
                }
            }
            totalMoves++;
            resetDrawing(anamenu, bm, tiles, droneFont);
        }

    }
}
