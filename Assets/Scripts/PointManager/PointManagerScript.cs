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

    public int TricksIndex = 0;
    public float bonusResetTime = 2f;
    private float _bonusResetTimer = 0f;
    private bool asStart = true;
    private int lastHundredScore = 0;

    private float BPM = 60f;
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
        if (asStart)
        {
            // Décrément du timer combo
            if (ComboMultiplier > _defaultMultiplier)
            {
                _bonusResetTimer += Time.deltaTime;
                if (_bonusResetTimer >= bonusResetTime)
                {
                    print($"Reset combo: {ComboMultiplier} -> {_defaultMultiplier} (defaultMultiplier={_defaultMultiplier})");
                    ComboMultiplier = _defaultMultiplier;
                    _bonusResetTimer = 0f;
                }
            }
        
            BPM = spinAroundPoint.BaseMultiplier + ComboMultiplier;
            points += Time.deltaTime * BPM;

            pointsText.text = points.ToString("F0") + "pts";
            BpmText.text = BPM.ToString("F2") + "x";
        }
    }

    
    void OnTrickPerformed()
    {
        TricksScriptable trick = tricks[TricksIndex];
        canvaGameScript.SpawnPhoto(trick.photo);
        canvaGameScript.SpawnPointsText(trick.score, trick.multiplier, trick.text);

        ComboMultiplier += trick.multiplier;
    
        int hundredsBefore = (int)(points / 100);
        points += trick.score * ComboMultiplier;
        int hundredsAfter = (int)(points / 100);

        int newHundreds = hundredsAfter - hundredsBefore;
        for (int i = 0; i < newHundreds; i++)
            _gameEvent.on100Score();

        _bonusResetTimer = 0f;
        TricksIndex = (TricksIndex + 1) % tricks.Length;
    }
}
