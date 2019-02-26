using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Math;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        /// <summary>
        /// dynamicly created window grid
        /// </summary>
        internal Grid GameGrid;
        /// <summary>
        /// instance of AI Player
        /// </summary>
        private AI AIPlayer;       
        /// <summary>
        /// true if game has finished
        /// </summary>
        internal bool GameEnded;
        /// <summary>
        /// property for game board
        /// </summary>
        private Board gameBoard;
        public Board GameBoard { get => gameBoard; set => gameBoard = value; }     
        /// <summary>
        /// dictionaries for storing row and column definitions
        /// used to acces object by index not by it's name
        /// </summary>
        internal Dictionary<ColumnDefinition, int> columns;
        internal Dictionary<RowDefinition, int> rows;
        /// <summary>
        /// dictionary for buttons representing 2D grid
        /// formula to get grid element
        /// row + size*column
        /// </summary>
        internal Dictionary<Button, int> buttons;
        /// <summary>
        /// Constructor for game window 
        /// </summary>
        /// <param name="size">
        /// parameter sets board size in pixels 
        /// and the number of button generated
        /// </param>
        public GameWindow(int size, int difficulty)
        {           
            //setting window size
            Height = 100 * size;
            Width = 100 * size;
            Title = size + "x" + size;
            
            GameGrid = new Grid();
            //dictionaries storing buttons, column and row definitions
            //to get acess to elements by index instead of name
            columns = new Dictionary<ColumnDefinition, int>();
            rows = new Dictionary<RowDefinition, int>();
            buttons = new Dictionary<Button, int>();
            //adding row and column definitions to grid
            for(int i = 0; i < size; i++)
            {
                columns.Add(new ColumnDefinition(), i);
                rows.Add(new RowDefinition(), i);
                GameGrid.ColumnDefinitions.Add(columns.Keys.ElementAt(i));
                GameGrid.RowDefinitions.Add(rows.Keys.ElementAt(i));
            }          
            //adding buttons 
            for (int j = 0; j < Pow(size,2); j++)
            {
                buttons.Add(new Button(),j);
                buttons.Keys.ElementAt(j).Click += GameButtonClick;
                GameGrid.Children.Add(buttons.Keys.ElementAt(j));
                //setting row by modulo operation
                Grid.SetRow(buttons.Keys.ElementAt(j), j % size);
                //setting column by division with implicit cast
                //values smaller than size will give column 0
                Grid.SetColumn(buttons.Keys.ElementAt(j), j / size);
            }
            //make sure generated grid is window's content
            Content = GameGrid;
            gameBoard = new Board(size);
            InitializeComponent();
            NewGame(difficulty);          
        }
        /// <summary>
        /// starts new game generating AI instance
        /// and clearing board
        /// </summary>
        /// <param name="difficulty">
        /// sets AI depth constant
        /// </param>
        private void NewGame(int difficulty)
        {
            ClearButtons();
            gameBoard.ClearBoard();         
            //make sure game hasn't finished
            GameEnded = false;
            //new instance of AI
            AIPlayer = new AI(difficulty);
        }
        /// <summary>
        /// handles events of button being clicked
        /// method checks if game has ended
        /// adds cotent to clicked button and
        /// responds with ai move
        /// </summary>
        /// <param name="sender"> which button was clicked </param>
        /// <param name="e"> event parameters </param>
        private void GameButtonClick(object sender, RoutedEventArgs e)
        {      
            //if game is finished player 
            //can't make any move
            if (GameEnded)                          
                NewGame(AIPlayer.depthConstant);
            //changing title to indicate whose turn it is
            Title = "Your move";
            //casting sender to button
            var playerField = (Button)sender;

            //which button was clicked
            var playerColumn = Grid.GetColumn(playerField);
            var playerRow = Grid.GetRow(playerField);

            //if clicked on not empty field method does nothing 
            if (gameBoard.BoardValues[playerRow, playerColumn] != FieldValue.Empty)
                return;

            //putting new value to boardValues array 
            gameBoard.BoardValues[playerRow, playerColumn] = FieldValue.Cross;
            //wrting X and setting colour
            playerField.Content = 'X';
            playerField.Foreground = Brushes.Indigo;
            //if player move ended game 
            CheckGameEnd();
            if (GameEnded)
                return;
            //changing title to indicate whose turn it is
            Title = "Wait, computer's move";
            //tuple returned by ai method make move with chosen filed indexes
            var aiReturned = AIPlayer.MakeMove(gameBoard);
            //finding chosen button using formule for accesing button dictionary
            var aiField = buttons.Keys.ElementAt(aiReturned.Item1 + gameBoard.BoardSize*aiReturned.Item2);

            var aiColumn = Grid.GetColumn(aiField);
            var aiRow = Grid.GetRow(aiField);

            //putting new value to boardValues array 
            gameBoard.BoardValues[aiRow, aiColumn] = FieldValue.Circle;           
            //wrting O and setting colour
            aiField.Content = 'O';
            aiField.Foreground = Brushes.SteelBlue;
            //if AI move endend game
            CheckGameEnd();
            if (GameEnded)
                return;        
        }
        /// <summary>
        /// checks if game has eneded
        /// and highlights board to indicate 
        /// game's outcome
        /// </summary>
        internal void CheckGameEnd()
        {
            var winner = gameBoard.CheckForWinner();
            //if game ends with draw highlight board in orange
            if (gameBoard.CheckForFullBoard())
            {
                HighlightOrange();
                GameEnded = true;
            }
            //if player won highlight board in green
            //if player lost highlight board in red
            if (gameBoard.CheckForWinner().Item1)
            {             
                if (winner.Item2 == FieldValue.Cross)
                    HighlightGreen();
                else
                    HighlightRed();

                GameEnded = true;
            }
        }
        /// <summary>
        /// methods highlighting board with different colors
        /// green indicates player victory, red player defeat and oragne draw
        /// </summary>
        internal void HighlightOrange()
        {
            GameGrid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Background = Brushes.Orange;
            });
        }

        internal void HighlightGreen()
        {
            GameGrid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Background = Brushes.GreenYellow;
            });
        }

        internal void HighlightRed()
        {
            GameGrid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Background = Brushes.Crimson;
            });
        }

        internal void ClearButtons()
        {
            GameGrid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.Background = Brushes.White;
            });
        }
    }
}
