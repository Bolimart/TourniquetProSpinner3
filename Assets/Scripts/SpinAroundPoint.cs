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
    //speed Up every 10 seconds
    private float speedUpTiming = 5f;

    private float lastSpeedUpTime = 0f;

    private float lastSpeedUpTimeFromTrucDuMilieu = 0f;

    [SerializeField] private GameEvent _gameEvent;

    [SerializeField] private ShouldSpeedUp shouldSpeedUp;
    
    // ———— Unity events ————

    void Update()
    {
        float deltaAngle = rotationSpeed * Time.deltaTime;
        // Maybe update the rotation not at all frame for better performance ? With an event ?
        this.transform.RotateAround(origin, axis, deltaAngle);
        if(shouldSpeedUp.value && Time.time - lastSpeedUpTimeFromTrucDuMilieu > speedUpTiming)
        {
            rotationSpeed *= 2f;
            lastSpeedUpTimeFromTrucDuMilieu = Time.time;
        }
        if(Time.time - lastSpeedUpTime > speedUpTiming)
        {
            speedUp();
        }
    }

    void speedUp()
    {
        lastSpeedUpTime = Time.time;
        rotationSpeed += 3f;
        //here now move it with score later
        if(!_gameEvent){return;}
        _gameEvent.Score100();
    }
    
    // ———— Methods ————
    

}
