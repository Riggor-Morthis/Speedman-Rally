public static class ChampionshipData
{
    #region Variables
    public static int accelerationStat { get; private set; }
    public static int topSpeedStat { get; private set; }
    public static int handlingStat { get; private set; }
    public static int brakingStat { get; private set; }
    public static int stageNumber { get; private set; }
    #endregion

    #region PublicMethods
    public static void InitData()
    {
        accelerationStat = 3;
        topSpeedStat = 3;
        handlingStat = 3;
        brakingStat = 3;
        stageNumber = 0;
    }

    public static void IncreaseAcceleration() => accelerationStat++;
    public static void IncreaseTopSpeed() => topSpeedStat++;
    public static void IncreaseHandling() => handlingStat++;
    public static void IncreaseBraking() => brakingStat++;
    public static void IncreaseStageNumber() => stageNumber++;
    public static int GetStatTotal() => accelerationStat + topSpeedStat
        + handlingStat + brakingStat;
    #endregion
}
