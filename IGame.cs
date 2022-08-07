interface IGame {
    int? CheckWin();
    void EveryMove(int player, Action<Action, bool> inner);
    void GetPlayerTurn();
    string Print();
}