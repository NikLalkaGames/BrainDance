using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Dialogs/DialogueText", order = 0)]
    public class DialogueText : ScriptableObject
    {
        public TextAsset textFile;

        public List<Message> messages;

        public bool endsWithChoices;

        [System.Serializable]
        public class Message
        {
            public string speakersName;
            
            [TextArea(3, 10)]
            public string text;
        }

        public void ParseTextFile()
        {
            var pattern = @"^([^\s]*)(\s)";
            var regex = new Regex(pattern);
            var textArray = textFile.text.Split('\n');

            foreach (var line in textArray)
            {
                var heroName = regex.Match(line).Value;
                var sentences = line.Remove(0, heroName.Length); 
                
                messages.Add(new Message
                { 
                    speakersName = heroName,
                    text = sentences
                });
            }
        }
    }
}