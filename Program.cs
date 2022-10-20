bool[][] map = new bool[][] {
    new bool[] { false, false, false, false, false },
    new bool[] { false, false, false, false, true },
    new bool[] { true, false, true, false, true },
    new bool[] { false, false, true, true, false },
    new bool[] { true, false, true, true, false }
};

Minimax Minimax = new Minimax(new Gerrymanderer(5, 5, 5, map));
Minimax.Play();