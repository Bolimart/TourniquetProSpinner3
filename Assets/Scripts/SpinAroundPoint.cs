using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class SpinAroundPoint : MonoBehaviour
{
    
    // ———— Fields ————

    public Vector3 origin;
    public Vector3 axis = new Vector3(0, 1, 0);
    public GameObject[] objectsToRotate;
    //public float angle;

    public float initialSpeed = 0f;
    public float maxSpeed = 100f;
    //speed Up every 10 seconds
    // private float speedUpTiming = 5f;
    //
    // private float lastSpeedUpTime = 0f;
    //
    // private float lastSpeedUpTimeFromTrucDuMilieu = 0f;

    private float rotationSpeed = 0f;
    public float accelerationTime = 120f; // The time it takes to accelerate to max speed
    public AnimationCurve accelerationCurve;
    private float accelerationTimer = 0f;
    public int BaseMultiplier = 0;
    private bool isAccelerating = false;
    
    private List<GameObject> _objectsToRotate;

    [SerializeField] private ShouldSpeedUp shouldSpeedUp;
    
    // ———— Unity events ————

    void Start()
    {
        foreach (var obj in objectsToRotate) _objectsToRotate.Add(obj);
    }
    
    void Update()
    {
        if (!isAccelerating) return;
        
        rotationSpeed = Mathf.Lerp(initialSpeed, maxSpeed, accelerationCurve.Evaluate(accelerationTimer / accelerationTime));
        float deltaAngle = rotationSpeed * Time.deltaTime;
        transform.RotateAround(origin, axis, deltaAngle);
        accelerationTimer += Time.deltaTime;
        BaseMultiplier = 50 * (int)(rotationSpeed / maxSpeed);
        
        
        foreach (var obj in _objectsToRotate)
        {
            obj.transform.RotateAround(origin, axis, deltaAngle);
        }
    }
    
    // ———— Methods ————
    
    public void StartTurning() => isAccelerating = true;
    
    public void StopTurning() => isAccelerating = false;
    
    public float GetRotationSpeed() => rotationSpeed;

}
