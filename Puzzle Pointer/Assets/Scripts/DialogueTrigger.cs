using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool hasBeenTalkedTo = false;

    public void TriggerDialogue()
    {
        if (!hasBeenTalkedTo)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
    }
}
