using System;

namespace TicTacToe
{
    /// <summary>
    /// class with minimax algorothm implementation
    /// </summary>
    class AI
    {       
        /// <summary>
        /// sets depth of minimax reccurence
        /// </summary>
        internal readonly int depthConstant;      
        /// <summary>
        /// default constructor
        /// </summary>        
        public AI(int depth)
        {          
            depthConstant = depth;
        }
        /// <summary>
        /// evaluates best possible move using minimax algorithm
        /// Board is immutable to avoid writing unwanted data 
        /// to game board array
        /// </summary>
        /// <param name="CurrentBoard">
        /// passed current situation in a game
        /// </param>
        /// <returns>
        /// tuple with chosen field indexes
        /// </returns>
        public Tuple<int, int> MakeMove(in Board CurrentBoard)
        {           
            int bestMoveValue = int.MinValue;
            int rowIndex = 0;
            int colIndex = 0;

            for (int i = 0; i < CurrentBoard.BoardSize; i++)
            {
                for (int j = 0; j < CurrentBoard.BoardSize; j++)
                {
                    if (CurrentBoard.BoardValues[i, j] == FieldValue.Empty)
                    {
                        //use alpha beta for boardsize bigger than 3 only
                        if (CurrentBoard.BoardSize > 4)
                        {
                            //only checks fileds with non empty adjacent fields
                            if (CurrentBoard.HasAdjecentField(i, j))
                            {
                                //make move
                                CurrentBoard.BoardValues[i, j] = FieldValue.Circle;
                                //use minimax to evaluate 
                                int currentMoveValue = MinimaxAlphaBeta(CurrentBoard, 0, false, int.MinValue, int.MaxValue, 0);
                                //undo move
                                CurrentBoard.BoardValues[i, j] = FieldValue.Empty;
                                //if move value if greater than best value switch
                                //and save current move indexes
                                if (currentMoveValue > bestMoveValue)
                                {
                                    rowIndex = i;
                                    colIndex = j;
                                    bestMoveValue = currentMoveValue;
                                }
                            }
                        }
                        //use minimax without alpha beta pruning
                        else
                        {
                            //make move
                            CurrentBoard.BoardValues[i, j] = FieldValue.Circle;
                            //use minimax to evaluate 
                            int currentMoveValue = Minimax(CurrentBoard, 0, false);
                            //undo move
                            CurrentBoard.BoardValues[i, j] = FieldValue.Empty;
                            //if move value if greater than best value switch
                            //and save current move indexes
                            if (currentMoveValue > bestMoveValue)
                            {
                                rowIndex = i;
                                colIndex = j;
                                bestMoveValue = currentMoveValue;
                            }
                        }   
                    }
                }    
            }
            return Tuple.Create(rowIndex, colIndex);           
        }
        /// <summary>
        /// recursively evaluates the best possible move
        /// method cuts tree branches which will not give 
        /// desired value assuming oponent plays optimally
        /// </summary>
        /// <param name="CurrentBoard">
        /// current situtation in game
        ///object is immutable for this method
        /// </param>
        /// <param name="depth">
        /// recurrence depth
        /// </param>
        /// <param name="isMaximizer">
        /// if true method evalates for maxiumum value
        ///if false for minimal
        /// </param>
        /// <param name="alpha">
        /// utility parameter for alpha beta pruning
        /// </param>
        /// <param name="beta">
        /// utility parameter for alpha beta pruning
        /// </param>
        /// <param name="seq">
        /// parameter for finding oponets smaller sequences
        /// than winning sequence in and subtracting from 
        /// best field value if sequence is found
        /// this way ai plays defensive strategy
        /// </param>
        /// <returns>
        /// 0 if there is no possible move
        /// or maxium recurrence depth is exceded
        /// 100 if move results in victory
        /// -100 if move results in defeat
        /// positive value smaller than 100 if move
        /// will result in victory in more than 1 turn
        /// negative value greater than -100 if move
        /// will result in defeat in more than 1 turn
        /// </returns>         
        private int MinimaxAlphaBeta(in Board CurrentBoard, int depth, bool isMaximizer, int alpha, int beta, int seq)
        {

            if (depth == depthConstant)
                return 0;

            //check if move results in victory
            var winner = CurrentBoard.CheckForWinner();
            //return 100 if it does and -100 if move results in defeat
            if (winner.Item1)
                return winner.Item2 == FieldValue.Circle ? 1000 : -1000;

            //check if move results in sequence
            var sequence = CurrentBoard.CheckForSequence();
            //if opponet has 3 sequence subtract from score to start defending
            if (sequence.Item1)
                seq = sequence.Item2 == FieldValue.Circle ? 100 : -100;


            if (CurrentBoard.CheckForFullBoard())
                return 0;

            if (isMaximizer)
            {
                int bestValue = int.MinValue;
                for (int i = 0; i < CurrentBoard.BoardSize; i++)
                {
                    for (int j = 0; j < CurrentBoard.BoardSize; j++)
                    {
                        //only checks fileds with non empty adjacent fields
                        if (CurrentBoard.HasAdjecentField(i, j))
                        {
                            if (CurrentBoard.BoardValues[i, j] == FieldValue.Empty)
                            {
                                CurrentBoard.BoardValues[i, j] = FieldValue.Circle;
                                int currentValue = MinimaxAlphaBeta(CurrentBoard, depth + 1, !isMaximizer, alpha, beta, seq);
                                bestValue = Math.Max(bestValue, currentValue);
                                alpha = Math.Max(alpha, bestValue);
                                CurrentBoard.BoardValues[i, j] = FieldValue.Empty;
                                //alpha beta pruning
                                if (beta <= alpha)
                                    break;
                            }
                        }
                    }
                }
                return (bestValue + depth + seq);
            }

            else
            {
                int bestValue = int.MaxValue;
                for (int i = 0; i < CurrentBoard.BoardSize; i++)
                {
                    for (int j = 0; j < CurrentBoard.BoardSize; j++)
                    {
                        if (CurrentBoard.HasAdjecentField(i, j))
                        {                                                    
                            if (CurrentBoard.BoardValues[i, j] == FieldValue.Empty)
                            {
                                CurrentBoard.BoardValues[i, j] = FieldValue.Cross;
                                int currentValue = MinimaxAlphaBeta(CurrentBoard, depth + 1, !isMaximizer, alpha, beta, seq);
                                bestValue = Math.Min(bestValue, currentValue);
                                beta = Math.Min(beta, bestValue);
                                CurrentBoard.BoardValues[i, j] = FieldValue.Empty;
                                if (beta <= alpha)
                                    break;
                            }
                        }
                    }
                }
                return (bestValue - depth + seq);
            }
        }

        private int Minimax(in Board CurrentBoard, int depth, bool isMaximizer)
        {
            //check if move results in victory
            var winner = CurrentBoard.CheckForWinner();
            //return 10 if it does and -10 if move results in defeat
            if (winner.Item1)
                return winner.Item2 == FieldValue.Circle ? 10 : -10;

            if (CurrentBoard.CheckForFullBoard())
                return 0;

            if (isMaximizer)
            {
                int bestValue = int.MinValue;
                for (int i = 0; i < CurrentBoard.BoardSize; i++)
                {
                    for (int j = 0; j < CurrentBoard.BoardSize; j++)
                    {
                        if (CurrentBoard.BoardValues[i, j] == FieldValue.Empty)
                        {
                            CurrentBoard.BoardValues[i, j] = FieldValue.Circle;
                            bestValue = Math.Max(bestValue, Minimax(CurrentBoard, depth + 1, !isMaximizer));
                            CurrentBoard.BoardValues[i, j] = FieldValue.Empty;
                        }
                    }
                }
                return bestValue;
            }

            else
            {
                int bestValue = int.MaxValue;
                for (int i = 0; i < CurrentBoard.BoardSize; i++)
                {
                    for (int j = 0; j < CurrentBoard.BoardSize; j++)
                    {
                        if (CurrentBoard.BoardValues[i, j] == FieldValue.Empty)
                        {
                            CurrentBoard.BoardValues[i, j] = FieldValue.Cross;
                            bestValue = Math.Min(bestValue, Minimax(CurrentBoard, depth + 1, !isMaximizer));
                            CurrentBoard.BoardValues[i, j] = FieldValue.Empty;
                        }
                    }
                }
                return bestValue;
            }
        }
    }
}
