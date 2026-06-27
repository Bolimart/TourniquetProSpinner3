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
    
    private bool _comboActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointManagerScript = GetComponent<PointManagerScript>();
    }

    public bool TryTrick()
    {
        if (Time.time - TrickedAt > TrickDuration)
        {
            return true;
        }
        return false;
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
    void Update()
    {
        if (_comboActive && Time.time - TrickedAt > TrickDuration + trickComboDuration)
        {
            _gameEvent.ResetTrickCombo();
            ResetTrickCombo();
            _comboActive = false;
        }
    }

    public void PerformTrick()
    {
        TrickedAt = Time.time;
        _comboActive = true;
        UpgradeTricks();
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
