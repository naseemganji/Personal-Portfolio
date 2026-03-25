using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class BlackJackRules
    {
        private static Dictionary<Face, int> _cardValues = new Dictionary<Face, int>()
        {
            [Face.Two] = 2,
            [Face.Three] = 3,
            [Face.Four] = 4,
            [Face.Five] = 5,
            [Face.Six] = 6,
            [Face.Seven] = 7,
            [Face.Eight] = 8,
            [Face.Nine] = 9,
            [Face.Ten] = 10,
            [Face.Jack] = 10,
            [Face.Queen] = 10,
            [Face.King] = 10,
            [Face.Ace] = 1
        };
        private static int[] GetAllPossibleHandValues(List<Card> hand)
        {
            int aceCount = hand.Count(card => card.Face == Face.Ace);
            int[] result = new int[aceCount + 1];
            int Value = hand.Sum(x => _cardValues[x.Face]);
            result[0] = Value;
            if (result.Length == 1) return result;

            for (int i = 1; i < result.Length; i++)
            {
                Value += (i * 10);
                result[i] = Value;
            }
            // REMOVED the filter - return all possible values, even if > 21
            return result;
        }

        public static bool CheckForBlackJack(List<Card> hand)
        {
            int[] possibleValues = GetAllPossibleHandValues(hand);
            // Filter here when checking for blackjack
            int[] validValues = possibleValues.Where(value => value <= 21).ToArray();
            if (validValues.Length > 0 && validValues.Max() == 21) return true;
            else return false;
        }
        public static bool IsBusted(List<Card> hand)
        {
            int[] possibleValues = GetAllPossibleHandValues(hand);
            // Check if ALL values exceed 21 (busted)
            if (possibleValues.Min() > 21) return true;
            else return false;

        }
        public static bool ShouldDealerStay(List<Card> hand)
        {
            int[] possibleHandValues = GetAllPossibleHandValues(hand);
            foreach (int value in possibleHandValues)
            {
                // Fixed: should be >= 17, not > 167 (typo)
                if (value >= 17 && value <= 21) return true;
            }
            return false;
        }
        public static bool? CompareHands(List<Card> playerHand, List<Card> dealerHand)
        {
            int[] playerResults = GetAllPossibleHandValues(playerHand);
            int[] dealerResults = GetAllPossibleHandValues(dealerHand);

            // Filter for valid values and handle empty cases
            int[] validPlayerValues = playerResults.Where(x => x <= 21).ToArray();
            int[] validDealerValues = dealerResults.Where(x => x <= 21).ToArray();

            // If either has no valid values, they're busted (shouldn't reach here normally)
            if (validPlayerValues.Length == 0) return false;
            if (validDealerValues.Length == 0) return true;

            int playerScore = validPlayerValues.Max();
            int dealerScore = validDealerValues.Max();

            if (playerScore > dealerScore) return true;
            else if (playerScore < dealerScore) return false;
            else return null;

        }
    }
}
