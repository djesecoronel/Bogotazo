using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Componentes")]
    private CharacterController controller;
    private Animator animator;

    [Header("Ajustes de Velocidad")]
    public float walkSpeed = 5.0f;
    public float runSpeed = 8.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Sensibilidad y Cámara")]
    public float mouseSensitivity = 100.0f;
    public Transform cameraHolder; // Arrastra tu Main Camera aquí
    private float xRotation = 0f;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        // 1. Ocultar y bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 2. Control de Rotación de Cámara con el Mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotar el cuerpo del personaje en horizontal (Y)
        transform.Rotate(Vector3.up * mouseX);

        // Rotar la cámara en vertical (X) manteniendo límites
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -35f, 60f); // Evita giros extraños hacia arriba/abajo

        if (cameraHolder != null)
        {
            cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // 3. Detección de suelo
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        else
        {
            isGrounded = controller.isGrounded;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 4. Lectura de Entradas (WASD) y Carrera
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isMoving = (x != 0 || z != 0);
        bool isSprinting = isMoving && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 5. Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 6. Gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 7. Animaciones
        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving && !isSprinting);
            animator.SetBool("isRunning", isSprinting);
            animator.SetBool("isJumping", !isGrounded);
        }

        // Presionar ESC para liberar el puntero si lo necesitas en edición
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}