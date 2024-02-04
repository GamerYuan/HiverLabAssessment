using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager
{
    private static float baseDifficulty = 1, difficulty = 1;
    private static float maxDifficulty = 3;

    public static float Difficulty { get => difficulty; private set => difficulty = value; }

    public static void IncreaseDifficulty(float difficultyModifier)
    {
        Difficulty = baseDifficulty + maxDifficulty * difficultyModifier;
    }
}
