using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float distanciaInteraccion = 7.0f;
    public KeyCode teclaInteraccion = KeyCode.E;

    [Header("UI y Referencias")]
    public TextMeshProUGUI textoInteraccion;

    private bool estaBloqueado = false;
    private Transform interactableActual;

    void Start()
    {
        OcultarTexto();
    }

    void Update()
    {
        // 1. Si hay un diálogo activo, verificamos la distancia o detenemos la interacción
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            OcultarTexto();

            // Si te alejas del objeto con el que hablas, cierra el diálogo
            if (interactableActual != null)
            {
                float dist = Vector3.Distance(transform.position, interactableActual.position);
                if (dist > distanciaInteraccion)
                {
                    DialogueManager.Instance.EndDialogue();
                    interactableActual = null;
                }
            }
            return;
        }

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
                interactableActual = hit.collider.transform;
                MostrarTexto("Presiona E para interactuar");

                if (Input.GetKeyDown(teclaInteraccion) || Input.GetMouseButtonDown(0))
                {
                    OcultarTexto();
                    interactable.Interact();
                }
                return;
            }
        }

        interactableActual = null;
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

    public void SetBloqueado(bool estado)
    {
        estaBloqueado = estado;
        if (estado)
        {
            OcultarTexto();
        }
    }
}