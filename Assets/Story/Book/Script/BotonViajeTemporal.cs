using UnityEngine;

public class BotonViajeTemporal : MonoBehaviour, IInteractable
{
    [Header("Referencias")]
    public TimeTravelManager timeTravelManager;

    public void Interact()
    {
        if (timeTravelManager != null)
        {
            // Llama exactamente a tu método existente
            timeTravelManager.StartTimeTravel();
        }
        else
        {
            Debug.LogError("[BotonViajeTemporal] Falta asignar el TimeTravelManager en el Inspector.");
        }
    }
}