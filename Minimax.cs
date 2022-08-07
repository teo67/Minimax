class Minimax {
    private IGame Game { get; }
    private int numOps { get; set; }
    public Minimax(IGame game) {
        this.Game = game;
        this.numOps = 0;
    }
    private int Run(int player = 1, int layer = 0) {
        int? res = Game.CheckWin();
        if(res != null) {
            return (int)res;
        }
        int? best = null;
        Action? bestMove = null;
        Game.EveryMove(player, (Action move) => {
            numOps++;
            if(numOps % 100000 == 0) {
                PrintOps();
            }
            int pass = Run(-player, layer + 1);
            if(best == null || pass * player > best * player) {
                best = pass;
                bestMove = move;
            }
        });
        if(layer == 0 && bestMove != null) {
            bestMove();
        }
        return best == null ? 0 : (int)best;
    }
    private string Print() {
        return Game.Print();
    }
    private void PrintOps() {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        string writing = $"Performed {numOps} operations";
        Console.Write(writing);
        Console.SetCursorPosition(0, Console.CursorTop);
    }
    public void Play() {
        int? result = Game.CheckWin();
        while(result == null) {
            Game.GetPlayerTurn();
            Console.WriteLine("You made your move! Here's the gamestate:");
            Console.WriteLine(Print());
            result = Game.CheckWin();
            if(result != null) {
                break;
            }
            numOps = 0;
            Run();
            Console.WriteLine("The algorithm made its move! Here's the gamestate:");
            Console.WriteLine(Print());
            result = Game.CheckWin();
        }
        if(result == -1) {
            Console.WriteLine("Player wins!");
        } else if(result == 0) {
            Console.WriteLine("Draw!");
        } else if(result == 1) {
            Console.WriteLine("Algorithm wins!");
        }
    }
}