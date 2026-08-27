using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// ¡IMPORTANTE! Debe llevar ', IInteractable' al lado de MonoBehaviour
public class PuertaCambioEscena : MonoBehaviour, IInteractable 
{
    [Header("Escena Destino")]
    public string nombreEscenaBogota = "Bogota";

    [Header("Transición Visual")]
    public Image panelFade;
    public float duracionFade = 1.0f;

    private bool cambiando = false;

    public void Interact()
    {
        if (cambiando) return;
        StartCoroutine(SecuenciaPuerta());
    }

    private IEnumerator SecuenciaPuerta()
    {
        cambiando = true;

        PlayerInteractor interactor = FindAnyObjectByType<PlayerInteractor>();
        if (interactor != null)
        {
            interactor.SetBloqueado(true);
        }

        float tiempo = 0f;
        if (panelFade != null)
        {
            Color c = panelFade.color;
            while (tiempo < duracionFade)
            {
                tiempo += Time.deltaTime;
                c.a = Mathf.Lerp(0f, 1f, tiempo / duracionFade);
                panelFade.color = c;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (!string.IsNullOrEmpty(nombreEscenaBogota))
        {
            SceneManager.LoadScene(nombreEscenaBogota);
        }
    }
}