using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class PlayerScript : MonoBehaviour
{
    private InputSystem_Actions playerInputActions;

    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] private float pitchClamp = 80f;
    [SerializeField] private float armLenght = 80f;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    
    [SerializeField] private float cameraOrbitRadius = 1f;
    
    private HandScript _leftHandScript;
    private HandScript _rightHandScript;
    public CursorScript cursorScript;
    public float radius = 0.2f;

    [SerializeField] private GameEvent gameEvent;

    [SerializeField] private SpinAroundPoint spinAroundPointScript;

    [SerializeField] private TricksManager tricksManager;
    
    private AudioSource audioSource;
    private float pitch = 1f;
    private float pitchChange = 1.059463094f;
    
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
        audioSource = GetComponent<AudioSource>();
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

        gameEvent.onTrickPerformed += OnTrickPerformedAudio;
        gameEvent.onResetTrickCombo += OnResetTrickCombo;
        
        SceneChanger sceneChanger = GetComponent<SceneChanger>();
        gameEvent.onGameOver = sceneChanger.StartSceneChange;
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
        gameEvent.onTrickPerformed -= OnTrickPerformedAudio;
        gameEvent.onResetTrickCombo -= OnResetTrickCombo;
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


    void MoveHands(CallbackContext context)
    {
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
    
        HandScript handToMove = context.action.name == "LeftClick" ? _leftHandScript : _rightHandScript;
        HandScript handScript = handToMove.GetComponent<HandScript>();

        if (handScript.isGrabbing)
            handScript.StopGrabbing();

        // Si rien de grabbable n'est touché, on return
        if (!Physics.SphereCast(ray, radius, out RaycastHit hit, armLenght) || !hit.collider.CompareTag("Grabbable"))
            return;

        handScript.SetTarget(hit.point);
    }
    
    private Ray _lastRay;
    private float _lastDistance;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_lastRay.origin, _lastRay.direction * armLenght);
    
        // Sphère au bout du cast
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_lastRay.origin + _lastRay.direction * armLenght, radius);
    
        // Sphère au point de départ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_lastRay.origin, radius);
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
        
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        bool canGrab = Physics.SphereCast(ray, radius, out RaycastHit hit, armLenght) 
                       && hit.collider.CompareTag("Grabbable");

        _lastRay = ray;
        // Dans CanGrab ou Update
        Debug.DrawRay(ray.origin, ray.direction * armLenght, Color.yellow);
        
        cursorScript.UpdateCursor(canGrab);
        
    }

    private Coroutine _repositionCoroutine;

    public void OnHandGrabbed()
    {
        if (_rightHandScript.isGrabbing && _leftHandScript.isGrabbing)
        {
            if (_repositionCoroutine != null)
                StopCoroutine(_repositionCoroutine);
            _repositionCoroutine = StartCoroutine(SmoothReposition());
        }
    }

    private IEnumerator SmoothReposition()
    {
        float smoothTime = 0.3f;
        Vector3 velocity = Vector3.zero;
        float fixedY = transform.position.y;

        while (_rightHandScript.isGrabbing && _leftHandScript.isGrabbing)
        {
            Vector3 leftPos = leftHand.transform.position;
            Vector3 rightPos = rightHand.transform.position;
            Vector3 midPoint = (leftPos + rightPos) / 2f;

            // Projection du milieu sur le cercle de rayon r centré en 0
            Vector3 flatMid = new Vector3(midPoint.x, 0f, midPoint.z);
            Vector3 targetPosition = flatMid.normalized * cameraOrbitRadius;
            targetPosition.y = fixedY;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
                yield break;

            yield return null;
        }
    }

    private void goFly()
    {
        Debug.Log("Both hands are not grabbing, go fly");
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        float speed = spinAroundPointScript.GetRotationSpeed() / 80;
        spinAroundPointScript.StopTurning();
        rb.AddForce((transform.up + transform.forward) * speed, ForceMode.Impulse);
    }
    
    private void OnTrickPerformedAudio()
    {
        pitch *= pitchChange;
        audioSource.pitch = pitch;
        audioSource.Play();
    }

    private void OnResetTrickCombo()
    {
        pitch = 1f;
    }
}
