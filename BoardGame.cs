class BoardGame : IGame {
    protected int[][] Board { get; }
    private char SpaceSymbol { get; }
    private char P1Symbol { get; }
    private char P2Symbol { get; }
    public BoardGame(int width, int height, char spacesymbol, char p1symbol, char p2symbol) {
        Board = new int[height][];
        for(int i = 0; i < height; i++) {
            Board[i] = new int[width];
        }
        SpaceSymbol = spacesymbol;
        P1Symbol = p1symbol;
        P2Symbol = p2symbol;
    }
    public virtual int? CheckWin() {
        return null;
    }
    public virtual void EveryMove(int player, Action<Action, bool> run) {
        for(int i = 0; i < Board.Length; i++) {
            for(int j = 0; j < Board[i].Length; j++) {
                if(Board[i][j] == 0) {
                    Board[i][j] = player;
                    int savedI = i;
                    int savedJ = j;
                    run(() => {
                        Board[savedI][savedJ] = player;
                    }, false);
                    Board[i][j] = 0;
                }
            }
        }
    }
    public virtual string Print() {
        string returning = "";
        for(int i = 0; i < Board.Length; i++) {
            for(int j = 0; j < Board[i].Length; j++) {
                returning += new char[] { P1Symbol, SpaceSymbol, P2Symbol }[Board[i][j] + 1] + "  ";
            }
            returning += "\n";
        }
        return returning;
    }
    public static string GetInput() {
        string? res = Console.ReadLine();
        while(res == null) {
            Console.WriteLine("No input was detected, try again!");
            res = Console.ReadLine();
        }
        return res;
    }
    public static int GetInt(int max) {
        int r = -1;
        while(r == -1) {
            string res = GetInput();
                try {
                    int p = Int32.Parse(res);
                    if(p < 0 || p > max) {
                        Console.WriteLine($"Please supply an integer from 0 to {max}!");
                    } else {
                        r = p;
                    }
                } catch {
                    Console.WriteLine("Please enter a valid integer input!");
                }
        }
        return r;
    }
    protected (int, int) GetRowCol() {
        Console.Write($"Enter your row number (0 - {Board.Length - 1}): ");
        int row = GetInt(Board.Length - 1);
        Console.Write($"Enter your column number (0 - {Board[0].Length - 1}): ");
        int col = GetInt(Board[0].Length - 1);
        return (row, col);
    }
    public virtual void GetPlayerTurn() {
        (int, int) res = GetRowCol();
        while(Board[res.Item1][res.Item2] != 0) {
            Console.WriteLine("That space is already taken, try again!");
            res = GetRowCol();
        }
        Board[res.Item1][res.Item2] = -1;
    }
}