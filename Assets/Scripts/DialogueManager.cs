using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueText _dialogueText;

    private Queue<DialogueText.Message> _dialogueQueue;

    [SerializeField] private TextMeshProUGUI uiCharacterName;
    [SerializeField] private TextMeshProUGUI uiDialogue;

    private void Start()
    {
        _dialogueQueue = new Queue<DialogueText.Message>();
        foreach (var message in _dialogueText.messages)
        {
            _dialogueQueue.Enqueue(message);
        }
    }

    public void ShowNextMessage()
    {
        var message = _dialogueQueue.Dequeue();
        
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