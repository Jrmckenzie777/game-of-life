using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOLStartUpTemplate1
{
    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[5, 5];

        bool[,] nextgeneration = new bool[5, 5];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            
            initBlock();
           
            
            timer.Interval = 500; // milliseconds
            timer.Tick += Timer_Tick;
           // timer.Enabled = true; // start timer running
        }

       
        Random r = new Random();
        private void initBlock()
        {
              for(int col = 0;col < 5; col++ )
              {
                 for(int row =0; row < 5; row++)
                {
                    if (r.Next(2) == 0)
                        universe[ col, row] = true;
                 }
             }
          

           
        }



          

        private int CountNeighbors(int x, int y)
        {
           

            int neighnorsAlive = 0;

            int maxX = universe.GetLength(0) - 1;
            int maxY = universe.GetLength(1) - 1;

            if ((x < maxX && y > 0) && universe[x + 1, y - 1]) neighnorsAlive++;

            if (x < maxX && universe[x + 1, y]) neighnorsAlive++;

            if ((x < maxX && y < maxY) && universe[x + 1, y + 1]) neighnorsAlive++;

            if ((x > 0 && y > 0) && universe[x - 1, y - 1]) neighnorsAlive++;

            if (x > 0 && universe[x - 1, y]) neighnorsAlive++;

            if ((x > 0 && y < maxY) && universe[x - 1, y + 1]) neighnorsAlive++;

            if (y > 0 && universe[x, y - 1]) neighnorsAlive++;

            if (y < maxY && universe[x, y + 1]) neighnorsAlive++;

            return neighnorsAlive;

        }

        // Calculate the next generation of cells
        private void CalculateNextGeneration()
        {


            // Increment generation count
            generations++;

            for (int col = 0; col < universe.GetLength(1); col++)
            {
                for (int row = 0; row < universe.GetLength(0); row++)
                {
                 
                    var neighborsalive = CountNeighbors(row, col);

                    if (universe[row, col])
                    {
                        //something alive
                        if (neighborsalive < 2)
                            nextgeneration[row, col] = false;
                        else if (neighborsalive > 3)
                            nextgeneration[row, col] = false;
                        else if (neighborsalive == 2 || neighborsalive == 3)
                            nextgeneration[row, col] = true;
                    }
                    else
                    {
                        //something dead
                        if (neighborsalive == 3)
                            nextgeneration[row, col] = true;
                    }
                }
            }


            universe = nextgeneration;
            

           
            graphicsPanel1.Refresh();



            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            CalculateNextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    Rectangle cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void gameStart_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void gamePause_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;

        }

        private void gameNext_Click(object sender, EventArgs e)
        {
            if (!timer.Enabled)
                CalculateNextGeneration();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isrunning = timer.Enabled;
            timer.Enabled = false;
            for(int col = 0; col< universe.GetLength(1); col++)
            {
                for (int row = 0; row < universe.GetLength(0); row++)
                {
                    universe[row, col] = false;
                }
            }
            timer.Enabled = isrunning;

        }
    }
}
