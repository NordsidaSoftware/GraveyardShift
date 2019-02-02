﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public class Overworld
    {
        int Seed;
        byte[,] Regions;
        Random rnd;

        public byte this[int x, int y] { get { return Regions[x, y]; } }
        public Overworld(int Seed)
        {
            this.Seed = Seed;
            Regions = new byte[50, 50];
            rnd = new Random(Seed);
        }

        public void Create(int iterations)
        {
            int counter = 0;
            while (counter < iterations)
            {
                int radius = rnd.Next(2, 8);
                int xh = rnd.Next(10, 40);
                int yh = rnd.Next(10, 40);


                for (int x = 0; x < 50; x++)
                {
                    for (int y = 0; y < 50; y++)
                    {
                        int height = radius * radius - ((x - xh) * (x - xh)) - ((y - yh) * (y - yh));
                        if (height > 0)
                            Regions[x, y] += (byte)height;
                    }
                }

                counter++;
            }
        }
    }

    public class Region
    {
        int Seed;
        byte[,] Cells;
        public byte this[int x, int y] {  get { return Cells[x, y]; }  set { Cells[x, y] = value; } }
        public Region() { Cells = new byte[200, 200]; }
    }
}