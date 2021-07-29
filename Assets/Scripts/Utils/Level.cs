using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Level")]
public class Level : ScriptableObject
{
    public string Name;
    public int PlatformCount;
    public int BeginingBallCount;
    public int BallCountForSuccess;

    [Space]
    [Space]
    public int[] RightMultipliers;
    public int[] LeftMultipliers;
}