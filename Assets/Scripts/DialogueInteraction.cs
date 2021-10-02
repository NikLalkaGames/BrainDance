using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueInteraction : MonoBehaviour, IPointerDownHandler
{
    public DialogueManager dialogueManager;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer clicked");
        
        dialogueManager.ShowNextMessage();
    }
}