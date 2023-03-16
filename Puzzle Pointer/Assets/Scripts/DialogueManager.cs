using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ReSharper disable InconsistentNaming
// ReSharper disable once Unity.PreferAddressByIdToGraphicsParams

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float textSpeed = 0.1f;
    
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI dialogueText;
    public Animator dialogueBoxAnimator;

    private Queue<string> sentences;

    [SerializeField] private PlayerMovement _playerMovement;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBoxAnimator.SetBool("IsOpen", true);
        
        NameText.text = dialogue.Name;
        
        sentences.Clear();

        foreach (string sentence in dialogue.sentences )
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void EndDialogue()
    {
        dialogueBoxAnimator.SetBool(IsOpen, false);
        _playerMovement.insideDialogue = false;
    }

}
