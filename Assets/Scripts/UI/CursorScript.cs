using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    // ———— Fields ————
    
    public Image LeftHand;
    public Image RightHand;
    
    // ———— Methods ————
    
    public void UpdateHand(bool isLeftHand, Color color, Sprite sprite)
    {
        if (isLeftHand)
        {
            LeftHand.color = color;
            LeftHand.sprite = sprite;
        }
        else
        {
            RightHand.color = color;
            RightHand.sprite = sprite;
        }
    }
}
