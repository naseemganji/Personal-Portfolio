using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class Card
    {
        public Suit Suit { get; }
        public Face Face { get; }
        public override string ToString()
        {
            return string.Format("{0} of {1}", Face, Suit);
        }

        public Card(Suit suit, Face face)
        {
            Suit = suit;
            Face = face;
        }
    }

    public enum Face
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    public enum Suit
    {
        Clubs,
        Hearts,
        Diamonds,
        Spades
    }

}
