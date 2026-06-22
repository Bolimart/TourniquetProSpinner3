using UnityEngine;


public class HandScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private float grabDuration = 5.0f;
    private Vector3 initialLocalPosition;
    private Vector3 targetLocalPosition;
    public bool isMoving = false;
    private float GrabStartAt;
    public bool isGrabbing;


    void Start()
    {
        initialLocalPosition = this.transform.localPosition;
    }

    public void SetTarget(Vector3 worldPoint)
    {
        Debug.Log("SetTarget called with worldPoint: " + worldPoint);
        Transform parent = transform.parent;
        targetLocalPosition = parent.InverseTransformPoint(worldPoint); //convert world point to local point
        isMoving = true;
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
            }
    }

    public void StopGrabbing()
    {
        isGrabbing = false;
        this.transform.SetParent(Camera.main.transform);
    }
    void Update()
    {
        if(isGrabbing)
        {
            if (Time.time - GrabStartAt > grabDuration)
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
        this.transform.SetParent(target.transform);
        this.transform.localPosition = Vector3.zero;
        GrabStartAt = Time.time;
        isGrabbing = true;
    }
}
