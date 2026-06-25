using System;
using UnityEngine;


public class HandScript : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private float grabDuration = 5.0f;
    public Color initialColor;
    public Color tiredColor;
    public bool isLeftHand;
    private Vector3 initialLocalPosition;
    private Vector3 targetLocalPosition;
    public Sprite handOpenSprite;
    public Sprite handCloseSprite;
    public bool isMoving = false;
    private float GrabStartAt;
    private Collider _collider;
    private SpriteRenderer _sprite;

    public GameObject grabbedObject;
    public bool isGrabbing;
    public CursorScript cursorScript;

    private Transform parent;
    private AudioSource _audioSource;


    void Start()
    {
        initialLocalPosition = transform.localPosition;
        _collider = GetComponent<Collider>();
        _sprite = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetTarget(Vector3 worldPoint)
    {
        //Debug.Log("SetTarget called with worldPoint: " + worldPoint);
        parent = transform.parent;
        targetLocalPosition = parent.InverseTransformPoint(worldPoint); //convert world point to local point
        isMoving = true;
        //_collider.enabled = true;
    }

    public void Move(Vector3 position)
    {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                position,
                moveSpeed * Time.deltaTime
            );
            if(Vector3.Distance(transform.localPosition, position) < 0.01f)
            {
                transform.localPosition = position;
                isMoving = false;
                _collider.enabled = true;
            }
    }

    public void StopGrabbing()
    {
        isGrabbing = false;
        transform.localScale = Vector3.one * 0.125f;
        _collider.enabled = false;
        transform.SetParent(parent);
        _sprite.sprite = handOpenSprite;
        _sprite.color = initialColor;
        cursorScript.UpdateHand(isLeftHand, initialColor, handOpenSprite);
    }
    void Update()
    {
        // The hand always look at the camera
        transform.LookAt(transform.parent);
        
        if(isGrabbing)
        {
            float timeLeft = Time.time - GrabStartAt;
            var color = Color.Lerp(initialColor, tiredColor, timeLeft / grabDuration);
            cursorScript.UpdateHand(isLeftHand, color, handCloseSprite);
            _sprite.color = color;
            if (timeLeft > grabDuration)
            {
                StopGrabbing();
            }
            return;
        }
        if (isMoving)
        {
            Move(targetLocalPosition);
        }
        else
        {
            Move(initialLocalPosition);
        }


    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Grabbable") && !isGrabbing)
        {
            StartGrabbing(collider.gameObject);
        }
    }

    private void StartGrabbing(GameObject target)
    {
        grabbedObject = target;
        transform.SetParent(null);
        transform.localScale = Vector3.one * 0.125f;
        GrabStartAt = Time.time;
        isGrabbing = true;
        _sprite.sprite = handCloseSprite;
        //SetTarget(target.transform.position);
        cursorScript.UpdateHand(isLeftHand, initialColor, handCloseSprite);
        OnGrabbed?.Invoke();
        _audioSource.Play();
    }
    
    public event Action OnGrabbed;
}
