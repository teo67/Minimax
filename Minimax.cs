class Minimax {
    private IGame Game { get; }
    public Minimax(IGame game) {
        this.Game = game;
    }
    public int Run(int player = 1, bool isOuter = true) {
        int? res = Game.CheckWin();
        if(res != null) {
            //Console.WriteLine("returning");
            return (int)res;
        }
        int? best = null;
        Game.EveryMove(player, () => {
            //Console.WriteLine("going deeper");
            int pass = Run(-player, false);
            if(best == null || pass * player > best * player) {
                best = pass;
                return isOuter;
            }
            return false;
        });
        if(best == null) {
            throw new Exception("test");
        }
        return best == null ? 0 : (int)best;
    }
    public string Print() {
        return Game.Print();
    }
    public void Play() {
        while(Game.CheckWin() == null) {
            Game.GetPlayerTurn();
            Console.WriteLine("You made your move! Here's the gamestate:");
            Console.WriteLine(Print());
            Run();
            Console.WriteLine("The algorithm made its move! Here's the gamestate:");
            Console.WriteLine(Print());
        }
    }
}