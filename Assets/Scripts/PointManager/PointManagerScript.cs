using PointManager;
using UnityEngine;
using UnityEngine.UI;

public class PointManagerScript : MonoBehaviour
{
    // ———— Fields ————

    public Trick[] tricks;
    public float points = 0f;
    public float multiplier = 0.6f;
    private float _defaultMultiplier;
    public CanvaGameScript canvaGameScript;
    public BonusTextScript bonusTextScript;

    public float bonusResetTime = 2f;
    private float _bonusResetTimer = 0f;
    
    // ———— Unity events ————
    
    void Start()
    {
        _defaultMultiplier = multiplier;
    }

    
    void Update()
    {
        
        if (multiplier <= _defaultMultiplier)
        {
            _bonusResetTimer += Time.deltaTime;
            if (_bonusResetTimer > bonusResetTime)
            {
                multiplier = _defaultMultiplier;
                bonusTextScript.SetAmount(multiplier);
                _bonusResetTimer = 0f;
            }
        }
    }

    void OnTrickPerformed()
    {
        Trick trick = tricks[Random.Range(0, tricks.Length)];
        canvaGameScript.SpawnPhoto(trick.photo);
        canvaGameScript.SpawnPointsText(trick.score, trick.multiplier, trick.text);
        bonusTextScript.SetAmount(trick.multiplier);
        multiplier += trick.multiplier;
        points += trick.score * multiplier;
        _bonusResetTimer = 0f;
    }
}
