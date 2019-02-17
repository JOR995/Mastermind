using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    class Player
    {
        int numPegs, numColours;

        /// <summary>
        /// Constructor for the Player class
        /// Called from the Game class when the user selects to start a Human vs AI game
        /// Initialises the values for the numPegs and numColours variable using values passed by the calling class
        /// </summary>
        /// <param name="numberOfPegs"></param>
        /// <param name="numberOfColours"></param>
        public Player(int numberOfPegs, int numberOfColours)
        {
            numPegs = numberOfPegs;
            numColours = numberOfColours;
        }

        /// <summary>
        /// Called when the user is required to enter their guess at the answer code
        /// </summary>
        /// <returns>The user's guess is returned in an array of CodeColour variables</returns>
        public CodeColours[] TakeTurn()
        {
            CodeColours[] playerGuess = new CodeColours[numPegs];

            bool validGuess = false;
            char[] inputArray;

            //Displayed text to the user prompts for them to enter their guess at the answer code
            //Tells the user the length of the required input and the letters that correspond to the available colours
            //This is done in a loop until the user inputs a valid guess
            do
            {
                Console.WriteLine("\nPlease enter your guess at the {0}-digit code:", numPegs);

                //Changes what available colours are displayed to the user to correctly match the numColours setting for the game
                switch (numColours)
                {
                    case 2:
                        Console.WriteLine("R = Red, G = Green");
                        break;
                    case 3:
                        Console.WriteLine("R = Red, G = Green, B = Blue");
                        break;
                    case 4:
                        Console.WriteLine("R = Red, G = Green, B= Blue, Y = Yellow");
                        break;
                    case 5:
                        Console.WriteLine("R = Red, G = Green, B = Blue, Y = Yellow, O = Orange");
                        break;
                    case 6:
                        Console.WriteLine("R = Red, G = Green, B = Blue, Y = Yellow, O = Orange, P = Purple");
                        break;
                    case 7:
                        Console.WriteLine("R = Red, G = Green, B = Blue, Y = Yellow, O = Orange, P = Purple, I = Indigo");
                        break;
                    case 8:
                        Console.WriteLine("R = Red, G = Green, B = Blue, Y = Yellow, O = Orange, P = Purple, I = Indigo, V = Violet");
                        break;
                }

                //Takes the inputted string of letters and breaks it down into an array of type char so that each letter is an entry in the array
                inputArray = Console.ReadLine().ToCharArray();

                //First checks that the length of the array matches the numPegs variable
                if (inputArray.Length == numPegs)
                {
                    //Then runs through the array and tries to parse each letter to check that is a value within the CodeColours enum
                    //If all letters successfully parse they are added to the playerGuess array and is considered valid and is passed back to the calling function
                    //If one of the letters does not parse then the input is considered invalid and the loop repeats
                    for (int i = 0; i < numPegs; i++)
                    {
                        if (Enum.TryParse<CodeColours>(inputArray[i].ToString().ToUpper(), out CodeColours colour))
                        {
                            if ((int)(CodeColours)colour >= 1 && (int)(CodeColours)colour <= numColours)
                            {
                                playerGuess[i] = colour;
                                validGuess = true;
                            }
                            else
                            {
                                validGuess = false;
                                break;
                            }
                        }
                        else
                        {
                            validGuess = false;
                            break;
                        }
                    }
                }
            }
            while (!validGuess);

            return playerGuess;
        }
    }
}
