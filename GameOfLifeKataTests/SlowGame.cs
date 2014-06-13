using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StatsdClient;

namespace GameOfLifeKataTests
{
    public class SlowGame : GameOfLifeKata.GameOfLife
    {
        public SlowGame(int[,] grid, IStatsd client)
            : base(grid, client)
        { }

        public override void Tick()
        {
            base.Tick();
            Thread.Sleep(5000);
        }
    }
}
