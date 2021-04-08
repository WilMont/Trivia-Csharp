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
        private readonly string[] _categories = new string[] { "Rock", "Techno" };

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;
        private string defaultCategory;

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
            for(int i=0; i< _categories.Length; i++)
            {
                Console.WriteLine(i + "-" + _categories[i]);
            }
            var choixCategorie = Console.ReadLine();
            switch (choixCategorie)
            {
                case "0":
                    defaultCategory = _categories[0];
                    break;
                case "1":
                    defaultCategory = _categories[1];
                    break;
                default:
                    Console.WriteLine("Please chose a correct value.");
                    break;
            }
            return defaultCategory;
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
                if (Int16.Parse(playersNumber) < 2)
                {
                    Console.WriteLine("Vous n'avez pas assez de joueur pour commencer");
                }
                else if (Int16.Parse(playersNumber) > 6)
                {
                    Console.WriteLine("Vous avez trop de joueur pour jouer");
                }
                else if (Int16.Parse(playersNumber) >= 2 && Int16.Parse(playersNumber) <= 6)
                {
                    for (int i = 0; i < Int16.Parse(playersNumber); i++)
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
            _players.Add(new Player { Name = playerName, Joker = 1 , IsInGame = true});
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
            Console.WriteLine(_players[_currentPlayer].Name + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 != 0)
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
                Console.WriteLine(_popQuestions.First());
                _popQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Science")
            {
                Console.WriteLine(_scienceQuestions.First());
                _scienceQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Sports")
            {
                Console.WriteLine(_sportsQuestions.First());
                _sportsQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Rock")
            {
                Console.WriteLine(_rockQuestions.First());
                _rockQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Techno")
            {
                Console.WriteLine(_technoQuestions.First());
                _technoQuestions.RemoveFirst();
            }
        }

        private string CurrentCategory()
        {
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
                    Console.WriteLine("Answer was correct!!!!");
                    _purses[_currentPlayer]++;
                    Console.WriteLine(_players[_currentPlayer].Name
                            + " now has "
                            + _purses[_currentPlayer]
                            + " Gold Coins.");

                    var winner = DidPlayerWin();
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;

                    return winner;
                }
                else
                {
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Answer was corrent!!!!");
                _purses[_currentPlayer]++;
                Console.WriteLine(_players[_currentPlayer].Name
                        + " now has "
                        + _purses[_currentPlayer]
                        + " Gold Coins.");

                var winner = DidPlayerWin();
                _currentPlayer++;
                if (_currentPlayer == _players.Count) _currentPlayer = 0;

                return winner;
            }
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[_currentPlayer].Name + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;
            return true;
        }

        public bool UseJoker()
        {
            if (_players[_currentPlayer].Joker >= 1)
            {
                Console.WriteLine("Do you want use your joker ?");
                Console.WriteLine("Write '0' for No or '1' for Yes");
                var response = Convert.ToInt32(Console.ReadLine());
                if(response == 1)
                {
                    _players[_currentPlayer].Joker--;
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;
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
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;
                    return true;
                }
            }

            return false;
        }

        public bool IsAlone()
        {
            if(_players.Count == 1)
            {
                Console.WriteLine(_players[_currentPlayer].Name + " is the winner because he is the last on the game !");
                return true;

            }
            return false;
        }


        private bool DidPlayerWin()
        {
            return !(_purses[_currentPlayer] == 6);
        }

        public class Player
        {
            public string Name { get; set; }
            public int Joker { get; set; }
            public bool IsInGame { get; set; }
        }
    }

}
