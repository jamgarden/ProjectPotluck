using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] DialogueCamera dialogueCamera;

    [SerializeField] GameObject canvasWorld;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;

    private DialogueNPC npc;
    private AudioSource audioS;
    private Dialogue dialogue;
    private Mouth mouth;
    private int index = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void DisplayCanvasWorld(Transform t)
    {
        canvasWorld.transform.position = t.position;
        canvasWorld.gameObject.SetActive(true);
    }
    public void HideCanvasWorld()
    {
        canvasWorld.gameObject.SetActive(false);
    }

    public void StartDialogue(DialogueNPC n, Dialogue d)
    {
        dialogueCamera.EnableCamera();
        npc = n;
        dialogue = d;
        audioS = npc.gameObject.GetComponent<AudioSource>();
        mouth = npc.gameObject.GetComponent<Mouth>();
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        //Set Camera
        dialogueCamera.RePoseCamera(dialogue.Sentences[index].cameraPosition, dialogue.Sentences[index].name);

        //Set name
        nameText.text = dialogue.Sentences[index].name;
        dialogueText.text = string.Empty;

        //Play mouth
        mouth.PlayMouth();

        //Play animation
        npc.Gesture(index);

        //Play audio
        audioS.clip = dialogue.Sentences[index].audioClip;
        audioS.Play();

        //Type each cahracter 1 by 1
        foreach (char c in dialogue.Sentences[index].sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        mouth.StopMouth();
    }
    void NextLine()
    {
        if(index < dialogue.Sentences.Length -1 )
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }

        else
        {
            npc.DialogueFinished();
            dialoguePanel.SetActive(false);
            npc = null;
            dialogue = null;
            index = 0;
            audioS.Stop();
            audioS = null;
            mouth = null;
            dialogueCamera.DisableCamera();
        }
    }

    private void Update()
    {
        if (dialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dialogueText.text == dialogue.Sentences[index].sentence.ToString())
                {
                    NextLine();
                }

                else
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogue.Sentences[index].sentence.ToString();
                }
            }
        }
    }
}
