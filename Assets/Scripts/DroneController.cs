using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference lookAction;
    [SerializeField] InputActionReference ascendAction;
    [SerializeField] InputActionReference descendAction;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float verticalSpeed = 6f;
    [SerializeField] float acceleration = 5f;
    [SerializeField] float lookSensitivity = 0.1f;
    [SerializeField] float minPitch = -20f;
    [SerializeField] float maxPitch = 60f;

    Rigidbody rb;
    Vector2 moveInput;
    Vector2 lookInput;
    float ascendInput;
    float descendInput;
    float yaw;
    float pitch;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        yaw = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
        lookAction.action.performed += OnLook;
        ascendAction.action.performed += OnAscend;
        ascendAction.action.canceled += OnAscend;
        descendAction.action.performed += OnDescend;
        descendAction.action.canceled += OnDescend;

        moveAction.action.Enable();
        lookAction.action.Enable();
        ascendAction.action.Enable();
        descendAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
        lookAction.action.performed -= OnLook;
        ascendAction.action.performed -= OnAscend;
        ascendAction.action.canceled -= OnAscend;
        descendAction.action.performed -= OnDescend;
        descendAction.action.canceled -= OnDescend;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput += ctx.ReadValue<Vector2>();
    }

    void OnAscend(InputAction.CallbackContext ctx)
    {
        ascendInput = ctx.ReadValue<float>();
    }

    void OnDescend(InputAction.CallbackContext ctx)
    {
        descendInput = ctx.ReadValue<float>();
    }

    void Update()
    {
        yaw += lookInput.x * lookSensitivity;
        pitch = Mathf.Clamp(pitch - lookInput.y * lookSensitivity, minPitch, maxPitch);
        lookInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        float vertical = ascendInput - descendInput;
        Quaternion yawRotation = Quaternion.Euler(0f, yaw, 0f);

        Vector3 targetVelocity = yawRotation * new Vector3(
            moveInput.x * moveSpeed,
            vertical * verticalSpeed,
            moveInput.y * moveSpeed);

        rb.linearVelocity = Vector3.Lerp(
            rb.linearVelocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime);

        rb.MoveRotation(Quaternion.Euler(pitch, yaw, 0f));
    }
}