using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessEngine
{
    class NodeMove
    {
        public Point piecePosition = new Point(-1, -1);
        public Point pieceFinalPosition = new Point(-1, -1);
    }
    class Node
    {
        public static int maxDepth = 2;
        public static NodeMove bestSearchedMove = new NodeMove();
        public bool whiteMove = true;
        public int pointsCurrentTotal = 0;
        public int childBestPoints = 0;
        public int depth = 0;
        public NodeMove nodeMove = new NodeMove();
        public List<NodeMove> possibleMovesNodes = new List<NodeMove>();
        public List<Node> possibleMoves = new List<Node>();

        public ChessPieces chessPieces = new ChessPieces();

        
        public NodeMove GetBestMoveNodeMove(bool maximizingPlayer)
        {
            List<NodeMove> result = new List<NodeMove>();
            if (maximizingPlayer)
            {
                int max = -9999;
                for (int i = 0; i < possibleMoves.Count; i++)
                {
                    if (max < possibleMoves[i].childBestPoints)
                    {
                        max = possibleMoves[i].childBestPoints;
                        result.Clear();
                        result.Add(possibleMoves[i].nodeMove);
                    }
                    else if (max == possibleMoves[i].childBestPoints)
                    {
                        result.Add(possibleMoves[i].nodeMove);
                    }
                }
            }
            else
            {
                int min = 9999;
                for (int i = 0; i < possibleMoves.Count; i++)
                {
                    if (min > possibleMoves[i].childBestPoints)
                    {
                        min = possibleMoves[i].childBestPoints;
                        result.Clear();
                        result.Add(possibleMoves[i].nodeMove);
                    }
                    else if (min == possibleMoves[i].childBestPoints)
                    {
                        result.Add(possibleMoves[i].nodeMove);
                    }
                }
            }
            
            Random rnd = new Random();
            if (result.Count <= 0) return new NodeMove();
            return result[rnd.Next(0,result.Count)];
        }
        public int CalculateGameScore()
        {
            pointsCurrentTotal = chessPieces.CalculatePoints(whiteMove);
            return pointsCurrentTotal;
        }
        public int GenerateBestMove(bool whitePlayer)
        {
            if (possibleMovesNodes.Count == 0)
            {
                if (whitePlayer)
                {
                    childBestPoints = -1000;
                    return -1000;
                }
                else
                {
                    childBestPoints = 1000;
                    return 1000;
                }
            }
            else if (depth >= maxDepth)
            {
                childBestPoints = CalculateGameScore();
                return childBestPoints;
            }
            else
            {
                if (whitePlayer)
                {
                    // maximum points
                    int result = -9999;
                    for(int i = 0; i < possibleMoves.Count; i++)
                    {
                        int childResult = possibleMoves[i].GenerateBestMove(!whitePlayer);
                        if (result < childResult)
                        {
                            result = childResult;
                        }
                    }
                    childBestPoints = result;
                    return result;
                }
                else
                {
                    // minimum points
                    int result = 9999;
                    for (int i = 0; i < possibleMoves.Count; i++)
                    {
                        int childResult = possibleMoves[i].GenerateBestMove(!whitePlayer);
                        if (result > childResult)
                        {
                            result = childResult;
                        }
                    }
                    childBestPoints = result;
                    return result;
                }
            }
        }
        public void GeneratePossibleMoves()
        {
            chessPieces.pieceMoves.GenerateMoves(chessPieces, whiteMove);
            for(int i = 0; i < chessPieces.pieceMoves.pieceMoveList.Count; i++)
            {
                for(int j = 0; j < chessPieces.pieceMoves.pieceMoveList[i].pieceFinalPosition.Count; j++)
                {
                    int startx = (int)chessPieces.pieceMoves.pieceMoveList[i].pieceActualPosition.X;
                    int starty = (int)chessPieces.pieceMoves.pieceMoveList[i].pieceActualPosition.Y;
                    int endx = (int)chessPieces.pieceMoves.pieceMoveList[i].pieceFinalPosition[j].X;
                    int endy = (int)chessPieces.pieceMoves.pieceMoveList[i].pieceFinalPosition[j].Y;
                    NodeMove tmpNodeMove = new NodeMove();
                    tmpNodeMove.piecePosition = new Point(startx, starty);
                    tmpNodeMove.pieceFinalPosition = new Point(endx, endy);
                    possibleMovesNodes.Add(tmpNodeMove);
                }
            }
            for(int i = 0; i < possibleMovesNodes.Count; i++)
            {
                Node nextNode = new Node();
                nextNode.whiteMove = !whiteMove;
                nextNode.depth = depth + 1;
                nextNode.nodeMove = possibleMovesNodes[i];
                // copy board and make move
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        nextNode.chessPieces.piecesBoard[x, y] = chessPieces.piecesBoard[x, y];
                    }
                }
                nextNode.chessPieces.ForceMovePieceToPosition((int)possibleMovesNodes[i].piecePosition.X,
                    (int)possibleMovesNodes[i].piecePosition.Y,
                    (int)possibleMovesNodes[i].pieceFinalPosition.X,
                    (int)possibleMovesNodes[i].pieceFinalPosition.Y);

                possibleMoves.Add(nextNode);

                if (depth < maxDepth) possibleMoves[possibleMoves.Count - 1].GeneratePossibleMoves();
            }
        }
    }
    class ComputerAi
    {
        public Node node = new Node();
        public void DeleteCheckAfterMove()
        {
            Point kingPosition = new Point(0, 0);
            for (int kingX = 0; kingX < 8; kingX++)
            {
                for (int kingY = 0; kingY < 8; kingY++)
                {
                    if (node.whiteMove == true)
                    {
                        if (node.chessPieces.piecesBoard[kingX, kingY] == 'k')
                        {
                            kingPosition = new Point(kingX, kingY);
                        }
                    }
                    else if (node.whiteMove == false)
                    {
                        if (node.chessPieces.piecesBoard[kingX, kingY] == 'K')
                        {
                            kingPosition = new Point(kingX, kingY);
                        }
                    }
                }
            }
            for (int a = 0; a < node.possibleMoves.Count; a++)
            {
                if (node.possibleMoves[a].nodeMove.piecePosition == kingPosition) continue;
                for (int b = 0; b < node.possibleMoves[a].possibleMoves.Count; b++)
                {
                    if (node.possibleMoves[a].possibleMoves[b].nodeMove.pieceFinalPosition == kingPosition)
                    {
                        node.possibleMoves.RemoveAt(a--);
                        //node.possibleMoves[a].possibleMoves.RemoveAt(b--);
                        break;
                    }
                }
            }
        }
        public NodeMove GenerateComputerMove(ChessPieces chessPieces, bool whiteMove)
        {
            node.chessPieces = chessPieces;
            node.whiteMove = whiteMove;
            node.GeneratePossibleMoves();
            DeleteCheckAfterMove();

            node.GenerateBestMove(whiteMove);
            return node.GetBestMoveNodeMove(whiteMove);
        }
    }
    class PlayerAi
    {
        public Node node = new Node();
        public void GenerateComputerMove(ChessPieces chessPieces, bool whiteMove)
        {
            node.chessPieces = new ChessPieces(chessPieces);
            node.whiteMove = whiteMove;
            node.GeneratePossibleMoves();
            node.GenerateBestMove(whiteMove);
            // return node.GetBestMoveNodeMove(whiteMove);
        }
    }
}
