using UnityEngine;

public enum GameMode
{
    Classic,
    Timed
}

public static class GameSettings
{
    public static GameMode SelectedMode = GameMode.Classic;
    public static int WordLength = 5;
}
