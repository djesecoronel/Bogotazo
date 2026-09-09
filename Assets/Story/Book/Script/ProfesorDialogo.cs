using UnityEngine;

public class ProfesorDialogo : MonoBehaviour, IInteractable
{
    [Header("Configuración del NPC")]
    [SerializeField] private string nombrePersonaje = "Profesor";
    [SerializeField] private string promptInteraccion = "Hablar con el Profesor";

    [Header("Historia del Bogotazo")]
    [TextArea(3, 5)]
    [SerializeField] private string[] lineasDialogo = new string[]
    {
        "Hola. Antes de que continúes explorando esta sala, debes comprender el contexto del objeto que tienes enfrente.",
        "El 9 de abril de 1948, el asesinato del líder popular Jorge Eliécer Gaitán desató 'El Bogotazo', transformando a Colombia para siempre.",
        "La ciudad se consumió en incendios y saqueos, marcando el inicio de una era de profunda violencia política.",
        "Observa bien los documentos del viaje temporal. Cada detalle guarda la memoria de aquel día."
    };

    // Si tu interfaz usa 'Interact' en vez de 'Interactuar', cambia el nombre del método abajo:
    public void Interact()
    {
        Debug.Log("¡Interacción detectada con el Profesor!");

        if (DialogueManager.Instance != null)
        {
            Debug.Log("DialogueManager encontrado, iniciando diálogo...");
            DialogueManager.Instance.StartDialogue(nombrePersonaje, lineasDialogo);
        }
        else
        {
            Debug.LogError("No se encontró el DialogueManager en la escena.");
        }
    }

    public void Interactuar()
    {
        Interact();
    }
}