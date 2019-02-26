using System;

namespace TicTacToe
{
    public class Board
    {
        /// <summary>
        /// stores current board size
        /// </summary>
        private readonly int boardSize;
        /// <summary>
        /// property for boardSize
        /// </summary>
        public int BoardSize => boardSize;
        /// <summary>
        /// array for storing values on the game field
        /// </summary>
        private FieldValue[,] boardValues;
        /// <summary>
        /// property for boardValues array
        /// </summary>
        public FieldValue[,] BoardValues { get => boardValues; set => boardValues = value; }
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="size"></param>
        public Board(int size)
        {
            boardSize = size;
            //new blank array of field values
            BoardValues = new FieldValue[size, size];
            ClearBoard();           
        }
        /// <summary>
        /// clear game board
        /// </summary>
        public void ClearBoard()
        {
            for (int row = 0; row < BoardValues.GetLength(0); row++)
            {
                for (int col = 0; col < BoardValues.GetLength(1); col++)
                {
                    BoardValues[row, col] = FieldValue.Empty;
                }
            }
        }
        /// <summary>
        /// methods checking for adjacent field values
        /// also checks the indexing to avoid out of range indexing
        /// </summary>
        /// <param name="rowIndex"> 
        /// row index of checked field
        /// </param>
        /// <param name="colIndex">
        /// column index of checked field 
        /// </param>
        /// <returns>
        /// returns type of value adjecent to passed field
        /// right, left, top, bottom or with one common vertex
        /// </returns>
        private FieldValue GetLeft(int rowIndex, int colIndex) =>
            (rowIndex > 0) ? BoardValues[rowIndex - 1, colIndex] : FieldValue.Empty;
        private FieldValue GetRight(int rowIndex, int colIndex) =>
            (rowIndex < (BoardSize - 1)) ? BoardValues[rowIndex + 1, colIndex] : FieldValue.Empty;
        private FieldValue GetTop(int rowIndex, int colIndex) =>
            (colIndex < (BoardSize - 1)) ? BoardValues[rowIndex, colIndex + 1] : FieldValue.Empty;
        private FieldValue GetBottom(int rowIndex, int colIndex) =>
            (colIndex > 0) ? BoardValues[rowIndex, colIndex - 1] : FieldValue.Empty;
        private FieldValue GetTopRight(int rowIndex, int colIndex) =>
            (rowIndex < (BoardSize - 1) && colIndex < (BoardSize - 1)) ? BoardValues[rowIndex + 1, colIndex + 1] : FieldValue.Empty;
        private FieldValue GetTopLeft(int rowIndex, int colIndex) =>
            (rowIndex > 0 && colIndex < (BoardSize - 1)) ? BoardValues[rowIndex - 1, colIndex + 1] : FieldValue.Empty;
        private FieldValue GetBottomRight(int rowIndex, int colIndex) =>
            (rowIndex < (BoardSize - 1) && colIndex > 0) ? BoardValues[rowIndex + 1, colIndex - 1] : FieldValue.Empty;
        private FieldValue GetBottomLeft(int rowIndex, int colIndex) =>
            (rowIndex > 0 && colIndex > 0) ? BoardValues[rowIndex - 1, colIndex - 1] : FieldValue.Empty;
        private FieldValue GetSecondLeft(int rowIndex, int colIndex) =>
            (rowIndex > 1) ? BoardValues[rowIndex - 2, colIndex] : FieldValue.Empty;
        private FieldValue GetSecondRight(int rowIndex, int colIndex) =>
            (rowIndex < (BoardSize - 2)) ? BoardValues[rowIndex + 2, colIndex] : FieldValue.Empty;
        private FieldValue GetSecondTop(int rowIndex, int colIndex) =>
            (colIndex < (BoardSize - 2)) ? BoardValues[rowIndex, colIndex + 2] : FieldValue.Empty;
        private FieldValue GetSecondBottom(int rowIndex, int colIndex) =>
            (colIndex > 1) ? BoardValues[rowIndex, colIndex - 2] : FieldValue.Empty;
        private FieldValue GetSecondTopRight(int rowIndex, int colIndex) =>
            (rowIndex < (BoardSize - 2) && colIndex < (BoardSize - 2)) ? BoardValues[rowIndex + 2, colIndex + 2] : FieldValue.Empty;
        private FieldValue GetSecondTopLeft(int rowIndex, int colIndex) =>
            (rowIndex > 1 && colIndex < (BoardSize - 2)) ? BoardValues[rowIndex - 2, colIndex + 2] : FieldValue.Empty;
        private FieldValue GetSecondBottomRight(int rowIndex, int colIndex) =>
            (rowIndex < (BoardSize - 2) && colIndex > 1) ? BoardValues[rowIndex + 2, colIndex - 2] : FieldValue.Empty;
        private FieldValue GetSecondBottomLeft(int rowIndex, int colIndex) =>
            (rowIndex > 1 && colIndex > 1) ? BoardValues[rowIndex - 2, colIndex - 2] : FieldValue.Empty;
        /// <summary>
        /// searches for sequence of three fields 
        /// with the same value
        /// </summary>
        /// <returns>
        /// true if sequence is found and Field Value of 
        /// this sequence Cross or Circle
        /// if sequence not found returns false and Empty
        /// </returns>
        private Tuple<bool,FieldValue> CheckForThreeSequence()
        {
            for (int row = 0; row < BoardValues.GetLength(0); row++)
            {
                for (int col = 0; col < BoardValues.GetLength(1); col++)
                {
                    //eveluate for cross sequence
                    if (BoardValues[row, col] == FieldValue.Cross)
                    {
                        if (GetRight(row, col) == FieldValue.Cross && GetLeft(row, col) == FieldValue.Cross)
                            return Tuple.Create(true,FieldValue.Cross);
                        else if (GetTop(row, col) == FieldValue.Cross && GetBottom(row, col) == FieldValue.Cross)
                            return Tuple.Create(true, FieldValue.Cross);
                        else if (GetTopRight(row, col) == FieldValue.Cross && GetBottomLeft(row, col) == FieldValue.Cross)
                            return Tuple.Create(true, FieldValue.Cross);
                        else if (GetTopLeft(row, col) == FieldValue.Cross && GetBottomRight(row, col) == FieldValue.Cross)
                            return Tuple.Create(true, FieldValue.Cross);
                    }
                    //evaluate for circle sequence
                    else if (BoardValues[row, col] == FieldValue.Circle)
                    {
                        if (GetRight(row, col) == FieldValue.Circle && GetLeft(row, col) == FieldValue.Circle)
                            return Tuple.Create(true, FieldValue.Circle);
                        else if (GetTop(row, col) == FieldValue.Circle && GetBottom(row, col) == FieldValue.Circle)
                            return Tuple.Create(true, FieldValue.Circle);
                        else if (GetTopRight(row, col) == FieldValue.Circle && GetBottomLeft(row, col) == FieldValue.Circle)
                            return Tuple.Create(true, FieldValue.Circle);
                        else if (GetTopLeft(row, col) == FieldValue.Circle && GetBottomRight(row, col) == FieldValue.Circle)
                            return Tuple.Create(true, FieldValue.Circle);
                    }
                    //do nothing if field is empty
                }
            }
            //if no winner found
            return Tuple.Create(false, FieldValue.Empty);
        }
        /// <summary>
        /// searches for sequence of five fields 
        /// with the same value
        /// </summary>
        /// <returns>
        /// true if sequence is found and Field Value of 
        /// this sequence Cross or Circle
        /// if sequence not found returns false and Empty
        /// </returns>
        private Tuple<bool,FieldValue> CheckForFiveSequence()
        {
            for (int row = 0; row < BoardValues.GetLength(0); row++)
            {
                for (int col = 0; col < BoardValues.GetLength(1); col++)
                {
                    //eveluate for cross sequence
                    if (BoardValues[row, col] == FieldValue.Cross)
                    {
                        if (GetRight(row, col) == FieldValue.Cross && GetLeft(row, col) == FieldValue.Cross
                            && GetSecondRight(row, col) == FieldValue.Cross && GetSecondLeft(row, col) == FieldValue.Cross)
                            return Tuple.Create(true, FieldValue.Cross);
                        else if (GetTop(row, col) == FieldValue.Cross && GetBottom(row, col) == FieldValue.Cross
                            && GetSecondTop(row, col) == FieldValue.Cross && GetSecondBottom(row, col) == FieldValue.Cross)
                            return Tuple.Create(true, FieldValue.Cross);
                        else if (GetTopRight(row, col) == FieldValue.Cross && GetBottomLeft(row, col) == FieldValue.Cross
                            && GetSecondTopRight(row, col) == FieldValue.Cross && GetSecondBottomLeft(row, col) == FieldValue.Cross)
                            return Tuple.Create(true, FieldValue.Cross);
                        else if (GetTopLeft(row, col) == FieldValue.Cross && GetBottomRight(row, col) == FieldValue.Cross
                            && GetSecondTopLeft(row, col) == FieldValue.Cross && GetSecondBottomRight(row, col) == FieldValue.Cross)
                            return Tuple.Create(true, FieldValue.Cross);
                    }
                    //evaluate for circle sequence
                    else if (BoardValues[row, col] == FieldValue.Circle)
                    {
                        if (GetRight(row, col) == FieldValue.Circle && GetLeft(row, col) == FieldValue.Circle
                            && GetSecondRight(row, col) == FieldValue.Circle && GetSecondLeft(row, col) == FieldValue.Circle)
                            return Tuple.Create(true, FieldValue.Circle);
                        else if (GetTop(row, col) == FieldValue.Circle && GetBottom(row, col) == FieldValue.Circle
                            && GetSecondTop(row, col) == FieldValue.Circle && GetSecondBottom(row, col) == FieldValue.Circle)
                            return Tuple.Create(true, FieldValue.Circle);
                        else if (GetTopRight(row, col) == FieldValue.Circle && GetBottomLeft(row, col) == FieldValue.Circle
                            && GetSecondTopRight(row, col) == FieldValue.Circle && GetSecondBottomLeft(row, col) == FieldValue.Circle)
                            return Tuple.Create(true, FieldValue.Circle);
                        else if (GetTopLeft(row, col) == FieldValue.Circle && GetBottomRight(row, col) == FieldValue.Circle
                            && GetSecondTopLeft(row, col) == FieldValue.Circle && GetSecondBottomRight(row, col) == FieldValue.Circle)
                            return Tuple.Create(true, FieldValue.Circle);
                    }
                    //do nothing if field is empty
                }
            }
            //if no winner found
            return Tuple.Create(false,FieldValue.Empty);
        }
        /// <summary>
        /// check for a wining sequence
        /// if board is bigger than 5x5 
        /// 5 elemtns are winning sequence
        /// if it's smaller 3 elements are winning sequence
        /// </summary>
        /// <returns>
        /// true if winnig sequence is found
        /// false if it's not found
        /// </returns>        
        public Tuple<bool,FieldValue> CheckForWinner()
        {
            if (BoardSize >= 5)
            {
                var result = CheckForFiveSequence();
                return result.Item1 ? Tuple.Create(true, result.Item2) : Tuple.Create(false, FieldValue.Empty);
            }
            else
            {
                var result = CheckForThreeSequence();
                return result.Item1 ? Tuple.Create(true, result.Item2) : Tuple.Create(false, FieldValue.Empty);
            }                        
        }
        /// <summary>
        /// public method checking for three sequence
        /// </summary>
        /// <returns>
        /// true if sequence found and field value
        /// of this sequence
        /// false and Empty if sequence isn't found
        /// </returns>
        public Tuple<bool,FieldValue> CheckForSequence()
        {
            var result = CheckForThreeSequence();
            return result.Item1 ? Tuple.Create(true, result.Item2) : Tuple.Create(false, FieldValue.Empty);
        }
        /// <summary>
        /// checks if passed field has any non empty adjecent field
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public bool HasAdjecentField(int rowIndex, int colIndex)
        {
            if (GetRight(rowIndex, colIndex) == FieldValue.Empty && GetLeft(rowIndex, colIndex) == FieldValue.Empty
                && GetTop(rowIndex, colIndex) == FieldValue.Empty && GetBottom(rowIndex, colIndex) == FieldValue.Empty
                && GetTopRight(rowIndex, colIndex) == FieldValue.Empty && GetTopLeft(rowIndex, colIndex) == FieldValue.Empty
                && GetBottomRight(rowIndex, colIndex) == FieldValue.Empty && GetBottomLeft(rowIndex, colIndex) == FieldValue.Empty)
            {
                return false;
            }

            else
            {
                return true;
            }
        }
        /// <summary>
        /// checks for full board
        /// </summary>
        /// <returns>
        /// true if board is full
        /// flase if board's not full
        /// </returns>
        public bool CheckForFullBoard()
        {
            for (int row = 0; row < BoardValues.GetLength(0); row++)
            {
                for (int col = 0; col < BoardValues.GetLength(1); col++)
                {
                    if (BoardValues[row, col] == FieldValue.Empty)
                        return false;
                }
            }
            return true;
        }
    }
}
