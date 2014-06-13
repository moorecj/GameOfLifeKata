using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StatsdClient;
using GameOfLifeKata;

namespace GameOfLifeKataTests
{
    [TestFixture]
    public class RealStuff
    {

        [Test]
        public void TestRealClient()
        {
            var client = new Statsd("191.238.44.201", 8125);

            int[,] grid = new int[100, 100];
            var random = new Random();

            for(int i =0; i<100; ++i )
            {
                for(int j = 0; j < 100; ++j )
                {
                    grid[i,j] = random.Next(0, 2) == 1 ? GameOfLife.ALIVE : GameOfLife.DEAD;
                }
            }
 
            GameOfLife game = new SlowGame(grid, client);

            while(true)
                game.Tick();
        }
    }
}
