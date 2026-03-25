using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class Player
    {
        public Player(string name) : this(name, 1000) 
        { 
        }
        public Player(string name, int beginningBalance) 
        {
            Hand = new List<Card>();
            Balance = beginningBalance;
            Name = name;
            isActivelyPlaying = true;
        }
        private List<Card> _hand = new List<Card>();
        public List<Card> Hand  { get { return _hand; } set { _hand = value; } }
        public int Balance { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
        public bool isActivelyPlaying { get; set; }

        public bool Stay{ get; set; }

        public bool Bet (int amount) 
        {
            if (amount > Balance)
            {
                Console.WriteLine("You cannot bet more than your balance.");
                return false;
            }
            else
            {
                Balance -= amount;
                Console.WriteLine($"{Name} has placed a bet of {amount}. Remaining balance: {Balance}");
                return true;
            }
        }

        public static Game operator +(Game game, Player player)
        {
            game.Players.Add(player);
            return game;
        }
        public static Game operator -(Game game, Player player)
        {
            game.Players.Remove(player);
            return game;
        }


    }
}
