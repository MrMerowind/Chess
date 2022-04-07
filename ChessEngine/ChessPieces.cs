using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    class ChessPieces
    {
        /**
         * Chess board (0,0) is at down left so when printing pieces on board we need to switch it
         * (x,y)
         * **/
        public ChessPieces(ChessPieces reference)
        {
            ResetChessPieces();
            CopyPiecesBoard(reference.piecesBoard);
            pieceMoves = new PieceMoves();
        }
        public ChessPieces()
        {
            ResetChessPieces();
            pieceMoves = new PieceMoves();
            //pieceMoves.GenerateMoves(this);
        }
        // CASTLE VARIABLES
        public bool whiteLeftWMoved = false;
        public bool whiteRightWMoved = false;
        public bool blackLeftWMoved = false;
        public bool blackRightWMoved = false;

        public char[,] piecesBoard = null;
        public PieceMoves pieceMoves = null;

        public bool HasGameEnded()
        {
            return pieceMoves.HasGameEnded();
        }

        public int CalculatePoints(bool whiteMove)
        {
            int result = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (piecesBoard[x, y] == ' ') continue;
                    else if (piecesBoard[x, y] == 'p') result += 1;
                    else if (piecesBoard[x, y] == 'P') result -= 1;
                    else if (piecesBoard[x, y] == 'w') result += 5;
                    else if (piecesBoard[x, y] == 'W') result -= 5;
                    else if (piecesBoard[x, y] == 's') result += 3;
                    else if (piecesBoard[x, y] == 'S') result -= 3;
                    else if (piecesBoard[x, y] == 'g') result += 3;
                    else if (piecesBoard[x, y] == 'G') result -= 3;
                    else if (piecesBoard[x, y] == 'h') result += 9;
                    else if (piecesBoard[x, y] == 'H') result -= 9;
                }
            }
            //if (whiteMove) result = -result;

            return result;
        }
        public void ForceMovePieceToPosition(int xstart, int ystart, int xend, int yend)
        {
            if (xstart < 0 || xstart > 7 || ystart < 0 || ystart > 7 || xend < 0 || xend > 7 || yend < 0 || yend > 7)
                return;

            piecesBoard[xend, yend] = piecesBoard[xstart, ystart];
            piecesBoard[xstart, ystart] = ' ';
            if (piecesBoard[xend, 7] == 'p') piecesBoard[xend, 7] = 'h';
            else if (piecesBoard[xend, 0] == 'P') piecesBoard[xend, 0] = 'H';
        }
        public void MovePieceToPosition(int xstart, int ystart, int xend, int yend)
        {
            if (xstart < 0 || xstart > 7 || ystart < 0 || ystart > 7 || xend < 0 || xend > 7 || yend < 0 || yend > 7)
            {
                MainWindow.gameStarted = false;
                MainWindow.gameFinished = true;
                return;
            }

            if(piecesBoard[xstart, ystart] == 'k')
            {
                if(!whiteLeftWMoved && xstart == 4 && ystart == 0 && xend == 2 && yend == 0)
                {
                    piecesBoard[3, 0] = piecesBoard[0, 0];
                    piecesBoard[0, 0] = ' ';
                }
                if (!whiteRightWMoved && xstart == 4 && ystart == 0 && xend == 6 && yend == 0)
                {
                    piecesBoard[5, 0] = piecesBoard[7, 0];
                    piecesBoard[7, 0] = ' ';
                }
            }
            else if (piecesBoard[xstart, ystart] == 'K')
            {
                if (!blackLeftWMoved && xstart == 4 && ystart == 7 && xend == 2 && yend == 7)
                {
                    piecesBoard[3, 7] = piecesBoard[0, 7];
                    piecesBoard[0, 7] = ' ';
                }
                if (!blackRightWMoved && xstart == 4 && ystart == 7 && xend == 6 && yend == 7)
                {
                    piecesBoard[5, 7] = piecesBoard[7, 7];
                    piecesBoard[7, 7] = ' ';
                }
            }

            if (!whiteLeftWMoved)
            {
                if (xstart == 0 && ystart == 0) whiteLeftWMoved = true;
                if (xstart == 4 && ystart == 0) whiteLeftWMoved = true;
            }
            if(!whiteRightWMoved)
            {
                if (xstart == 7 && ystart == 0) whiteRightWMoved = true;
                if (xstart == 4 && ystart == 0) whiteRightWMoved = true;
            }
            if (!blackLeftWMoved)
            {
                if (xstart == 0 && ystart == 7) blackLeftWMoved = true;
                if (xstart == 4 && ystart == 7) blackLeftWMoved = true;
            }
            if (!blackRightWMoved)
            {
                if (xstart == 7 && ystart == 7) blackRightWMoved = true;
                if (xstart == 4 && ystart == 7) blackRightWMoved = true;
            }

            piecesBoard[xend, yend] = piecesBoard[xstart, ystart];
            piecesBoard[xstart, ystart] = ' ';

            if (piecesBoard[xend, 7] == 'p') piecesBoard[xend, 7] = 'h';
            else if (piecesBoard[xend, 0] == 'P') piecesBoard[xend, 0] = 'H';
        }
        public void CopyPiecesBoard(char[, ] reference)
        {
            piecesBoard = new char[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    piecesBoard[x, y] = reference[x, y];
                }
            }
        }
        public void ResetChessPieces()
        {
            piecesBoard = new char[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for(int x = 0; x < 8; x++)
                {
                    piecesBoard[x, y] = ' ';
                }
            }
            // White side
            piecesBoard[0, 0] = 'w';
            piecesBoard[1, 0] = 's';
            piecesBoard[2, 0] = 'g';
            piecesBoard[3, 0] = 'h';
            piecesBoard[4, 0] = 'k';
            piecesBoard[5, 0] = 'g';
            piecesBoard[6, 0] = 's';
            piecesBoard[7, 0] = 'w';
            piecesBoard[0, 1] = 'p';
            piecesBoard[1, 1] = 'p';
            piecesBoard[2, 1] = 'p';
            piecesBoard[3, 1] = 'p';
            piecesBoard[4, 1] = 'p';
            piecesBoard[5, 1] = 'p';
            piecesBoard[6, 1] = 'p';
            piecesBoard[7, 1] = 'p';
            // Black side
            piecesBoard[0, 7] = 'W';
            piecesBoard[1, 7] = 'S';
            piecesBoard[2, 7] = 'G';
            piecesBoard[3, 7] = 'H';
            piecesBoard[4, 7] = 'K';
            piecesBoard[5, 7] = 'G';
            piecesBoard[6, 7] = 'S';
            piecesBoard[7, 7] = 'W';
            piecesBoard[0, 6] = 'P';
            piecesBoard[1, 6] = 'P';
            piecesBoard[2, 6] = 'P';
            piecesBoard[3, 6] = 'P';
            piecesBoard[4, 6] = 'P';
            piecesBoard[5, 6] = 'P';
            piecesBoard[6, 6] = 'P';
            piecesBoard[7, 6] = 'P';
        }
        
    }
}
