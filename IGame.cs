interface IGame {
    int? CheckWin();
    void EveryMove(int player, Func<bool> inner);
    void GetPlayerTurn();
    string Print();
}