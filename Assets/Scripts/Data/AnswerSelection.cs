using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Dialogs/AnswerSelection", order = 0)]
    public class AnswerSelection : ScriptableObject
    {
        public List<string> answers;
    }
}