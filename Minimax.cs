class Minimax {
    private IGame Game { get; }
    private int numOps { get; set; }
    private ConsoleColor Before { get; }
    public Minimax(IGame game) {
        this.Game = game;
        this.numOps = 0;
        this.Before = Console.ForegroundColor;
    }
    private ((int, int), Action?) Run(int debugLayer, int player = 1, int layer = 0) {
        (int, int)? res = Game.CheckWin();
        if(res != null) {
            return (res.Value, null);
        }
        (int, int)? best = null;
        Action? bestMove = null;
        Action? nextAction = null;
        Game.EveryMove(player, (Action move, bool goAgain) => {
            numOps++;
            if(numOps % 1000000 == 0) {
                //PrintOps();
            }
            ((int, int), Action?) pass = Run(debugLayer, goAgain ? player : -player, layer + 1);
            if(best == null || (player == -1 ? pass.Item1.Item1 : pass.Item1.Item2) > (player == -1 ? best.Value.Item1 : best.Value.Item2)) {
                best = pass.Item1;
                bestMove = move;
                nextAction = goAgain ? pass.Item2 : null;
            }
            if(layer <= debugLayer) {
                Console.WriteLine($"Finished operation on layer {layer} - -");
            }
        });
        return (best == null ? (0, 0) : best.Value, () => {
            if(bestMove != null) {
                bestMove();
            }
            if(nextAction != null) {
                Console.WriteLine("The algorithm made its move! Here's the gamestate: ");
                Console.WriteLine(Print());
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("The algorithm gets to go again!");
                Console.ForegroundColor = Before;
                nextAction();
            }
        });
    }
    public string Print() {
        return Game.Print();
    }
    private void PrintOps() {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        string writing = $"Performed {numOps} operations";
        Console.Write(writing);
        Console.SetCursorPosition(0, Console.CursorTop);
    }
    private (int, int)? AlgoRun(int debugLayer) {
        numOps = 0;
        Action? action = Run(debugLayer).Item2;
        if(action != null) {
            action();
        }
        Console.WriteLine("The algorithm made its move! Here's the gamestate:");
        Console.WriteLine(Print());
        return Game.CheckWin();
    }
    public void Play(bool algoFirst = false, int debugLayer = -1) {
        Console.WriteLine(Print());
        (int, int)? result = Game.CheckWin();
        if(algoFirst) {
            result = AlgoRun(debugLayer);
        }
        while(result == null) {
            Game.GetPlayerTurn();
            Console.WriteLine("You made your move! Here's the gamestate:");
            Console.WriteLine(Print());
            result = Game.CheckWin();
            if(result != null) {
                break;
            }
            result = AlgoRun(debugLayer);
        }
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Game Over!");
        Console.ForegroundColor = Before;
    }
}