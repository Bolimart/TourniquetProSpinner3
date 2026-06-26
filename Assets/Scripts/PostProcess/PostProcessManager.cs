using UnityEngine;

public class PostProcessManager : MonoBehaviour
{
    
    // ———— Fields ————
    
    public Material nausea;
    public GameEvent gameEvent;
    public float nauseaAmount;
    
    // ———— Unity events ————
    
    void Start()
    {
        nausea.SetFloat("_Amplitude", 0f);
        nausea.SetFloat("_Zoom", 0f);
        gameEvent.onGameOver += updateNausea;
    }

    void Update()
    {
        
    }
    
    // ———— Methods ————
    
    void updateNausea()
    {
        float amplitude = 0.01f * nauseaAmount;
        nausea.SetFloat("_Amplitude", amplitude);
        nausea.SetFloat("_Zoom", 1f);
    }
}
