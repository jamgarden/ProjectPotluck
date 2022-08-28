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
        public enum Expression {Neutral , Happy, Angry, Sad };
        public Expression expression;
        public enum BodyLanguage {Worry, Jumpy, HandShake, Idle};
        public BodyLanguage bodyLanguage;

        [TextArea]
        public string sentence;
        public AudioClip audioClip;

        [Space(5)]
        public Vector3 cameraPosition;
    }

    public Sentence []Sentences;
}
