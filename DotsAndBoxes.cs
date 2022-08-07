class DotsAndBoxes : IGame {
    private int P1Score { get; set; }
    private int P2Score { get; set; }
    private bool[][] Horizontals { get; }
    private bool[][] Verticals { get; } // Board represents horizontals
    public DotsAndBoxes(int width, int height) {
        P1Score = 0;
        P2Score = 0;
        Horizontals = new bool[height + 1][];
        for(int i = 0; i <= height; i++) {
            Horizontals[i] = new bool[width];
        }
        Verticals = new bool[height][];
        for(int i = 0; i < height; i++) {
            Verticals[i] = new bool[width + 1];
        }
    }
    private bool IsOver() {
        for(int i = 0; i < Horizontals.Length; i++) {
            for(int j = 0; j < Horizontals[i].Length; j++) {
                if(!Horizontals[i][j]) {
                    return false;
                }
            }
        }
        for(int i = 0; i < Verticals.Length; i++) {
            for(int j = 0; j < Verticals[i].Length; j++) {
                if(!Verticals[i][j]) {
                    return false;
                }
            }
        }
        return true;
    }
    public int? CheckWin() {
        if(!IsOver()) {
            return null;
        }
        if(P1Score > P2Score) {
            return -1;
        }
        if(P2Score > P1Score) {
            return 1;
        }
        return 0;
    }
    private void UpdateScore(int player, int howMuch = 1) {
        if(player == -1) {
            P1Score += howMuch;
        } else {
            P2Score += howMuch;
        }
    }
    private int Check(int player, bool[][] arr1, bool[][] arr2, (int, int, int) Is, (int, int, int) Js) {
        if(arr1[Is.Item1][Js.Item1] && arr2[Is.Item2][Js.Item2] && arr2[Is.Item3][Js.Item3]) {
            UpdateScore(player);
            return 1;
        }
        return 0;
    }
    private int UpdateScoreHorizontal(int player, int i, int j) {
        int returning = 0;
        if(i > 0) {
            returning += Check(player, Horizontals, Verticals, (i - 1, i - 1, i - 1), (j, j, j + 1));
        }
        if(i < Horizontals.Length - 1) {
            returning += Check(player, Horizontals, Verticals, (i + 1, i, i), (j, j, j + 1));
        }
        return returning;
    }
    private int UpdateScoreVertical(int player, int i, int j) {
        int returning = 0;
        if(j > 0) {
            returning += Check(player, Verticals, Horizontals, (i, i, i + 1), (j - 1, j - 1, j - 1));
        }
        if(j < Verticals[0].Length - 1) {
            returning += Check(player, Verticals, Horizontals, (i, i, i + 1), (j + 1, j, j));
        }
        return returning;
    }
    public void EveryMove(int player, Action<Action, bool> run) {
        for(int i = 0; i < Horizontals.Length; i++) {
            for(int j = 0; j < Horizontals[i].Length; j++) {
                if(!Horizontals[i][j]) {
                    Horizontals[i][j] = true;
                    int savedI = i;
                    int savedJ = j;
                    int scoreChange = UpdateScoreHorizontal(player, i, j);
                    run(() => {
                        Horizontals[savedI][savedJ] = true;
                        UpdateScoreHorizontal(player, savedI, savedJ);
                    }, scoreChange > 0);
                    Horizontals[i][j] = false;
                    UpdateScore(player, -scoreChange);
                }
            }
        }
        for(int i = 0; i < Verticals.Length; i++) {
            for(int j = 0; j < Verticals[i].Length; j++) {
                if(!Verticals[i][j]) {
                    Verticals[i][j] = true;
                    int savedI = i;
                    int savedJ = j;
                    int scoreChange = UpdateScoreVertical(player, i, j);
                    run(() => {
                        Verticals[savedI][savedJ] = true;
                        UpdateScoreVertical(player, savedI, savedJ);
                    }, scoreChange > 0);
                    Verticals[i][j] = false;
                    UpdateScore(player, -scoreChange);
                }
            }
        }
    }
    private bool[][] GetChosen() {
        Console.Write("Would you like to make a horizontal or vertical line (h/horizontal or v/vertical)? ");
        Dictionary<string, bool[][]> key = new Dictionary<string, bool[][]>() {
            { "h", Horizontals },
            { "horizontal", Horizontals },
            { "v", Verticals }, 
            { "vertical", Verticals }
        };
        string response = BoardGame.GetInput();
        while(!key.ContainsKey(response)) {
            Console.Write("Your response should be either h, horizontal, v, or vertical! Try again: ");
            response = BoardGame.GetInput();
        }
        return key[response];
    }
    private (int, int) GetRowCol(bool[][] chosen) {
        Console.Write($"Please select your row (0 - {chosen.Length - 1}). For horizontal lines, this number corresponds to the row under your line: ");
        int row = BoardGame.GetInt(chosen.Length - 1);
        Console.Write($"Please select your column (0 - {chosen[row].Length - 1}). For vertical lines, this number corresponds to the column to the right of your line: ");
        int col = BoardGame.GetInt(chosen[row].Length - 1);
        return (row, col);
    }
    public void GetPlayerTurn() {
        bool moving = true;
        while(moving) {
            bool[][] chosen = GetChosen();
            (int, int) res = GetRowCol(chosen);
            while(chosen[res.Item1][res.Item2]) {
                Console.Write("That line is already taken, please try again: ");
                chosen = GetChosen();
                res = GetRowCol(chosen);
            }
            chosen[res.Item1][res.Item2] = true;
            int adding = chosen == Horizontals ? UpdateScoreHorizontal(-1, res.Item1, res.Item2) : UpdateScoreVertical(-1, res.Item1, res.Item2);
            if(adding > 0 && !IsOver()) {
                ConsoleColor previous = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("You get to go again! Here's the board:");
                Console.ForegroundColor = previous;
                Console.WriteLine(Print());
            } else {
                moving = false;
            }
        }
    }
    public string Print() {
        string returning = "";
        for(int i = 0; i < Horizontals.Length; i++) {
            for(int j = 0; j < Horizontals[i].Length; j++) {
                returning += ".";
                returning += Horizontals[i][j] ? "---" : "   ";
            }
            returning += ".";
            if(i != Horizontals.Length - 1) {
                returning += "\n";
                for(int j = 0; j < Verticals[i].Length; j++) {
                    returning += Verticals[i][j] ? "|   " : "    ";
                }
            }
            returning += "\n";
        }
        returning += $"Player score: {P1Score}\n";
        returning += $"Algorithm score: {P2Score}";
        return returning;
    }
}