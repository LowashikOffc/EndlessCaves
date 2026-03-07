using UnityEngine;

[System.Serializable]
public class Achievements
{
    public static Achievements Instance { get; private set; }

    public bool Welcome_to_depth;
    public bool Point_of_no_return;
    public bool Hooked;
    public bool Amateur_speleologist;
    public bool Thrifty;
    public bool Rock_bottom;
    public bool Dust_collector;

    public Achievements()
    {
        Instance = this;
    }
}
