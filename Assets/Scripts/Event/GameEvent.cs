using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{

    public Action onGameOver;

    public Action on100Score;

    public Action onTrucDuMilieuAccélérant;

    public Action onTrickPerformed;

    public void GameOver()
    {
        onGameOver?.Invoke();
    }

    public void Score100()
    {
        on100Score?.Invoke();
    }

    public void TrucDuMilieuAccélérant()
    {
        onTrucDuMilieuAccélérant?.Invoke();
        onTrucDuMilieuAccélérant?.BeginInvoke(null, null);
    }

    public void TrickPerformed()
    {
        onTrickPerformed?.Invoke();
    }
    
}
