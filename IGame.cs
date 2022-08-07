interface IGame {
    int? CheckWin();
    void EveryMove(int player, Action<Action> inner);
    void GetPlayerTurn();
    string Print();
}