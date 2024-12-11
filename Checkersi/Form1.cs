using System;
using System.Drawing;
using System.Windows.Forms;
using CheckersGameNamespace; 

namespace Checkersi
{
    public partial class Form1 : Form
    {
        private const int BoardSize = 8;
        private Button[,] buttons = new Button[BoardSize, BoardSize];
        private CheckersGameNamespace.CheckersGame game = new CheckersGameNamespace.CheckersGame();
        private int? selectedX = null;
        private int? selectedY = null;
        private Label promptLabel;
        private Label turnLabel;

        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
            InitializeLabels();
            UpdateTurnLabel();
        }

        private void InitializeBoard()
        {
            this.ClientSize = new Size(BoardSize * 60, BoardSize * 60 + 60);
            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    var button = new Button
                    {
                        Location = new Point(x * 60, y * 60),
                        Size = new Size(60, 60),
                        BackColor = (x + y) % 2 == 0 ? Color.White : Color.Gray,
                        Tag = new Point(x, y)
                    };
                    button.Click += Button_Click;
                    buttons[y, x] = button;
                    this.Controls.Add(button);
                }
            }
            UpdateBoard();
        }

        private void InitializeLabels()
        {
            promptLabel = new Label
            {
                Location = new Point(0, BoardSize * 60),
                Size = new Size(BoardSize * 60, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            this.Controls.Add(promptLabel);

            turnLabel = new Label
            {
                Location = new Point(0, BoardSize * 60 + 30),
                Size = new Size(BoardSize * 60, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            this.Controls.Add(turnLabel);
        }

        private void UpdateTurnLabel()
        {
            turnLabel.Text = $"Player {game.GetCurrentPlayer()}'s turn";
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is Point position)
            {
                int x = position.X;
                int y = position.Y;

                if (selectedX == null || selectedY == null)
                {
                    selectedX = x;
                    selectedY = y;
                    promptLabel.Text = $"Selected piece at ({x}, {y}). Choose a destination.";
                    HighlightPossibleMoves(x, y);
                }
                else
                {
                    if (game.MovePiece(selectedX.Value, selectedY.Value, x, y))
                    {
                        UpdateBoard();
                        if (game.IsGameOver())
                        {
                            MessageBox.Show($"Game over! Player {game.GetCurrentPlayer()} has no moves left.");
                        }
                        else
                        {
                            UpdateTurnLabel();
                        }
                    }
                    else
                    {
                        promptLabel.Text = "Invalid move. Try again.";
                    }
                    selectedX = null;
                    selectedY = null;
                    ClearHighlights();
                }
            }
        }

        private void HighlightPossibleMoves(int startX, int startY)
        {
            ClearHighlights();
            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    if (game.IsValidMove(startX, startY, x, y))
                    {
                        buttons[y, x].BackColor = Color.LightGreen;
                    }
                }
            }
        }

        private void ClearHighlights()
        {
            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    buttons[y, x].BackColor = (x + y) % 2 == 0 ? Color.White : Color.Gray;
                }
            }
        }

        private void UpdateBoard()
        {
            var board = game.GetBoard();
            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    var piece = board[y, x];
                    if (piece.Owner == Player.Player1)
                        buttons[y, x].Text = piece.Type == PieceType.King ? "K1" : "P1";
                    else if (piece.Owner == Player.Player2)
                        buttons[y, x].Text = piece.Type == PieceType.King ? "K2" : "P2";
                    else
                        buttons[y, x].Text = "";
                }
            }
        }
    }
}
