public class PlayerDetails
{
    public enum Situtation
    {
        HOMELESS_LOW,
        HOMELESS_MID,
        HOMELESS_HIGH,
        PEASANT_LOW,
        PEASANT_MID,
        PEASANT_HIGH,
        RICH_LOW,
        RICH_MID,
        RICH_HIGH,
        KING_LOW,
        KING_MID,
        KING_HIGH,
    }

    public string name;
    public string genre;
    public string hairBack;
    public string skin;
    public string eyes;
    public string face;
    public string hairFront;
    public string clothes;
    public Situtation situtation;
    public int yearCountSurvive;
    public int generationCountSurvive;
    public int food;
    public int work;
    public int sleep;
}
