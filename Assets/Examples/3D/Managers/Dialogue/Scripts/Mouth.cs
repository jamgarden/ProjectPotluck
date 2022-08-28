using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    [SerializeField] private Material mouth;

    private float till;

    public void PlayMouth()
    {
        StartCoroutine(MouthTime());
    }

    public void StopMouth()
    {
        StopAllCoroutines();
        mouth.SetTextureOffset("_MainTex", new Vector2(0, 0));
    }

    IEnumerator MouthTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            till += 0.25f;
            if (till > 1)
            {
                till = 0;
            }

            mouth.SetTextureOffset("_MainTex", new Vector2(till, 0));
        }
    }
}
    

