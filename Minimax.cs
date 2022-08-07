class Minimax {
    private IGame Game { get; }
    private int numOps { get; set; }
    private ConsoleColor Before { get; }
    public Minimax(IGame game) {
        this.Game = game;
        this.numOps = 0;
        this.Before = Console.ForegroundColor;
    }
    private (int, Action?) Run(int debugLayer, int player = 1, int layer = 0) {
        int? res = Game.CheckWin();
        if(res != null) {
            return ((int)res, null);
        }
        int? best = null;
        Action? bestMove = null;
        Action? nextAction = null;
        Game.EveryMove(player, (Action move, bool goAgain) => {
            numOps++;
            if(numOps % 1000000 == 0) {
                PrintOps();
            }
            (int, Action?) pass = Run(debugLayer, goAgain ? player : -player, layer + 1);
            if(best == null || pass.Item1 * player > best * player) {
                best = pass.Item1;
                bestMove = move;
                nextAction = goAgain ? pass.Item2 : null;
            }
            if(layer <= debugLayer) {
                Console.WriteLine($"Finished operation on layer {layer} - -");
            }
        });
        return (best == null ? 0 : (int)best, () => {
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
    private int? AlgoRun(int debugLayer) {
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
        int? result = Game.CheckWin();
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
        if(result == -1) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Player wins!");
        } else if(result == 0) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("It's a draw!");
        } else if(result == 1) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Algorithm wins!");
        }
        Console.ForegroundColor = Before;
    }
}