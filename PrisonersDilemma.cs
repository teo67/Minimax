class PrisonersDilemma : IGame {
    private int PlayerSentence { get; set; }
    private int AlgoSentence { get; set; }
    private int TurnsLeft { get; set; }
    private bool? Cooperates { get; set; }
    private int BothCoopValue { get; }
    private int CoopValue { get; }
    private int DefectValue { get; }
    private int BothDefectValue { get; }
    public PrisonersDilemma(int numTurns, int bothCoopValue = 3, int coopValue = 0, int defectValue = 5, int bothDefectValue = 1) {
        this.PlayerSentence = 0;
        this.AlgoSentence = 0;
        this.TurnsLeft = numTurns;
        BothCoopValue = bothCoopValue;
        CoopValue = coopValue;
        DefectValue = defectValue;
        BothDefectValue = bothDefectValue;
    }
    public (int, int)? CheckWin() {
        if(TurnsLeft > 0) {
            return null;
        }
        return (PlayerSentence, AlgoSentence);
    }
    private void UpdateSentence(int player, int numYears = 1) {
        if(player == -1) {
            PlayerSentence += numYears;
        } else {
            AlgoSentence += numYears;
        }
    }
    private (int, int) UpdateSentences(int player, bool cooperates, bool print) {
        if(Cooperates == null) {
            return (0, 0);
        }
        ConsoleColor previous = ConsoleColor.White;
        if(print) {
            previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n");
        }
        (int, int) returning = (0, 0);
        if((bool)Cooperates && cooperates) {
            if(print) {
                Console.WriteLine($"Both players chose to cooperate, so {BothCoopValue} years were removed from both of their sentences.");
            }
            UpdateSentence(player, BothCoopValue);
            UpdateSentence(-player, BothCoopValue);
            returning = (BothCoopValue, BothCoopValue);
        } else if((bool)Cooperates) {
            if(print) {
                Console.WriteLine($"The {(player == -1 ? "human" : "algorithm")} defected, so {DefectValue} years will be removed from their sentence and {CoopValue} years will be removed from the other player's sentence.");
            }
            UpdateSentence(player, DefectValue);
            UpdateSentence(-player, CoopValue);
            returning = (CoopValue, DefectValue);
        } else if(cooperates) {
            if(print) {
                Console.WriteLine($"The {(player == -1 ? "algorithm" : "human")} defected, so {DefectValue} years will be removed from their sentence and {CoopValue} years will be removed from the other player's sentence.");
            }
            UpdateSentence(-player, DefectValue);
            UpdateSentence(player, CoopValue);
            returning = (DefectValue, CoopValue);
        } else {
            if(print) {
                Console.WriteLine($"Both players defected, so {BothDefectValue} years was removed from both of their sentences.");
            }
            UpdateSentence(player, BothDefectValue);
            UpdateSentence(-player, BothDefectValue);
            returning = (BothDefectValue, BothDefectValue);
        }
        if(print) {
            Console.Write("\n");
            Console.ForegroundColor = previous;
        }
        return returning;
    }
    public void EveryMove(int player, Action<Action, bool> run) {
        for(int i = 0; i < 2; i++) {
            bool cooperates = i == 1;
            if(Cooperates == null) {
                Cooperates = cooperates;
                run(() => {
                    Cooperates = cooperates;
                }, false);
                Cooperates = null;
            } else {
                (int, int) res = UpdateSentences(player, cooperates, false);
                bool savedCoop = (bool)Cooperates;
                Cooperates = null;
                TurnsLeft--;
                run(() => {
                    Cooperates = savedCoop;
                    UpdateSentences(player, cooperates, true);
                    Cooperates = null;
                    TurnsLeft--;
                }, false);
                UpdateSentence(-player, -res.Item1);
                UpdateSentence(player, -res.Item2);
                Cooperates = savedCoop;
                TurnsLeft++;
            }
        }
    }
    public void GetPlayerTurn() {
        Console.Write($"If you both cooperate, your sentences will both be decreased by {BothCoopValue} years. If one of you defects, that person's sentence will be decreased by {DefectValue} years while the other sentence will be decreased by {CoopValue} years. If both of you defect, both of your sentences will be decreased by {BothDefectValue} years.\n\nDo you choose to cooperate (y/yes)? ");
        string response = BoardGame.GetInput();
        bool choosesToCooperate = response == "y" || response == "yes";
        if(Cooperates == null) {
            Cooperates = choosesToCooperate;
        } else {
            UpdateSentences(-1, choosesToCooperate, true);
            Cooperates = null;
            TurnsLeft--;
        }
    }
    public string Print() {
        return $"Player years removed from sentence: {PlayerSentence}.\nAlgorithm years removed from sentence: {AlgoSentence}.";
    }
}