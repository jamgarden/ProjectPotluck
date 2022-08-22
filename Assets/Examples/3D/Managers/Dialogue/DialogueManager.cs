using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialogueBox;

    public AudioSource audioSource;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI SpokenText;
    public float textSpeed = 0.2f;

    private int index; private bool notTalking = true;
    public Dialogue dialogue;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && notTalking)
        {
            StartCoroutine(SpeakLine(dialogue.Sentences[index].sentence, dialogue.Sentences[index].clip));
        }

        if (Input.GetKeyDown(KeyCode.Space) && !notTalking)
        {
            if (SpokenText.text == dialogue.Sentences[index].sentence.ToString())
            {
                NextLine();
            }

            else
            {
                StopAllCoroutines();
                SpokenText.text = dialogue.Sentences[index].sentence;
            }
        }
    }

    IEnumerator SpeakLine (string s, AudioClip clip)
    {
        if (!DialogueBox.gameObject.activeInHierarchy) DialogueBox.SetActive(true);
        speakerName.text = dialogue.Sentences[index].name;
        notTalking = false;

        if(clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

        //Type text
        foreach (char c in s.ToCharArray())
        {
            SpokenText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < dialogue.Sentences.Length -1)
        {
            index++;
            SpokenText.text = string.Empty;
            StartCoroutine(SpeakLine(dialogue.Sentences[index].sentence, dialogue.Sentences[index].clip));
        }

        else
        {
            DialogueBox.SetActive(false);
            notTalking = true;
            index = 0;
        }

    }
}
