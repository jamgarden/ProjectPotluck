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

    private int index = 0; private bool notTalking = true;
    public Dialogue dialogue;

    public Animator animator;
    public EyeBlinker eyeBlinker;
    public Material mouth;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && notTalking)
        {
            StartCoroutine(SpeakLine(dialogue.Sentences[index].sentence, dialogue.Sentences[index].audioClip));
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

        if(animator)
        {
            animator.CrossFade("metarig|" + dialogue.Sentences[index].bodyLanguage.ToString(), 0.2f);
        }

        if(clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

        if(dialogue.Sentences[index].expression == Dialogue.Sentence.Expression.Angry)
        {
            eyeBlinker.eyeMat.SetTexture("_MainTex", eyeBlinker.eyeAngry);
        }
        else if (dialogue.Sentences[index].expression == Dialogue.Sentence.Expression.Sad)
        {
            eyeBlinker.eyeMat.SetTexture("_MainTex", eyeBlinker.eyeSad);
        }
        else if (dialogue.Sentences[index].expression == Dialogue.Sentence.Expression.Neutral)
        {
            eyeBlinker.eyeMat.SetTexture("_MainTex", eyeBlinker.eyeDefault);
        }
        else
        {
            //Do nothing
        }

        float till = 0;

        //Type text
        foreach (char c in s.ToCharArray())
        {
            SpokenText.text += c;
            yield return new WaitForSeconds(textSpeed);

            //talking mouth
            if (mouth)
            {
   
                till += 0.25f;
                if (till > 1)
                {
                    till = 0;
                }

                mouth.SetTextureOffset("_MainTex", new Vector2(till, 0));
            }
        }
    }

    void NextLine()
    {
        if(index < dialogue.Sentences.Length -1)
        {
            index++;
            SpokenText.text = string.Empty;
            StartCoroutine(SpeakLine(dialogue.Sentences[index].sentence, dialogue.Sentences[index].audioClip));
        }

        else
        {
            DialogueBox.SetActive(false);
            notTalking = true;
            index = 0;
            eyeBlinker.eyeMat.SetTexture("_MainTex", eyeBlinker.eyeDefault);
            animator.CrossFade("metarig|Idle", 0.2f);
            audioSource.Stop();
        }

    }
}
