using UnityEngine;

public class CrowdScript : MonoBehaviour
{
    
    public Sprite[] sprites;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
