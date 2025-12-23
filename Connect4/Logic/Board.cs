namespace Ex02.Logic
{
    public class Board
    {
        private readonly int m_Rows;
        private readonly int m_Cols;
        private eToken[,] m_Cells;
        private int[] m_NextEmptyRowPerColumn;
        private int m_FilledCellsCount;

        public Board(int i_Rows, int i_Cols)
        {
            m_Rows = i_Rows;
            m_Cols = i_Cols;

            initializeCells();
            initializeNextEmptyRowArray();
            m_FilledCellsCount = 0;
        }

        public int GetRows()
        {
            return m_Rows;
        }

        public int GetCols()
        {
            return m_Cols;
        }

        public eToken GetCell(int i_RowIndex, int i_ColIndex)
        {
            return m_Cells[i_RowIndex, i_ColIndex];
        }

        public void Clear()
        {
            initializeCells();
            initializeNextEmptyRowArray();

            m_FilledCellsCount = 0;
        }

        public bool IsColumnFull(int i_ColumnIndex)
        {
            return m_NextEmptyRowPerColumn[i_ColumnIndex] < 0;
        }

        public bool IsBoardFull()
        {
            return m_FilledCellsCount == m_Rows * m_Cols;
        }

        public bool TryDropToken(int i_ColumnIndex, eToken i_Token, out int o_RowIndexPlaced)
        {
            o_RowIndexPlaced = -1;

            if (IsColumnFull(i_ColumnIndex))
            {
                return false;
            }

            int rowToPlace = m_NextEmptyRowPerColumn[i_ColumnIndex];

            m_Cells[rowToPlace, i_ColumnIndex] = i_Token;
            o_RowIndexPlaced = rowToPlace;

            m_NextEmptyRowPerColumn[i_ColumnIndex]--;
            m_FilledCellsCount++;

            return true;
        }

        private void initializeCells()
        {
            m_Cells = new eToken[m_Rows, m_Cols];

            for (int row = 0; row < m_Rows; row++)
            {
                for (int col = 0; col < m_Cols; col++)
                {
                    m_Cells[row, col] = eToken.Empty;
                }
            }
        }

        private void initializeNextEmptyRowArray()
        {
            m_NextEmptyRowPerColumn = new int[m_Cols];

            for (int col = 0; col < m_Cols; col++)
            {
                m_NextEmptyRowPerColumn[col] = m_Rows - 1;
            }
        }
    }
}