using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeBlinker : MonoBehaviour
{
    public Material eyeMat;

    public Texture eyeAngry, eyeSad, eyeDefault;

    private void Start()
    {
        StartCoroutine(Blinking());
    }

    IEnumerator Blinking()
    {
        
        while(gameObject.activeSelf)
        {
            float till = 0;

            //Time between blink
            yield return new WaitForSeconds(Random.Range(3, 8));

            //count
            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(0.15f);
                till += 0.25f;

                if(till > 1)
                {
                    till = 0;
                }

                SetNewFrame(till);
            }

        }
    }

    void SetNewFrame(float t)
    {
        eyeMat.SetTextureOffset("_MainTex" , new Vector2(t, 0));
    }

}
