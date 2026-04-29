using UnityEngine;
public class PlayerMoment : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;     // Normal walking speed
    public float sprintSpeed = 8f;    // Running speed when holding sprint key
    public float crouchSpeed = 2.5f;  // Slower speed when crouching
    public float jumpHeight = 2f;     // How high the character jumps
    
    [Header("Ground Settings")]
    public Transform groundCheck;     // Position to check for ground
    public float groundDistance = 0.4f; // Distance to check for ground
    public LayerMask groundMask;      // Which layers count as ground
    
    [Header("Camera Settings")]
    public Transform cameraTransform;  // Reference to the player's camera
    public float mouseSensitivity = 100f; // Mouse look sensitivity
    public bool lockCursor = true;     // Whether to lock mouse to game window
    
    // Private variables
    private CharacterController controller; // Unity's built-in character controller
    private Vector3 velocity;             // Current movement velocity
    private bool isGrounded;              // Is player touching ground?
    private float xRotation = 0f;         // Camera up/down rotation
    private float currentSpeed;           // Current movement speed
    private float gravity = -9.81f;       // Gravity strength
    
    void Start()
    {
        // Get the character controller component
        controller = GetComponent<CharacterController>();
        currentSpeed = moveSpeed;  // Start with normal move speed
        
        // Lock and hide cursor for first-person look
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    void Update()
    {
        // Check if grounded using a sphere cast
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        // Reset downward velocity when grounded to prevent accumulation
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value instead of zero for better grounding
        }
        
        // Mouse look handling
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        // Calculate camera rotation - vertical rotation
        xRotation -= mouseY;  // Subtract to invert the mouse Y axis
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent looking too far up/down
        
        // Apply rotations - vertical to camera, horizontal to player
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);  // Rotate player horizontally
        
        // Movement inputs from keyboard
        float x = Input.GetAxis("Horizontal");  // A/D or left/right arrows
        float z = Input.GetAxis("Vertical");    // W/S or up/down arrows
        
        // Speed control - sprint, crouch, or normal movement
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;  // Sprint when shift is held
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = crouchSpeed;  // Crouch when ctrl is held
        }
        else
        {
            currentSpeed = moveSpeed;    // Normal speed otherwise
        }
        
        // Calculate movement direction relative to where the player is facing
        Vector3 move = transform.right * x + transform.forward * z;
        
        // Apply movement using character controller
        controller.Move(move * currentSpeed * Time.deltaTime);
        
        // Jumping when grounded and jump button pressed
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Jump formula: v = sqrt(h * -2 * g) - physics formula for initial velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        // Apply gravity to vertical velocity
        velocity.y += gravity * Time.deltaTime;
        
        // Apply vertical movement (jumping/falling)
        controller.Move(velocity * Time.deltaTime);
    }
}