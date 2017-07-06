using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Kurna.Models;
using Kurna.Views;
using System.Linq;

namespace Kurna
{
    class AStarSearch
    {
        public State search(State pocetnoStanje)
        {
            List<State> stanjaZaObradu = new List<State>();
            Hashtable predjeniPut = new Hashtable();
            stanjaZaObradu.Add(pocetnoStanje);

            while (stanjaZaObradu.Count > 0)
            {
                State naObradi = getBest(stanjaZaObradu);
                if (!predjeniPut.ContainsKey(naObradi.GetHashCode()))
                {
                    if (naObradi.isKrajnjeStanje())
                    {
                        return naObradi;
                    }
                    predjeniPut.Add(naObradi.GetHashCode(), null);
                    List<State> sledecaStanja = naObradi.mogucaSledecaStanja();

                    foreach (State s in sledecaStanja)
                    {
                        stanjaZaObradu.Add(s);
                    }
                }
                stanjaZaObradu.Remove(naObradi);
            }
            return null;
        }


        /////////// Board Pattern //////////
        //     0         1         2  
        // 0: [0]       [1]       [2]
        // 1: [3]       [4]       [5]
        // 2: [6]       [7]       [8]

        public double heuristicFunction(State s)
        {
            double red1 = 0, red2 = 0;
            double kol1 = 0, kol2 = 0 ;
            double povrat = 0;
            Tile trenutni = null;
            foreach (Tile tile in Board.game.Tiles)
            {
                if (int.Parse(tile.TileName) == s.polje)
                {
                    trenutni = tile;
                }
            }
            
            foreach(Tile tile in Board.game.Tiles)
            {
                if(tile.Row == trenutni.Row && tile.Status == TileStatus.P1)
                {
                    red1++;
                }
                if(tile.Row == trenutni.Row && tile.Status == TileStatus.P2)
                {
                    red2++;
                }
                if (tile.Column == trenutni.Column && tile.Status == TileStatus.P1)
                {
                    kol1++;
                }
                if (tile.Column == trenutni.Column && tile.Status == TileStatus.P2)
                {
                    kol2++;
                }
            }

        

            if (red2 < 2 || kol2 < 2)
            {
                povrat = 2;
            }
            else if (red1 < 2 || kol1 < 2)
            {
                povrat = 1;
            }

            if (red1 == 0 && kol1 == 0)
            {
                povrat = 1.5;
            }

            if (red1 == 2 || kol1 == 2)
            {
                povrat = 3;
            }
            if (red2 == 2 || kol2 == 2)
            {
                povrat = 4;
            }

            return povrat;
        }

    

       

        public State getBest(List<State> stanja)
        {
            State rez = null;
            double min = -1;

            foreach (State s in stanja)
            {
                double h = heuristicFunction(s);
                if (h > min)
                {
                    min = h;
                    rez = s;
                }
            }
            return rez;
        }



    }
}
