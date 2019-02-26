using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        /// <summary>
        /// handles for size and level combo box 
        /// </summary>
        internal bool SizeHandle = true;
        internal bool LevelHandle = true;
        /// <summary>
        /// chosen board size passed to game window constructor
        /// default board size is 3
        /// </summary>
        internal int ChosenBoardSize = 3;
        /// <summary>
        /// chosen difficulty level controlling depth 
        /// of AI reccurence
        /// default value is 3
        /// </summary>
        internal int ChosenDifficultyLevel = 5;
        /// <summary>
        /// starts game and generates given size board
        /// </summary>
        public StartupWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// creates board of passed size
        /// </summary>
        /// <param name="boardSize"></param>
        private void GenerateBoard(int boardSize,int difficultyLevel)
        {
            GameWindow window = new GameWindow(boardSize,difficultyLevel);
            window.Show();
        }
        /// <summary>
        /// Handles event of PLAY button being clicked
        /// starts new game and generates board with given parameter
        /// </summary>
        private void PlayButtonClick(object sender, RoutedEventArgs e) 
            => GenerateBoard(ChosenBoardSize,ChosenDifficultyLevel);
        /// <summary>
        /// Handles event of changing selection in combo box
        /// </summary>
        private void SizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            SizeHandle = !cmb.IsDropDownOpen;
            HandleSizeComboBox();
        }
        /// <summary>
        /// Handles event of closing combo box
        /// </summary>
        private void SizeComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (SizeHandle) HandleSizeComboBox();
            SizeHandle = true;
        }
        /// <summary>
        /// Handles event of changing selection in combo box
        /// </summary>
        private void LevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            LevelHandle = !cmb.IsDropDownOpen;
            HandleSizeComboBox();
        }
        /// <summary>
        /// Handles event of closing combo box
        /// </summary>
        private void LevelComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (SizeHandle) HandleLevelComboBox();
            LevelHandle = true;
        }
        /// <summary>
        /// handels events of selecting items in level combo box
        /// sets depthConstant in AI class
        /// </summary>
        private void HandleLevelComboBox()
        {
            switch (SizeSelect.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "Easy":
                    ChosenDifficultyLevel = 3;
                    break;
                case "Medium":
                    ChosenDifficultyLevel = 5;
                    break;
                case "Hard":
                    ChosenDifficultyLevel = 10;
                    break;
                default:
                    ChosenDifficultyLevel = 5;
                    break;
            }
        }
        /// <summary>
        /// Handles events of selecting items in combo box
        /// sets board size variable depending on which box 
        /// was choosen. Default size is 3.
        /// </summary>
        private void HandleSizeComboBox()
        {
            switch (SizeSelect.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "3x3":
                    ChosenBoardSize = 3;
                    break;
                case "4x4":
                    ChosenBoardSize = 4;
                    break;
                case "5x5":
                    ChosenBoardSize = 5;
                    break;
                case "6x6":
                    ChosenBoardSize = 6;
                    break;
                case "10x10":
                    ChosenBoardSize = 10;
                    break;
                default:
                    ChosenBoardSize = 3;
                    break;
            }
        }
    }
}
