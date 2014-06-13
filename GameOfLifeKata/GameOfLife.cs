using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StatsdClient;

namespace GameOfLifeKata
{
    public class GameOfLife
    {
        private int[,] grid;
        private int totalGridLength;
        private IStatsd client;

        public const int DEAD = 0;
        public const int ALIVE = 1;
        public const String BIRTH_METRIC = "game-of-life.birth";
        public const String DEATH_METRIC = "game-of-life.death";

        public GameOfLife(int[,] grid, IStatsd client)
        {
            this.grid = new int[grid.GetLength(0), grid.GetLength(1)];
            totalGridLength = ( grid.GetLength(0) * grid.GetLength(1) );
            Array.Copy(grid, this.grid, totalGridLength);

            this.client = client;
        }

        public int[,] GetGrid()
        {
            return grid;
        }

        public virtual void Tick()
        {
            int[,] new_grid = new int[grid.GetLength(0),grid.GetLength(1)];

            Array.Copy(grid, new_grid, totalGridLength);

            for (int row = 0; row < grid.GetLength(0); ++row)
            {
                for (int col = 0; col < grid.GetLength(1); ++col)
                {
                    int neighborCount = GetNeighborCount(row, col);

                    var wasAlive = new_grid[row, col] == ALIVE;
                    var wasDead = new_grid[row, col] == DEAD;

                    if (neighborCount < 2)
                        new_grid[row, col] = DEAD;
                        
                    if(neighborCount > 3 )
                        new_grid[row, col] = DEAD;
            
                    if (neighborCount == 3)
                        new_grid[row, col] = ALIVE;

                    var isDead = new_grid[row, col] == DEAD;   
                    var isAlive = new_grid[row, col] == ALIVE;

                    if (wasAlive && isDead)
                        client.LogCount(DEATH_METRIC);

                    if (wasDead && isAlive)
                        client.LogCount(BIRTH_METRIC);
                }

            }

            Array.Copy(new_grid, grid, totalGridLength);
        }


        private int GetNeighborCount( int row , int col )
        {
            int neighborCount = 0;


            neighborCount += GetNeighborCountFromAboveRow(row, col);

            neighborCount += GetNeighborCountFromMiddleRow(row, col);

            neighborCount += GetNeighborCountFromBelowRow(row, col);

            
            return neighborCount;

        }

        private int GetNeighborCountFromAboveRow( int row, int col )
        {
            int rowCount = 0;

            if ( RowBeforeExists ( row )) 
            {
                if (ColunmBeforeExists( col ))
                {
                    if (grid[row - 1, col - 1] == ALIVE)
                        ++rowCount;
                }

                if (grid[row - 1, col] == ALIVE)
                    ++rowCount;

                if (ColunmAfterExists( col ))
                {
                    if (grid[row - 1, col + 1] == ALIVE)
                        ++rowCount;
                }
            }

            return rowCount;

        }

        int GetNeighborCountFromMiddleRow(int row, int col)
        {
            int rowCount = 0;

            if ( ColunmBeforeExists( col ))
            {
                if (grid[row, col - 1] == ALIVE)
                    ++rowCount;
            }

            if (ColunmAfterExists( col ))
            {
                if (grid[row, col + 1] == ALIVE)
                    ++rowCount;
            }

            return rowCount;

        }

        private int GetNeighborCountFromBelowRow(int row, int col)
        {
            int rowCount = 0;
             
            if (row + 1 < grid.GetLength(0))
            {
                if (ColunmBeforeExists( col ))
                {
                    if (grid[row + 1, col - 1] == ALIVE)
                        ++rowCount;
                }

                if (grid[row + 1, col] == ALIVE)
                    ++rowCount;

                if (ColunmAfterExists( col ))
                {
                    if (grid[row + 1, col + 1] == ALIVE)
                        ++rowCount;
                }
            }

            return rowCount;

        }

        bool RowBeforeExists( int row )
        {
            return ( row - 1 >= 0 );
        }

        bool ColunmBeforeExists( int col )
        {
            return (col - 1 >= 0);
        }

        bool RowAfterExists( int row )
        {
            return (row + 1 < grid.GetLength(0));
        }

        bool ColunmAfterExists( int col )
        {
            return (col + 1 < grid.GetLength(1));
        }
    }
}
