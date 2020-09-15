using System.Collections.Generic;

[System.Serializable]
public class Steps
{
    public Step homeless;
    public Step peasant;
    public Step rich;
    public Step king;
}
[System.Serializable]
public class SituationImagesPath
{
    public string low;
    public string mid;
    public string high;
}
[System.Serializable]
public class Stat
{
    public int id;
    public float startValue;
    public string icon;
    public string name;
}
[System.Serializable]
public class Stats
{
    public Stat food;
    public Stat work;
    public Stat sleep;
}
[System.Serializable]
public class Step
{
    public SituationImagesPath situation_images_path;
    public Stats stats;
}

