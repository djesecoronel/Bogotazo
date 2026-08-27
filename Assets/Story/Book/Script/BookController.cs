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

    [Header("Botones")]
    public Button previousButton;
    public Button nextButton;
    public Button closeButton;

    [Header("Contenido del libro")]
    public TextAsset bookText;

    [Header("Eventos")]
    public UnityEvent OnBookClosed; // Evento para avisar que el libro se cerró

    private string[] pages;
    private int currentPage = 0;

    private void Start()
    {
        LoadPages();
        
        // Asignar listeners por código a los botones para evitar fallos
        if (previousButton != null) previousButton.onClick.AddListener(PreviousPage);
        if (nextButton != null) nextButton.onClick.AddListener(NextPage);
        if (closeButton != null) closeButton.onClick.AddListener(CloseBook);

        CloseBook();
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
        currentPage = 0;
        if (bookPanel != null) bookPanel.SetActive(true);
        ShowPage();
    }

    public void CloseBook()
    {
        if (bookPanel != null) bookPanel.SetActive(false);
        OnBookClosed?.Invoke(); // Dispara el evento al cerrar
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
            pageText.text = "El libro no contiene páginas.";
            return;
        }

        pageText.text = pages[currentPage];

        if (pageNumberText != null)
        {
            pageNumberText.text = "Página " + (currentPage + 1) + " de " + pages.Length;
        }

        if (previousButton != null)
        {
            previousButton.interactable = currentPage > 0;
        }

        if (nextButton != null)
        {
            nextButton.interactable = currentPage < pages.Length - 1;
        }
    }
}