using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    //Public enums containing the available code peg colours and answer peg colours represented by single letters
    //These enums are accessed and used by all other classes
    public enum CodeColours { X, R, G, B, Y, O, P, I, V };
    public enum AnswerColours { X, W, B };

    /// <summary>
    /// Main class for running the program, handles the creation of instances of other classes as well as calling methods from those classes
    /// Also generates menus and handles user input for them
    /// </summary>
    class Game
    {
        //Two private enums for carrying different menu choices between methods within the class
        enum GameType { HumanvsAI, AIvsAI }
        GameType gameType;

        enum SettingType { Pegs, Colours, Guesses }
        SettingType settingType;

        Random rnd;
        Player player;
        CodeBreaker codeBreaker;
        CodeMaker codeMaker;
        GameBoard gameBoard;

        bool gameFinished, isPlaying;
        int numPegs, numColours, numGuesses;

        /// <summary>
        /// Constructor for class, initialises variables and creates objects of other classes
        /// </summary>
        public Game()
        {
            isPlaying = true;

            //Generates new random seed to be used throughout the program
            rnd = new Random();
            int tempPegNum, tempColourNum;

            //Sets initial settings for the game
            numPegs = 4;
            numColours = 6;
            numGuesses = 12;

            do
            {
                gameFinished = false;
                int menuChoice;

                //Calls method to display the main menu to the user, stores the returned int as the menu choice variable
                menuChoice = MainMenu();

                tempPegNum = numPegs;
                tempColourNum = numColours;

                //Using the menu choice variable in a switch statement the program creates the correct objects for the chosen gamemode
                switch (menuChoice)
                {
                    case 1:
                        //Creates an instance of the Player class for the user to play as the codebreaker
                        player = new Player(numPegs, numColours);
                        gameType = GameType.HumanvsAI;
                        break;
                    case 2:
                        //Creates an instance of the CodeBreaker class for the AI as the codebreaker
                        //Also caps the number of pegs and colours available for the AI codebreaker, this is to avoid overly long processing times and out of bounds errors with higher settings
                        if (tempPegNum > 6) tempPegNum = 6;

                        if (tempColourNum > 7) tempColourNum = 7;

                        codeBreaker = new CodeBreaker(rnd, tempPegNum, tempColourNum);
                        gameType = GameType.AIvsAI;
                        break;
                    case 5:
                        //If the user selects to exit the program, the console window is closed
                        Environment.Exit(0);
                        break;
                }

                //Regardless of the game mode chosen by the user, instances of the CodeMaker and GameBoard classes are created, passing the game settings
                codeMaker = new CodeMaker(rnd, tempPegNum, tempColourNum);
                gameBoard = new GameBoard(codeMaker, tempPegNum, numGuesses);

                //PlayGame method is finally called which starts the game
                PlayGame();
                Console.ReadLine();
            }
            while (isPlaying);
        }

        /// <summary>
        /// Called to display the main menu to the user
        /// This allows the user to select which game mode to run the game in, open the settings menu or exit the program
        /// Detects the input of the user and returns it
        /// </summary>
        /// <returns>int value corresponding to the users' menu choice</returns>
        private int MainMenu()
        {
            ConsoleKeyInfo input;
            bool validSelection = false;
            int selection;

            //do loop used to keep the menu open until the user enters a valid input
            do
            {
                Console.Clear();
                Console.WriteLine("     Mastermind\n~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine(" Menu\n Please choose a number from below:");
                Console.WriteLine("\n1) Play against AI");
                Console.WriteLine("2) AI vs AI");
                Console.WriteLine("3) Settings");
                Console.WriteLine("4) How to Play");
                Console.WriteLine("5) Quit");

                input = Console.ReadKey();

                //Switch function using the key pressed by the user,
                //Checks whether the key is any of the valid options; which are number keys 1 - 4 and their number pad equivalents
                //However input of number 3 will not exit the loop but will call the SettingsMenu method
                switch (input.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        selection = 1;
                        validSelection = true;
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        selection = 2;
                        validSelection = true;
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        selection = 3;
                        validSelection = false;
                        SettingsMenu();
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        selection = 4;
                        validSelection = false;
                        HowToPlay();
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        selection = 5;
                        validSelection = true;
                        break;
                    default:
                        selection = 0;
                        validSelection = false;
                        break;
                }
            }
            while (!validSelection);

            //Once a valid input is detected it is returned to the calling method
            return selection;
        }

        /// <summary>
        /// Called from the MainMenu method and displays the settings menu to the user
        /// From this the user can select which game setting they wish to change or return to the main menu
        /// This method uses the same structure of detecting and checking user input as the MainMenu method
        /// </summary>
        private void SettingsMenu()
        {
            ConsoleKeyInfo input;
            bool validSelection = false;

            do
            {
                Console.Clear();
                Console.WriteLine("     Settings\n~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine(" Menu\n Please choose a number from below:");
                Console.WriteLine("\n1) Change number of pegs per guess (current = {0})", numPegs);
                Console.WriteLine("2) Change numer of code colours (current = {0})", numColours);
                Console.WriteLine("3) Change number of available guesses (current = {0})", numGuesses);
                Console.WriteLine("4) Return to main menu");

                //If the settings are set above this threshold, the user is informed of the restrictions put onto the AI codebreaker
                //These restrictions are not placed on the user as the codebreaker however
                if (numPegs > 6 || numColours > 7)
                {
                    Console.WriteLine("\nDue to long processing times the AI codebreaker is limited to a maximum of 6 pegs and 7 colours");
                }

                input = Console.ReadKey();

                //If user input matches one of the valid options, calls the ChangeSetting method and sets the SettingType variable to match the menu option chosen
                switch (input.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        settingType = SettingType.Pegs;
                        ChangeSetting();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        settingType = SettingType.Colours;
                        ChangeSetting();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        settingType = SettingType.Guesses;
                        ChangeSetting();
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        validSelection = true;
                        break;
                    default:
                        validSelection = false;
                        break;
                }
            }
            while (!validSelection);
        }

        /// <summary>
        /// Called to change one of the settings chosen by the user
        /// Uses the same structure to check for valid inputs as the previous two methods
        /// Once a valid input is entered and the setting is changed this will return back to the calling function
        /// </summary>
        private void ChangeSetting()
        {
            string input;
            bool validSelection = false;

            do
            {
                Console.Clear();

                //This will display the header to the user depending on the setting they chose to change from the calling method
                switch (settingType)
                {
                    case SettingType.Pegs:
                        Console.WriteLine(" Number of Pegs\n Please enter a number between 2 - 12:");
                        break;
                    case SettingType.Colours:
                        Console.WriteLine(" Number of Colours\n Please enter a number between 2 - 8:");
                        break;
                    case SettingType.Guesses:
                        Console.WriteLine(" Number of Guesses\n Please enter a number between 3 - 30:");
                        break;
                }

                input = Console.ReadLine();

                //Attempts to parse the string input as an integer value, if succesful it will then check the number is within the valid range dependant on the setting chosen
                //If the input is valid then the value of the variable for the setting is changed to the input
                if (int.TryParse(input, out int inputNumber))
                {
                    switch (settingType)
                    {
                        case SettingType.Pegs:
                            if (inputNumber >= 2 && inputNumber <= 12)
                            {
                                numPegs = inputNumber;
                                validSelection = true;
                            }
                            else validSelection = false;
                            break;
                        case SettingType.Colours:
                            if (inputNumber >= 2 && inputNumber <= 8)
                            {
                                numColours = inputNumber;
                                validSelection = true;
                            }
                            else validSelection = false;

                            break;
                        case SettingType.Guesses:
                            if (inputNumber >= 3 && inputNumber <= 30)
                            {
                                numGuesses = inputNumber;
                                validSelection = true;
                            }
                            else validSelection = false;
                            break;
                    }
                }
                else validSelection = false;
            }
            while (!validSelection);
        }


        private void HowToPlay()
        {
            //Displays the game instructions to the player

            Console.Clear();
            Console.WriteLine("####################");

            for (int i = 0; i < 4; i++)
            {
                Console.Write("# ");
                for (int x = 0; x < 4; x++)
                {
                    Console.Write("X");
                }
                Console.Write("|");
                for (int x = 0; x < 4; x++)
                {
                    Console.Write("X");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("# ------------------");

            Console.WriteLine("\n\n The game board is represented like the grid above:");
            Console.WriteLine("\tEach row repesents a guess at the code \n\tThe number of X's shows the number of pegs in the code " +
                "\n\tInputted guesses will appear on the left and the answers to them appear on the right followed by the guess number");
            Console.WriteLine("\nColours in guesses are represented by single letters \nColours in answers are represented as W = White or B = Black");
            Console.WriteLine("\tWhite means a colour is correct but in the wrong space \n\tBlack means a colour is correct and in the correct space");
            Console.ReadLine();
        }


        /// <summary>
        /// Called when the user selects to start the game
        /// Goes through different method calls to different classes to make up the structure of the turns in the game
        /// Runs through different method calls depending on whether the game type is Human vs. AI or AI vs. AI
        /// </summary>
        private void PlayGame()
        {
            //Creates new arrays for the CodeBreaker's guesses and the CodeMaker's responses
            CodeColours[] codeBreakerGuess = new CodeColours[numPegs];
            AnswerColours[] guessResponse = new AnswerColours[numPegs];

            //Runs through this loop whilst the game is still being played, has not yet been won by either player
            do
            {
                if (gameType == GameType.HumanvsAI)
                {
                    //Fills the codeBreakerGuess array using the return value from the TakeTurn method within the Player class
                    //The guessResponse array is then filled using the return value from the CheckGuess within the CodeMaker class by passing it the codeBreakerGuess array
                    //Finally the gameFinished bool is set by passing both arrays to the GenerateBoard method in the GameBoard class, if this returns a true value the do loop is exited
                    codeBreakerGuess = player.TakeTurn();
                    guessResponse = codeMaker.CheckGuess(codeBreakerGuess);
                    gameFinished = gameBoard.GenerateBoard(codeBreakerGuess, guessResponse);
                }
                else if (gameType == GameType.AIvsAI)
                {
                    //Runs through a similar structure as with the HumanvsAI game type however the codeBreakerGuess array is filled using the TakeTurn method in the CodeBreaker class
                    //Also the CheckRemainingCombos method is called in the CodeBreaker class passing the guessReponse array
                    Console.ReadLine();
                    codeBreakerGuess = codeBreaker.TakeTurn();
                    guessResponse = codeMaker.CheckGuess(codeBreakerGuess);
                    codeBreaker.CheckRemainingCombos(guessResponse);
                    gameFinished = gameBoard.GenerateBoard(codeBreakerGuess, guessResponse);
                }
            }
            while (!gameFinished);
        }
    }
}
