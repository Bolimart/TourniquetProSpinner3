using UnityEngine;

[CreateAssetMenu(fileName = "TricksScriptable", menuName = "Scriptable Objects/TricksScriptable")]
public class TricksScriptable : ScriptableObject
{    
    public int score;
    public float multiplier;
    public string text;
    public Sprite photo;
}
