using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerScript : MonoBehaviour
{
    private InputSystem_Actions playerInputActions;

    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] private float pitchClamp = 80f;

    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;

    private float yaw;
    private float pitch;
    private Vector2 mousePosition;
    Vector2 screenCenter;


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
    }

    void OnDisable()
    {
        playerInputActions.Player.Look.performed -= OnLook;
        playerInputActions.Player.LeftClick.performed -= MoveHands;
        playerInputActions.Player.RightClick.performed -= MoveHands;
        playerInputActions.Disable();
    }

    void OnLook(CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();

        yaw += mousePosition.x * lookSensitivity;
        pitch -= mousePosition.y * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }


    void MoveHands(CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f) && !hitInfo.collider.CompareTag("Grabbable"))
        {
            Debug.Log("No object hit by raycast.");
            return;
        }
        GameObject handToMove;
        // Determine which hand triggered the action
        if (context.action.name == "LeftClick")
        {
            handToMove = LeftHand;
        }
        else
        {
            handToMove = RightHand;
        }
        HandScript handScript = handToMove.GetComponent<HandScript>();
        handScript.SetTarget(hitInfo.point);
    }

}
