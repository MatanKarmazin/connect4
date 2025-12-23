using System;
using Ex02.Logic;

namespace Ex02.ConsoleUI
{
    public class ConsoleGameRunner
    {
        private const string k_Msg_EnterRows = "Please enter the number of rows (4-8):";
        private const string k_Msg_EnterCols = "Please enter the number of columns (4-8):";
        private const string k_Msg_InvalidNumber = "Invalid number. Please try again.";
        private const string k_Msg_InvalidBoardSize = "Invalid board size. Allowed range is 4 to 8.";
        private const string k_Msg_ChooseGameType = "Choose game type: 1 - Player vs Player, 2 - Player vs Computer";
        private const string k_Msg_InvalidChoice = "Invalid choice. Please try again.";
        private const string k_Msg_PlayerTurn = "Player {0}, choose a column (A-{1}) or Q to quit:";
        private const string k_Msg_ComputerTurn = "Computer is making a move...";
        private const string k_Msg_InvalidInput = "Invalid input. Please try again.";
        private const string k_Msg_ColumnFull = "This column is full. Please choose another column.";
        private const string k_Msg_PlayerQuit = "Player {0} quit. The other player gets a point.";
        private const string k_Msg_PlayerWon = "Player {0} won this round!";
        private const string k_Msg_Draw = "The round ended in a draw.";
        private const string k_Msg_Score = "Score: Player 1 = {0}, Player 2 = {1}";
        private const string k_Msg_PlayAnotherRound = "Play another round? (Y/N)";
        private const string k_Msg_Thanks = "Thanks for playing!";
        private const string k_Input_Quit = "Q";
        private const string k_Input_Yes = "Y";
        private const string k_Input_No = "N";

        private readonly GameRunner m_GameRunner;
        private readonly BoardRenderer m_BoardRenderer;

        public ConsoleGameRunner()
        {
            m_GameRunner = new GameRunner();
            m_BoardRenderer = new BoardRenderer();
        }

        public void Run()
        {
            runGameLoop();
        }

        private void runGameLoop()
        {
            GameConfig gameConfig;
            bool playAnotherRound = true;

            setupNewGame(out gameConfig);
            m_GameRunner.StartNewGame(gameConfig);

            while (playAnotherRound)
            {
                m_GameRunner.StartNewRound();
                runSingleRound();
                printScores();
                playAnotherRound = askForAnotherRound();
            }

            Console.Clear();
            Console.WriteLine(k_Msg_Thanks);
        }

        private void setupNewGame(out GameConfig o_GameConfig)
        {
            int rows;
            int cols;
            ePlayerType player2Type;
            string errorMessage;
            bool isValidConfig;

            readBoardSize(out rows, out cols);
            readGameType(out player2Type);

            o_GameConfig = new GameConfig(rows, cols, player2Type);
            isValidConfig = o_GameConfig.IsValid(out errorMessage);

            while (!isValidConfig)
            {
                Console.WriteLine(errorMessage);

                readBoardSize(out rows, out cols);
                readGameType(out player2Type);

                o_GameConfig = new GameConfig(rows, cols, player2Type);
                isValidConfig = o_GameConfig.IsValid(out errorMessage);
            }
        }

        private void runSingleRound()
        {
            while (m_GameRunner.GetGameState() == eGameState.InProgress)
            {
                printBoard();

                if (isComputerTurn())
                {
                    handleComputerTurn();
                }
                else
                {
                    handleHumanTurn();
                }
            }

            printBoard();
            printRoundEndMessage();
        }

        private bool isComputerTurn()
        {
            bool isComputer = false;

            if (m_GameRunner.GetPlayer2Type() == ePlayerType.Computer &&
                m_GameRunner.GetCurrentPlayer() == ePlayerIndex.Player2)
            {
                isComputer = true;
            }

            return isComputer;
        }

        private void handleComputerTurn()
        {
            int colIndex = m_GameRunner.GenerateComputerMove();

            Console.WriteLine(k_Msg_ComputerTurn);
            m_GameRunner.TryMakeMove(colIndex);
        }

        private void handleHumanTurn()
        {
            bool isQuit;
            int colIndex;
            bool moveSucceeded;

            readTurnInput(out isQuit, out colIndex);

            if (isQuit)
            {
                m_GameRunner.QuitCurrentRound();
            }
            else
            {
                moveSucceeded = m_GameRunner.TryMakeMove(colIndex);

                while (!moveSucceeded)
                {
                    Console.WriteLine(k_Msg_ColumnFull);
                    readTurnInput(out isQuit, out colIndex);

                    if (isQuit)
                    {
                        m_GameRunner.QuitCurrentRound();
                        moveSucceeded = true;
                    }
                    else
                    {
                        moveSucceeded = m_GameRunner.TryMakeMove(colIndex);
                    }
                }
            }
        }

        private void printBoard()
        {
            Console.Clear();
            Console.WriteLine(m_BoardRenderer.Render(m_GameRunner.GetBoard()));
        }

        private void readBoardSize(out int o_Rows, out int o_Cols)
        {
            o_Rows = readIntInRange(k_Msg_EnterRows, 4, 8);
            o_Cols = readIntInRange(k_Msg_EnterCols, 4, 8);
        }

        private int readIntInRange(string i_Prompt, int i_Min, int i_Max)
        {
            int value;
            bool parsed;
            string input;

            Console.WriteLine(i_Prompt);
            input = Console.ReadLine();
            parsed = int.TryParse(input, out value);

            while (!parsed || value < i_Min || value > i_Max)
            {
                Console.WriteLine(!parsed ? k_Msg_InvalidNumber : k_Msg_InvalidBoardSize);
                Console.WriteLine(i_Prompt);

                input = Console.ReadLine();
                parsed = int.TryParse(input, out value);
            }

            return value;
        }

        private void readGameType(out ePlayerType o_Player2Type)
        {
            int choice;
            bool parsed;
            string input;

            Console.WriteLine(k_Msg_ChooseGameType);
            input = Console.ReadLine();
            parsed = int.TryParse(input, out choice);

            while (!parsed || (choice != 1 && choice != 2))
            {
                Console.WriteLine(k_Msg_InvalidChoice);
                Console.WriteLine(k_Msg_ChooseGameType);

                input = Console.ReadLine();
                parsed = int.TryParse(input, out choice);
            }

            o_Player2Type = (choice == 1) ? ePlayerType.Human : ePlayerType.Computer;
        }

        private void readTurnInput(out bool o_IsQuit, out int o_ColumnIndex)
        {
            string input;
            bool isValidColumn;
            int cols = m_GameRunner.GetBoard().GetCols();
            char maxColLetter = (char)('A' + cols - 1);

            o_IsQuit = false;
            o_ColumnIndex = -1;

            Console.WriteLine(string.Format(k_Msg_PlayerTurn, getCurrentPlayerNumber(), maxColLetter));
            input = Console.ReadLine();

            input = (input == null) ? string.Empty : input.Trim().ToUpper();

            if (isQuitCommand(input))
            {
                o_IsQuit = true;
            }
            else
            {
                isValidColumn = tryParseColumn(input, cols, out o_ColumnIndex);

                while (!isValidColumn)
                {
                    Console.WriteLine(k_Msg_InvalidInput);
                    Console.WriteLine(string.Format(k_Msg_PlayerTurn, getCurrentPlayerNumber(), maxColLetter));

                    input = Console.ReadLine();
                    input = (input == null) ? string.Empty : input.Trim().ToUpper();

                    if (isQuitCommand(input))
                    {
                        o_IsQuit = true;
                        isValidColumn = true;
                    }
                    else
                    {
                        isValidColumn = tryParseColumn(input, cols, out o_ColumnIndex);
                    }
                }
            }
        }

        private bool tryParseColumn(string i_Input, int i_MaxCols, out int o_ColumnIndex)
        {
            bool isValid = false;
            o_ColumnIndex = -1;

            if (!string.IsNullOrEmpty(i_Input) && i_Input.Length == 1)
            {
                char c = i_Input[0];

                if (c >= 'A' && c <= ('A' + i_MaxCols - 1))
                {
                    o_ColumnIndex = c - 'A';
                    isValid = true;
                }
            }

            return isValid;
        }

        private bool isQuitCommand(string i_Input)
        {
            return i_Input == k_Input_Quit;
        }

        private int getCurrentPlayerNumber()
        {
            return (m_GameRunner.GetCurrentPlayer() == ePlayerIndex.Player1) ? 1 : 2;
        }

        private void printRoundEndMessage()
        {
            eGameState state = m_GameRunner.GetGameState();

            if (state == eGameState.Quit)
            {
                int playerWhoQuit = getOtherPlayerNumber(m_GameRunner.GetLastWinner());
                Console.WriteLine(string.Format(k_Msg_PlayerQuit, playerWhoQuit));
            }
            else if (state == eGameState.Won)
            {
                int winner = getPlayerNumber(m_GameRunner.GetLastWinner());
                Console.WriteLine(string.Format(k_Msg_PlayerWon, winner));
            }
            else if (state == eGameState.Draw)
            {
                Console.WriteLine(k_Msg_Draw);
            }
        }

        private int getPlayerNumber(ePlayerIndex i_Player)
        {
            return (i_Player == ePlayerIndex.Player1) ? 1 : 2;
        }

        private int getOtherPlayerNumber(ePlayerIndex i_Winner)
        {
            return (i_Winner == ePlayerIndex.Player1) ? 2 : 1;
        }

        private void printScores()
        {
            Console.WriteLine(string.Format(k_Msg_Score, m_GameRunner.GetPlayer1Score(), m_GameRunner.GetPlayer2Score()));
        }

        private bool askForAnotherRound()
        {
            bool playAgain = false;
            bool gotValidInput = false;
            string input;

            Console.WriteLine(k_Msg_PlayAnotherRound);
            input = Console.ReadLine();
            input = (input == null) ? string.Empty : input.Trim().ToUpper();

            while (!gotValidInput)
            {
                if (input == k_Input_Yes)
                {
                    playAgain = true;
                    gotValidInput = true;
                }
                else if (input == k_Input_No)
                {
                    playAgain = false;
                    gotValidInput = true;
                }
                else
                {
                    Console.WriteLine(k_Msg_InvalidInput);
                    Console.WriteLine(k_Msg_PlayAnotherRound);

                    input = Console.ReadLine();
                    input = (input == null) ? string.Empty : input.Trim().ToUpper();
                }
            }

            return playAgain;
        }
    }
}