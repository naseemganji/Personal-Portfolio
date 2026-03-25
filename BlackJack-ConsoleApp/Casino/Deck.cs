using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class Deck
    {
        public Deck()
        {
            Cards = new List<Card>();

            /*
            foreach (Face face in Enum.GetValues(typeof(Face)))
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    Card card = new Card(suit, face);
                    Cards.Add(card);
                }
            }
            */
            /*
            // Alternative way to populate the deck using LINQ
            Cards = (from Face face in Enum.GetValues(typeof(Face))
                     from Suit suit in Enum.GetValues(typeof(Suit))
                     select new Card(suit, face)).ToList();
          */  
            // Alternative way to populate the deck using nested loops
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j <= 14; j++)  // Changed from j = 0; j < 13 to j = 2; j <= 14
                {
                    Card card = new Card(suit: (Suit)i, face: (Face)j);
                    
                    Cards.Add(card);
                }
            }  
           
                
        }
        public List<Card> Cards { get; set; }

        public void Shuffle( out int timesShuffled, int times = 1)
        {
            timesShuffled = 0;
            for (int i = 0; i < times; i++)
            {
                timesShuffled++;
                Random random = new Random();
                List<Card> shuffledCards = new List<Card>(); // Create a copy of the original list
                while (Cards.Count > 0)
                {
                    int randomIndex = random.Next(0, Cards.Count); // Get a random index
                    shuffledCards.Add(Cards[randomIndex]); // Add the card at the random index to the shuffled list
                    Cards.RemoveAt(randomIndex); // Remove the card from the original list
                }
                this.Cards = shuffledCards;             // Assign the shuffled list back to the deck
            }     
        }
    }
}
