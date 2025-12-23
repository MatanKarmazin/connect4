namespace Ex02.Logic
{
    public class GameConfig
    {
        private const int k_MinBoardSize = 4;
        private const int k_MaxBoardSize = 8;
        private int m_Rows;
        private int m_Cols;
        private ePlayerType m_Player2Type;

        public GameConfig()
        {
            m_Rows = k_MinBoardSize;
            m_Cols = k_MinBoardSize;
            m_Player2Type = ePlayerType.Human;
        }

        public GameConfig(int i_Rows, int i_Cols, ePlayerType i_Player2Type)
        {
            m_Rows = i_Rows;
            m_Cols = i_Cols;
            m_Player2Type = i_Player2Type;
        }

        public int GetRows()
        {
            return m_Rows;
        }

        public int GetCols()
        {
            return m_Cols;
        }

        public ePlayerType GetPlayer2Type()
        {
            return m_Player2Type;
        }

        public bool IsValid(out string o_ErrorMessage)
        {
            bool isValid = true;
            string errorMessage = string.Empty;

            if (m_Rows < k_MinBoardSize || m_Rows > k_MaxBoardSize)
            {
                isValid = false;
                errorMessage = "Invalid number of rows. Allowed range is 4 to 8.";
            }
            else if (m_Cols < k_MinBoardSize || m_Cols > k_MaxBoardSize)
            {
                isValid = false;
                errorMessage = "Invalid number of columns. Allowed range is 4 to 8.";
            }

            o_ErrorMessage = errorMessage;

            return isValid;
        }
    }
}