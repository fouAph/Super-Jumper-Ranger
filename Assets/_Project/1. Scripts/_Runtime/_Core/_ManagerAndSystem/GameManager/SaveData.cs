using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    private static SaveData _current;

    public static SaveData Current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
        set
        {
            if (value != null)
            {
                _current = value;
            }
        }
    }

    public Level[] levels;
    public PlayerProfile profile;
}
[System.Serializable]
public struct Level
{
    public int highScore;
    public bool unlocked;
}
