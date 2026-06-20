using UnityEngine;

public class PostProcessManager : MonoBehaviour
{
    
    // ———— Fields ————
    
    public Material nausea;
    public float nauseaAmount;
    public bool nauseaZoom;
    
    // ———— Unity events ————
    
    void Start()
    {
        updateNausea();
    }

    void Update()
    {
        
    }
    
    // ———— Methods ————
    
    void updateNausea()
    {
        float amplitude = 0.01f * nauseaAmount;
        nausea.SetFloat("_Amplitude", amplitude);
        nausea.SetFloat("_Zoom", nauseaZoom ? 1f : 0f);
    }
}
