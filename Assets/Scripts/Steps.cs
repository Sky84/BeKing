using System.Collections.Generic;

[System.Serializable]
public class Steps
{
    public Homeless homeless;
    public Peasant peasant;
    public Rich rich;
    public King king;
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
    public double startValue;
    public string icon;
    public string name;
}
[System.Serializable]
public class Homeless
{
    public SituationImagesPath situation_images_path;
    public List<Stat> stats;
}
[System.Serializable]
public class Peasant
{
    public SituationImagesPath situation_images_path;
    public List<Stat> stats;
}
[System.Serializable]
public class Rich
{
    public SituationImagesPath situation_images_path;
    public List<Stat> stats;
}
[System.Serializable]
public class King
{
    public SituationImagesPath situation_images_path;
    public List<Stat> stats;
}

