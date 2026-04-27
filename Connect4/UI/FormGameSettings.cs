using System;
using System.Drawing;
using System.Windows.Forms;
using Ex05.Logic;
using Ex05.Logic.Enums;

namespace Ex05.UI
{
    public class FormGameSettings : Form
    {
        private Label m_LabelPlayers;
        private Label m_LabelPlayer1;
        private Label m_LabelPlayer2;
        private TextBox m_TextBoxPlayer1;
        private TextBox m_TextBoxPlayer2;
        private CheckBox m_CheckBoxPlayer2;

        private Label m_LabelBoardSize;
        private Label m_LabelRows;
        private Label m_LabelCols;
        private NumericUpDown m_NumericRows;
        private NumericUpDown m_NumericCols;

        private Button m_ButtonStart;

        public FormGameSettings()
        {
            initializeComponent();
        }

        private void initializeComponent()
        {
            Text = "Game Settings";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            ClientSize = new Size(320, 220);

            m_LabelPlayers = new Label();
            m_LabelPlayers.Text = "Players:";
            m_LabelPlayers.Location = new Point(10, 15);
            m_LabelPlayers.AutoSize = true;

            m_LabelPlayer1 = new Label();
            m_LabelPlayer1.Text = "Player 1:";
            m_LabelPlayer1.Location = new Point(25, 45);
            m_LabelPlayer1.AutoSize = true;

            m_TextBoxPlayer1 = new TextBox();
            m_TextBoxPlayer1.Location = new Point(100, 42);
            m_TextBoxPlayer1.Size = new Size(200, 20);
            m_TextBoxPlayer1.Text = "Player 1";

            m_CheckBoxPlayer2 = new CheckBox();
            m_CheckBoxPlayer2.Location = new Point(28, 75);
            m_CheckBoxPlayer2.Size = new Size(15, 15);
            m_CheckBoxPlayer2.Checked = false;
            m_CheckBoxPlayer2.CheckedChanged += m_CheckBoxPlayer2_CheckedChanged;

            m_LabelPlayer2 = new Label();
            m_LabelPlayer2.Text = "Player 2:";
            m_LabelPlayer2.Location = new Point(45, 73);
            m_LabelPlayer2.AutoSize = true;

            m_TextBoxPlayer2 = new TextBox();
            m_TextBoxPlayer2.Location = new Point(100, 72);
            m_TextBoxPlayer2.Size = new Size(200, 20);
            m_TextBoxPlayer2.Enabled = false;
            m_TextBoxPlayer2.Text = "[Computer]";

            m_LabelBoardSize = new Label();
            m_LabelBoardSize.Text = "Board Size:";
            m_LabelBoardSize.Location = new Point(10, 110);
            m_LabelBoardSize.AutoSize = true;

            m_LabelRows = new Label();
            m_LabelRows.Text = "Rows:";
            m_LabelRows.Location = new Point(25, 140);
            m_LabelRows.AutoSize = true;

            m_NumericRows = new NumericUpDown();
            m_NumericRows.Location = new Point(75, 138);
            m_NumericRows.Size = new Size(55, 20);
            m_NumericRows.Minimum = 4;
            m_NumericRows.Maximum = 8;
            m_NumericRows.Value = 4;
            m_NumericRows.TextAlign = HorizontalAlignment.Center;

            m_LabelCols = new Label();
            m_LabelCols.Text = "Cols:";
            m_LabelCols.Location = new Point(145, 140);
            m_LabelCols.AutoSize = true;

            m_NumericCols = new NumericUpDown();
            m_NumericCols.Location = new Point(195, 138);
            m_NumericCols.Size = new Size(55, 20);
            m_NumericCols.Minimum = 4;
            m_NumericCols.Maximum = 8;
            m_NumericCols.Value = 4;
            m_NumericCols.TextAlign = HorizontalAlignment.Center;

            m_ButtonStart = new Button();
            m_ButtonStart.Text = "Start!";
            m_ButtonStart.Location = new Point(10, 175);
            m_ButtonStart.Size = new Size(290, 30);
            m_ButtonStart.Click += m_ButtonStart_Click;

            Controls.Add(m_LabelPlayers);
            Controls.Add(m_LabelPlayer1);
            Controls.Add(m_TextBoxPlayer1);
            Controls.Add(m_CheckBoxPlayer2);
            Controls.Add(m_LabelPlayer2);
            Controls.Add(m_TextBoxPlayer2);
            Controls.Add(m_LabelBoardSize);
            Controls.Add(m_LabelRows);
            Controls.Add(m_NumericRows);
            Controls.Add(m_LabelCols);
            Controls.Add(m_NumericCols);
            Controls.Add(m_ButtonStart);
        }

        private void m_CheckBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            if (m_CheckBoxPlayer2.Checked)
            {
                m_TextBoxPlayer2.Enabled = true;

                if (m_TextBoxPlayer2.Text == "[Computer]")
                {
                    m_TextBoxPlayer2.Text = "Player 2";
                    m_TextBoxPlayer2.SelectAll();
                }
            }
            else
            {
                m_TextBoxPlayer2.Enabled = false;
                m_TextBoxPlayer2.Text = "[Computer]";
            }
        }

        private void m_ButtonStart_Click(object sender, EventArgs e)
        {
            ePlayerType player2Type = m_CheckBoxPlayer2.Checked ? ePlayerType.Human : ePlayerType.Computer;

            GameConfig config = new GameConfig(
                (int)m_NumericRows.Value,
                (int)m_NumericCols.Value,
                player2Type);

            string errorMessage;
            if (!config.IsValid(out errorMessage))
            {
                MessageBox.Show(errorMessage, "Invalid Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string player1Name = m_TextBoxPlayer1.Text;
            string player2Name = m_TextBoxPlayer2.Text;

            FormGame gameForm = new FormGame(config, player1Name, player2Name);
            Hide();
            gameForm.ShowDialog();
            Close();
        }
    }
}