using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    public abstract class SFXEvent : ScriptableObject
    {
        [Header("General Settings")]
        [SerializeField] AudioClip[] possibleClips = new AudioClip[0];
        [SerializeField] AudioMixerGroup mixer = null;
        [Space]

        [SerializeField]
        [Range(0, 128)]
        int priority = 128;

        [SerializeField][Range(0,1)]
        float volume = 0.8f;

        [SerializeField][Range(-3, 3)]
        float pitch = 1.0f;

        [SerializeField]
        [Range(-1, 1)]
        private float stereoPan = 0;

        [SerializeField]
        [Range(0, 1)]
        float spatialBlend = 0;

        [Header("3D Settings")]

        [SerializeField] float attenuationMin = 1;
        [SerializeField] float attenuationMax = 500;

        int clipIndex = 0;

        public AudioClip Clip => possibleClips[clipIndex];
        public AudioMixerGroup Mixer => mixer;

        public int Priority => priority;
        public float Volume => volume; //{ get; private set; }
        public float Pitch => pitch;
        public float StereoPan => stereoPan;

        public float SpatialBlend => spatialBlend;

        public float AttenuationMin => attenuationMin;
        public float AttenuationMax => attenuationMax;

        protected void SetVariationValues()
        {
            clipIndex = Random.Range(0, possibleClips.Length); // Should randomize clip but not working.
            Debug.Log("SetVariationValues got called");
        }
    }
}
