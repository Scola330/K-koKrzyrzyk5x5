namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        private readonly Button[,] buttons = new Button[5, 5];
        private bool isPlayerTurn = true;
        private readonly Button resetButton = new(); // Inicjalizacja pola
        private readonly Label currentPlayerLabel = new(); // Etykieta aktywnego gracza
        private readonly Label gamesPlayedLabel = new(); // Etykieta liczby rozegranych gier
        private readonly Label winsLabel = new(); // Etykieta liczby wygranych
        private int gamesPlayed = 0; // Liczba rozegranych gier
        private int playerWins = 0; // Liczba wygranych gracza
        private int computerWins = 0; // Liczba wygranych komputera

        public Form1()
        {
            InitializeComponent();
            InitializeGameBoard();
            InitializeResetButton();
            InitializeCurrentPlayerLabel();
            InitializeGamesPlayedLabel();
            InitializeWinsLabel();
        }

        private void InitializeGameBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    buttons[i, j] = new Button
                    {
                        Width = 60,
                        Height = 60,
                        Location = new System.Drawing.Point(i * 60, j * 60),
                        Font = new System.Drawing.Font("Arial", 24, System.Drawing.FontStyle.Bold)
                    };
                    buttons[i, j].Click += Button_Click;
                    this.Controls.Add(buttons[i, j]);
                }
            }
        }

        private void InitializeResetButton()
        {
            resetButton.Text = "Resetuj";
            resetButton.Width = 300;
            resetButton.Height = 40;
            resetButton.Location = new System.Drawing.Point(0, 300);
            resetButton.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            resetButton.Click += ResetButton_Click;
            this.Controls.Add(resetButton);
        }

        private void InitializeCurrentPlayerLabel()
        {
            currentPlayerLabel.Text = "Aktywny gracz: Gracz";
            currentPlayerLabel.Width = 300;
            currentPlayerLabel.Height = 40;
            currentPlayerLabel.Location = new System.Drawing.Point(0, 350);
            currentPlayerLabel.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            this.Controls.Add(currentPlayerLabel);
        }

        private void InitializeGamesPlayedLabel()
        {
            gamesPlayedLabel.Text = "Liczba rozegranych gier: 0";
            gamesPlayedLabel.Width = 300;
            gamesPlayedLabel.Height = 40;
            gamesPlayedLabel.Location = new System.Drawing.Point(0, 400);
            gamesPlayedLabel.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            this.Controls.Add(gamesPlayedLabel);
        }

        private void InitializeWinsLabel()
        {
            winsLabel.Text = "Gracz : Komputer = 0 : 0";
            winsLabel.Width = 300;
            winsLabel.Height = 40;
            winsLabel.Location = new System.Drawing.Point(0, 450);
            winsLabel.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            this.Controls.Add(winsLabel);
        }

        private void ResetButton_Click(object? sender, EventArgs e)
        {
            ResetGame();
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Text == "")
                {
                    if (isPlayerTurn)
                    {
                        button.Text = "X";
                        isPlayerTurn = false;
                        UpdateCurrentPlayerLabel();
                        CheckForWinner();
                        ComputerMove();
                    }
                }
            }
        }

        private void ComputerMove()
        {
            // Prosta logika ruchu komputera - wybiera pierwszy wolny przycisk
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (buttons[i, j].Text == "")
                    {
                        buttons[i, j].Text = "O";
                        isPlayerTurn = true;
                        UpdateCurrentPlayerLabel();
                        CheckForWinner();
                        return;
                    }
                }
            }
        }

        private void CheckForWinner()
        {
            // Sprawdzenie wierszy, kolumn i przek¹tnych
            for (int i = 0; i < 5; i++)
            {
                if (CheckLine(buttons[i, 0], buttons[i, 1], buttons[i, 2], buttons[i, 3], buttons[i, 4]) ||
                    CheckLine(buttons[0, i], buttons[1, i], buttons[2, i], buttons[3, i], buttons[4, i]))
                {
                    ShowWinner();
                    return;
                }
            }

            if (CheckLine(buttons[0, 0], buttons[1, 1], buttons[2, 2], buttons[3, 3], buttons[4, 4]) ||
                CheckLine(buttons[0, 4], buttons[1, 3], buttons[2, 2], buttons[3, 1], buttons[4, 0]))
            {
                ShowWinner();
                return;
            }

            // Sprawdzenie remisu
            if (CheckForTie())
            {
                MessageBox.Show("Remis!", "Koniec gry");
                ResetGame();
            }
        }

        private static bool CheckLine(params Button[] bts)
        {
            string first = bts[0].Text;
            if (first == "") return false;
            foreach (Button bt in bts)
            {
                if (bt.Text != first) return false;
            }
            return true;
        }

        private bool CheckForTie()
        {
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    return false;
                }
            }
            return true;
        }

        private void ShowWinner()
        {
            string winner = isPlayerTurn ? "Komputer" : "Gracz";
            MessageBox.Show($"{winner} wygrywa!", "Koniec gry");
            gamesPlayed++;
            if (isPlayerTurn)
            {
                computerWins++;
            }
            else
            {
                playerWins++;
            }
            UpdateGamesPlayedLabel();
            UpdateWinsLabel();
            SaveGameToFile(winner);
            ResetGame();
        }

        private void ResetGame()
        {
            foreach (Button button in buttons)
            {
                button.Text = "";
            }
            isPlayerTurn = true;
            UpdateCurrentPlayerLabel();
        }

        private void UpdateCurrentPlayerLabel()
        {
            currentPlayerLabel.Text = isPlayerTurn ? "Aktywny gracz: Gracz" : "Aktywny gracz: Komputer";
        }

        private void UpdateGamesPlayedLabel()
        {
            gamesPlayedLabel.Text = $"Liczba rozegranych gier: {gamesPlayed}";
        }

        private void UpdateWinsLabel()
        {
            winsLabel.Text = $"Gracz : Komputer = {playerWins} : {computerWins}";
        }

        private void SaveGameToFile(string winner)
        {
            string filePath = "game_results.txt";
            string currentDate = DateTime.Now.ToString("MM/dd/yyyy");
            string gameResult = $"Gra {gamesPlayed} ({currentDate}): {winner} wygrywa! (Gracz: {playerWins}, Komputer: {computerWins})\n";
            File.AppendAllText(filePath, gameResult);
        }
    }
}
