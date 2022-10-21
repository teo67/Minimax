class Gerrymanderer2 : IGame {
    private int Width { get; }
    private int Height { get; }
    private int Size { get; }
    private bool[][] Map { get; }
    private int[][] CurrentMap { get; }
    private List<(int, int)> Borders { get; set; }
    private int CurrentSize { get; set; }
    private int CurrentNum { get; set; }
    private int NumPossibleWins { get; }
    private bool ItsAllOver { get; set; }
    public Gerrymanderer2(int width, int height, int districtSize, bool[][] map) {
        if((width * height) % districtSize != 0) {
            throw new Exception("The districts will not fit in the area evenly!");
        }
        this.Size = districtSize;
        this.Width = width;
        this.Height = height;
        this.Map = map;
        this.CurrentSize = 0;
        this.CurrentNum = 1;
        this.Borders = new List<(int, int)>();
        this.Borders.Add((0, 0));
        this.CurrentMap = new int[Height][];

        int trueCount = 0;
        for(int i = 0; i < Height; i++) {
            this.CurrentMap[i] = new int[Width];
            for(int j = 0; j < Width; j++) {
                if(map[i][j]) {
                    trueCount++;
                }
            }
        }
        int minwin = (int)Math.Floor(Size / 2.0) + 1;
        this.NumPossibleWins = (int)Math.Floor(trueCount / minwin + 0.0);
        this.ItsAllOver = false;
    }
    public (int, int)? CheckWin() {
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
            if(2 * sums[i] > Size) {
                total++;
            }
        }
        if(total >= NumPossibleWins) {
            ItsAllOver = true;
        }
        return (total, total);
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
        
        if(ItsAllOver || CurrentNum > Width * Height / Size) {
            return;
        }
        if(CurrentSize >= Size) {
            if(CurrentNum == Width * Height / Size) {
                return;
            }
            (int, int) nextPos = (-1, -1);
            for(int i = 0; i < Height && nextPos == (-1, -1); i++) {
                for(int j = 0; j < Width && nextPos == (-1, -1); j++) {
                    if(CurrentMap[i][j] == 0) {
                        nextPos = (j, i);
                    }
                }
            }
            if(nextPos != (-1, -1)) {
                CurrentNum++;
                CurrentSize = 0;
                List<(int, int)> _borders = new List<(int, int)>();
                _borders.Add(nextPos);
                List<(int, int)> saved = Borders;
                Borders = _borders;
                run(() => {}, true);
                CurrentNum--;
                CurrentSize = Size;
                Borders = saved;
            }
            return;
        }
        CurrentSize++;
        int savedNum = CurrentNum;
        for(int i = 0; i < Borders.Count; i++) {
            int savedX = Borders[i].Item1;
            int savedY = Borders[i].Item2;
            if(CurrentSize == Size) {
                CurrentMap[Borders[i].Item2][Borders[i].Item1] = savedNum;
                run(() => {
                    CurrentMap[savedY][savedX] = savedNum;
                }, true);
            } else {
                List<(int, int)> adding = new List<(int, int)>();
                if(savedY > 0 && CurrentMap[savedY - 1][savedX] == 0 && !Borders.Contains((savedX, savedY - 1))) {
                    Borders.Add((savedX, savedY - 1));
                    adding.Add((savedX, savedY - 1));
                }
                if(savedY < Height - 1 && CurrentMap[savedY + 1][savedX] == 0 && !Borders.Contains((savedX, savedY + 1))) {
                    Borders.Add((savedX, savedY + 1));
                    adding.Add((savedX, savedY + 1));
                }
                if(savedX > 0 && CurrentMap[savedY][savedX - 1] == 0 && !Borders.Contains((savedX - 1, savedY))) {
                    Borders.Add((savedX - 1, savedY));
                    adding.Add((savedX - 1, savedY));
                }
                if(savedX < Width - 1 && CurrentMap[savedY][savedX + 1] == 0 && !Borders.Contains((savedX + 1, savedY))) {
                    Borders.Add((savedX + 1, savedY));
                    adding.Add((savedX + 1, savedY));
                }
                CurrentMap[Borders[i].Item2][Borders[i].Item1] = savedNum;
                Borders.RemoveAt(i);
                run(() => {
                    CurrentMap[savedY][savedX] = savedNum;
                }, true);
                Borders.Insert(i, (savedX, savedY));
                foreach((int, int) pos in adding) {
                    Borders.Remove(pos);
                }
            }
            CurrentMap[savedY][savedX] = 0;
        }
        CurrentSize--;
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