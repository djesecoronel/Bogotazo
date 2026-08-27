using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 5.0f;
    public float runSpeed = 8.0f;
    public float jumpHeight = 1.5f;

    [Header("Física y Gravedad")]
    public float gravity = -19.62f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Seguridad (Caída al vacío)")]
    public float limiteCaidaY = -15.0f;
    public Vector3 posicionReaparicion = new Vector3(0f, 1f, 0f);

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Detección de suelo
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        else
        {
            isGrounded = controller.isGrounded;
        }

        // Reiniciar velocidad de gravedad cuando toca el piso
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // 2. Entradas de movimiento (WASD / Flechas)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Determinar si está corriendo (Shift Izquierdo)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Calcular dirección basada en la orientación LOCAL del personaje
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 3. Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 4. Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 5. Sistema de seguridad si cae fuera del mapa
        if (transform.position.y < limiteCaidaY)
        {
            Reaparecer();
        }
    }

    void Reaparecer()
    {
        // Desactivar temporalmente el controller para permitir la teleportación directa
        controller.enabled = false;
        transform.position = posicionReaparicion;
        velocity = Vector3.zero;
        controller.enabled = true;
    }
}