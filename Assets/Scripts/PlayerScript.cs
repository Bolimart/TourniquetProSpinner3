using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerScript : MonoBehaviour
{
    private InputSystem_Actions playerInputActions;

    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] private float pitchClamp = 80f;

    [SerializeField] private HandScript LeftHandScript;
    [SerializeField] private HandScript RightHandScript;

    [SerializeField] private GameEvent _gameEvent;

    [SerializeField] private SpinAroundPoint spinAroundPointScript;

    [SerializeField] private TricksManager tricksManager;


    private float yaw;
    private float pitch;
    private Vector2 mousePosition;
    Vector2 screenCenter;
    private bool isPlaying = false;



    
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

        if(LeftHandScript.isGrabbing && RightHandScript.isGrabbing && tricksManager.TryTrick())
        {
            if(LeftHandScript.grabbedObject != LeftLastGrabbedObject || RightHandScript.grabbedObject != RightLastGrabbedObject)
            {
                tricksManager.BigTricks();
            }

            Debug.Log("Both hands are grabbing, performing trick");
            _gameEvent.TrickPerformed();
            tricksManager.PerformTrick();
            LeftLastGrabbedObject = LeftHandScript.grabbedObject;
            RightLastGrabbedObject = RightHandScript.grabbedObject;
        }
    }


    void OnLook(CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();

        yaw += mousePosition.x * lookSensitivity;
        pitch -= mousePosition.y * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }


    void MoveHands(CallbackContext context){
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        
        HandScript handToMove;
        // Determine which hand triggered the action
        if (context.action.name == "LeftClick")
        {
            handToMove = LeftHandScript;
        }
        else
        {
            handToMove = RightHandScript;
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
        if (!isPlaying && (LeftHandScript.isGrabbing || RightHandScript.isGrabbing))
        {
            isPlaying = true;
        }
        if(!LeftHandScript.isGrabbing && !RightHandScript.isGrabbing && isPlaying)
        {
            isPlaying = false;
            _gameEvent.GameOver();
            goFly();
            this.playerInputActions.Disable();
        }

    }

    private void goFly()
    {
        Debug.Log("Both hands are not grabbing, go fly");
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce((transform.up + transform.forward) * spinAroundPointScript.GetRotationSpeed()/10f, ForceMode.Impulse);
    }
}
