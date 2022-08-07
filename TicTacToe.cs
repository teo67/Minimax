class TicTacToe : IGame {
    private int[] Board { get; }
    public TicTacToe() {
        Board = new int[9];
    }
    public int? CheckWin() {
        for(int i = 0; i < 3; i++) {
            if(Board[i] == Board[i + 3] && Board[i] == Board[i + 6] && Board[i] != 0) {
                return Board[i];
            }
            if(Board[i * 3] == Board[i * 3 + 1] && Board[i * 3] == Board[i * 3 + 2] && Board[i * 3] != 0) {
                return Board[i * 3];
            }
        }
        if(Board[0] == Board[4] && Board[0] == Board[8] && Board[0] != 0) {
            return Board[0];
        }
        if(Board[2] == Board[4] && Board[2] == Board[6] && Board[2] != 0) {
            return Board[2];
        }
        if(Board.Contains(0)) {
            return null;
        }
        return 0;
    }
    public void EveryMove(int player, Func<bool> run) {
        int latest = -1;
        for(int i = 0; i < 9; i++) {
            if(Board[i] == 0) {
                Board[i] = player;
                if(run()) {
                    latest = i;
                }
                Board[i] = 0;
            }
        }
        if(latest != -1) {
            Board[latest] = player;
        }
    }
    public string Print() {
        string returning = "";
        for(int i = 0; i < 9; i++) {
            if(i % 3 == 0) {
                returning += "\n";
            }
            returning += new string[] { "O", "_", "X" }[Board[i] + 1];
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
    private int GetRowCol() {
        Console.Write("Enter your row number (0-2): ");
        int row = GetInt();
        Console.Write("Enter your column number (0-2): ");
        int col = GetInt();
        return row * 3 + col;
    }
    public void GetPlayerTurn() {
        int res = GetRowCol();
        while(Board[res] != 0) {
            Console.WriteLine("That space is already taken, try again!");
            res = GetRowCol();
        }
        Board[res] = -1;
    }
}