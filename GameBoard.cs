using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    class GameBoard
    {
        CodeColours[] answerCode;
        CodeColours[,] turnGuesses;
        AnswerColours[,] turnAnswers;
        CodeMaker codeMaker;

        bool gameFinish;
        int turnNum, numPegs, numGuesses;

        /// <summary>
        /// Constructor for the class, called from the Game class when the user has chosen a gamemode
        /// Initialises variable values using passed parameters, also gets the answerCode from the CodeMaker class
        /// Also calls the GenerateBoard method to display the game board
        /// </summary>
        /// <param name="codeMakerObj">The instance of the CodeMaker class created within the Game class</param>
        /// <param name="numberOfPegs"></param>
        /// <param name="numberOfGuesses"></param>
        public GameBoard(CodeMaker codeMakerObj, int numberOfPegs, int numberOfGuesses)
        {
            codeMaker = codeMakerObj;
            turnNum = 0;
            numPegs = numberOfPegs;
            numGuesses = numberOfGuesses;
            gameFinish = false;
            turnGuesses = new CodeColours[numPegs, numGuesses];
            turnAnswers = new AnswerColours[numPegs, numGuesses];
            answerCode = codeMaker.CodeAnswer;

            GenerateBoard(null, null);
        }

        /// <summary>
        /// Called to display the game board to the user
        /// Adds the two parameter arrays to the board upon being displayed
        /// </summary>
        /// <param name="guess">The guess from either the Player of CodeBreaker class for this turn</param>
        /// <param name="guessAnswer">The response from the CodeMaker class for this turn</param>
        /// <returns>Returns a boolean value to state whether the game has been completed or not</returns>
        public bool GenerateBoard(CodeColours[] guess, AnswerColours[] guessAnswer)
        {
            //As long as the turn number is greater than 0 (parameters are therefore not null) UpdateArrays method is called to update game board display
            if (turnNum > 0)
            {
                UpdateArrays(guess, guessAnswer);
            }

            //Writes out the game board using different ASCII keys and the turnGuesses and turnAnswers arrays
            Console.Clear();
            Console.WriteLine("####################");

                for (int i = 0; i < turnGuesses.GetLength(1); i++)
                {
                    Console.Write("# ");
                    for (int x = 0; x < turnGuesses.GetLength(0); x++)
                    {
                        Console.Write(turnGuesses[x, i]);
                    }
                    Console.Write("|");
                    for (int x = 0; x < turnGuesses.GetLength(0); x++)
                    {
                        Console.Write(turnAnswers[x, i]);
                    }

                    if (turnNum >= (i + 1))
                    {
                        Console.WriteLine(" {0}", (i + 1));
                    }
                    else Console.WriteLine("");
                }
            Console.WriteLine("# ------------------");

            //If the game has finished then will also display the answer code and a message stating who won
            if (gameFinish)
            {
                Console.Write("# ");
                for (int i = 0; i < answerCode.Length; i++)
                {
                    Console.Write(answerCode[i]);
                }
                Console.WriteLine("\n####################");
                Console.WriteLine("\nGame Finished, Code Breaker Wins!");
            }
            else if (turnNum == numGuesses)
            {
                Console.Write("# ");
                for (int i = 0; i < answerCode.Length; i++)
                {
                    Console.Write(answerCode[i]);
                }
                Console.WriteLine("\n####################");
                Console.WriteLine("\nGame Finished, Code Maker Wins!");
                gameFinish = true;
            }
            else if (!gameFinish)
            {
                Console.Write("# ");
                for (int i = 0; i < answerCode.Length; i++)
                {
                    Console.Write("*");
                }
                Console.WriteLine("\n####################");
            }

            turnNum++;

            return gameFinish;
        }

        /// <summary>
        /// Called to update the contents of the turnGuesses and turnAnswers arrays using the code guess and response for the current turn
        /// These are added to the arrays so that all previous' turns guess and responses are saved and can be displayed on the game board
        /// Also checks whether the correct code has been guessed
        /// </summary>
        /// <param name="newGuess"></param>
        /// <param name="newAnswer"></param>
        private void UpdateArrays(CodeColours[] newGuess, AnswerColours[] newAnswer)
        {
            int numBlack = 0;
            for (int i = 0; i < numPegs; i++)
            {
                turnGuesses[i, turnNum - 1] = newGuess[i];
                turnAnswers[i, turnNum - 1] = newAnswer[i];

                if (newAnswer[i] == AnswerColours.B) numBlack++;
            }

            //Checks for black pegs within the response array
            //if the number of black pegs is equal to the number of pegs in the code then the code has been broken
            if (numBlack == numPegs) gameFinish = true;
        }
    }
}
