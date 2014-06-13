using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeKata;

namespace GameOfLifeKataTests
{
    [TestFixture]
    public class GameOfLifeTests
    {
        private FakeStatsdClient client;

        [SetUp]
        public void Setup()
        {
            client = new FakeStatsdClient();
        }

        [Test]
        public void EmptyGrid_ShouldReturnEmptyGrid()
        {

            int[,] emptyGrid = new int[,] { { GameOfLife.DEAD, GameOfLife.DEAD }, 
                                            { GameOfLife.DEAD, GameOfLife.DEAD } };

            GameOfLife game = new GameOfLife(emptyGrid, client);

            Assert.That( game.GetGrid(), Is.EquivalentTo( emptyGrid ));
        }

        [Test]
        public void OneTickOnAnEmptyGrid_ShouldReturnEmptyGrid()
        {

            int[,] grid = new int[,] { { GameOfLife.DEAD, GameOfLife.DEAD }, 
                                       { GameOfLife.DEAD, GameOfLife.DEAD } };

            GameOfLife game = new GameOfLife(grid, client);

            game.Tick();

            Assert.That(game.GetGrid(), Is.EquivalentTo(grid));
        }

        [Test]
        public void AnyLiveCellWithNoLiveNeighbors_ShouldDieAfterOneTick()
        {

            int[,] grid = new int[,] { { GameOfLife.ALIVE, GameOfLife.DEAD }, 
                                       { GameOfLife.DEAD,  GameOfLife.DEAD } };

            GameOfLife game = new GameOfLife(grid, client);

            game.Tick();

            Assert.That(game.GetGrid()[0,0], Is.EqualTo(GameOfLife.DEAD));

        }

        [Test]
        public void AnyLiveCellWithTwoLiveNeighbors_ShouldLiveAfterOneTick()
        {

            int[,] grid = new int[,] { { GameOfLife.ALIVE, GameOfLife.ALIVE }, 
                                       { GameOfLife.ALIVE,  GameOfLife.DEAD } };

            GameOfLife game = new GameOfLife(grid, client);

            game.Tick();

            Assert.That(game.GetGrid()[0, 0], Is.EqualTo(GameOfLife.ALIVE));

        }

        [Test]
        public void AnyLiveCellWithThreeLiveNeighbors_ShouldLiveAfterOneTick()
        {

            int[,] grid = new int[,] { { GameOfLife.ALIVE, GameOfLife.ALIVE }, 
                                       { GameOfLife.ALIVE,  GameOfLife.ALIVE } };

            GameOfLife game = new GameOfLife(grid, client);

            game.Tick();

            Assert.That(game.GetGrid()[0, 0], Is.EqualTo(GameOfLife.ALIVE));

        }

        [Test]
        public void AnyLiveCellWithMoreThenThreeLiveNeighbors_ShouldDieAfterOneTick()
        {

            int[,] grid = new int[,] { { GameOfLife.ALIVE, GameOfLife.ALIVE, GameOfLife.ALIVE  }, 
                                       { GameOfLife.DEAD,  GameOfLife.ALIVE, GameOfLife.ALIVE  },
                                       { GameOfLife.DEAD,  GameOfLife.DEAD,  GameOfLife.DEAD   }};

            GameOfLife game = new GameOfLife(grid, client);

            game.Tick();

            Assert.That(game.GetGrid()[0, 1], Is.EqualTo(GameOfLife.DEAD));
            Assert.That(game.GetGrid()[1, 1], Is.EqualTo(GameOfLife.DEAD));

        }

        [Test]
        public void AnyDeadLiveCellWithExactlyThreeLiveNeighbors_ShouldBecomeLiveAfterOneTick()
        {

            int[,] grid = new int[,] { { GameOfLife.ALIVE, GameOfLife.ALIVE, GameOfLife.ALIVE  }, 
                                       { GameOfLife.DEAD,  GameOfLife.DEAD, GameOfLife.DEAD  },
                                       { GameOfLife.DEAD,  GameOfLife.DEAD,  GameOfLife.DEAD  }};

            GameOfLife game = new GameOfLife(grid, client);

            game.Tick();

            Assert.That(game.GetGrid()[1, 1], Is.EqualTo(GameOfLife.ALIVE));

        }


        [Test]
        public void OneTickOnAnEmptyGrid_ShouldResultinNoDataBeingLogged()
        {

            int[,] grid = new int[,] { { GameOfLife.DEAD, GameOfLife.DEAD }, 
                                       { GameOfLife.DEAD, GameOfLife.DEAD } };

            GameOfLife game = new GameOfLife(grid, client);

            game.Tick();

            client.AssertNothingLogged();
        }

        [Test]
        public void OneTickOnAnAGridWithOneLiveCell_ShouldResultinOneDeathBeingLogged()
        {

            int[,] grid = new int[,] { { GameOfLife.ALIVE, GameOfLife.DEAD }, 
                                       { GameOfLife.DEAD, GameOfLife.DEAD } };


            GameOfLife game = new GameOfLife(grid, client);

            game.Tick();

            client.AssertDeathsLogged(1);
        }

      
        [Test]
        public void AnyDeadLiveCellWithExactlyThreeLiveNeighbors_ShoulLogsDeathsAndBirths()
        {

            int[,] grid = new int[,] { { GameOfLife.ALIVE, GameOfLife.ALIVE, GameOfLife.ALIVE  }, 
                                       { GameOfLife.DEAD,  GameOfLife.DEAD, GameOfLife.DEAD  },
                                       { GameOfLife.DEAD,  GameOfLife.DEAD,  GameOfLife.DEAD  }};

            GameOfLife game = new GameOfLife(grid, client);

            game.Tick();

            client.AssertDeathsLogged(2);
            client.AssertBirthsLogged(1);

        }


        [Test]
        public void AnyLiveCellWithMoreThenThreeLiveNeighbors_ShouldLogsDeathsAndBirths()
        {

            int[,] grid = new int[,] { { GameOfLife.ALIVE, GameOfLife.ALIVE, GameOfLife.ALIVE  }, 
                                       { GameOfLife.DEAD,  GameOfLife.ALIVE, GameOfLife.ALIVE  },
                                       { GameOfLife.DEAD,  GameOfLife.DEAD,  GameOfLife.DEAD   }};

            GameOfLife game = new GameOfLife(grid, client);

            game.Tick();

            client.AssertDeathsLogged(2);
            client.AssertBirthsLogged(1);


        }





    }
}
