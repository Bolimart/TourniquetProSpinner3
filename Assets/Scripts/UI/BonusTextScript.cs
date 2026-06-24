using TMPro;
using UnityEngine;

public class BonusTextScript : MonoBehaviour
{
    // ———— Fields ————

    private float bonusAmount = 0.6f;
    private TextMeshProUGUI _tmp;
    
    // ———— Unity Events ————
    
    void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        SetAmount();
    }

    void Update()
    {
        
    }
    
    // ———— Methods ————

    public void SetAmount(float amount = 0.6f)
    {
        bonusAmount = amount;
        _tmp.text = bonusAmount.ToString("F2") + "x";
    }
}
