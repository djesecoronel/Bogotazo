using UnityEngine;

public class PeriodicoInteractable : MonoBehaviour, IInteractable
{
    [Header("UI del Periódico")]
    public GameObject periodicoUIPanel;

    private bool estaAbierto = false;
    private bool recienAbierto = false;

    void Update()
    {
        // Si se acaba de abrir en este frame, ignoramos la E en el Update para no cerrarlo de inmediato
        if (recienAbierto)
        {
            recienAbierto = false;
            return;
        }

        // Si el periódico está abierto y presionas 'E' o 'Escape', se cierra
        if (estaAbierto && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CerrarPeriodico();
        }
    }

    public void Interact()
    {
        if (periodicoUIPanel != null)
        {
            if (!estaAbierto)
            {
                AbrirPeriodico();
            }
        }
    }

    public void AbrirPeriodico()
    {
        estaAbierto = true;
        recienAbierto = true; // Marca que se abrió en este frame

        if (periodicoUIPanel != null)
        {
            periodicoUIPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerInteractor interactor = FindAnyObjectByType<PlayerInteractor>();
        if (interactor != null) interactor.SetBloqueado(true);
    }

    public void CerrarPeriodico()
    {
        estaAbierto = false;
        
        if (periodicoUIPanel != null)
        {
            periodicoUIPanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerInteractor interactor = FindAnyObjectByType<PlayerInteractor>();
        if (interactor != null) interactor.SetBloqueado(false);
    }
}