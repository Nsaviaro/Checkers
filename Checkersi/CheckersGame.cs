using System;
using System.Collections.Generic;

namespace CheckersGameNamespace
{
    public enum PieceType { Normal, King }

    public enum Player { None, Player1, Player2 }

    public class Piece
    {
        public Player Owner { get; set; }
        public PieceType Type { get; set; }
    }

    public class CheckersGame
    {
        private const int BoardSize = 8;
        private Piece[,] board = new Piece[BoardSize, BoardSize];
        private Player currentPlayer = Player.Player1;

        public CheckersGame()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    board[y, x] = new Piece { Owner = Player.None, Type = PieceType.Normal };
                }
            }

            for (int y = 0; y < 3; y++)
            {
                for (int x = (y % 2); x < BoardSize; x += 2)
                {
                    board[y, x].Owner = Player.Player2;
                }
            }

            for (int y = BoardSize - 1; y >= BoardSize - 3; y--)
            {
                for (int x = (y % 2); x < BoardSize; x += 2)
                {
                    board[y, x].Owner = Player.Player1;
                }
            }
        }

        public Piece[,] GetBoard()
        {
            return board;
        }

        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }

        public bool MovePiece(int startX, int startY, int endX, int endY)
        {
            if (!IsValidMove(startX, startY, endX, endY))
                return false;

            board[endY, endX] = board[startY, startX];
            board[startY, startX] = new Piece { Type = PieceType.Normal, Owner = Player.None };

            if (endY == 0 && board[endY, endX].Owner == Player.Player1)
                board[endY, endX].Type = PieceType.King;
            else if (endY == BoardSize - 1 && board[endY, endX].Owner == Player.Player2)
                board[endY, endX].Type = PieceType.King;

            currentPlayer = currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
            return true;
        }

        public bool IsValidMove(int startX, int startY, int endX, int endY)
        {
            if (startX < 0 || startX >= BoardSize || startY < 0 || startY >= BoardSize ||
                endX < 0 || endX >= BoardSize || endY < 0 || endY >= BoardSize)
                return false;

            var piece = board[startY, startX];
            if (piece.Owner != currentPlayer)
                return false;

            var target = board[endY, endX];
            if (target.Owner != Player.None)
                return false;

            int dx = endX - startX;
            int dy = endY - startY;

            if (piece.Type == PieceType.Normal)
            {
                if (piece.Owner == Player.Player1 && dy != -1)
                    return false;
                if (piece.Owner == Player.Player2 && dy != 1)
                    return false;
            }

            if (Math.Abs(dx) == 1 && Math.Abs(dy) == 1)
                return true;

            if (Math.Abs(dx) == 2 && Math.Abs(dy) == 2)
            {
                int midX = (startX + endX) / 2;
                int midY = (startY + endY) / 2;
                var midPiece = board[midY, midX];
                if (midPiece.Owner != Player.None && midPiece.Owner != currentPlayer)
                {
                    board[midY, midX] = new Piece { Type = PieceType.Normal, Owner = Player.None };
                    return true;
                }
            }

            return false;
        }

        public bool IsGameOver()
        {
            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    if (board[y, x].Owner == currentPlayer)
                    {
                        for (int dy = -2; dy <= 2; dy++)
                        {
                            for (int dx = -2; dx <= 2; dx++)
                            {
                                if (IsValidMove(x, y, x + dx, y + dy))
                                    return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
