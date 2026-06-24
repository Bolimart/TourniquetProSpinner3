using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class SpinAroundPoint : MonoBehaviour
{
    
    // ———— Fields ————

    public Vector3 origin;
    public Vector3 axis = new Vector3(0, 1, 0);
    //public float angle;

    public float initialSpeed = 0f;
    public float maxSpeed = 100f;
    //speed Up every 10 seconds
    // private float speedUpTiming = 5f;
    //
    // private float lastSpeedUpTime = 0f;
    //
    // private float lastSpeedUpTimeFromTrucDuMilieu = 0f;

    private float rotationSpeed = 10f;
    public float accelerationTime = 120f; // The time it takes to accelerate to max speed
    public AnimationCurve accelerationCurve;
    private float accelerationTimer = 0f;
    
    

    [SerializeField] private GameEvent _gameEvent;

    [SerializeField] private ShouldSpeedUp shouldSpeedUp;
    
    // ———— Unity events ————

    void Update()
    {
        rotationSpeed = Mathf.Lerp(initialSpeed, maxSpeed, accelerationCurve.Evaluate(accelerationTimer / accelerationTime));
        float deltaAngle = rotationSpeed * Time.deltaTime;
        transform.RotateAround(origin, axis, deltaAngle);
        accelerationTimer += Time.deltaTime;
    }
    
    // ———— Methods ————
    
    public float GetRotationSpeed() => rotationSpeed;

}
