bool[][] map = new bool[][] {
    new bool[] { false, false, false, false, false, true }, 
    new bool[] { false, true, true, true, false, false },
    new bool[] { false, true, true, true, true, false },
    new bool[] { false, false, false, false, false, false },
    new bool[] { false, true, false, true, true, false },
    new bool[] { false, false, false, false, false, true }
};
// bool[][] map = new bool[][] {
//     new bool[] { true, true, false }, 
//     new bool[] { false, true, true },
//     new bool[] { false, true, true }
// };

Minimax Minimax = new Minimax(new Gerrymanderer2(6, 6, 6, map));
Minimax.Play(true, 3);