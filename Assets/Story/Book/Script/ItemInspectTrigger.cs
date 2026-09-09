using UnityEngine;

public class ItemInspectTrigger : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject interactionText;
    public GameObject inspectPanel;

    private bool isInside = false;

    private void Start()
    {
        if (interactionText != null) interactionText.SetActive(false);
        if (inspectPanel != null) inspectPanel.SetActive(false);
    }

    private void Update()
    {
        if (isInside && Input.GetKeyDown(KeyCode.E))
        {
            if (inspectPanel != null)
            {
                bool newState = !inspectPanel.activeSelf;
                inspectPanel.SetActive(newState);

                if (interactionText != null)
                {
                    interactionText.SetActive(!newState);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Esto imprime en la Consola el nombre exacto de todo lo que choca con la mesa
        Debug.Log("¡ALGO TOCÓ EL TRIGGER!: " + other.gameObject.name + " | Tag: " + other.gameObject.tag);

        if (other.CompareTag("Player") || other.name.ToLower().Contains("player"))
        {
            Debug.Log("--> ¡Es el jugador! Activando TextoInteraccion.");
            isInside = true;
            if (interactionText != null)
            {
                interactionText.SetActive(true);
            }
            else
            {
                Debug.LogError("Error: La casilla 'Interaction Text' está vacía en el Inspector.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.name.ToLower().Contains("player"))
        {
            isInside = false;
            if (interactionText != null) interactionText.SetActive(false);
            if (inspectPanel != null) inspectPanel.SetActive(false);
        }
    }
}