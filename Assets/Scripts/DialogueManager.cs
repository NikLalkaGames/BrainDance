using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Queue<DialogueText.Message> dialogueQueue;

    [SerializeField] private Text uiCharacterName;
    [SerializeField] private Text uiDialogue;

    private void Awake()
    {
        dialogueQueue = new Queue<DialogueText.Message>();
    }

    public void LoadDialogToQueue(DialogueText dialogueText)
    {
        dialogueQueue.Clear();
        dialogueText.messages.Clear();
        dialogueText.ParseTextFile();

        // if (dialogueText.messages.Count == 0)
        // {
        //     dialogueText.ParseTextFile();
        // }
        
        dialogueQueue = new Queue<DialogueText.Message>();
        foreach (var message in dialogueText.messages)
        {
            dialogueQueue.Enqueue(message);
        }
    }

    public void ShowNextMessage()
    {
        var message = dialogueQueue.Dequeue();
        
        StopAllCoroutines();

        uiCharacterName.text = " ";
        uiCharacterName.text = message.speakersName;
        StartCoroutine(TypeText(message.text));
    }
    
    private IEnumerator TypeText(string sentence)
    {
        uiDialogue.text = " ";
        foreach (char letter in sentence)
        {
            uiDialogue.text += letter;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }


}