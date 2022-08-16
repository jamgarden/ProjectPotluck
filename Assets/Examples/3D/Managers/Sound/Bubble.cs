using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class Bubble : MonoBehaviour
{
    [SerializeField] SFXOneShot sound;

    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlayOneShot(sound, transform.position);
        transform.position = new Vector3(transform.position.x, 5, transform.position.z);
    }
}
