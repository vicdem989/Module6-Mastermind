


using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Utils;
namespace MasterMind
{


    public enum GAME_TYPE : int
    {
        PLAYER_VS_NPC,
        PLAYER_VS_PLAYER
    }

    public enum COLORS : int
    {
        RED,
        YELLOW,
        GREEN,
        BLUE,
        MAGENTA,
        CYAN,
    }

    public class MasterMindGame : GameEngine.IScene
    {

        const int CORRECT_COLOR_CORRECT_SPOT = 1;
        const int JUST_CORECT_COLOR = 0;
        const int WRONG = -1;

        GAME_TYPE type;
        public Action<Type, object[]> OnExitScreen { get; set; }

        int[] solution;
        int[] guess;
        int[] evaluation;

        List<int[]> guesses;
        List<int[]> evaluations;

        private int numberOfAttempts = 0;
        private bool duplicates = true;
        private int elementsInHiddenSequence = 4;
        private int amountColorsInHiddenSequence = 0;

        private int currentAttempts = 0;
        private int attemptsLeft = 0;

        bool dirty = false;

        public MasterMindGame(GAME_TYPE gameType)
        {
            type = GAME_TYPE.PLAYER_VS_NPC;
            evaluations = new List<int[]>();
            guesses = new List<int[]>();
            currentAttempts = 0;
            attemptsLeft = 0;
            numberOfAttempts = 0;
            elementsInHiddenSequence = 4;
            amountColorsInHiddenSequence = 0;
        }

        #region MasterMind game functions

        private int[] CreateSequence(int[] colors, int length = 4, bool duplicates = true)
        {
            List<int> sequence = new();
            Random rnd = new((int)DateTime.Now.Ticks);

            while (sequence.Count < length)
            {
                int index = rnd.Next(0, colors.Length);
                int peg = colors[index];

                if (duplicates == false)
                {
                    if (sequence.IndexOf(peg) == -1)
                    {
                        sequence.Add(peg);
                    }
                }
                else
                {
                    sequence.Add(peg);
                }
            }


            return sequence.ToArray();
        }

        int[] askAndRetriveGuessFromUser()
        {
            if (guess == null)
            {
                Console.WriteLine(Output.Align("Guess a sequence ", Alignment.CENTER));
                string response = Console.ReadLine();

                while (!ValidateInput(response))
                {
                    Console.WriteLine(Output.Align("Input a new input", Alignment.CENTER));
                    response = Console.ReadLine();
                    ValidateInput(response);
                }

                string[] temp = response.Split(" "); // Create a array of strings.
                int[] intTemp = Array.ConvertAll(temp, s => int.Parse(s)); // Convert to an array of int
                return intTemp;
            }
            return guess;
        }

        bool ValidateInput(string input)
        {
            string[] temp = input.Split(' ');
            if (temp.Length < solution.Length || temp.Length > solution.Length)
                return false;
            foreach (string element in temp)
            {
                if (!int.TryParse(element, out int result))
                    return false;
            }
            return true;
        }


        int[] Compare(int[] sourceList, int[] guessList)
        {

            int[] output = new int[guessList.Length];

            for (int index = 0; index < guessList.Length; index++)
            {
                int score = WRONG;
                int correct = sourceList[index];
                int guess = guessList[index];

                if (correct == guess)
                {
                    score = CORRECT_COLOR_CORRECT_SPOT;
                }
                else if (IsPartOfSolution(sourceList, guess))
                {
                    score = JUST_CORECT_COLOR;
                }
                output[index] = score;
            }


            return output;
        }

        bool IsPartOfSolution(int[] solution, int guess)
        {
            bool found = false;
            for (int index = 0; index < solution.Length; index++)
            {
                if (solution[index] == guess)
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        private int GetAmountOfAttempts()
        {
            Output.Write(Output.Align("\nHow many attempts do you want? Max 10", Alignment.CENTER), true);
            return CheckIntInput(Console.ReadLine().ToLower(), 1, 10);
        }

        private int CheckIntInput(string input, int min, int max)
        {
            int result = 0;
            while (int.TryParse(input, out result) == false || result > max || result < min)
            {
                Output.Write(Output.Align("Input a valid number of attempts\n", Alignment.CENTER), true);
                Console.SetCursorPosition(Console.WindowWidth / 2, 0);
                input = Console.ReadLine();
            }
            return result;
        }

        private bool GetDuplicate()
        {
            Output.Write(Output.Align("\nDo you want duplicates? y/n", Alignment.CENTER), true);
            ANSICodes.Positioning.SetCursorPos(Console.WindowWidth / 2, Console.WindowHeight);
            string userInput = Console.ReadLine().ToLower();
            while (userInput != "y" && userInput != "n")
            {
                Output.Write(Output.Align("Only valid input is y or n", Alignment.CENTER), true);
                userInput = Console.ReadLine().ToLower();
            }
            if (userInput == "y")
            {
                return true;
            }
            return false;
        }

        private int GetElementsInHiddenSequence()
        {
            Output.Write(Output.Align("\nHow many elements in hidden sequence? Max 10", Alignment.CENTER), true);
            ANSICodes.Positioning.SetCursorPos(Console.WindowWidth / 2, Console.WindowHeight);
            return CheckIntInput(Console.ReadLine().ToLower(), 1, 10);
        }

        private int GetAmountColorsInHiddenSequence()
        {
            Output.Write(Output.Align("\nHow many different colors in the hidden sequence?, Max 4", Alignment.CENTER), true);
            ANSICodes.Positioning.SetCursorPos(Console.WindowWidth / 2, Console.WindowHeight);
            return CheckIntInput(Console.ReadLine().ToLower(), 1, 4);
        }

        private void CheckGameState()
        {
            for (int i = 0; i < solution.Length; i++)
            {
                if (guess[i] != solution[i])
                {
                    break;
                }
                YouWon();
            }
            if (attemptsLeft <= 0)
            {
                NoMoreAttempts();
            }
        }

        private void YouWon()
        {
            Console.Clear();
            Output.Write(Output.Align("Y)OIYIASDHAS WIN!", Alignment.CENTER), true);
            Output.Write(Output.Align("Playa again? y/n", Alignment.CENTER), true);
            string input = Console.ReadLine().ToLower();
            while (input != "y" && input != "n")
            {
                Output.Write(Output.Align("Its a yes (y) or no (n) question..."));
                input = Console.ReadLine().ToLower();
            }
            Console.Clear();
            if (input == "y")
            {
                OnExitScreen(typeof(MasterMindGame), new object[] { GAME_TYPE.PLAYER_VS_NPC });
            }
            else if (input == "n")
            {
                Output.Write(Output.Align("Thanky you for playing", Alignment.CENTER), true);
                Environment.Exit(0);
                return;
            }
        }

        private void NoMoreAttempts()
        {
            Console.Clear();
            Output.Write(Output.Align("No more attempts unlucky", Alignment.CENTER), true);
            Output.Write(Output.Align("Play again? y/n", Alignment.CENTER), true);
            string input = Console.ReadLine().ToLower();
            while (input != "y" && input != "n")
            {
                Output.Write(Output.Align("Its a yes (y) or no (n) question..."));
                input = Console.ReadLine().ToLower();
            }
            Console.Clear();
            if (input == "y")
            {
                OnExitScreen(typeof(MasterMindGame), new object[] { GAME_TYPE.PLAYER_VS_NPC });
            }
            else if (input == "n")
            {
                MenuScreen.menuItems[0] = "Start new Game";
                MenuScreen.menuItems[1] = "Settings";
                MenuScreen.menuItems[2] = "Quit";
                OnExitScreen(typeof(MenuScreen), null);
                return;
            }
        }

        #endregion

        #region GameEngine.IScene
        public void init()
        {
            if (type == GAME_TYPE.PLAYER_VS_NPC)
            {

                Console.Clear();
                numberOfAttempts = GetAmountOfAttempts();
                Console.Clear();
                duplicates = GetDuplicate();
                Console.Clear();
                elementsInHiddenSequence = GetElementsInHiddenSequence();
                Console.Clear();
                amountColorsInHiddenSequence = GetAmountColorsInHiddenSequence();
                Console.Clear();

                int[] colors = new[] { (int)COLORS.BLUE, (int)COLORS.CYAN, (int)COLORS.GREEN, (int)COLORS.MAGENTA };
                solution = CreateSequence(colors, elementsInHiddenSequence, duplicates);
                Console.WriteLine(string.Join(",", solution));
            }
        }

        public void input()
        {
            if (type == GAME_TYPE.PLAYER_VS_PLAYER)
            {
                // If it is the "AI" player the input from the player must be akin to -1 for wrong 0 for correct color, and 1 for correct collor and space
            }
            else
            {
                guess = askAndRetriveGuessFromUser();
                currentAttempts++;
            }
        }



        public void update()
        {
            attemptsLeft = numberOfAttempts - currentAttempts;
            CheckGameState();
            if (type == GAME_TYPE.PLAYER_VS_PLAYER)
            {

            }
            else
            {

                if (guess != null)
                {
                    evaluation = Compare(solution, guess);
                    guesses.Add(guess);
                    evaluations.Add(evaluation);
                    guess = null;
                    evaluation = null;
                    dirty = true;
                }
            }
        }

        public void draw()
        {

            if (dirty)
            {
                Console.Clear();
                dirty = false;
                Output.Write(Output.Align($"Guesses left: {attemptsLeft}", Alignment.CENTER), true);
                int limit = evaluations.Count();
                for (int i = 0; i < limit; i++)
                {
                    string gu = String.Join(' ', guesses[i]);
                    string ev = String.Join(' ', evaluations[i]);
                    Console.WriteLine(Output.Align($"{gu}  |  {ev}", Alignment.CENTER));
                }
            }


        }

        #endregion



    }
}