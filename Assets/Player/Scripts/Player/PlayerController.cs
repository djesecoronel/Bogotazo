using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento Base")]
    public float walkSpeed = 5.0f;
    public float runSpeed = 100.0f;
    public float jumpHeight = 1.5f;

    [Header("Modo Vuelo")]
    public float flySpeed = 40.0f;
    public float flyVerticalSpeed = 30.0f;
    private bool isFlying = false;

    [Header("Física y Gravedad")]
    public float gravity = -19.62f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Animaciones")]
    public Animator anim;

    [Header("Seguridad (Caída al vacío)")]
    public float limiteCaidaY = -15.0f;
    public Vector3 posicionReaparicion = new Vector3(0f, 1f, 0f);

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Asignación automática del Animator en el objeto o sus hijos
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        // 1. Alternar modo Vuelo con la tecla T
        if (Input.GetKeyDown(KeyCode.T))
        {
            isFlying = !isFlying;
            velocity = Vector3.zero; // Reiniciar inercia al cambiar de modo
        }

        // 2. Lógica según el modo activo
        if (isFlying)
        {
            ManejarVuelo();
        }
        else
        {
            ManejarMovimientoTerrestre();
        }

        // 3. Sistema de seguridad por si cae fuera del mapa
        if (transform.position.y < limiteCaidaY)
        {
            Reaparecer();
        }
    }

    void ManejarMovimientoTerrestre()
    {
        // Detección de suelo
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

        // Entradas de movimiento (WASD)
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Evaluaciones de estado
        bool isMoving = (x != 0 || z != 0);
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        // Desplazamiento local únicamente si hay entradas
        if (isMoving)
        {
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            Vector3 move = (transform.right * x + transform.forward * z).normalized;
            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        // Salto (Espacio)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (anim != null)
            {
                anim.SetTrigger("doJump");
            }
        }

        // Enviar parámetros al Animator (incluye isJumping)
        if (anim != null)
        {
            anim.SetBool("isWalking", isMoving);
            anim.SetBool("isRunning", isRunning);
            anim.SetBool("isJumping", !isGrounded); // true si está en el aire, false al tocar suelo
        }

        // Aplicar gravedad constante y acumulación de física
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void ManejarVuelo()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = (transform.right * x + transform.forward * z).normalized;

        // Elevación y descenso con clics del ratón
        float y = 0f;
        if (Input.GetMouseButton(1)) // Clic Derecho (Subir)
        {
            y = 1f;
        }
        else if (Input.GetMouseButton(0)) // Clic Izquierdo (Bajar)
        {
            y = -1f;
        }

        Vector3 flyDirection = move * flySpeed + transform.up * (y * flyVerticalSpeed);
        controller.Move(flyDirection * Time.deltaTime);

        // Apagar animaciones terrestres en el aire
        if (anim != null)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", false);
        }
    }

    // Compatibilidad con ObstacleCoursePack (Bounce.cs)
    public void HitPlayer(Vector3 velocityForce)
    {
        velocity = velocityForce;
    }

    public void HitPlayer(Vector3 velocityForce, float delay)
    {
        velocity = velocityForce;
    }

    public void HitPlayer(Vector3 velocityForce, Vector3 extraForce)
    {
        velocity = velocityForce + extraForce;
    }

    void Reaparecer()
    {
        controller.enabled = false;
        transform.position = posicionReaparicion;
        velocity = Vector3.zero;
        isFlying = false;
        controller.enabled = true;
    }
}