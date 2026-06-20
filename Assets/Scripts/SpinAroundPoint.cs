using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class SpinAroundPoint : MonoBehaviour
{
    
    // ———— Fields ————

    public Vector3 origin;
    public Vector3 axis = new Vector3(0, 1, 0);
    public float angle;
    private Transform _transform;
    
    // ———— Unity events ————
    
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        // Maybe update the rotation not at all frame for better performance ? With an event ?
        _transform.RotateAround(origin, axis, angle);
    }
    
    // ———— Methods ————
    

}
