using UnityEngine;

public class ImageScript : MonoBehaviour
{
    // ———— Fields ————

    public float showTime = 1f;
    public Vector2 rotationRange = new Vector2(-15, 15);
    private Animator _animator;

    private bool _isInCorner = false;
    private float _showTimer = 0f;
    
    // ———— Unity events ————
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        float randomRotation = Random.Range(rotationRange.x, rotationRange.y);
        GetComponent<Transform>().localRotation = Quaternion.Euler(0, 0, randomRotation);
    }

    void Update()
    {
        if (!_isInCorner && showTime <= _showTimer)
        {
            _animator.SetTrigger("ToCorner");
            _isInCorner = true;
            OnCorner?.Invoke();
        }
        else _showTimer += Time.deltaTime;
    }
    
    // ———— Events ————

    public event System.Action OnCorner;
}
