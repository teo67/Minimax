bool[][] map = new bool[][] {
    new bool[] { false, true, false, false },
    new bool[] { true, true, false, false },
    new bool[] { true, true, true, true },
    new bool[] { false, true, false, true }
};

Minimax Minimax = new Minimax(new Gerrymanderer(4, map));
Minimax.Play();