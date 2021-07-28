using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Level")]
public class Level : ScriptableObject
{
    public string Name;
    public int PlatformCount;
    public int BeginingBallCount;

    [Space]
    [Space]
    public int[] RightMultipliers;
    public int[] LeftMultipliers;

    // Ball count for success
}