using System.Collections.Generic;
using System.Collections;

namespace Kurna.Models
{
    class DepthFirstSearch
    {
        public State search(State pocetnoStanje)
        {
            List<State> stanjaNaObradi = new List<State>();
            Hashtable predjeniPut = new Hashtable();
            stanjaNaObradi.Add(pocetnoStanje);
            while (stanjaNaObradi.Count > 0)
            {
                State naObradi = stanjaNaObradi[0];

                if (!predjeniPut.ContainsKey(naObradi.GetHashCode()))
                {
                    if (naObradi.isKrajnjeStanje())
                    {
                        return naObradi;
                    }
                    predjeniPut.Add(naObradi.GetHashCode(), null);
                    List<State> mogucaSledecaStanja = naObradi.mogucaSledecaStanja();
                    foreach (State sledeceStanje in mogucaSledecaStanja)
                    {
                        stanjaNaObradi.Add(sledeceStanje);
                    }
                }
                stanjaNaObradi.Remove(naObradi);
            }
            return null;
        }
    }
}
