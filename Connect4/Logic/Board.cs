using Ex05.Logic.Enums;

namespace Ex05.Logic
{
    public class Board
    {
        private readonly int r_Rows;
        private readonly int r_Cols;
        private eToken[,] m_Cells;
        private int[] m_NextEmptyRowPerColumn;
        private int m_FilledCellsCount;

        public Board(int i_Rows, int i_Cols)
        {
            r_Rows = i_Rows;
            r_Cols = i_Cols;

            initializeCells();
            initializeNextEmptyRowArray();
            m_FilledCellsCount = 0;
        }

        public int GetRows()
        {
            return r_Rows;
        }

        public int GetCols()
        {
            return r_Cols;
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
            return m_FilledCellsCount == r_Rows * r_Cols;
        }

        public bool TryDropToken(int i_ColumnIndex, eToken i_Token, out int o_RowIndexPlaced)
        {
            bool isTokenDroppedSuccessfully = false;
            o_RowIndexPlaced = -1;

            if (!IsColumnFull(i_ColumnIndex))
            {
                int rowToPlace = m_NextEmptyRowPerColumn[i_ColumnIndex];

                m_Cells[rowToPlace, i_ColumnIndex] = i_Token;
                o_RowIndexPlaced = rowToPlace;

                m_NextEmptyRowPerColumn[i_ColumnIndex]--;
                m_FilledCellsCount++;

                isTokenDroppedSuccessfully = true;
            }

            return isTokenDroppedSuccessfully;
        }

        private void initializeCells()
        {
            m_Cells = new eToken[r_Rows, r_Cols];

            for (int row = 0; row < r_Rows; row++)
            {
                for (int col = 0; col < r_Cols; col++)
                {
                    m_Cells[row, col] = eToken.Empty;
                }
            }
        }

        private void initializeNextEmptyRowArray()
        {
            m_NextEmptyRowPerColumn = new int[r_Cols];

            for (int col = 0; col < r_Cols; col++)
            {
                m_NextEmptyRowPerColumn[col] = r_Rows - 1;
            }
        }
    }
}