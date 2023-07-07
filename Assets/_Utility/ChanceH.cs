using UnityEngine;

public static class ChanceH
{
    public static bool ChanceRoll(float chance, float outOf = 100f)
    {
        float roll = Random.Range(0f, outOf);
        return roll <= chance;
    }
}
