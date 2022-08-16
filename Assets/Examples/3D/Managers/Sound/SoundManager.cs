using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] int _startingPoolSize = 5;
    SoundPool soundPool;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        Initialize();
    }

    public void PlayOneShot(SFXOneShot soundEvent, Vector3 soundPosition)
    {
        if (soundEvent.Clip == null)
        {
            Debug.LogWarning("SoundManager.PlayOneShot: no clip specified");
            return;
        }

        AudioSource newSource = soundPool.Get();
        newSource.clip = soundEvent.Clip;
        
        newSource.outputAudioMixerGroup = soundEvent.Mixer;
        newSource.priority = soundEvent.Priority;
        newSource.volume = soundEvent.Volume;
        newSource.pitch = soundEvent.Pitch;
        newSource.panStereo = soundEvent.StereoPan;
        newSource.spatialBlend = soundEvent.SpatialBlend;

        newSource.minDistance = soundEvent.AttenuationMin;
        newSource.maxDistance = soundEvent.AttenuationMax;
        
        newSource.transform.position = soundPosition;
        
        ActivatePooledSound(newSource);
    }

    void Initialize()
    {
        soundPool = new SoundPool(this.transform, _startingPoolSize);
    }
    private void ActivatePooledSound(AudioSource newSource)
    {
        newSource.gameObject.SetActive(true);
        newSource.Play();

        StartCoroutine(DisableAfterCompleteRoutine(newSource));
    }
    IEnumerator DisableAfterCompleteRoutine(AudioSource source)
    {
        // ensure that looping isn't false. We don't want to disable a looping sound
        source.loop = false;

        float clipDuration = source.clip.length;
        yield return new WaitForSeconds(clipDuration);
        
        // disable
        source.Stop();
        soundPool.Return(source);
    }
}
