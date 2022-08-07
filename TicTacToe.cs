class TicTacToe : BoardGame {
    public TicTacToe(int width = 3, int height = 3) : base(width, height, '_', 'O', 'X') {}
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
    public override int? CheckWin() {
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
        for(int i = 0; i < Board[0].Length; i++) {
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
}