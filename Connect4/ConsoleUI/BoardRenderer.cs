using System.Text;
using Ex02.Logic;

namespace Ex02.ConsoleUI
{
    public class BoardRenderer
    {
        private const char k_ColumnSeparatorChar = '|';
        private const char k_RowSeparatorChar = '=';
        private const char k_EmptyCellChar = ' ';

        public string Render(Board i_Board)
        {
            StringBuilder builder = new StringBuilder();

            appendHeaderRow(builder, i_Board.GetCols());

            for (int row = 0; row < i_Board.GetRows(); row++)
            {
                appendBoardRow(builder, i_Board, row);

                if (row < i_Board.GetRows() - 1)
                {
                    appendSeparatorRow(builder, i_Board.GetCols());
                }
            }

            return builder.ToString();
        }

        private void appendHeaderRow(StringBuilder io_Builder, int i_Cols)
        {
            for (int col = 0; col < i_Cols; col++)
            {
                io_Builder.Append("  ");
                io_Builder.Append((char)('A' + col));
                io_Builder.Append(" ");
            }

            io_Builder.AppendLine();
        }

        private void appendBoardRow(StringBuilder io_Builder, Board i_Board, int i_Row)
        {
            for (int col = 0; col < i_Board.GetCols(); col++)
            {
                io_Builder.Append(k_ColumnSeparatorChar);
                io_Builder.Append(" ");
                io_Builder.Append(tokenToChar(i_Board.GetCell(i_Row, col)));
                io_Builder.Append(" ");
            }

            io_Builder.Append(k_ColumnSeparatorChar);
            io_Builder.AppendLine();
        }

        private void appendSeparatorRow(StringBuilder io_Builder, int i_Cols)
        {
            int length = (i_Cols * 4) + 1;

            for (int i = 0; i < length; i++)
            {
                io_Builder.Append(k_RowSeparatorChar);
            }

            io_Builder.AppendLine();
        }

        private char tokenToChar(eToken i_Token)
        {
            char result;

            switch (i_Token)
            {
                case eToken.Player1:
                    result = 'X';
                    break;
                case eToken.Player2:
                    result = 'O';
                    break;
                default:
                    result = k_EmptyCellChar;
                    break;
            }

            return result;
        }
    }
}