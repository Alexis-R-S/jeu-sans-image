using UnityEngine;

// Handles everything related to player movement
public class PlayerController : MonoBehaviour
{
    private PlayerControlsMap playerControls;
    private Rigidbody rb;

    [SerializeField] private float speed = 15;
    [SerializeField] private float sensitivity = 50;
    [SerializeField] private float maxSpeed = 10;

    // Awake is always called before Start. Often used to initialize variables
    void Awake()
    {
        playerControls = new PlayerControlsMap();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Often used to initialize variables which are dependent to other scripts/components
    void Start()
    {
        
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // Update player direction. Very basic.
        Vector2 lookInput = playerControls.Player.Look.ReadValue<Vector2>();
        lookInput *= sensitivity * Time.deltaTime;
        Quaternion currentRotation = transform.rotation;
        currentRotation *= Quaternion.AngleAxis(lookInput.x, Vector3.up);
        transform.rotation = currentRotation;
        // We ignore vertical rotation as it would have no impact on the game

        // Update player acceleration, using a force
        Vector2 mvtInput = playerControls.Player.Move.ReadValue<Vector2>();
        mvtInput *= speed * Time.deltaTime;
        rb.AddForce(currentRotation * new Vector3(mvtInput.x, 0, mvtInput.y), ForceMode.VelocityChange);

        // Limit max speed. Warning: both maxSpeed and rigidbody linear damping affect max speed 
        if (rb.linearVelocity.magnitude > maxSpeed) {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
}
