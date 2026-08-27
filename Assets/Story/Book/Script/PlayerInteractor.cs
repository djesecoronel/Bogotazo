using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float distanciaInteraccion = 7.0f;
    public KeyCode teclaInteraccion = KeyCode.E;

    [Header("UI de Mensaje")]
    public TextMeshProUGUI textoInteraccion;

    // Control de estado para no mostrar texto en cinemáticas o menús
    private bool estaBloqueado = false;

    void Start()
    {
        OcultarTexto();
    }

    void Update()
    {
        // Si la interacción está bloqueada (leyendo libro o viajando), no procesamos nada
        if (estaBloqueado)
        {
            OcultarTexto();
            return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaInteraccion))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                MostrarTexto("Presiona E para interactuar");

                if (Input.GetKeyDown(teclaInteraccion) || Input.GetMouseButtonDown(0))
                {
                    OcultarTexto();
                    interactable.Interact();
                }
                return;
            }
        }

        OcultarTexto();
    }

    public void MostrarTexto(string mensaje)
    {
        if (estaBloqueado) return;

        if (textoInteraccion != null)
        {
            textoInteraccion.text = mensaje;
            if (!textoInteraccion.gameObject.activeSelf)
            {
                textoInteraccion.gameObject.SetActive(true);
            }
        }
    }

    public void OcultarTexto()
    {
        if (textoInteraccion != null && textoInteraccion.gameObject.activeSelf)
        {
            textoInteraccion.gameObject.SetActive(false);
        }
    }

    // Permite bloquear/desbloquear la interacción desde otros scripts (Libro, Cinemáticas, etc.)
    public void SetBloqueado(bool estado)
    {
        estaBloqueado = estado;
        if (estado)
        {
            OcultarTexto();
        }
    }
}