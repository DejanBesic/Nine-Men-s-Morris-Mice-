using ReactiveUI;

namespace Kurna.Models
{
    public class Game : ReactiveObject
    {
        private string winner = "";
        private GameState state;
        private ReactiveCollection<Tile> tiles;

        public Tile CurrentlyMovingPiece { get; set; }

        public Game()
        {
            state = GameState.Placing;
            CreateTiles();
        }

        public GameState State
        {
            get { return state; }
            set { this.RaiseAndSetIfChanged(ref state, value); }
        }

        public ReactiveCollection<Tile> Tiles
        {
            get { return tiles; }
            set { this.RaiseAndSetIfChanged(ref tiles, value); }
        }

        public string Winner
        {
            get { return winner; }
            set { this.RaiseAndSetIfChanged(ref winner, value);  }
        }

        protected void CreateTiles()
        {
            // Create the initial tiles
            Tiles = new ReactiveCollection<Tile>(
                new[]
                {
                    // Outer
                    new Tile { TileName = "0", Row = 0, Column = 0 },
                    new Tile { TileName = "1", Row = 0, Column = 1},
                    new Tile { TileName = "2", Row = 0, Column = 2 },
                    new Tile { TileName = "3", Row = 1, Column = 0 },
                    new Tile { TileName = "4", Row = 1, Column = 1 },
                    new Tile { TileName = "5", Row = 1, Column = 2 },
                    new Tile { TileName = "6", Row = 2, Column = 0 },
                    new Tile { TileName = "7", Row = 2, Column = 1 },
                    new Tile { TileName = "8", Row = 2, Column = 2 },
                });

            // Make the connections
            //0 1 2
            //3 4 5
            //6 7 8
            // Outer
            Tiles[0].AdjacentTiles = new[] { Tiles[1], Tiles[3] };
            Tiles[1].AdjacentTiles = new[] { Tiles[0], Tiles[2], Tiles[4] };
            Tiles[2].AdjacentTiles = new[] { Tiles[1], Tiles[5] };
            Tiles[3].AdjacentTiles = new[] { Tiles[0], Tiles[4], Tiles[6] };
            Tiles[4].AdjacentTiles = new[] { Tiles[1], Tiles[3], Tiles[5], Tiles[7] };
            Tiles[5].AdjacentTiles = new[] { Tiles[2], Tiles[4], Tiles[8] };
            Tiles[6].AdjacentTiles = new[] { Tiles[3], Tiles[7]};
            Tiles[7].AdjacentTiles = new[] { Tiles[4], Tiles[6], Tiles[8] };
            Tiles[8].AdjacentTiles = new[] { Tiles[5], Tiles[7] };
            
            this.RaisePropertyChanged(x => x.Tiles);
        }
    }
}