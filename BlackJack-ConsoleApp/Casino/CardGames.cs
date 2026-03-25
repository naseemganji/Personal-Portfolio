using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Interfaces;

namespace Casino
{
    public class CardGames : Game, IWalkAway
    {
        public CardGamesDealer Dealer { get; set; }

        public CardGames()
        {
            Players = new List<Player>();
            Bets = new Dictionary<Player, int>();
        }

        public override void Play()
        {
            Dealer = new CardGamesDealer();
            Bets = new Dictionary<Player, int>(); // Reset bets for new round

            foreach (Player player in Players)
            {
                player.Hand = new List<Card>();
                player.Stay = false;
            }
            Dealer.Hand = new List<Card>();
            Dealer.Stay = false;
            Dealer.isBusted = false;
            int timesShuffled;
            Deck deck = new Deck();
            deck.Shuffle(out timesShuffled, 3);
            Dealer.Deck = deck;
            Console.WriteLine("Please place your bet.");
            foreach (Player player in Players)
            {
                // CHECK IF PLAYER HAS MONEY TO BET
                if (player.Balance <= 0)
                {
                    Console.WriteLine($"{player.Name}, you have no money left to bet. Game over!");
                    player.isActivelyPlaying = false;
                    return;
                }

                bool validBet = false;
                int bet = 0;

                while (!validBet)
                {
                    Console.WriteLine($"{player.Name} has a balance of {player.Balance}");
                    bool validInput = int.TryParse(Console.ReadLine(), out bet);
                    if (!validInput)
                    {
                        Console.WriteLine("Invalid bet amount. Please enter a valid number.");
                        continue;
                    }
                    if (bet <0)
                    {
                        throw new FraudException("Bet amount cannot be negative. Security! Kick this person out");
                    }

                    validBet = player.Bet(bet);
                    if (validBet)
                    {
                        Bets[player] = bet;
                    }
                }
            }
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Dealing...");
                foreach (Player player in Players)
                {
                    Console.WriteLine("{0}:", player.Name);
                    Dealer.Deal(player.Hand);
                    if (i == 1)
                    {
                        bool blackjack = BlackJackRules.CheckForBlackJack(player.Hand);
                        if (blackjack)
                        {
                            Console.WriteLine("Blackjack! {0} wins {1}!", player.Name, Bets[player]);
                            player.Balance += Convert.ToInt32(Bets[player] * 1.5 + Bets[player]);
                            return;
                            //ListPlayers();
                            //WalkAway(player);
                        }
                        Console.WriteLine("Dealer:");
                        //Console.WriteLine(Dealer.Hand[0].Face + " of " + Dealer.Hand[0].Suit);
                        Console.WriteLine(Dealer.Hand[0].ToString());
                    }
                }
                Console.WriteLine("Dealer: ");
                Dealer.Deal(Dealer.Hand);
                if (i == 1)
                {
                    bool BlackJack = BlackJackRules.CheckForBlackJack(Dealer.Hand);
                    if (BlackJack)
                    {
                        Console.WriteLine("Dealer has BlackJack, everyone lost!");
                        foreach (KeyValuePair<Player, int> entry in Bets)
                        {
                            Dealer.Balance += entry.Value;
                        }
                        return;
                    }
                }

            }
            foreach (Player player in Players)
            {

                while (!player.Stay && player.isActivelyPlaying)
                {

                    Console.WriteLine($"{player.Name} your cards are:");
                    foreach (Card card in player.Hand)
                    {
                        Console.WriteLine("{0}", card.ToString());
                    }

                    Console.WriteLine("\n\n Would you like to Hit or Stay?");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "stay")
                    {
                        player.Stay = true;
                        Console.WriteLine($"{player.Name} has decided to stay.");
                        break;
                    }
                    else if (answer == "hit")
                    {
                        Dealer.Deal(player.Hand);
                    }
                    bool busted = BlackJackRules.IsBusted(player.Hand);
                    if (busted)
                    {
                        Dealer.Balance += Bets[player];
                        Console.WriteLine("{0} Busted! You lost your bet of {1}. Your balance is now {2}.", player.Name, Bets[player], player.Balance);

                        // Check if player has money left BEFORE asking to play again
                        if (player.Balance <= 0)
                        {
                            Console.WriteLine("You've lost all your money! Game over.");
                            player.isActivelyPlaying = false;
                            break;
                        }

                        Console.WriteLine("Do you want to play again?");
                        answer = Console.ReadLine().ToLower();
                        if (answer == "yes" || answer == "y")
                        {
                            player.isActivelyPlaying = true;
                            return;
                        }
                        else
                        {
                            player.isActivelyPlaying = false;
                            return;
                        }
                    }
                }
            }
            Dealer.isBusted = BlackJackRules.IsBusted(Dealer.Hand);
            Dealer.Stay = BlackJackRules.ShouldDealerStay(Dealer.Hand);
            while (!Dealer.Stay && !Dealer.isBusted)
            {
                Console.WriteLine("Dealer is hitting");
                Dealer.Deal(Dealer.Hand);
                Dealer.isBusted = BlackJackRules.IsBusted(Dealer.Hand);
                Dealer.Stay = BlackJackRules.ShouldDealerStay(Dealer.Hand);

            }
            if (Dealer.isBusted)
            {
                Console.WriteLine("Dealer is busted! Everyone who is still playing wins!");

                foreach (KeyValuePair<Player, int> entry in Bets)
                {
                    Console.WriteLine("{0} won {1}", entry.Key.Name, entry.Value);
                    Players.Where(x => x.Name == entry.Key.Name).First().Balance += (entry.Value * 2);
                    Dealer.Balance -= entry.Value;
                }
                return;
            }

            foreach (Player player in Players)
            {
                bool? playerWon = BlackJackRules.CompareHands(player.Hand, Dealer.Hand);
                if (playerWon == null)
                {
                    Console.WriteLine("Push! No one wins.");
                    player.Balance += Bets[player];

                }
                else if (playerWon == true)
                {
                    Console.WriteLine("{0} won {1}!", player.Name, Bets[player]);
                    player.Balance += (Bets[player] * 2);
                    Dealer.Balance -= Bets[player];
                }
                else
                {
                    Console.WriteLine("Dealer won {0}.", Bets[player]);
                    Dealer.Balance += Bets[player];
                }
                Console.WriteLine("Play again?");
                string answer = Console.ReadLine().ToLower();
                if (answer == "yes" || answer == "y")
                {
                    player.isActivelyPlaying = true;
                }
                else
                {
                    player.isActivelyPlaying = false;
                }

            }

        }



        public override void ListPlayers()
        {
            Console.WriteLine($"Players: {string.Join(",", Players)}");
        }
        public void WalkAway(Player player)
        {
            Console.WriteLine($"{player.Name} has decided to walk away from the game.");
            player.isActivelyPlaying = false;
        }
    }
}
