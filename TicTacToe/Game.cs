using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Game
    {
        private char[,] _field;
        private int _player1Score;
        private int _player2Score;

        public void Start()
        {
            Console.Clear();
            _player1Score = 0;
            _player2Score = 0;
            Console.Write("Welcome to Tic-tac-toe game! Choose a count of rounds: ");

            int rounds;

            while (true)
            {
                bool success = int.TryParse(Console.ReadLine(), out rounds);
                if (success && rounds > 0) break;
                Console.Write("You need to write a number: ");
            }
            Play(rounds);
        }
        private void Play(int rounds)
        {
            Console.Clear();

            for (int i = 0; i < rounds; i++)
                PlayRound(i + 1, rounds, i % 2 == 0 ? 'x' : 'o');

            if (_player1Score == _player2Score) Console.WriteLine("Draw!");
            else
            {
                int winner = _player1Score > _player2Score ? 1 : 2;
                Console.WriteLine($"Congratulations! Player {winner} win!");
            }
            Console.WriteLine($"The final score is: {_player1Score}:{_player2Score}");
            Console.Write("Wanna play again? yes/no: ");
            while (true)
            {
                string answer = Console.ReadLine();

                if (answer == "yes" || answer == "y")
                {
                    Start();
                    break;
                }
                else if (answer == "no" || answer == "n") break;
                else Console.Write("Incorrect answer, type yes/no: ");
            }
        }

        private void PlayRound(int round, int rounds, char player1Char)
        {
            _field = new char[3, 3];
            SortedSet<byte> freeCells = new();
            for (byte i = 1; i < 10; i++) freeCells.Add(i);
            bool player1Turn = player1Char == 'x' ? true : false;
            char player2Char = player1Char == 'x' ? 'o' : 'x';

            char winner;

            while (true)
            {
                Console.WriteLine($"Round: {round}/{rounds}!");
                DrawField();
                string player = player1Turn ? $"Player 1({Char.ToUpper(player1Char)})" : $"Player 2({Char.ToUpper(player2Char)})";

                byte result;
                while (true)
                {
                    Console.Write($"Player {player}'s turn! Choose your move(1-9): ");
                    bool success = byte.TryParse(Console.ReadLine(), out result);

                    if (success && freeCells.Contains(result))
                    {
                        freeCells.Remove(result);
                        break;
                    }
                }
                _field[(result - 1) / 3, (result - 1) % 3] = player1Turn ? player1Char : player2Char;
                player1Turn = !player1Turn;

                Console.Clear();

                winner = CheckWin();
                if (winner != '-') break;

            }

            if (winner == 'd')
            {
                Console.WriteLine("Wow! Draw!");
            }
            else
            {
                int winnerPlayer = winner == player1Char ? 1 : 2;

                if (winnerPlayer == 1) _player1Score++;
                else _player2Score++;
                Console.WriteLine($"Wow! Player {winnerPlayer} wins! Congratulations!");
            }

            Console.WriteLine($"Current score: {_player1Score}:{_player2Score}");
            DrawField();
            Console.WriteLine("Press any key to continue!");
            Console.ReadKey();
            Console.Clear();

        }
        private void DrawField()
        {
            string[,,] values = {
                { { "  ", "1 " }, { "  ", "2 " }, { "  ", "3 " } },
                { { "4 ", "  "},  {"5 ", "  " }, {"6 ", "  " } },
                { { "7 ", "  "}, {"8 ", "  " }, {"9 ", "  " } }
            };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_field[i, j] == 'x')
                    {
                        values[i, j, 0] = "\\/";
                        values[i, j, 1] = "/\\";
                    }
                    else if (_field[i, j] == 'o')
                    {
                        values[i, j, 0] = "/\\";
                        values[i, j, 1] = "\\/";
                    }
                }
            }

            Console.WriteLine('\n');
            Console.WriteLine($"  {values[0, 0, 0]}  |  {values[0, 1, 0]}  |  {values[0, 2, 0]}");
            Console.WriteLine($"  {values[0, 0, 1]}  |  {values[0, 1, 1]}  |  {values[0, 2, 1]}");
            Console.WriteLine("______|______|______");
            Console.WriteLine("      |      |");
            Console.WriteLine($"  {values[1, 0, 0]}  |  {values[1, 1, 0]}  |  {values[1, 2, 0]}");
            Console.WriteLine($"  {values[1, 0, 1]}  |  {values[1, 1, 1]}  |  {values[1, 2, 1]}");
            Console.WriteLine("______|______|______");
            Console.WriteLine("      |      |");
            Console.WriteLine($"  {values[2, 0, 0]}  |  {values[2, 1, 0]}  |  {values[2, 2, 0]}");
            Console.WriteLine($"  {values[2, 0, 1]}  |  {values[2, 1, 1]}  |  {values[2, 2, 1]}");
            Console.WriteLine("      |      |");
        }



        private char CheckWin()
        {
            for (int i = 0; i < 3; i++)
                if (_field[i, 0] != '\0' && _field[i, 1] != '\0' && _field[i, 2] != '\0'
                    && _field[i, 0] == _field[i, 1] && _field[i, 1] == _field[i, 2])
                    return _field[i, 0];

            for (int j = 0; j < 3; j++)
                if (_field[0, j] != '\0' && _field[1, j] != '\0' && _field[2, j] != '\0' &&
                    _field[0, j] == _field[1, j] && _field[1, j] == _field[2, j])
                    return _field[0, j];

            if (_field[0, 0] != '\0' && _field[1, 1] != '\0' && _field[2, 2] != '\0'
                && _field[0, 0] == _field[1, 1] && _field[1, 1] == _field[2, 2]
                || _field[0, 2] != '\0' && _field[1, 1] != '\0' && _field[2, 0] != '\0'
                && _field[0, 2] == _field[1, 1] && _field[1, 1] == _field[2, 0])
                return _field[1, 1];

            if (HasEmptyCells()) return '-';

            return 'd';
        }

        private bool HasEmptyCells()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!(_field[i, j] == 'x' || _field[i, j] == 'o')) return true;
                }
            }
            return false;
        }

    }
}