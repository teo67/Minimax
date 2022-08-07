class TicTacToe : IGame {
    private int[][] Board { get; }
    public TicTacToe(int width = 3, int height = 3) {
        Board = new int[height][];
        for(int i = 0; i < height; i++) {
            Board[i] = new int[width];
        }
    }
    private void SetCheck(int?[] arr, int index, int i, int j) {
        if(index < 0 || index >= arr.Length) {
            return;
        }
        if(arr[index] == null) {
            arr[index] = Board[i][j];
        } else if(arr[index] != Board[i][j]) {
            arr[index] = 0;
        }
    }
    public int? CheckWin() {
        // null: not yet evaluated, 0: no winstate
        int?[] cols = new int?[Board[0].Length];
        bool full = true;
        int diagonalLength = Math.Min(Board.Length, Board[0].Length);
        int?[] xforwardDiagonals = new int?[Board[0].Length - diagonalLength + 1];
        int?[] yforwardDiagonals = new int?[Board.Length - diagonalLength + 1];
        int?[] xbackwardDiagonals = new int?[Board[0].Length - diagonalLength + 1];
        int?[] ybackwardDiagonals = new int?[Board.Length - diagonalLength + 1];
        for(int i = 0; i < Board.Length; i++) {
            int? row = null;
            for(int j = 0; j < Board[i].Length; j++) {
                if(Board[i][j] == 0) {
                    full = false;
                }
                if(row == null) {
                    row = Board[i][j];
                } else if(row != Board[i][j]) {
                    row = 0;
                }
                SetCheck(cols, j, i, j);
                SetCheck(xforwardDiagonals, j - i, i, j);
                SetCheck(yforwardDiagonals, i - j, i, j);
                SetCheck(xbackwardDiagonals, i + j - diagonalLength + 1, i, j);
                SetCheck(ybackwardDiagonals, i + j - diagonalLength + 1, i, j);
            }
            if(row != 0 && row != null) {
                return row;
            }
        }
        for(int i = 0; i < Board.Length; i++) {
            if(cols[i] != 0 && cols[i] != null) {
                return cols[i];
            }
        }
        for(int i = 0; i < Board[0].Length - diagonalLength + 1; i++) {
            if(xforwardDiagonals[i] != 0 && xforwardDiagonals[i] != null) {
                return xforwardDiagonals[i];
            }
            if(xbackwardDiagonals[i] != 0 && xbackwardDiagonals[i] != null) {
                return xbackwardDiagonals[i];
            }
        }
        for(int i = 0; i < Board.Length - diagonalLength + 1; i++) {
            if(yforwardDiagonals[i] != 0 && yforwardDiagonals[i] != null) {
                return yforwardDiagonals[i];
            }
            if(ybackwardDiagonals[i] != 0 && ybackwardDiagonals[i] != null) {
                return ybackwardDiagonals[i];
            }
        }
        return full ? 0 : null;
    }
    public void EveryMove(int player, Action<Action> run) {
        for(int i = 0; i < Board.Length; i++) {
            for(int j = 0; j < Board[i].Length; j++) {
                if(Board[i][j] == 0) {
                    Board[i][j] = player;
                    int savedI = i;
                    int savedJ = j;
                    run(() => {
                        Board[savedI][savedJ] = player;
                    });
                    Board[i][j] = 0;
                }
            }
        }
    }
    public string Print() {
        string returning = "";
        for(int i = 0; i < Board.Length; i++) {
            for(int j = 0; j < Board[i].Length; j++) {
                returning += new string[] { "O", "_", "X" }[Board[i][j] + 1] + "  ";
            }
            returning += "\n";
        }
        return returning;
    }
    private int GetInt() {
        int r = -1;
        while(r == -1) {
            string? res = Console.ReadLine();
            if(res != null) {
                try {
                    int p = Int32.Parse(res);
                    if(p < 0 || p > 2) {
                        Console.WriteLine("Please supply an integer from 0 to 2!");
                    } else {
                        r = p;
                    }
                } catch {
                    Console.WriteLine("Please enter a valid integer input!");
                }
            } else {
                Console.WriteLine("No input was detected, try again!");
            }
        }
        return r;
    }
    private (int, int) GetRowCol() {
        Console.Write("Enter your row number (0-2): ");
        int row = GetInt();
        Console.Write("Enter your column number (0-2): ");
        int col = GetInt();
        return (row, col);
    }
    public void GetPlayerTurn() {
        (int, int) res = GetRowCol();
        while(Board[res.Item1][res.Item2] != 0) {
            Console.WriteLine("That space is already taken, try again!");
            res = GetRowCol();
        }
        Board[res.Item1][res.Item2] = -1;
    }
}