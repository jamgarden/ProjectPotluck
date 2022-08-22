using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Conversation", fileName = "name")]
public class Dialogue : ScriptableObject
{
    [System.Serializable]
    public class Sentence
    {
        public string name;
        [TextArea]
        public string sentence;
        public AudioClip clip;
    }

    public Sentence []Sentences;
}
