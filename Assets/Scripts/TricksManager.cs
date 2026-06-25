using UnityEngine;

public class TricksManager : MonoBehaviour
{
    [SerializeField] GameEvent _gameEvent;
     private PointManagerScript pointManagerScript;

    private int maxIndexTricks = 1;
    private int minIndexTricks = 0;
    private float trickComboDuration = 2f;

    private float TrickedAt = 0f;
    private float TrickDuration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointManagerScript = GetComponent<PointManagerScript>();
    }

    void OnEnable()
    {
        _gameEvent.onResetTrickCombo += ResetTrickCombo;
    }

    void OnDisable()
    {
        _gameEvent.onResetTrickCombo -= ResetTrickCombo;
    }

    public bool TryTrick()
    {
        if (Time.time - TrickedAt > TrickDuration)
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        if (Time.time - TrickedAt > TrickDuration + trickComboDuration)
        {
            ResetTrickCombo();
        }
    }

    public void PerformTrick()
    {
        TrickedAt = Time.time;
        UpgradeTricks();
    }
    public void BigTricks()
    {
        
        if(maxIndexTricks >= pointManagerScript.tricks.Length - 1)
        {
            return;
        }
        minIndexTricks += 2;
        maxIndexTricks += 2;

    }

    private void UpgradeTricks()
    {
        if(pointManagerScript.TricksIndex < maxIndexTricks)
        {
            pointManagerScript.TricksIndex++;
            Debug.Log("Trick index upgraded to " + pointManagerScript.TricksIndex);
        }
    }

    private void ResetTrickCombo()
    {
        Debug.Log("Resetting trick combo");
        minIndexTricks = 0;
        maxIndexTricks = 1;
        pointManagerScript.TricksIndex = minIndexTricks;
        pointManagerScript.ComboMultiplier = 0.6f;
    }
}
