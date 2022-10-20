class Gerrymanderer : IGame {
    private int Width { get; }
    private int Height { get; }
    private int Size { get; }
    private bool[][] Map { get; }
    private int[][] CurrentMap { get; }
    private int[] DistrictCounts { get; }
    private int NextInt { get; set; }
    private int X { get; set; }
    private int Y { get; set; }
    public Gerrymanderer(int width, int height, int districtSize, bool[][] map) {
        if((width * height) % districtSize != 0) {
            throw new Exception("The districts will not fit in the area evenly!");
        }
        this.NextInt = 1;
        this.Size = districtSize;
        this.Width = width;
        this.Height = height;
        this.Map = map;
        this.DistrictCounts = new int[Width * Height / districtSize];
        this.X = 0;
        this.Y = 0;
        this.CurrentMap = new int[Height][];
        for(int i = 0; i < Height; i++) {
            this.CurrentMap[i] = new int[Width];
        }
    }
    public (int, int)? CheckWin() {
        if(CheckEquals("1111122223444234553000000")) {
            Console.WriteLine(Print());
        }
        int[] sums = new int[Width * Height / Size];
        for(int i = 0; i < Height; i++) {
            for(int j = 0; j < Width; j++) {
                if(CurrentMap[i][j] == 0) {
                    return null;
                }
                if(Map[i][j]) {
                    sums[CurrentMap[i][j] - 1]++;
                }
            }
        }
        int total = 0;
        for(int i = 0; i < Width * Height / Size; i++) {
            if(DistrictCounts[i] != Size) {
                return null;
            }
            if(2 * sums[i] > Size) {
                total++;
            }
        }
        //Console.WriteLine(total);
        return (total, total);
    }
    private void IncrementPosition() {
        X++;
        if(X >= Width) {
            X = 0;
            Y++;
        }
    }
    private void DecrementPosition() {
        X--;
        if(X < 0) {
            X = Width - 1;
            Y--;
        }
    }
    private bool CanBlockOffLeft(int x, int y) {
        if(x == 0) {
            return true;
        }
        int left = CurrentMap[y][x - 1];
        if(y < Height - 1 || DistrictCounts[left - 1] >= Size) {
            return true;
        }
        if(y == 0) {
            return false; // not sure when this would even happen
        }
        int index = Array.LastIndexOf(CurrentMap[y - 1], left);
        return index > x;
    }
    private bool CanBlockOffTop(int x, int y) {
        if(y == 0) {
            return true;
        }
        int above = CurrentMap[y - 1][x];
        if(DistrictCounts[above - 1] >= Size) {
            return true;
        }
        if(y < Height - 1 && x > 0) {
            int indexInCurrent = Array.IndexOf(CurrentMap[y], above);
            if(indexInCurrent != -1) {
                return true;
            }
        }
        int index = Array.LastIndexOf(CurrentMap[y - 1], above);
        return index > x;
    }
    private bool CheckEquals(string input) {
        int counter = 0;
        for(int i = 0; i < Height; i++) {
            for(int j = 0; j < Width; j++) {
                if(Int32.Parse(input[counter].ToString()) != CurrentMap[i][j]) {
                    return false;
                }
                counter++;
            }
        }
        return true;
    }
    public void EveryMove(int player, Action<Action, bool> run) {
        if(X >= Width || Y >= Height) {
            return;
        }
        int savedX = X;
        int savedY = Y;
        bool canBlockOffTop = CanBlockOffTop(X, Y);
        bool canBlockOffLeft = CanBlockOffLeft(X, Y);
        List<int> already = new List<int>();
        if(X > 0) {
            int left = CurrentMap[Y][X - 1];
            if(DistrictCounts[left - 1] < Size && (canBlockOffTop || (Y > 0 && CurrentMap[Y - 1][X] == left))) {
                DistrictCounts[left - 1]++;
                CurrentMap[Y][X] = left;
                IncrementPosition();
                already.Add(left);
                run(() => {
                    CurrentMap[savedY][savedX] = left;
                    DistrictCounts[left - 1]++;
                    IncrementPosition();
                }, true);
                CurrentMap[savedY][savedX] = 0;
                DistrictCounts[left - 1]--;
                DecrementPosition();
            }
        }
        if(Y > 0) {
            int above = CurrentMap[Y - 1][X];
            if(!already.Contains(above) && DistrictCounts[above - 1] < Size && canBlockOffLeft) {
                DistrictCounts[above - 1]++;
                CurrentMap[Y][X] = above;
                IncrementPosition();
                already.Add(above);
                run(() => {
                    CurrentMap[savedY][savedX] = above;
                    DistrictCounts[above - 1]++;
                    IncrementPosition();
                }, true);
                CurrentMap[savedY][savedX] = 0;
                DistrictCounts[above - 1]--;
                DecrementPosition();
            }
        }
        if(X < Width - 1 && Y > 0 && canBlockOffLeft && canBlockOffTop) {
            int[] aboveRow = CurrentMap[Y - 1];
            for(int i = X + 1; i < Width; i++) {
                // if(!already.Contains(aboveRow[i]))
            }
        }
        if(NextInt <= Width * Height / Size && canBlockOffTop && canBlockOffLeft) {
            int savedNext = NextInt;
            CurrentMap[Y][X] = NextInt;
            DistrictCounts[NextInt - 1]++;
            NextInt++;
            IncrementPosition();
            run(() => {
                CurrentMap[savedY][savedX] = savedNext;
                NextInt = savedNext + 1;
                DistrictCounts[savedNext - 1]++;
                IncrementPosition();
            }, true);
            CurrentMap[savedY][savedX] = 0;
            DistrictCounts[savedNext - 1]--;
            NextInt--;
            DecrementPosition();
        }
    }
    public void GetPlayerTurn() {}
    public string Print() {
        string returning = "";
        for(int i = 0; i < Height; i++) {
            for(int j = 0; j < Width; j++) {
                returning += CurrentMap[i][j] + " ";
            }
            returning += "\n";
        }
        return returning;
    }
}