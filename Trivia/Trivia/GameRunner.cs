using System;

namespace Trivia
{
    public class GameRunner
    {
        private static bool _notAWinner;

        public static void Main(string[] args)
        {
            var aGame = new Game();

            aGame.InitializePlayers();

            aGame.AskDefaultCategory();

            if (aGame.IsPlayable())
            {

                var rand = new Random();

                do
                {
                    aGame.Roll(rand.Next(5) + 1);
                    if (aGame.IsAlone())
                        break;
                    if (!aGame.WantLeave())
                    { 
                        if (!aGame.UseJoker())
                        {
                            if (rand.Next(9) == 7)
                            {
                                _notAWinner = aGame.WrongAnswer();
                            }
                            else
                            {
                                _notAWinner = aGame.WasCorrectlyAnswered();
                            }
                        }
                        else
                        {
                            _notAWinner = true;
                        }
                    }
                    else
                    {
                        _notAWinner = true;
                    }
                } while (_notAWinner );
            }
        }
    }
}