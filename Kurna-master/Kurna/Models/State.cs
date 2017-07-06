using Kurna.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurna.Models
{
    public class State
    {
        public State parent;
        private static int brojacPlace;
        public int polje;
        private static int hash = 1;
        public int brojacDubine;
        public Game trenutnaTabla;

        public State()
        {
            trenutnaTabla = new Game();
            trenutnaTabla.State = GameState.Moving;
        }

        public State sledeceStanjeM(State rez)
        {
            rez.brojacDubine = this.brojacDubine+1;
            rez.parent = this;
            return rez;
        }

        public State sledeceStanje(int polje)
        {
            State rez = new State();            
            rez.polje = polje;
            rez.parent = this;
            return rez;
        }
        /////////// Board Pattern //////////
        //
        //  [0]  [1]  [2]
        //  [3]  [4]  [5]
        //  [6]  [7]  [8]
        //
        public List<State> mogucaSledecaStanja()
        {
            List<State> rez = new List<State>();
            if (Board.game.State == GameState.Placing)
            {
                #region
                var openTiles = Board.game.Tiles.Where(x => x.Status == TileStatus.Unoccupied);
                foreach (Tile tile in openTiles)
                {
                        rez.Add(sledeceStanje(int.Parse(tile.TileName)));   
                }
                #endregion
            }
            else if(Board.game.State == GameState.Moving)
            {
                foreach(Tile tile in trenutnaTabla.Tiles)
                {
                    if(brojacDubine%2 == 0)
                    {
                        if(tile.Status == TileStatus.P2)
                        {
                            foreach(State s in dodavanjeStanja(tile))
                            {
                                rez.Add(s);
                            }
                        }
                    }else
                    {
                        if (tile.Status == TileStatus.P1)
                        {
                            foreach (State s in dodavanjeStanja(tile))
                            {
                                rez.Add(s);
                            }
                        }
                    }
                   
                 }
            }
            return rez;
        }

        private List<State> dodavanjeStanja(Tile tile)
        {
            List<State> rez = new List<State>();
                foreach (Tile komsija in tile.AdjacentTiles)
                {
                    if (komsija.Status == TileStatus.Unoccupied)
                    {
                        State novoStanje = new State();
                        int brojac = 0;
                        //formiranje table sledeceg stanja
                        foreach (Tile t in trenutnaTabla.Tiles)
                        {
                            if (t.TileName == tile.TileName)
                            {
                                novoStanje.trenutnaTabla.Tiles[brojac].Status = TileStatus.Unoccupied;
                            }
                            else if (komsija.TileName == t.TileName)
                            {
                                novoStanje.trenutnaTabla.Tiles[brojac].Status = tile.Status;
                            }
                            else
                            {
                                novoStanje.trenutnaTabla.Tiles[brojac].Status = t.Status;
                            }
                        brojac++;
                    }
                        rez.Add(sledeceStanjeM(novoStanje));
                    }
                }
            return rez;
        }

      


        public override int GetHashCode()
        {
            if (Board.game.State == GameState.Placing)
            {
                hash++;
                return brojacPlace * 123 + 12 + polje * 1322 + hash * 10000;
            }else
            {
                int povrat = 0;
                foreach(Tile t in trenutnaTabla.Tiles)
                {
                    if(t.Status == TileStatus.P1)
                    {
                        povrat = povrat * 10 + 1;
                    }
                    else if (t.Status == TileStatus.P2)
                    {
                        povrat = povrat * 10 + 2;
                    }else
                    {
                        povrat *= 10;
                    }
                }
                return povrat;
            }
        }

        private bool proveriTablu()
        {
            int brojac = 0;
            for(int i=0; i<trenutnaTabla.Tiles.Count; i++)
            {
                if (trenutnaTabla.Tiles[i].Status == TileStatus.P2)
                {
                    brojac++;
                }
                    if(i == 2 || i == 5 || i == 8)
                    {
                        if(brojac == 3)
                        {
                            return true;
                        }
                        brojac = 0;
                    }
            }
            brojac = 0;
            for (int i = 0; i < trenutnaTabla.Tiles.Count; i+=3)
            {
                if (trenutnaTabla.Tiles[i].Status == TileStatus.P2)
                {
                    brojac++;
                }
                    if (i == 6 || i == 7 || i == 8)
                    {
                        if (brojac == 3)
                        {
                            return true;
                        }
                        if(i == 6)
                        {
                            i = 1;
                        }
                        else if(i == 7)
                        {
                            i = 2;
                        }
                        brojac = 0;
                    }
                
            }
            return false;
        }


        public bool isKrajnjeStanje()
        {
            brojacPlace++;
            if(Board.game.State == GameState.Placing  && brojacPlace>1)
            {
                brojacPlace = 0;
                return true;
            }
            if(Board.game.State == GameState.Moving && proveriTablu())
            {
                return true;
            }
            return false;
        }

        public List<State> path()
        {
            List<State> putanja = new List<State>();
            State tt = this;
            while (tt != null)
            {
                putanja.Insert(0, tt);
                tt = tt.parent;
            }
            return putanja;
        }


    }
}
