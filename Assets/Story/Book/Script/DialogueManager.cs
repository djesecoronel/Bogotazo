using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public bool IsDialogueActive { get; private set; } = false;

    [Header("Referencias de UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueBodyText;
    
    [Tooltip("Objeto de texto o UI que dice 'Presiona E para continuar'")]
    [SerializeField] private GameObject promptContinuarUI; 

    [Header("Configuración de Texto")]
    [SerializeField] private float typingSpeed = 0.03f;

    private Queue<string> sentences = new Queue<string>();
    private bool isTyping = false;
    private string currentSentence = "";
    private bool justStarted = false;
    private bool isEnding = false; // Evita reinteractuar en el mismo frame

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(string speakerName, string[] dialogueSentences)
    {
        if (isEnding) return;

        IsDialogueActive = true;
        justStarted = true;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (speakerNameText != null)
            speakerNameText.text = speakerName;

        if (promptContinuarUI != null)
            promptContinuarUI.SetActive(false);

        sentences.Clear();
        foreach (string sentence in dialogueSentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    private void Update()
    {
        if (!IsDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Evita que la E con la que abres el diálogo consuma la primera frase
            if (justStarted)
            {
                justStarted = false;
                return;
            }

            if (isTyping)
            {
                // Si está escribiendo, autocompleta la frase de golpe
                StopAllCoroutines();
                dialogueBodyText.text = currentSentence;
                isTyping = false;

                if (promptContinuarUI != null)
                    promptContinuarUI.SetActive(true);
            }
            else
            {
                // Pasa a la siguiente frase o cierra el panel
                DisplayNextSentence();
            }
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (promptContinuarUI != null)
            promptContinuarUI.SetActive(false);

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueBodyText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueBodyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (promptContinuarUI != null)
        {
            promptContinuarUI.SetActive(true);
        }
    }

    public void EndDialogue()
    {
        IsDialogueActive = false;
        StopAllCoroutines();
        
        if (promptContinuarUI != null)
            promptContinuarUI.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        // Bloqueo de un breve frame para consumir la 'E' de cierre sin reiniciar el diálogo
        StartCoroutine(ResetEndingFlag());
    }

    private IEnumerator ResetEndingFlag()
    {
        isEnding = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.2f);
        isEnding = false;
    }
}