using Ex05.Logic;
using Ex05.Logic.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ex05.UI
{
    public class FormGame : Form
    {
        private const int k_CellSize = 45;
        private const int k_CellGap = 5;
        private const int k_TopMargin = 10;
        private const int k_SideMargin = 10;

        private readonly GameRunner r_GameRunner;
        private readonly GameConfig r_Config;

        private readonly string r_Player1Name;
        private readonly string r_Player2Name;

        private Button[] m_ColumnButtons;
        private Button[,] m_BoardButtons;

        private Label m_LabelScore1;
        private Label m_LabelScore2;

        public FormGame(GameConfig i_Config, string i_Player1Name, string i_Player2Name)
        {
            r_Config = i_Config;
            r_Player1Name = string.IsNullOrEmpty(i_Player1Name) ? "Player 1" : i_Player1Name;
            r_Player2Name = i_Config.Player2Type == ePlayerType.Computer ? "Computer" : (string.IsNullOrEmpty(i_Player2Name) ? "Player 2" : i_Player2Name);

            r_GameRunner = new GameRunner();
            r_GameRunner.StartNewGame(r_Config);

            initializeComponent();
            buildBoardUI();
            refreshBoardUI();
            refreshScoresUI();
        }

        private void initializeComponent()
        {
            Text = "4 in a Row !!";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            int cols = r_Config.Cols;
            int rows = r_Config.Rows;

            int boardWidth = (cols * k_CellSize) + ((cols - 1) * k_CellGap);
            int boardHeight = (rows * k_CellSize) + ((rows - 1) * k_CellGap);

            int topButtonsHeight = k_CellSize;
            int bottomLabelsHeight = 30;

            ClientSize = new Size(
                k_SideMargin * 2 + boardWidth,
                k_TopMargin * 2 + topButtonsHeight + k_CellGap + boardHeight + k_CellGap + bottomLabelsHeight);

            m_LabelScore1 = new Label();
            m_LabelScore1.AutoSize = true;
            m_LabelScore1.Location = new Point(k_SideMargin, ClientSize.Height - bottomLabelsHeight);

            m_LabelScore2 = new Label();
            m_LabelScore2.AutoSize = true;
            m_LabelScore2.Location = new Point(ClientSize.Width / 2, ClientSize.Height - bottomLabelsHeight);

            Controls.Add(m_LabelScore1);
            Controls.Add(m_LabelScore2);
        }

        private void buildBoardUI()
        {
            int cols = r_Config.Cols;
            int rows = r_Config.Rows;

            m_ColumnButtons = new Button[cols];
            m_BoardButtons = new Button[rows, cols];

            int xStart = k_SideMargin;
            int yTopButtons = k_TopMargin;

            for (int col = 0; col < cols; col++)
            {
                Button btn = new Button();
                btn.Size = new Size(k_CellSize, k_CellSize);
                btn.Location = new Point(xStart + col * (k_CellSize + k_CellGap), yTopButtons);
                btn.Text = (col + 1).ToString();
                btn.Tag = col;
                btn.Click += columnButton_Click;

                m_ColumnButtons[col] = btn;
                Controls.Add(btn);
            }

            int yBoardStart = yTopButtons + k_CellSize + k_CellGap;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Button cell = new Button();
                    cell.Size = new Size(k_CellSize, k_CellSize);
                    cell.Location = new Point(
                        xStart + col * (k_CellSize + k_CellGap),
                        yBoardStart + row * (k_CellSize + k_CellGap));
                    cell.Enabled = false;
                    cell.Font = new Font(FontFamily.GenericSansSerif, 18, FontStyle.Bold);
                    cell.TextAlign = ContentAlignment.MiddleCenter;

                    m_BoardButtons[row, col] = cell;
                    Controls.Add(cell);
                }
            }
        }

        private void columnButton_Click(object sender, EventArgs e)
        {
            bool shouldContinue = true;
            Button columnButton = sender as Button;

            if (columnButton == null)
            {
                shouldContinue = false;
            }

            if (shouldContinue)
            {
                int col = (int)columnButton.Tag;
                bool succeeded = r_GameRunner.TryMakeMove(col);

                if (!succeeded)
                {
                    shouldContinue = false;
                }
            }

            if (shouldContinue)
            {
                refreshBoardUI();
                updateColumnButtonsEnabledState();

                if (r_GameRunner.GameState == eGameState.InProgress
                    && r_GameRunner.Player2Type == ePlayerType.Computer
                    && r_GameRunner.CurrentPlayer == ePlayerIndex.Player2)
                {
                    int computerCol = r_GameRunner.GenerateComputerMove();
                    r_GameRunner.TryMakeMove(computerCol);

                    refreshBoardUI();
                    updateColumnButtonsEnabledState();
                }

                handleEndOfRoundIfNeeded();
            }
        }

        private void refreshBoardUI()
        {
            int rows = r_Config.Rows;
            int cols = r_Config.Cols;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    eToken token = r_GameRunner.Board.GetCell(row, col);
                    m_BoardButtons[row, col].Text = tokenToDisplay(token);
                }
            }
        }

        private void updateColumnButtonsEnabledState()
        {
            int cols = r_Config.Cols;

            for (int col = 0; col < cols; col++)
            {
                bool isFull = r_GameRunner.Board.IsColumnFull(col);
                m_ColumnButtons[col].Enabled = !isFull;
            }
        }

        private void refreshScoresUI()
        {
            m_LabelScore1.Text = string.Format("{0}: {1}", r_Player1Name, r_GameRunner.Player1Score);
            m_LabelScore2.Text = string.Format("{0}: {1}", r_Player2Name, r_GameRunner.Player2Score);
        }

        private void handleEndOfRoundIfNeeded()
        {
            if (r_GameRunner.GameState == eGameState.InProgress)
            {
                return;
            }

            string title;
            string message;

            if (r_GameRunner.GameState == eGameState.Won)
            {
                title = "A Win!";
                message = string.Format("{0} Won!!{1}Another Round?", getWinnerName(), Environment.NewLine);
            }
            else if (r_GameRunner.GameState == eGameState.Draw)
            {
                title = "A Tie!";
                message = string.Format("Tie!!{0}Another Round?", Environment.NewLine);
            }
            else
            {
                title = "Round Ended";
                message = string.Format("Another Round?");
            }

            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                r_GameRunner.StartNewRound();
                refreshBoardUI();
                updateColumnButtonsEnabledState();
                refreshScoresUI();
            }
            else
            {
                Close();
            }
        }

        private string getWinnerName()
        {
            string winnerName;

            if (r_GameRunner.LastWinner == ePlayerIndex.Player1)
            {
                winnerName = r_Player1Name;
            }
            else
            {
                winnerName = r_Player2Name;
            }

            return winnerName;
        }

        private string tokenToDisplay(eToken i_Token)
        {
            string display;

            if (i_Token == eToken.Player1)
            {
                display = "X";
            }
            else if (i_Token == eToken.Player2)
            {
                display = "O";
            }
            else
            {
                display = string.Empty;
            }

            return display;
        }
    }
}