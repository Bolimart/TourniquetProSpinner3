using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{

    public Action onGameOver;

    public Action on100Score;

    public Action onSpinnerAccellerate;

    public Action onTrickPerformed;

    public Action onResetTrickCombo;

    public void ResetTrickCombo()
    {
        onResetTrickCombo?.Invoke();
    }

    public void GameOver()
    {
        onGameOver?.Invoke();
    }

    public void Score100()
    {
        on100Score?.Invoke();
    }
    public void TrickPerformed()
    {
        onTrickPerformed?.Invoke();
    }
    
}
