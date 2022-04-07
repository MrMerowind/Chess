using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessEngine
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool whiteMove = true;
        public static Canvas mainCanvas = null;
        public static Point pieceUp = new Point(-1,-1);
        public static bool whitePlayerPerson = true;
        public static bool blackPlayerPerson = true;
        public static bool gameStarted = false;
        public static bool gameFinished = false;
        private static bool skipATick = false;
        ChessBackground chessBackground;
        ChessPieces chessPieces;
        ChessPiecesRenderer chessPiecesRenderer;
        public MainWindow()
        {
            InitializeComponent();
            mainCanvas = Board;
            CompositionTarget.Rendering += ComputerMove;

            // dodac bicie w przelocie
            // bicie szachujacego usuwa szacha
            // zamienic Point na Point2i

        }

        public void InitializeGame()
        {
            chessBackground = new ChessBackground();
            chessPieces = new ChessPieces();
            chessPiecesRenderer = new ChessPiecesRenderer();
            whiteMove = true;
        }

        public void RenderScreen()
        {
            if(gameStarted || gameFinished)
            {
                if (gameFinished) gameFinished = false;
                mainCanvas.Children.Clear();
                chessBackground.DrawChessBoardBackground();
                chessPiecesRenderer.RenderPieces(mainCanvas, chessPieces);
            }
        }

        public void ComputerMove(object sender, EventArgs e)
        {
            if (gameFinished)
            {
                RenderScreen();
                return;
            }
            if (skipATick)
            {
                skipATick = false;
                return;
            }
            if (gameStarted)
            {
                if (whiteMove && !whitePlayerPerson)
                {
                    ComputerAi computerAi = new ComputerAi();
                    NodeMove computerMoveWhite = computerAi.GenerateComputerMove(chessPieces, whiteMove);
                    chessPieces.MovePieceToPosition((int)computerMoveWhite.piecePosition.X,
                        (int)computerMoveWhite.piecePosition.Y,
                        (int)computerMoveWhite.pieceFinalPosition.X,
                        (int)computerMoveWhite.pieceFinalPosition.Y);

                    RenderScreen();
                    whiteMove = !whiteMove;
                }
                else if (!whiteMove && !blackPlayerPerson)
                {
                    ComputerAi computerAi = new ComputerAi();
                    NodeMove computerMoveBlack = computerAi.GenerateComputerMove(chessPieces, whiteMove);
                    chessPieces.MovePieceToPosition((int)computerMoveBlack.piecePosition.X,
                        (int)computerMoveBlack.piecePosition.Y,
                        (int)computerMoveBlack.pieceFinalPosition.X,
                        (int)computerMoveBlack.pieceFinalPosition.Y);

                    RenderScreen();
                    whiteMove = !whiteMove;
                }
            }
        }

        private void ButtonPlayWithPerson_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            whitePlayerPerson = true;
            blackPlayerPerson = true;
            gameStarted = true;
            RenderScreen();
        }

        private void ButtonPlayWithComputerAsWhite_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            whitePlayerPerson = true;
            blackPlayerPerson = false;
            gameStarted = true;
            RenderScreen();
        }

        private void ButtonPlayWithComputerAsBlack_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            whitePlayerPerson = false;
            blackPlayerPerson = true;
            gameStarted = true;
            RenderScreen();
        }

        private void ButtonPlayTwoComputers_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            whitePlayerPerson = false;
            blackPlayerPerson = false;
            gameStarted = true;
            RenderScreen();
        }

        private void Board_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!gameStarted) return;
            // pieceUp position
            int x = (int) e.GetPosition(mainCanvas).X / 62;
            int y = 7 - ((int) e.GetPosition(mainCanvas).Y / 62);

            if (x < 0 || x > 7 || y < 0 || y > 7) return;

            pieceUp = new Point(x, y);

            if(gameStarted)
            {
                if(whiteMove && whitePlayerPerson)
                {
                    chessPieces.pieceMoves = new PieceMoves();
                    Point kingPosition = new Point(0,0);
                    for(int kingX = 0; kingX < 8; kingX++)
                    {
                        for (int kingY = 0; kingY < 8; kingY++)
                        {
                            if(chessPieces.piecesBoard[kingX, kingY] == 'k')
                            {
                                kingPosition = new Point(kingX, kingY);
                            }
                        }
                    }
                    chessPieces.pieceMoves.GenerateMoves(chessPieces, true);

                    if(pieceUp != kingPosition)
                    {
                        PlayerAi realPlayerMoveTree = new PlayerAi();
                        realPlayerMoveTree.GenerateComputerMove(chessPieces, true);
                        for (int a = 0; a < realPlayerMoveTree.node.possibleMoves.Count; a++)
                        {
                            for (int b = 0; b < realPlayerMoveTree.node.possibleMoves[a].possibleMoves.Count; b++)
                            {
                                if (realPlayerMoveTree.node.possibleMoves[a].possibleMoves[b].nodeMove.pieceFinalPosition == kingPosition)
                                {
                                    chessPieces.pieceMoves.DeleteMovesInCheck(realPlayerMoveTree.node.possibleMoves[a].nodeMove);
                                }
                            }
                        }
                    }

                    chessPieces.pieceMoves.DeletePiecesThatCantMove();
                    
                    for(int i = 0; i < chessPieces.pieceMoves.pieceMoveList.Count; i++)
                    {
                        if(chessPieces.pieceMoves.pieceMoveList[i].pieceActualPosition.Equals(pieceUp))
                        {
                            Point reversedPieceUp = new Point(pieceUp.X, 7 - pieceUp.Y);
                            chessBackground.highlightedPieceUp = reversedPieceUp;
                            foreach(Point preReversedDestination in chessPieces.pieceMoves.pieceMoveList[i].pieceFinalPosition)
                            {
                                Point reversedDestination = new Point(preReversedDestination.X, 7 - preReversedDestination.Y);
                                chessBackground.highlightedPositions.Add(reversedDestination);
                            }
                            break;
                        }
                    }
                }
                else if (!whiteMove && blackPlayerPerson)
                {
                    chessPieces.pieceMoves = new PieceMoves();

                    Point kingPosition = new Point(0, 0);
                    for (int kingX = 0; kingX < 8; kingX++)
                    {
                        for (int kingY = 0; kingY < 8; kingY++)
                        {
                            if (chessPieces.piecesBoard[kingX, kingY] == 'K')
                            {
                                kingPosition = new Point(kingX, kingY);
                            }
                        }
                    }

                    chessPieces.pieceMoves.GenerateMoves(chessPieces, false);


                    if (pieceUp != kingPosition)
                    {
                        PlayerAi realPlayerMoveTree = new PlayerAi();
                        realPlayerMoveTree.GenerateComputerMove(chessPieces, false);
                        for (int a = 0; a < realPlayerMoveTree.node.possibleMoves.Count; a++)
                        {
                            for (int b = 0; b < realPlayerMoveTree.node.possibleMoves[a].possibleMoves.Count; b++)
                            {
                                if (realPlayerMoveTree.node.possibleMoves[a].possibleMoves[b].nodeMove.pieceFinalPosition == kingPosition)
                                {
                                    chessPieces.pieceMoves.DeleteMovesInCheck(realPlayerMoveTree.node.possibleMoves[a].nodeMove);
                                }
                            }
                        }
                    }

                    chessPieces.pieceMoves.DeletePiecesThatCantMove();

                    for (int i = 0; i < chessPieces.pieceMoves.pieceMoveList.Count; i++)
                    {
                        if (chessPieces.pieceMoves.pieceMoveList[i].pieceActualPosition.Equals(pieceUp))
                        {
                            Point reversedPieceUp = new Point(pieceUp.X, 7 - pieceUp.Y);
                            chessBackground.highlightedPieceUp = reversedPieceUp;
                            foreach (Point preReversedDestination in chessPieces.pieceMoves.pieceMoveList[i].pieceFinalPosition)
                            {
                                Point reversedDestination = new Point(preReversedDestination.X, 7 - preReversedDestination.Y);
                                chessBackground.highlightedPositions.Add(reversedDestination);
                            }
                            break;
                        }
                    }
                }
            }
            RenderScreen();

        }

        private void Board_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!gameStarted) return;
            // pieceFinal position
            if (pieceUp.Equals(new Point(-1, -1))) return;

            int x = (int)e.GetPosition(mainCanvas).X / 62;
            int y = 7 - ((int)e.GetPosition(mainCanvas).Y / 62);

            if (x < 0 || x > 7 || y < 0 || y > 7) return;
            Point pieceDownFinalPosition = new Point(x, y);

            if (gameStarted)
            {
                if ((whiteMove && whitePlayerPerson) || (!whiteMove && blackPlayerPerson))
                {
                    for(int i = 0; i < chessPieces.pieceMoves.pieceMoveList.Count; i++)
                    {
                        List<PieceMove> tmp = chessPieces.pieceMoves.pieceMoveList;
                        Point tmpPoint = tmp[i].pieceActualPosition;
                        if(tmpPoint.Equals(pieceUp))
                        {
                            for(int j = 0; j < tmp[i].pieceFinalPosition.Count; j++)
                            {
                                if (tmp[i].pieceFinalPosition[j].Equals(pieceDownFinalPosition))
                                {
                                    Point tmpPoint2 = tmp[i].pieceFinalPosition[j];
                                    chessPieces.MovePieceToPosition((int)tmpPoint.X, (int)tmpPoint.Y, (int)tmpPoint2.X, (int)tmpPoint2.Y);
                                    whiteMove = !whiteMove;
                                }

                            }
                        }
                    }
                }
            }

            chessBackground.highlightedPieceUp = new Point(-1, -1);
            chessBackground.highlightedPositions.Clear();
            skipATick = true;
            RenderScreen();
        }
    }
}
