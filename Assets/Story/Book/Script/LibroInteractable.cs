using UnityEngine;

public class LibroInteractable : MonoBehaviour, IInteractable
{
    [Header("UI del Libro")]
    public GameObject libroUIPanel;

    public void Interact()
    {
        if (libroUIPanel != null)
        {
            libroUIPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Bloquear el texto de interacción mientras el libro está abierto
            PlayerInteractor interactor = FindAnyObjectByType<PlayerInteractor>();
            if (interactor != null) interactor.SetBloqueado(true);
        }
    }

    public void CerrarLibro()
    {
        if (libroUIPanel != null)
        {
            libroUIPanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Desbloquear la interacción al cerrar el libro
        PlayerInteractor interactor = FindAnyObjectByType<PlayerInteractor>();
        if (interactor != null) interactor.SetBloqueado(false);
    }
}