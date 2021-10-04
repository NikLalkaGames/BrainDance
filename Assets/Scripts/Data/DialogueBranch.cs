using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Dialogs/DialogBranch", order = 0)]
    public class DialogueBranch : ScriptableObject
    {
        public DialogueText[] variants;
    }
}