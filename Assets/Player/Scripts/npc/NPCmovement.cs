using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float speed = 2.5f;
    public Vector3 direction = Vector3.forward; // Dirección local o global

    [Header("Animaciones")]
    public Animator anim;

    void Start()
    {
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }

        // Activa la animación de caminar si existe el parámetro
        if (anim != null)
        {
            anim.SetBool("isWalking", true);
        }
    }

    void Update()
    {
        // Desplaza al NPC en la dirección hacia la que está mirando
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.Self);
    }
}