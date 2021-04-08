using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private List<Player> _players = new List<Player>();

        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];

        private readonly bool[] _inPenaltyBox = new bool[6];

        private readonly LinkedList<string> _popQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _scienceQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _sportsQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _rockQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _technoQuestions = new LinkedList<string>();
        private readonly string[] _defaultCategory = new string[] { "Rock", "Techno" };
        private readonly string[] _categories = new string[] { "Pop", "Sciences", "Sports", "Rock", "Techno" };

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;
        private string defaultCategory;

        Random rand = new Random();

        List<string> leaderBord = new List<string>();

        public Game()
        {
            for (var i = 0; i < 50; i++)
            {
                _popQuestions.AddLast("Pop Question " + i);
                _scienceQuestions.AddLast(("Science Question " + i));
                _sportsQuestions.AddLast(("Sports Question " + i));
                _rockQuestions.AddLast(CreateRockQuestion(i));
                _technoQuestions.AddLast(CreateTechnoQuestion(i));
            }
        }

        public string CreateRockQuestion(int index)
        {
            return "Rock Question " + index;
        }

        public string CreateTechnoQuestion(int index)
        {
            return "Techno Question " + index;
        }

        public string AskDefaultCategory()
        {
            Console.WriteLine("Choose a category for the default category: ");
            for (int i = 0; i < _defaultCategory.Length; i++)
            {
                Console.WriteLine(i + "-" + _defaultCategory[i]);
            }
            var choixCategorie = Console.ReadLine();
            switch (choixCategorie)
            {
                case "0":
                    defaultCategory = _defaultCategory[0];
                    break;
                case "1":
                    defaultCategory = _defaultCategory[1];
                    break;
                default:
                    Console.WriteLine("Please chose a correct value.");
                    break;
            }
            return defaultCategory;
        }

        int goldForWinning = 6;
        public void AskGoldForWinning()
        {
            bool goldForWinningIsOk = false;

            while (!goldForWinningIsOk)
            {
                Console.WriteLine("How many gold does a player needs to win the game (6 mini) ?: ");
                string goldForWinningStr = Console.ReadLine();
                if (Int32.Parse(goldForWinningStr) >= 6)
                {
                    goldForWinning = Int32.Parse(goldForWinningStr);
                    goldForWinningIsOk = true;
                }
                else
                {
                    Console.WriteLine("The gold amount needed to win must be superior or equal to 6 !");
                }
            }

        }

        public bool IsPlayable()
        {
            return (HowManyPlayers() >= 2 && HowManyPlayers() <= 6);
        }

        public void InitializePlayers()
        {
            bool initializing = true;

            while (initializing)
            {
                Console.WriteLine("How many players will play ? (2 players minimum, 6 max) \n Your choice: ");
                var playersNumber = Console.ReadLine();
                if (Int32.Parse(playersNumber) < 2)
                {
                    Console.WriteLine("Vous n'avez pas assez de joueur pour commencer");
                }
                else if (Int32.Parse(playersNumber) > 6)
                {
                    Console.WriteLine("Vous avez trop de joueur pour jouer");
                }
                else if (Int32.Parse(playersNumber) >= 2 && Int32.Parse(playersNumber) <= 6)
                {
                    
                    for (int i = 0; i < Int32.Parse(playersNumber); i++)
                    {
                        Console.WriteLine("What's the name of this player ?: ");
                        var playerName = Console.ReadLine();
                        this.Add(playerName);
                    }
                    initializing = false;
                }
            }
        }

        public bool Add(string playerName)
        {
            _players.Add(new Player { Name = playerName, Joker = 1, IsInGame = true, nbTimeInPrison = 0 }) ;
            _places[HowManyPlayers()] = 0;
            _purses[HowManyPlayers()] = 0;
            _inPenaltyBox[HowManyPlayers()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
            return true;
        }

        public int HowManyPlayers()
        {
            return _players.Count;
        }

        public void Roll(int roll)
        {
            Console.WriteLine("######################################");
            Console.WriteLine(_players[_currentPlayer].Name + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer])
            {
                Console.WriteLine("Chance : " + Convert.ToDecimal(Convert.ToDecimal(1) / Convert.ToDecimal(_players[_currentPlayer].nbTimeInPrison)));
                if (rand.Next(1,_players[_currentPlayer].nbTimeInPrison) == 1)
                {
                    _isGettingOutOfPenaltyBox = true;
                    _inPenaltyBox[_currentPlayer] = false;
                    Console.WriteLine(_players[_currentPlayer].Name + " is getting out of the penalty box");
                    _places[_currentPlayer] = _places[_currentPlayer] + roll;
                    if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                    Console.WriteLine(_players[_currentPlayer].Name
                            + "'s new location is "
                            + _places[_currentPlayer]);
                    Console.WriteLine("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(_players[_currentPlayer].Name + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                _places[_currentPlayer] = _places[_currentPlayer] + roll;
                if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                Console.WriteLine(_players[_currentPlayer].Name
                        + "'s new location is "
                        + _places[_currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == "Pop")
            {
                if (_popQuestions.Count == 0)
                    AddQuestion("Pop");
                Console.WriteLine(_popQuestions.First());
                _popQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Science")
            {
                if (_scienceQuestions.Count == 0)
                    AddQuestion("Science");
                Console.WriteLine(_scienceQuestions.First());
                _scienceQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Sports")
            {
                if (_sportsQuestions.Count == 0)
                    AddQuestion("Sports");
                Console.WriteLine(_sportsQuestions.First());
                _sportsQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Rock")
            {
                if (_rockQuestions.Count == 0)
                    AddQuestion("Rock");
                Console.WriteLine(_rockQuestions.First());
                _rockQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Techno")
            {
                if (_technoQuestions.Count == 0)
                    AddQuestion("Techno");
                Console.WriteLine(_technoQuestions.First());
                _technoQuestions.RemoveFirst();
            }
        }

        private void AddQuestion(string category)
        {

            for (var i = 0; i < 50; i++)
            {
                if (category == "Pop")
                {
                    _popQuestions.AddLast("Pop Question " + i);
                }
                if (category == "Science")
                {
                    _scienceQuestions.AddLast(("Science Question " + i));
                }
                if (category == "Sports")
                {
                    _sportsQuestions.AddLast(("Sports Question " + i));
                }
                if (category == "Rock")
                {
                    _rockQuestions.AddLast(CreateRockQuestion(i));
                }
                if (category == "Techno")
                {
                    _technoQuestions.AddLast(CreateTechnoQuestion(i));
                }
            }
        }
       

        public void ShowLeaderBoard()
        {

            Console.WriteLine("######################################");
            Console.WriteLine("The leaderboard is :");
            int cpt = 1;
            foreach(string element in leaderBord)
            {
                Console.WriteLine(cpt + " - " + element);
                cpt++;
            };
        }

        private string CurrentCategory()
        {
            //if (_places[_currentPlayer] == 0) return "Pop";
            //if (_places[_currentPlayer] == 4) return "Pop";
            //if (_places[_currentPlayer] == 8) return "Pop";
            //if (_places[_currentPlayer] == 1) return "Science";
            //if (_places[_currentPlayer] == 5) return "Science";
            //if (_places[_currentPlayer] == 9) return "Science";
            //if (_places[_currentPlayer] == 2) return "Sports";
            //if (_places[_currentPlayer] == 6) return "Sports";
            //if (_places[_currentPlayer] == 10) return "Sports";

            if (_places[_currentPlayer] == 0) return "Pop";
            if (_places[_currentPlayer] == 4) return "Pop";
            if (_places[_currentPlayer] == 8) return "Pop";
            if (_places[_currentPlayer] == 1) return "Science";
            if (_places[_currentPlayer] == 5) return "Science";
            if (_places[_currentPlayer] == 9) return "Science";
            if (_places[_currentPlayer] == 2) return "Sports";
            if (_places[_currentPlayer] == 6) return "Sports";
            if (_places[_currentPlayer] == 10) return "Sports";
            return defaultCategory;
        }

        public bool WasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {

                    Console.WriteLine("------------------------");
                    Console.WriteLine("Answer was correct!!!!");
                    _players[_currentPlayer].GASerie += 1;
                    _purses[_currentPlayer] += _players[_currentPlayer].GASerie;
                    Console.WriteLine(_players[_currentPlayer].Name
                            + " now has "
                            + _purses[_currentPlayer]
                            + " Gold Coins.");

                    var winner = DidPlayerWin();
                    _currentPlayer++;
                    if (_currentPlayer >= _players.Count) _currentPlayer = 0;

                    return winner;
                }
                else
                {
                    _currentPlayer++;
                    if (_currentPlayer >= _players.Count) _currentPlayer = 0;
                    return true;
                }
            }
            else
            {
                Console.WriteLine("------------------------");

                Console.WriteLine("Answer was correct!!!!");
                _players[_currentPlayer].GASerie += 1;
                _purses[_currentPlayer] += _players[_currentPlayer].GASerie;
                Console.WriteLine(_players[_currentPlayer].Name
                        + " now has "
                        + _purses[_currentPlayer]
                        + " Gold Coins.");

                if (_purses[_currentPlayer] >= goldForWinning)
                {
                    Console.WriteLine(_players[_currentPlayer].Name + " finished because he reached " + goldForWinning + " points !");
                }

                var winner = DidPlayerWin();
                _currentPlayer++;
                if (_currentPlayer >= _players.Count) _currentPlayer = 0;

                return winner;
            }
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Question was incorrectly answered");
            //Console.WriteLine("Please choose the next category before going to the penalty box: ");
            //for (int i = 0; i < _categories.Length; i++)
            //{
            //    Console.WriteLine(i + "-" + _categories[i]);
            //}
            //Console.WriteLine("Votre choix: ");
            //var categorieChoice = Console.ReadLine();
            //CurrentCategory() = _categories[Int16.Parse(categorieChoice)];

            Console.WriteLine(_players[_currentPlayer].Name + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;
            _players[_currentPlayer].GASerie = 1;
            _players[_currentPlayer].nbTimeInPrison ++;


            _currentPlayer++;
            if (_currentPlayer >= _players.Count) _currentPlayer = 0;
            return true;
        }

        public bool UseJoker()
        {
            if (_players[_currentPlayer].Joker >= 1)
            {
                Console.WriteLine("Do you want use your joker ?");
                Console.WriteLine("Write '0' for No or '1' for Yes");
                var response = Convert.ToInt32(Console.ReadLine());
                if (response == 1)
                {
                    _players[_currentPlayer].Joker--;
                    _currentPlayer++;
                    if (_currentPlayer >= _players.Count) _currentPlayer = 0;
                    return true;
                }

            }
            return false;
        }

        public bool WantLeave()
        {
            if (_players[_currentPlayer].IsInGame)
            {
                Console.WriteLine("Do you want to leave the game ?");
                Console.WriteLine("Write '0' for No or '1' for Yes");
                var response = Convert.ToInt32(Console.ReadLine());
                if (response == 1)
                {
                    Console.WriteLine("You leave the game !");
                    _players.RemoveAt(_currentPlayer);

                    _currentPlayer++;
                    if (_currentPlayer >= _players.Count) _currentPlayer = 0;
                    return true;
                }
            }

            return false;
        }

        public bool IsFinished()
        {
            if (leaderBord.Count == 3 || ((_players.Count == 0) && (leaderBord.Count == 2)))
            {
                return true;
            }
            return false;
        }


        private bool DidPlayerWin()
        {
            if(_purses[_currentPlayer] >= goldForWinning)
            {
                leaderBord.Add(_players[_currentPlayer].Name);
                _players.RemoveAt(_currentPlayer);
            }
            if (leaderBord.Count == 3 || ((_players.Count == 0) && (leaderBord.Count == 2)))
                return false;
            else
                return true;
        }

        //Console.WriteLine(_players[_currentPlayer].Name + " is the winner because he has " + goldForWinning + "points !");

        public class Player
        {
            public string Name { get; set; }
            public int Joker { get; set; }
            public bool IsInGame { get; set; }
            public int GASerie { get; set; } // Good Answer Serie
            public int nbTimeInPrison { get; set; }
        }
    }

}
