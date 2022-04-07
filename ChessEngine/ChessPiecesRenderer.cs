using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessEngine
{
    class ChessPiecesRenderer
    {
        public void RenderPieces(Canvas canvas, ChessPieces piecePositions)
        {
            for(int y = 0; y < 8; y++)
            {
                for(int x = 0; x < 8; x++)
                {
                    if (piecePositions.piecesBoard[x, y] == ' ') continue;
                    string imageName = "";
                    if(Char.IsLower(piecePositions.piecesBoard[x, y]))
                    {
                        imageName = "b";
                    }
                    else
                    {
                        imageName = "c";
                    }
                    imageName += Char.ToLower(piecePositions.piecesBoard[x, y]);

                    Image myImage = new Image();
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    string sourceUrl = "images/" + imageName + ".png";
                    bi.UriSource = new Uri(sourceUrl, UriKind.Relative);
                    bi.EndInit();

                    myImage.BeginInit();
                    myImage.Stretch = Stretch.Fill;
                    myImage.Name = imageName;
                    myImage.Height = 50;
                    myImage.Width = 50;
                    myImage.Source = bi;
                    myImage.EndInit();

                    Canvas.SetTop(myImage, ((7 - y) * 62) + 6);
                    Canvas.SetLeft(myImage, (x * 62) + 6);
                    canvas.Children.Add(myImage);
                    

                }
            }

            
        }
    }
}
