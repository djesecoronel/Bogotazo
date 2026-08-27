using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TimeTravelManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject videoUIPanel;
    public VideoPlayer videoPlayer;
    public Image flashImage;

    [Header("Configuración de Tiempos")]
    public float duracionFlashEntrada = 0.3f;
    public float duracionFlashSalida = 0.5f;

    [Header("Configuración Escena")]
    public string targetSceneName = "Nivel1_1948"; // Nombre exacto de tu escena

    private bool isTraveling = false;

    public void StartTimeTravel()
    {
        if (isTraveling) return;
        isTraveling = true;

        PlayerInteractor interactor = FindAnyObjectByType<PlayerInteractor>();
        if (interactor != null)
        {
            interactor.SetBloqueado(true);
        }

        StartCoroutine(SequenceWithFlashbang());
    }

    private IEnumerator SequenceWithFlashbang()
    {
        // 1. FLASH DE ENTRADA (Pantallazo blanco)
        yield return StartCoroutine(FadeFlash(0f, 1f, duracionFlashEntrada));

        // 2. ACTIVAR Y REPRODUCIR VIDEO
        if (videoUIPanel != null) videoUIPanel.SetActive(true);

        if (videoPlayer != null)
        {
            bool videoTerminado = false;

            // Escuchar cuándo termina el video realmente
            VideoPlayer.EventHandler endHandler = (vp) => { videoTerminado = true; };
            videoPlayer.loopPointReached += endHandler;

            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }

            videoPlayer.Play();

            // Esperar 2 frames para asegurar que el primer fotograma se proyecte en la RenderTexture
            yield return null;
            yield return null;

            // 3. QUITAR EL BLANCO PARA REVELAR EL VIDEO
            yield return StartCoroutine(FadeFlash(1f, 0f, duracionFlashSalida));

            // 4. ESPERAR A QUE TERMINE EL VIDEO
            while (!videoTerminado)
            {
                yield return null;
            }

            // Desuscribir el evento por seguridad
            videoPlayer.loopPointReached -= endHandler;
        }

        // 5. FLASH DE SALIDA (Pantallazo blanco final)
        yield return StartCoroutine(FadeFlash(0f, 1f, duracionFlashEntrada));

        // 6. CAMBIO DE ESCENA
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("[TimeTravelManager] No se especificó el nombre de la escena de destino.");
        }
    }

    private IEnumerator FadeFlash(float alphaInicio, float alphaFin, float duracion)
    {
        if (flashImage == null) yield break;

        float tiempo = 0f;
        Color colorActual = flashImage.color;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            colorActual.a = Mathf.Lerp(alphaInicio, alphaFin, tiempo / duracion);
            flashImage.color = colorActual;
            yield return null;
        }

        colorActual.a = alphaFin;
        flashImage.color = colorActual;
    }
}