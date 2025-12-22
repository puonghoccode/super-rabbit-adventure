public enum EndMenuOutcome
{
    None,
    Victory,
    GameOver
}

public static class EndMenuData
{
    public static EndMenuOutcome Outcome { get; private set; } = EndMenuOutcome.None;
    public static int Coins { get; private set; }
    public static int NextWorld { get; private set; }
    public static int NextStage { get; private set; }
    public static bool HasNextLevel { get; private set; }

    public static void RecordVictory(int coins, int nextWorld, int nextStage)
    {
        Outcome = EndMenuOutcome.Victory;
        Coins = coins;
        NextWorld = nextWorld;
        NextStage = nextStage;
        HasNextLevel = true;
    }

    public static void RecordGameOver(int coins)
    {
        Outcome = EndMenuOutcome.GameOver;
        Coins = coins;
        HasNextLevel = false;
    }

    public static void Clear()
    {
        Outcome = EndMenuOutcome.None;
        Coins = 0;
        NextWorld = 0;
        NextStage = 0;
        HasNextLevel = false;
    }
}
