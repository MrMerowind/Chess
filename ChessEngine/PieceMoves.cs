using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessEngine
{
    class PieceMoves
    {
        public bool gameEnded = false;
        public List<PieceMove> pieceMoveList = new List<PieceMove>();
        public void DeleteMovesInCheck(NodeMove nodeMove)
        {
            for(int a = 0; a < pieceMoveList.Count; a++)
            {
                if (pieceMoveList[a].pieceActualPosition == nodeMove.piecePosition)
                {
                    for(int b = 0; b < pieceMoveList[a].pieceFinalPosition.Count; b++)
                    {
                        if(pieceMoveList[a].pieceFinalPosition[b] == nodeMove.pieceFinalPosition)
                        {
                            pieceMoveList[a].pieceFinalPosition.RemoveAt(b--);
                        }
                    }
                    
                }
            }
            
        }
        public void GenerateMoves(ChessPieces chessPieces, bool whiteSide)
        {
            char[,] board = chessPieces.piecesBoard;
            for(int y = 0; y < 8; y++)
            {
                for(int x = 0; x < 8; x++)
                {
                    PieceMove pieceMove = new PieceMove();
                    if(board[x,y] == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        pieceMove.whitePlayer = !Char.IsUpper(board[x, y]);
                        pieceMove.pieceChar = Char.ToLower(board[x, y]);
                        pieceMove.pieceActualPosition = new Point(x, y);

                        if(pieceMove.pieceChar == 'w')
                        {
                            GenerateWMoves(pieceMove, board, whiteSide);
                        }
                        else if (pieceMove.pieceChar == 's')
                        {
                            GenerateSMoves(pieceMove, board, whiteSide);
                        }
                        else if (pieceMove.pieceChar == 'g')
                        {
                            GenerateGMoves(pieceMove, board, whiteSide);
                        }
                        else if (pieceMove.pieceChar == 'k')
                        {
                            GenerateKMoves(pieceMove, board, chessPieces, whiteSide);
                        }
                        else if (pieceMove.pieceChar == 'h')
                        {
                            GenerateHMoves(pieceMove, board, whiteSide);
                        }
                        else if (pieceMove.pieceChar == 'p')
                        {
                            GeneratePMoves(pieceMove, board, whiteSide);
                        }
                        pieceMoveList.Add(pieceMove);

                    }
                }
            }
            DeletePiecesThatCantMove();
            DeleteKingCheckMoves();
            DeleteKingCastleThroughCheck();
            //DeleteOtherMovesThanKingIfCheck(whiteSide);
            DeletePiecesThatCantMove();
            DeleteOtherPlayerMoves(whiteSide);
            gameEnded = HasGameEnded();
        }
        public bool HasGameEnded()
        {
            if (pieceMoveList.Count == 0) return true;
            else return false;
        }
        public void DeleteOtherMovesThanKingIfCheck(bool whiteSide)
        {
            bool check = false;
            int kingI = -1;
            int enemyChecking = -1;
            bool moreThanOneCheking = false;
            for (int i = 0; i < pieceMoveList.Count; i++)
            {
                if (pieceMoveList[i].pieceChar == 'k' && pieceMoveList[i].whitePlayer == whiteSide)
                {
                    for (int j = 0; j < pieceMoveList.Count; j++)
                    {
                        if (pieceMoveList[j].pieceChar != 'k' && pieceMoveList[j].whitePlayer != whiteSide)
                        {
                            for (int k = 0; k < pieceMoveList[j].pieceFinalPosition.Count; k++)
                            {
                                if (pieceMoveList[i].pieceActualPosition.Equals(pieceMoveList[j].pieceFinalPosition[k]))
                                {
                                    // HERE DONT REMOVE POSITIONS HIDING KING
                                    if(check)
                                    {
                                        moreThanOneCheking = true;
                                    }
                                    else
                                    {
                                        check = true;
                                        kingI = i;
                                        enemyChecking = j;
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            }
            //
            if(moreThanOneCheking)
            {
                for (int l = 0; l < pieceMoveList.Count; l++)
                {
                    if (pieceMoveList[l].pieceChar != 'k')
                    {
                        pieceMoveList.RemoveAt(l--);
                    }
                }
            }
            else if(check)
            {
                List<Point> placesThatCanUnCheck = new List<Point>();
                if(pieceMoveList[enemyChecking].pieceChar == 's' || pieceMoveList[enemyChecking].pieceChar == 'p')
                {
                    placesThatCanUnCheck.Add(pieceMoveList[enemyChecking].pieceActualPosition);
                }
                else if(pieceMoveList[enemyChecking].pieceChar == 'w' || pieceMoveList[enemyChecking].pieceChar == 'g' || pieceMoveList[enemyChecking].pieceChar == 'h')
                {
                    for(int b = (int)pieceMoveList[enemyChecking].pieceActualPosition.Y, a = (int)pieceMoveList[enemyChecking].pieceActualPosition.X; b != (int)pieceMoveList[kingI].pieceActualPosition.Y || a != (int)pieceMoveList[kingI].pieceActualPosition.X;)
                    {
                        placesThatCanUnCheck.Add(new Point(a, b));

                        if (b < (int)pieceMoveList[kingI].pieceActualPosition.Y) b++;
                        else if (b > (int)pieceMoveList[kingI].pieceActualPosition.Y) b--;
                        if (a < (int)pieceMoveList[kingI].pieceActualPosition.X) a++;
                        else if (a > (int)pieceMoveList[kingI].pieceActualPosition.X) a--;
                    }
                }

                for (int j = 0; j < pieceMoveList.Count; j++)
                {
                    if (pieceMoveList[j].pieceChar != 'k' && pieceMoveList[j].whitePlayer == whiteSide)
                    {
                        for (int k = 0; k < pieceMoveList[j].pieceFinalPosition.Count; k++)
                        {
                            if (!placesThatCanUnCheck.Contains(pieceMoveList[j].pieceFinalPosition[k]))
                            {
                                pieceMoveList[j].pieceFinalPosition.RemoveAt(k--);
                            }
                        }
                    }
                }
            }


        }
        public void DeleteKingCastleThroughCheck()
        {
            for (int x = 0; x < pieceMoveList.Count; x++)
            {
                if (pieceMoveList[x].pieceChar == 'k' && pieceMoveList[x].whitePlayer)
                {
                    if(!pieceMoveList[x].pieceFinalPosition.Contains(new Point(5,0)))
                    {
                        pieceMoveList[x].pieceFinalPosition.Remove(new Point(6, 0));
                    }
                    if (!pieceMoveList[x].pieceFinalPosition.Contains(new Point(3, 0)))
                    {
                        pieceMoveList[x].pieceFinalPosition.Remove(new Point(2, 0));
                    }
                }
                else if (pieceMoveList[x].pieceChar == 'k' && !pieceMoveList[x].whitePlayer)
                {
                    if (!pieceMoveList[x].pieceFinalPosition.Contains(new Point(5, 7)))
                    {
                        pieceMoveList[x].pieceFinalPosition.Remove(new Point(6, 7));
                    }
                    if (!pieceMoveList[x].pieceFinalPosition.Contains(new Point(3, 7)))
                    {
                        pieceMoveList[x].pieceFinalPosition.Remove(new Point(2, 7));
                    }
                }
            }
        }
        public void DeleteKingCheckMoves()
        {
            for (int x = 0; x < pieceMoveList.Count; x++)
            {
                if (pieceMoveList[x].pieceChar == 'k' && pieceMoveList[x].whitePlayer)
                {
                    
                    
                    for(int y = 0; y < pieceMoveList.Count; y++)
                    {
                        if(pieceMoveList[y].whitePlayer == false)
                        {
                            for(int z = 0; z < pieceMoveList[y].pieceFinalPosition.Count; z++)
                            {
                                pieceMoveList[x].pieceFinalPosition.Remove(pieceMoveList[y].pieceFinalPosition[z]);
                            }
                        }
                    }
                }
                else if (pieceMoveList[x].pieceChar == 'k' && !pieceMoveList[x].whitePlayer)
                {
                    for (int y = 0; y < pieceMoveList.Count; y++)
                    {
                        if (pieceMoveList[y].whitePlayer == true)
                        {
                            for (int z = 0; z < pieceMoveList[y].pieceFinalPosition.Count; z++)
                            {
                                pieceMoveList[x].pieceFinalPosition.Remove(pieceMoveList[y].pieceFinalPosition[z]);
                            }
                        }
                    }
                }
            }
        }
        public void DeletePiecesThatCantMove()
        {
            for(int x = 0; x < pieceMoveList.Count; x++)
            {
                if(pieceMoveList[x].pieceFinalPosition.Count == 0)
                {
                    pieceMoveList.RemoveAt(x--);
                }
            }
        }
        public void DeleteOtherPlayerMoves(bool whiteSide)
        {
            for (int x = 0; x < pieceMoveList.Count; x++)
            {
                if (pieceMoveList[x].whitePlayer != whiteSide)
                {
                    pieceMoveList.RemoveAt(x--);
                }
            }
        }
        // Returns true if position is empty
        public void AddCastle(PieceMove pieceMove, char[,] board, ChessPieces chessPieces, bool whiteSide)
        {
            // this has no effect because check from rook is anyway not leting to move there
            if (pieceMove.pieceChar == 'k' && pieceMove.whitePlayer != whiteSide) return;
            if (pieceMove.pieceChar != 'k') return;
            if (pieceMove.whitePlayer)
            {
                if(!chessPieces.whiteLeftWMoved && board[1, 0] == ' ' && board[2, 0] == ' ' && board[3, 0] == ' ')
                {
                    pieceMove.pieceFinalPosition.Add(new Point(2, 0));
                }
                if (!chessPieces.whiteRightWMoved && board[6, 0] == ' ' && board[5, 0] == ' ')
                {
                    pieceMove.pieceFinalPosition.Add(new Point(6, 0));
                }
            }
            else
            {
                if (!chessPieces.blackLeftWMoved && board[1, 7] == ' ' && board[2, 7] == ' ' && board[3, 7] == ' ')
                {
                    pieceMove.pieceFinalPosition.Add(new Point(2, 7));
                }
                if (!chessPieces.blackRightWMoved && board[6, 7] == ' ' && board[5, 7] == ' ')
                {
                    pieceMove.pieceFinalPosition.Add(new Point(6, 7));
                }
            }
        }
        public bool AddPieceMoveP(PieceMove pieceMove, int x, int y, char[,] board, bool whiteSide, bool fight = false)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7) return false;
            if (pieceMove.whitePlayer)
            {
                if (pieceMove.whitePlayer != whiteSide && !fight)
                {
                    return false;
                }
                else if((board[x, y] == ' ' && !fight))
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return true;
                }
                else if ((Char.IsUpper(board[x, y]) && fight) || !whiteSide)
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return false;
                }
                else return false;
            }
            else
            {
                if (pieceMove.whitePlayer != whiteSide && !fight)
                {
                    return false;
                }
                else if ((board[x, y] == ' ' && !fight))
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return true;
                }
                else if ((Char.IsLower(board[x, y]) && fight) || whiteSide)
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return false;
                }
                else return false;
            }
        }
        public bool AddPieceMoveOnce(PieceMove pieceMove, int x, int y, char[,] board, bool whiteSide, ChessPieces chessPieces = null)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7) return false;
            if (pieceMove.whitePlayer)
            {
                if (board[x, y] == ' ' || board[x, y] == 'K')
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return true;
                }
                else if (Char.IsUpper(board[x, y]) || !whiteSide)
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return false;
                }
                else return false;
            }
            else
            {
                if (board[x, y] == ' ' || board[x, y] == 'k')
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return true;
                }
                else if (Char.IsLower(board[x, y]) || whiteSide)
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return false;
                }
                else return false;
            }
        }
        // Returns true if position is clear and false if ther is other piece
        public bool AddPieceMove(PieceMove pieceMove, int x, int y, char[,] board, bool whiteSide, ChessPieces chessPieces = null)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7) return false;
            if(pieceMove.whitePlayer)
            {
                if (board[x, y] == ' ' || board[x, y] == 'K')
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return true;
                }
                else if (Char.IsUpper(board[x, y]) || !whiteSide)
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return false;
                }
                else return false;
            }
            else
            {
                if (board[x, y] == ' ' || board[x, y] == 'k')
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return true;
                }
                else if (Char.IsLower(board[x, y]) || whiteSide)
                {
                    pieceMove.pieceFinalPosition.Add(new Point(x, y));
                    return false;
                }
                else return false;
            }
        }
        public void GeneratePMoves(PieceMove pieceMove, char[,] board, bool whiteSide)
        {
            int horizontal = (int)pieceMove.pieceActualPosition.X;
            int vertical = (int)pieceMove.pieceActualPosition.Y;
            if(pieceMove.whitePlayer)
            {
                AddPieceMoveP(pieceMove, horizontal - 1, vertical + 1, board, whiteSide, true);
                AddPieceMoveP(pieceMove, horizontal + 1, vertical + 1, board, whiteSide, true);
                if (AddPieceMoveP(pieceMove, horizontal + 0, vertical + 1, board, whiteSide) && vertical == 1)
                {
                    AddPieceMoveP(pieceMove, horizontal + 0, vertical + 2, board, whiteSide);
                }
            }
            else
            {
                AddPieceMoveP(pieceMove, horizontal - 1, vertical - 1, board, whiteSide, true);
                AddPieceMoveP(pieceMove, horizontal + 1, vertical - 1, board, whiteSide, true);
                if (AddPieceMoveP(pieceMove, horizontal + 0, vertical - 1, board, whiteSide) && vertical == 6)
                {
                    AddPieceMoveP(pieceMove, horizontal + 0, vertical - 2, board, whiteSide);
                }
            }
            
        }
        public void GenerateWMoves(PieceMove pieceMove, char[,] board, bool whiteSide)
        {
            int horizontal = (int)pieceMove.pieceActualPosition.X;
            int vertical = (int)pieceMove.pieceActualPosition.Y;

            int v, h;

            v = vertical;
            h = horizontal;

            while (AddPieceMove(pieceMove, ++h, v, board, whiteSide)) ;

            v = vertical;
            h = horizontal;

            while (AddPieceMove(pieceMove, h, ++v, board, whiteSide)) ;

            v = vertical;
            h = horizontal;

            while (AddPieceMove(pieceMove, --h, v, board, whiteSide)) ;

            v = vertical;
            h = horizontal;

            while (AddPieceMove(pieceMove, h, --v, board, whiteSide)) ;
        }
        public void GenerateGMoves(PieceMove pieceMove, char[,] board, bool whiteSide)
        {
            int horizontal = (int)pieceMove.pieceActualPosition.X;
            int vertical = (int)pieceMove.pieceActualPosition.Y;

            int v, h;

            v = vertical;
            h = horizontal;

            while (AddPieceMove(pieceMove, ++h, ++v, board, whiteSide)) ;

            v = vertical;
            h = horizontal;

            while (AddPieceMove(pieceMove, --h, ++v, board, whiteSide)) ;

            v = vertical;
            h = horizontal;

            while (AddPieceMove(pieceMove, ++h, --v, board, whiteSide)) ;

            v = vertical;
            h = horizontal;

            while (AddPieceMove(pieceMove, --h, --v, board, whiteSide)) ;
        }
        public void GenerateSMoves(PieceMove pieceMove, char[,] board, bool whiteSide)
        {
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X - 2, (int)pieceMove.pieceActualPosition.Y - 1, board, whiteSide);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X - 2, (int)pieceMove.pieceActualPosition.Y + 1, board, whiteSide);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X + 2, (int)pieceMove.pieceActualPosition.Y + 1, board, whiteSide);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X + 2, (int)pieceMove.pieceActualPosition.Y - 1, board, whiteSide);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X - 1, (int)pieceMove.pieceActualPosition.Y + 2, board, whiteSide);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X - 1, (int)pieceMove.pieceActualPosition.Y - 2, board, whiteSide);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X + 1, (int)pieceMove.pieceActualPosition.Y + 2, board, whiteSide);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X + 1, (int)pieceMove.pieceActualPosition.Y - 2, board, whiteSide);
        }
        public void GenerateKMoves(PieceMove pieceMove, char[,] board, ChessPieces chessPieces, bool whiteSide)
        {
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X - 1, (int)pieceMove.pieceActualPosition.Y - 1, board, whiteSide, chessPieces);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X - 1, (int)pieceMove.pieceActualPosition.Y - 0, board, whiteSide, chessPieces);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X - 1, (int)pieceMove.pieceActualPosition.Y + 1, board, whiteSide, chessPieces);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X - 0, (int)pieceMove.pieceActualPosition.Y - 1, board, whiteSide, chessPieces);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X - 0, (int)pieceMove.pieceActualPosition.Y + 1, board, whiteSide, chessPieces);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X + 1, (int)pieceMove.pieceActualPosition.Y - 1, board, whiteSide, chessPieces);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X + 1, (int)pieceMove.pieceActualPosition.Y - 0, board, whiteSide, chessPieces);
            AddPieceMove(pieceMove, (int)pieceMove.pieceActualPosition.X + 1, (int)pieceMove.pieceActualPosition.Y + 1, board, whiteSide, chessPieces);
            AddCastle(pieceMove, board, chessPieces, whiteSide);
        }
        public void GenerateKMovesOnce(PieceMove pieceMove, char[,] board, ChessPieces chessPieces, bool whiteSide)
        {
            AddPieceMoveOnce(pieceMove, (int)pieceMove.pieceActualPosition.X - 1, (int)pieceMove.pieceActualPosition.Y - 1, board, whiteSide, chessPieces);
            AddPieceMoveOnce(pieceMove, (int)pieceMove.pieceActualPosition.X - 1, (int)pieceMove.pieceActualPosition.Y - 0, board, whiteSide, chessPieces);
            AddPieceMoveOnce(pieceMove, (int)pieceMove.pieceActualPosition.X - 1, (int)pieceMove.pieceActualPosition.Y + 1, board, whiteSide, chessPieces);
            AddPieceMoveOnce(pieceMove, (int)pieceMove.pieceActualPosition.X - 0, (int)pieceMove.pieceActualPosition.Y - 1, board, whiteSide, chessPieces);
            AddPieceMoveOnce(pieceMove, (int)pieceMove.pieceActualPosition.X - 0, (int)pieceMove.pieceActualPosition.Y + 1, board, whiteSide, chessPieces);
            AddPieceMoveOnce(pieceMove, (int)pieceMove.pieceActualPosition.X + 1, (int)pieceMove.pieceActualPosition.Y - 1, board, whiteSide, chessPieces);
            AddPieceMoveOnce(pieceMove, (int)pieceMove.pieceActualPosition.X + 1, (int)pieceMove.pieceActualPosition.Y - 0, board, whiteSide, chessPieces);
            AddPieceMoveOnce(pieceMove, (int)pieceMove.pieceActualPosition.X + 1, (int)pieceMove.pieceActualPosition.Y + 1, board, whiteSide, chessPieces);
            AddCastle(pieceMove, board, chessPieces, whiteSide);
        }
        public void GenerateHMoves(PieceMove pieceMove, char[,] board, bool whiteSide)
        {
            GenerateWMoves(pieceMove, board, whiteSide);
            GenerateGMoves(pieceMove, board, whiteSide);
        }
    }

    class PieceMove
    {
        public char pieceChar;
        public bool whitePlayer;
        public Point pieceActualPosition;
        public List<Point> pieceFinalPosition = new List<Point>();
    }
}
