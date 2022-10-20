class Gerrymanderer : IGame {
    private int BoardSize { get; }
    private bool[][] Map { get; }
    private int[][] CurrentMap { get; }
    private int NextInt { get; set; }
    private int X { get; set; }
    private int Y { get; set; }
    public Gerrymanderer(int boardSize, bool[][] map) {
        this.NextInt = 1;
        this.BoardSize = boardSize;
        this.Map = map;
        this.X = 0;
        this.Y = 0;
        this.CurrentMap = new int[boardSize][];
        for(int i = 0; i < boardSize; i++) {
            this.CurrentMap[i] = new int[boardSize];
        }
    }
    public (int, int)? CheckWin() {
        int[] totalAreaSums = new int[BoardSize];
        int[] sums = new int[BoardSize];
        for(int i = 0; i < BoardSize; i++) {
            for(int j = 0; j < BoardSize; j++) {
                if(CurrentMap[i][j] == 0) {
                    return null;
                }
                if(totalAreaSums[CurrentMap[i][j] - 1] >= BoardSize) {
                    return null;
                }
                totalAreaSums[CurrentMap[i][j] - 1]++;
                if(Map[i][j]) {
                    sums[CurrentMap[i][j] - 1]++;
                }
            }
        }
        int total = 0;
        for(int i = 0; i < BoardSize; i++) {
            if(totalAreaSums[i] != BoardSize) {
                return null;
            }
            if(2 * sums[i] > BoardSize) {
                total++;
            }
        }
        return (total, total);
    }
    private void IncrementPosition() {
        X++;
        if(X >= BoardSize) {
            X = 0;
            Y++;
        }
    }
    private void DecrementPosition() {
        X--;
        if(X < 0) {
            X = BoardSize - 1;
            Y--;
        }
    }
    public void EveryMove(int player, Action<Action, bool> run) {
        if(X >= BoardSize || Y >= BoardSize) {
            return;
        }
        int savedX = X;
        int savedY = Y;
        if(X > 0) {
            int left = CurrentMap[Y][X - 1];
            CurrentMap[Y][X] = left;
            IncrementPosition();
            run(() => {
                CurrentMap[savedY][savedX] = left;
                IncrementPosition();
            }, false);
            CurrentMap[savedY][savedX] = 0;
            DecrementPosition();
        }
        if(Y > 0) {
            int above = CurrentMap[Y - 1][X];
            CurrentMap[Y][X] = above;
            IncrementPosition();
            run(() => {
                CurrentMap[savedY][savedX] = above;
                IncrementPosition();
            }, false);
            CurrentMap[savedY][savedX] = 0;
            DecrementPosition();
        }
        if(NextInt <= BoardSize) {
            int savedNext = NextInt;
            CurrentMap[Y][X] = NextInt;
            NextInt++;
            IncrementPosition();
            run(() => {
                CurrentMap[savedY][savedX] = savedNext;
                NextInt = savedNext + 1;
                IncrementPosition();
            }, false);
            CurrentMap[savedY][savedX] = 0;
            NextInt--;
            DecrementPosition();
        }
    }
    public void GetPlayerTurn() {}
    public string Print() {
        string returning = "";
        for(int i = 0; i < BoardSize; i++) {
            for(int j = 0; j < BoardSize; j++) {
                returning += CurrentMap[i][j] + " ";
            }
            returning += "\n";
        }
        return returning;
    }
}