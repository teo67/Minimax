/*

NO LONGER IN USE!!! gerrymanderer2 is better!!!! #noncontiguousmoment

*/



// class Gerrymanderer : IGame {
//     private int Width { get; }
//     private int Height { get; }
//     private int Size { get; }
//     private bool[][] Map { get; }
//     private int[][] CurrentMap { get; }
//     private int[] DistrictCounts { get; }
//     private int NextInt { get; set; }
//     private int X { get; set; }
//     private int Y { get; set; }
//     public Gerrymanderer(int width, int height, int districtSize, bool[][] map) {
//         if((width * height) % districtSize != 0) {
//             throw new Exception("The districts will not fit in the area evenly!");
//         }
//         this.NextInt = 1;
//         this.Size = districtSize;
//         this.Width = width;
//         this.Height = height;
//         this.Map = map;
//         this.DistrictCounts = new int[Width * Height / districtSize];
//         this.X = 0;
//         this.Y = 0;
//         this.CurrentMap = new int[Height][];
//         for(int i = 0; i < Height; i++) {
//             this.CurrentMap[i] = new int[Width];
//         }
//     }
//     public (int, int)? CheckWin() {
//         if(CheckEquals("1111122223444234553000000")) {
//             Console.WriteLine(Print());
//         }
//         int[] sums = new int[Width * Height / Size];
//         for(int i = 0; i < Height; i++) {
//             for(int j = 0; j < Width; j++) {
//                 if(CurrentMap[i][j] <= 0) {
//                     return null;
//                 }
//                 if(Map[i][j]) {
//                     sums[CurrentMap[i][j] - 1]++;
//                 }
//             }
//         }
//         int total = 0;
//         for(int i = 0; i < Width * Height / Size; i++) {
//             if(DistrictCounts[i] != Size) {
//                 return null;
//             }
//             if(2 * sums[i] > Size) {
//                 total++;
//             }
//         }
//         //Console.WriteLine(total);
//         return (total, total);
//     }
//     private void IncrementPosition() {
//         X++;
//         if(X >= Width) {
//             X = 0;
//             Y++;
//         }
//     }
//     private void DecrementPosition() {
//         X--;
//         if(X < 0) {
//             X = Width - 1;
//             Y--;
//         }
//     }
//     private bool CanBlockOffLeft(int x, int y, int left) {
//         if(left == -1) {
//             return true;
//         }
//         if(y < Height - 1 || DistrictCounts[Math.Abs(left) - 1] >= Size) {
//             return true;
//         }
//         if(y == 0) {
//             return false; // not sure when this would even happen
//         }
//         int index = Array.LastIndexOf(CurrentMap[y - 1], left);
//         return index > x;
//     }
//     private bool CanBlockOffTop(int x, int y, int above) {
//         if(above == -1) {
//             return true;
//         }
//         if(DistrictCounts[Math.Abs(above) - 1] >= Size) {
//             return true;
//         }
//         if(y < Height - 1 && x > 0) {
//             int indexInCurrent = Array.IndexOf(CurrentMap[y], above);
//             if(indexInCurrent != -1) {
//                 return true;
//             }
//         }
//         int index = Array.LastIndexOf(CurrentMap[y - 1], above);
//         return index > x;
//     }
//     private bool CheckEquals(string input) {
//         int counter = 0;
//         for(int i = 0; i < Height; i++) {
//             for(int j = 0; j < Width; j++) {
//                 if(Int32.Parse(input[counter].ToString()) != CurrentMap[i][j]) {
//                     return false;
//                 }
//                 counter++;
//             }
//         }
//         return true;
//     }
//     private void TakeMove(Action<Action, bool> run, int x, int y, int choice) {
//         DistrictCounts[choice - 1]++;
//         CurrentMap[y][x] = choice;
//         IncrementPosition();
//         run(() => {
//             CurrentMap[y][x] = choice;
//             DistrictCounts[choice - 1]++;
//             IncrementPosition();
//         }, true);
//         CurrentMap[y][x] = 0;
//         DistrictCounts[choice - 1]--;
//         DecrementPosition();
//     }
//     public void EveryMove(int player, Action<Action, bool> run) {
//         if(X >= Width || Y >= Height) {
//             return;
//         }
//         int savedX = X;
//         int savedY = Y;
//         int above = (Y == 0) ? -1 : CurrentMap[Y - 1][X];
//         int actualAbove = Math.Abs(above);
//         int left = (X == 0) ? -2 : CurrentMap[Y][X - 1];
//         int actualLeft = Math.Abs(left);
//         bool canBlockOffTop = CanBlockOffTop(X, Y, above);
//         bool canBlockOffLeft = CanBlockOffLeft(X, Y, left);
//         List<int> already = new List<int>();
//         if(X > 0) {
//             if(DistrictCounts[actualLeft - 1] < Size && (canBlo
//             ckOffLeft || actualAbove == actualLeft)) {
//                 already.Add(actualLeft);
//                 TakeMove(run, savedX, savedY, left);
//             }
//         }
//         if(Y > 0) {
//             if(!already.Contains(actualAbove) && DistrictCounts[actualAbove - 1] < Size && canBlockOffLeft) {
//                 already.Add(actualAbove);
//                 TakeMove(run, savedX, savedY, above);
//             }
//         }
//         if(canBlockOffLeft && canBlockOffTop) {
//             if(X > 1) {
//                 int[] currentRow = CurrentMap[Y];
//                 for(int i = X - 2; i >= 0; i--) {
//                     int actualVal = Math.Abs(currentRow[i]);
//                     if(!already.Contains(actualVal) && DistrictCounts[actualVal  - 1] < Size && actualVal != Math.Abs(currentRow[X - 1]) && (Y == 0 || actualVal != C(CurrentMap[Y - 1][X]))) {
//                         already.Add(actualVal);
//                         TakeMove(run, savedX, savedY, currentRow[i] + (Width * Height / Size));
//                     }
//                 }
//             }
//             if(X < Width - 1 && Y > 0) { // noncontiguous groups
//                 int[] aboveRow = CurrentMap[Y - 1];
//                 for(int i = Width - 1; i > X; i--) {
//                     int actualVal = C(aboveRow[i]);
//                     if(!already.Contains(actualVal) && DistrictCounts[actualVal - 1] < Size && actualVal != C(aboveRow[X]) && (X == 0 || actualVal != C(CurrentMap[Y][X - 1]))) {
//                         already.Add(actualVal);
//                         TakeMove(run, savedX, savedY, aboveRow[i] + (Width * Height / Size));
//                     }
//                 }
//             }
//         }
        
//         if(NextInt <= Width * Height / Size && canBlockOffTop && canBlockOffLeft) { // add a new one
//             int savedNext = NextInt;
//             CurrentMap[Y][X] = NextInt;
//             DistrictCounts[NextInt - 1]++;
//             NextInt++;
//             IncrementPosition();
//             run(() => {
//                 CurrentMap[savedY][savedX] = savedNext;
//                 NextInt = savedNext + 1;
//                 DistrictCounts[savedNext - 1]++;
//                 IncrementPosition();
//             }, true);
//             CurrentMap[savedY][savedX] = 0;
//             DistrictCounts[savedNext - 1]--;
//             NextInt--;
//             DecrementPosition();
//         }
//     }
//     public void GetPlayerTurn() {}
//     public string Print() {
//         string returning = "";
//         for(int i = 0; i < Height; i++) {
//             for(int j = 0; j < Width; j++) {
//                 returning += CurrentMap[i][j] + " ";
//             }
//             returning += "\n";
//         }
//         return returning;
//     }
// }