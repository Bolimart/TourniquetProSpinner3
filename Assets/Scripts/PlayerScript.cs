using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerScript : MonoBehaviour
{
    private InputSystem_Actions playerInputActions;

    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] private float pitchClamp = 80f;
    [SerializeField] private float armLenght = 80f;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    
    private HandScript _leftHandScript;
    private HandScript _rightHandScript;

    [SerializeField] private GameEvent gameEvent;

    [SerializeField] private SpinAroundPoint spinAroundPointScript;

    [SerializeField] private TricksManager tricksManager;
    
    public bool canMove = true;

    private float _yaw;
    private float _pitch;
    private Vector2 _mousePosition;
    private Vector2 screenCenter;
    private bool _isPlaying = false;
    
    private GameObject LeftLastGrabbedObject = null;
    private GameObject RightLastGrabbedObject = null;


    void Awake()
    {
        playerInputActions = new InputSystem_Actions();
        playerInputActions.Enable();
        
        
    }

    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        _leftHandScript = leftHand.GetComponent<HandScript>();
        _rightHandScript = rightHand.GetComponent<HandScript>();
        if (canMove)
        {
            _leftHandScript.OnGrabbed += OnHandGrabbed;
            _rightHandScript.OnGrabbed += OnHandGrabbed;
        }
    }

    void OnEnable()
    {
        playerInputActions.Player.Look.performed += OnLook;
        playerInputActions.Player.LeftClick.performed += MoveHands;
        playerInputActions.Player.RightClick.performed += MoveHands;
        playerInputActions.Player.TricksButton.performed += isTrickable;
    }

    void OnDisable()
    {
        playerInputActions.Player.Look.performed -= OnLook;
        playerInputActions.Player.LeftClick.performed -= MoveHands;
        playerInputActions.Player.RightClick.performed -= MoveHands;
        playerInputActions.Player.TricksButton.performed -= isTrickable;
        playerInputActions.Disable();
    }

    void isTrickable(CallbackContext context)
    {

        if(_leftHandScript.isGrabbing && _rightHandScript.isGrabbing && tricksManager.TryTrick())
        {
            if(_leftHandScript.grabbedObject != LeftLastGrabbedObject || _rightHandScript.grabbedObject != RightLastGrabbedObject)
            {
                tricksManager.BigTricks();
            }

            Debug.Log("Both hands are grabbing, performing trick");
            gameEvent.TrickPerformed();
            tricksManager.PerformTrick();
            LeftLastGrabbedObject = _leftHandScript.grabbedObject;
            RightLastGrabbedObject = _rightHandScript.grabbedObject;
        }
    }


    void OnLook(CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();

        _yaw += _mousePosition.x * lookSensitivity;
        _pitch -= _mousePosition.y * lookSensitivity;
        _pitch = Mathf.Clamp(_pitch, -pitchClamp, pitchClamp);

        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }


    void MoveHands(CallbackContext context){
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        
        HandScript handToMove;
        // Determine which hand triggered the action
        if (context.action.name == "LeftClick")
        {
            handToMove = _leftHandScript;
        }
        else
        {
            handToMove = _rightHandScript;
        }

        HandScript handScript = handToMove.GetComponent<HandScript>();
        if(handScript.isGrabbing)
        {
            handScript.StopGrabbing();
        }

        float radius = 0.4f; // Rayon de tolérance

        if (Physics.SphereCast(ray, radius, out RaycastHit hit, 100) && !hit.collider.CompareTag("Grabbable"))
        {
            //Debug.Log("No object hit by spherecast.");
            return;
        }

        /*if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f) && !hitInfo.collider.CompareTag("Grabbable"))
        {
            Debug.Log("No object hit by raycast.");
            return;
        }*/

        handScript.SetTarget(hit.point);
    }

    private void Update()
    {
        if (!_isPlaying && (_leftHandScript.isGrabbing || _rightHandScript.isGrabbing))
        {
            _isPlaying = true;
        }
        if(!_leftHandScript.isGrabbing && !_rightHandScript.isGrabbing && _isPlaying)
        {
            _isPlaying = false;
            gameEvent.GameOver();
            goFly(); 
            playerInputActions.Disable();
        }
        
    }

    public void OnHandGrabbed()
    {
        if (_rightHandScript.isGrabbing && _leftHandScript.isGrabbing)
        {
            // Put the player between the two hands and with a distance of the arm
            Vector3 middlePosition = (leftHand.transform.position + rightHand.transform.position) / 2f;
            print(middlePosition);
            print(middlePosition + transform.forward * armLenght);
            transform.position = middlePosition + transform.forward * armLenght;
        }
    }

    private void goFly()
    {
        Debug.Log("Both hands are not grabbing, go fly");
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce((transform.up + transform.forward) * spinAroundPointScript.GetRotationSpeed()/10f, ForceMode.Impulse);
    }
}
