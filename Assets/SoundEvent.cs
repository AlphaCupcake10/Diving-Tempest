using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvent : MonoBehaviour
{
    [System.Serializable]
    public class SoundGroup
    {
        public AudioClip[] clips;
        [Range(0,1)]
        public float volume = 1;
        public float spatialBlend = 1;
    }

    public SoundGroup[] groups;

    public void PlaySoundGroup(int index)
    {
        PlaySound(groups[index].clips[Random.Range(0,groups[index].clips.Length-1)],groups[index].volume,groups[index].spatialBlend);
    }
    public void PlaySound(AudioClip clip,float volume,float spatialBlend)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = spatialBlend;
        source.Play();
        Destroy(source,source.clip.length);
    }
}
