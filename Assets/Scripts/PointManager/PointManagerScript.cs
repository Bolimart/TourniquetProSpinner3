using PointManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointManagerScript : MonoBehaviour
{
    // ———— Fields ————
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI BpmText;

    [SerializeField] private SpinAroundPoint spinAroundPoint;

    public TricksScriptable[] tricks;

    public float points = 0f;
    public float ComboMultiplier = 0.6f;
    private float _defaultMultiplier;
    public CanvaGameScript canvaGameScript;
    public BonusTextScript bonusTextScript;

    public int TricksIndex = 0;
    public float bonusResetTime = 2f;
    private float _bonusResetTimer = 0f; 

    private int BPM = 100;
    // ———— Unity events ————
    
    [SerializeField] private GameEvent _gameEvent;

    void OnEnable()
    {
        _gameEvent.onTrickPerformed += OnTrickPerformed;
    }

    void OnDisable()
    {
        _gameEvent.onTrickPerformed -= OnTrickPerformed;
    }
    void Start()
    {
        _defaultMultiplier = ComboMultiplier;
    }

    void Update()
    {
        
        /*if (ComboMultiplier <= _defaultMultiplier)
        {
            _bonusResetTimer += Time.deltaTime;
            if (_bonusResetTimer > bonusResetTime)
            {
                ComboMultiplier = _defaultMultiplier;
                bonusTextScript.SetAmount(ComboMultiplier);
                _bonusResetTimer = 0f;
            }
        }*/
        BPM = 100 + spinAroundPoint.BaseMultiplier + (int)ComboMultiplier;
        Debug.Log("BPM: " + BPM.ToString() + " ComboMultiplier: " + ComboMultiplier.ToString());
        Debug.Log("BaseMultiplier: " + spinAroundPoint.BaseMultiplier.ToString());
        BpmText.text = "BPM: " + BPM.ToString() ;
        pointsText.text = points.ToString("F0") + "pts" ;
    }

    void OnTrickPerformed()
    {
        TricksScriptable trick = tricks[TricksIndex];
        canvaGameScript.SpawnPhoto(trick.photo);
        canvaGameScript.SpawnPointsText(trick.score, trick.multiplier, trick.text);
        bonusTextScript.SetAmount(trick.multiplier);
        ComboMultiplier += trick.multiplier;
        points += trick.score * ComboMultiplier;
        _bonusResetTimer = 0f;
    }
}
