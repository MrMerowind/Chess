using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ChessEngine
{
    class ChessBackground
    {
        public List<Point> highlightedPositions = new List<Point>();
        public Point highlightedPieceUp = new Point(-1, -1);
        public void DrawChessBoardBackground()
        {
            Canvas chessBoardBackground = MainWindow.mainCanvas;
            DrawRectangles(chessBoardBackground);
        }
        public const int chessSquareSize = 62;
        public void DrawRectangles(Canvas MyCanvas) // 62x62px
        {
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    Rectangle rectangle = new Rectangle
                    {
                        Height = chessSquareSize,
                        Width = chessSquareSize,
                    };

                    if(highlightedPieceUp.Equals(new Point(i, j)))
                    {
                        rectangle.Fill = ((i + j) % 2 == 0) ? Brushes.Blue : Brushes.DarkBlue;
                    }
                    else if(highlightedPositions.Contains(new Point(i,j)))
                    {
                        rectangle.Fill = ((i + j) % 2 == 0) ? Brushes.Pink : Brushes.Red;
                    }
                    else
                    {
                        rectangle.Fill = ((i + j) % 2 == 0) ? Brushes.White : Brushes.Gray;
                    }
                    
                    MyCanvas.Children.Add(rectangle);

                    Canvas.SetLeft(rectangle, i * (chessSquareSize + 0));
                    Canvas.SetTop(rectangle, j * (chessSquareSize + 0));
                }
            }
        }
    }
}
