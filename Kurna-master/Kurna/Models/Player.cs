using System.Linq;
using System.Windows.Media;
using ReactiveUI;
using System.Collections.Generic;

namespace Kurna.Models
{
    public class Player : ReactiveObject
    {
        private bool isComputer = false;
        private bool isPlayersTurn;
        private string name;
        private int piecesLeft;
        private Brush background;
        private ReactiveCollection<Mill> previousMills;
        private int piecesCanRemove;
        private int invisiblePieces;
        public static readonly Brush InactiveColor = Brushes.White;
        public static readonly Brush ActiveColor = Brushes.Red;

        public Player()
        {
            InvisiblePieces = 3;
            PiecesLeft = 0;
            previousMills = new ReactiveCollection<Mill>();
        }

        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }

        public bool IsPlayersTurn
        {
            get { return isPlayersTurn; }
            set
            {
                this.RaiseAndSetIfChanged(ref isPlayersTurn, value);
                Background = IsPlayersTurn ? ActiveColor : InactiveColor;
            }
        }

        public int Turn { get; set; }

        public Brush Background
        {
            get { return background; }
            set { this.RaiseAndSetIfChanged(ref background, value); }
        }

        public bool IsComputer
        {
            get { return isComputer; }
            set { this.RaiseAndSetIfChanged(ref isComputer, value); }
        }

        public int PiecesCanRemove
        {
            get { return piecesCanRemove; }
            set { this.RaiseAndSetIfChanged(ref piecesCanRemove, value); }
        }

        public int InvisiblePieces
        {
            get { return invisiblePieces; }
            set { this.RaiseAndSetIfChanged(ref invisiblePieces, value); }
        }

        public int PiecesLeft
        {
            get { return piecesLeft; }
            set { this.RaiseAndSetIfChanged(ref piecesLeft, value); }
        }

        public ReactiveCollection<Mill> PreviousMills
        {
            get { return previousMills; }
            set { this.RaiseAndSetIfChanged(ref previousMills, value); }
        }

        private void checkMill(int a, int b, int c, Tile movedTile, TileStatus ts,ReactiveCollection<Tile> tiles, ref Mill tofill)
        {
            if (tiles[a].Status == ts && tiles[b].Status == ts && tiles[c].Status == ts &&
                (movedTile == tiles[a] ||
                 movedTile == tiles[b] ||
                 movedTile == tiles[c]))
                tofill = new Mill(tiles[a], tiles[b], tiles[c]);
        }

        /////////// Board Pattern //////////
        //
        //  [0] [1] [2]  
        //  [3] [4] [5]     
        //  [6] [7] [8]
        //
        public bool AddNewMills(ReactiveCollection<Tile> tiles, TileStatus ts, Tile movedTile)
        {
            Mill currentMill = null;

            checkMill(0, 1, 2, movedTile, ts, tiles, ref currentMill);
            checkMill(3, 4, 5, movedTile, ts, tiles, ref currentMill);
            checkMill(6, 7, 8, movedTile, ts, tiles, ref currentMill);

            checkMill(0, 3, 6, movedTile, ts, tiles, ref currentMill);
            checkMill(1, 4, 7, movedTile, ts, tiles, ref currentMill);
            checkMill(2, 5, 8, movedTile, ts, tiles, ref currentMill);

            // Check validity
            if (currentMill == null) return false;
            currentMill.Turn = Turn;

            if (currentMill.First != movedTile &&
                currentMill.Second != movedTile &&
                currentMill.Third != movedTile)
                return false;

            PreviousMills.Add(currentMill);

            return true;
        }
    }
}
