using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class SpinAroundPoint : MonoBehaviour
{
    
    // ———— Fields ————

    public Vector3 origin;
    public Vector3 axis = new Vector3(0, 1, 0);
    //public float angle;

   public float rotationSpeed = 10f;
    private Transform _transform;

    //speed Up every 10 seconds
    private float speedUpTiming = 10f;

    private float lastSpeedUpTime = 0f;

    [SerializeField] private GameEvent _gameEvent;
    
    // ———— Unity events ————

    void Update()
    {
        float deltaAngle = rotationSpeed * Time.deltaTime;
        // Maybe update the rotation not at all frame for better performance ? With an event ?
        _transform.RotateAround(origin, axis, deltaAngle);
        if(Time.time - lastSpeedUpTime > speedUpTiming)
        {
            speedUp();
        }
    }

    void speedUp()
    {
        lastSpeedUpTime = Time.time;
        Debug.Log("Speeding up rotation");
        rotationSpeed *= 2f;
        //here now move it with score later
        _gameEvent.Score100();
    }
    
    // ———— Methods ————
    

}
