using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DialogueNPC : MonoBehaviour
{
    [SerializeField] Transform canvasWorldHolder;
    [SerializeField] Dialogue dialogue;
    [Space(5)]
    [SerializeField] private EyeBlinker eyeBlinker;
    [Space(5)]
    [SerializeField] Transform dialoguePositionForPlayer;


    private Animator animator;
    private Transform target; //Player to look at

    private Mouth mouth;

    private bool lookPlayer; 
    private bool closeEnough;

    private bool readyToTalk;
    private bool isTalking; 
    static float t = 0.0f;

    private void Start()
    {
        mouth = GetComponent<Mouth>();
        animator = GetComponent<Animator>();
        StartCoroutine(CoolDownAfterDialogue());
    }

    [SerializeField] Rig rig;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            lookPlayer = true;
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            lookPlayer = false;
            target = null;
        }
    }

    private void Update()
    {
        rigWeight();

        if (lookPlayer && !isTalking)
        {
            calculateDistanceToPlayer();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isTalking && closeEnough && readyToTalk)
            {
                DialogueManager.Instance.StartDialogue(this, dialogue);
                isTalking = true;
                readyToTalk = false;
                target.GetComponent<DPlayer>().enabled = false;
                target.position = dialoguePositionForPlayer.position;

                //Rotate player's character to NPC
                Vector3 targetDir = transform.position - target.position;
                targetDir.y = 0;
                target.GetChild(0).transform.rotation = Quaternion.LookRotation(targetDir);
            }
        }

        if(isTalking && closeEnough)
        {
            DialogueManager.Instance.HideCanvasWorld();
            closeEnough = false;
        }


    }

    void rigWeight()
    {
        if (lookPlayer && rig.weight < 1)
        {
            t += 0.5f * Time.deltaTime;

        }

        else if (!lookPlayer && rig.weight > 0)
        {
            t -= 0.5f * Time.deltaTime;
        }

        rig.weight = Mathf.Lerp(0, 1, t);
    }

    void calculateDistanceToPlayer()
    {
        float dist = (transform.position - target.position).sqrMagnitude;
            if (dist < 8 && !closeEnough)
            {
                closeEnough = true;
                DialogueManager.Instance.DisplayCanvasWorld(canvasWorldHolder);
            }

            else if (dist > 8 && closeEnough)
            {
                closeEnough = false;
                DialogueManager.Instance.HideCanvasWorld();
            }
    }

    public void DialogueFinished()
    {
        mouth.StopAllCoroutines();
        target.GetComponent<DPlayer>().enabled = true;
        isTalking = false;

        animator.CrossFade("metarig|Idle", 0.2f);
        StartCoroutine(CoolDownAfterDialogue());
    }

    IEnumerator CoolDownAfterDialogue()
    {
        yield return new WaitForSeconds(1);

        readyToTalk = true;
    }

    public void Gesture(int index)
    {
        animator.CrossFade("metarig|" + dialogue.Sentences[index].bodyLanguage.ToString(), 0.2f);

        if (dialogue.Sentences[index].expression == Dialogue.Sentence.Expression.Angry)
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
    }
}

/*
 *         animator.CrossFade("metarig|" + dialogue.Sentences[index].bodyLanguage.ToString(), 0.2f);
 * 
            animator.CrossFade("metarig|Idle", 0.2f);
            animator = null;
*/