using System;

namespace Ex02.Logic
{
    public class GameRunner
    {
        private const int k_WinSequenceLength = 4;

        private readonly Random m_Random;

        private GameConfig m_GameConfig;
        private Board m_Board;

        private ePlayerIndex m_CurrentPlayer;
        private int m_Player1Score;
        private int m_Player2Score;

        private eGameState m_GameState;
        private ePlayerIndex m_LastWinner;

        public GameRunner()
        {
            m_Random = new Random();
            m_CurrentPlayer = ePlayerIndex.Player1;
            m_GameState = eGameState.InProgress;
            m_LastWinner = ePlayerIndex.Player1;
        }

        public void StartNewGame(GameConfig i_GameConfig)
        {
            m_GameConfig = i_GameConfig;
            m_Board = new Board(i_GameConfig.GetRows(), i_GameConfig.GetCols());

            m_Player1Score = 0;
            m_Player2Score = 0;

            StartNewRound();
        }

        public void StartNewRound()
        {
            m_Board.Clear();
            m_GameState = eGameState.InProgress;
            m_CurrentPlayer = ePlayerIndex.Player1;
        }

        public Board GetBoard()
        {
            return m_Board;
        }

        public ePlayerIndex GetCurrentPlayer()
        {
            return m_CurrentPlayer;
        }

        public ePlayerType GetPlayer2Type()
        {
            return m_GameConfig.GetPlayer2Type();
        }

        public int GetPlayer1Score()
        {
            return m_Player1Score;
        }

        public int GetPlayer2Score()
        {
            return m_Player2Score;
        }

        public eGameState GetGameState()
        {
            return m_GameState;
        }

        public ePlayerIndex GetLastWinner()
        {
            return m_LastWinner;
        }

        public bool TryMakeMove(int i_ColumnIndex)
        {
            bool moveSucceeded = false;

            if (m_GameState == eGameState.InProgress)
            {
                eToken token = getTokenByPlayer(m_CurrentPlayer);
                int rowPlaced;

                if (m_Board.TryDropToken(i_ColumnIndex, token, out rowPlaced))
                {
                    moveSucceeded = true;

                    evaluateGameStateAfterMove(rowPlaced, i_ColumnIndex, token);

                    if (m_GameState == eGameState.InProgress)
                    {
                        advanceTurn();
                    }
                }
            }

            return moveSucceeded;
        }

        public void QuitCurrentRound()
        {
            if (m_GameState == eGameState.InProgress)
            {
                ePlayerIndex otherPlayer = getOtherPlayer(m_CurrentPlayer);

                awardPointToPlayer(otherPlayer);
                m_LastWinner = otherPlayer;
                m_GameState = eGameState.Quit;
            }
        }

        public int GenerateComputerMove()
        {
            int chosenColumn = 0;
            int cols = m_Board.GetCols();
            int[] availableColumns = new int[cols];
            int availableCount = 0;

            for (int col = 0; col < cols; col++)
            {
                if (!m_Board.IsColumnFull(col))
                {
                    availableColumns[availableCount] = col;
                    availableCount++;
                }
            }

            if (availableCount > 0)
            {
                chosenColumn = availableColumns[m_Random.Next(availableCount)];
            }

            return chosenColumn;
        }

        private void advanceTurn()
        {
            m_CurrentPlayer = getOtherPlayer(m_CurrentPlayer);
        }

        private ePlayerIndex getOtherPlayer(ePlayerIndex i_Player)
        {
            return (ePlayerIndex)(1 - (int)i_Player);
        }

        private eToken getTokenByPlayer(ePlayerIndex i_Player)
        {
            return (i_Player == ePlayerIndex.Player1) ? eToken.Player1 : eToken.Player2;
        }

        private void evaluateGameStateAfterMove(int i_Row, int i_Col, eToken i_Token)
        {
            if (checkWinFromCell(i_Row, i_Col, i_Token))
            {
                m_GameState = eGameState.Won;
                m_LastWinner = m_CurrentPlayer;

                awardPointToPlayer(m_CurrentPlayer);
            }
            else if (m_Board.IsBoardFull())
            {
                m_GameState = eGameState.Draw;
            }
        }

        private bool checkWinFromCell(int i_Row, int i_Col, eToken i_Token)
        {
            bool isWin = false;

            if (!isWin)
            {
                isWin = countInLine(i_Row, i_Col, 0, 1, i_Token) >= k_WinSequenceLength;
            }

            if (!isWin)
            {
                isWin = countInLine(i_Row, i_Col, 1, 0, i_Token) >= k_WinSequenceLength;
            }

            if (!isWin)
            {
                isWin = countInLine(i_Row, i_Col, 1, 1, i_Token) >= k_WinSequenceLength;
            }

            if (!isWin)
            {
                isWin = countInLine(i_Row, i_Col, 1, -1, i_Token) >= k_WinSequenceLength;
            }

            return isWin;
        }

        private int countInLine(int i_Row, int i_Col, int i_DeltaRow, int i_DeltaCol, eToken i_Token)
        {
            int count = 1;

            count += countDirection(i_Row, i_Col, i_DeltaRow, i_DeltaCol, i_Token);
            count += countDirection(i_Row, i_Col, -i_DeltaRow, -i_DeltaCol, i_Token);

            return count;
        }

        private int countDirection(int i_Row, int i_Col, int i_DeltaRow, int i_DeltaCol, eToken i_Token)
        {
            int count = 0;
            int row = i_Row + i_DeltaRow;
            int col = i_Col + i_DeltaCol;

            while (isWithinBounds(row, col) && m_Board.GetCell(row, col) == i_Token)
            {
                count++;
                row += i_DeltaRow;
                col += i_DeltaCol;
            }

            return count;
        }

        private bool isWithinBounds(int i_Row, int i_Col)
        {
            return i_Row >= 0 &&
                   i_Row < m_Board.GetRows() &&
                   i_Col >= 0 &&
                   i_Col < m_Board.GetCols();
        }

        private void awardPointToPlayer(ePlayerIndex i_Player)
        {
            if (i_Player == ePlayerIndex.Player1)
            {
                m_Player1Score++;
            }
            else
            {
                m_Player2Score++;
            }
        }
    }
}