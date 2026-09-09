using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BookController : MonoBehaviour
{
    [Header("Interfaz")]
    public GameObject bookPanel;
    public TMP_Text pageText;
    public TMP_Text pageNumberText;
    public Image pageImage;

    [Header("Botones")]
    public Button previousButton;
    public Button nextButton;
    public Button closeButton;

    [Header("Contenido del libro")]
    public TextAsset bookText;
    public Sprite[] pageImages;

    [Header("Eventos")]
    public UnityEvent OnBookClosed;

    private string[] pages;
    private int currentPage = 0;

    private void Awake()
    {
        LoadPages();

        // Asignar listeners a los botones por código
        if (previousButton != null) previousButton.onClick.AddListener(PreviousPage);
        if (nextButton != null) nextButton.onClick.AddListener(NextPage);
        if (closeButton != null) closeButton.onClick.AddListener(CloseBook);
    }

    private void OnEnable()
    {
        // Se ejecuta automáticamente cada vez que el libro se activa en pantalla
        currentPage = 0;
        ShowPage();
    }

    private void LoadPages()
    {
        if (bookText == null)
        {
            Debug.LogError("BookController: No se asignó el archivo del libro.");
            return;
        }

        pages = bookText.text.Split(
            new string[] { "[[PAGE]]" },
            System.StringSplitOptions.RemoveEmptyEntries
        );

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i] = pages[i].Trim();
        }
    }

    public void OpenBook()
    {
        if (bookPanel != null) bookPanel.SetActive(true);
        currentPage = 0;
        ShowPage();
    }

    public void CloseBook()
    {
        if (bookPanel != null) bookPanel.SetActive(false);
        OnBookClosed?.Invoke();
    }

    public void NextPage()
    {
        if (pages == null) return;

        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage();
        }
    }

    public void PreviousPage()
    {
        if (pages == null) return;

        if (currentPage > 0)
        {
            currentPage--;
            ShowPage();
        }
    }

    private void ShowPage()
    {
        if (pages == null || pages.Length == 0)
        {
            if (pageText != null) pageText.text = "El libro no contiene páginas.";
            return;
        }

        // 1. Cargar Texto
        if (pageText != null) pageText.text = pages[currentPage];

        // 2. Cargar Imagen de la página izquierda
        if (pageImage != null)
        {
            if (pageImages != null && currentPage < pageImages.Length && pageImages[currentPage] != null)
            {
                pageImage.gameObject.SetActive(true);
                pageImage.sprite = pageImages[currentPage];
            }
            else
            {
                pageImage.gameObject.SetActive(false);
            }
        }

        // 3. Cargar Número de Página
        if (pageNumberText != null)
        {
            pageNumberText.gameObject.SetActive(true);
            pageNumberText.text = "Página " + (currentPage + 1) + " de " + pages.Length;
        }

        // 4. Estado de Botones
        if (previousButton != null)
        {
            previousButton.interactable = currentPage > 0;
        }

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(currentPage < pages.Length - 1);
            nextButton.interactable = currentPage < pages.Length - 1;
        }

        if (closeButton != null)
        {
            closeButton.gameObject.SetActive(currentPage == pages.Length - 1);
        }
    }
}