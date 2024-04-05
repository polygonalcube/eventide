using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class SoundManager : MonoBehaviour
{
    [SerializeField][Range(0, 1)] private float volumeMultiplier = 1;

    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    [SerializeField] private List<SoundEffect> soundEffects;
    //public void OnValidate()
    //{
    //    for (int i = 0; i < soundEffects.Count; i++)
    //    {
    //        soundEffects[i].OnValidate();
    //    }
    //}
    public void PlayClickSoundEffect() => PlaySoundEffect("click");
    /// <summary>
    /// Plays a certain sound from an instance of the SoundManager. Volume is constant
    /// </summary>
    /// <param name="soundType">The type of sound that will play</param>
    public void PlaySoundEffect(string soundName) => PlaySoundEffect(soundName, 0);
    /// <summary>
    /// Plays a certain sound from an instance of the SoundManager. Volume is constant
    /// </summary>
    /// <param name="soundType">The type of sound that will play</param>
    /// <param name="modifier">Modifier on the pitch of the sound</param>
    public void PlaySoundEffect(string soundName, int modifier)
    {
        var matchingEffects = soundEffects.Where(s => s.SoundName.Equals(soundName)).ToList();
        var soundEffect = matchingEffects[Random.Range(0, matchingEffects.Count)];
        var chosenClip = soundEffect.Clips[Random.Range(0, soundEffect.Clips.Count)];
        var newSoundEffect = new GameObject($"Sound: {soundName}, {chosenClip.length}s");
        newSoundEffect.transform.parent = transform;
        Destroy(newSoundEffect, chosenClip.length * 1.5f);
        var source = newSoundEffect.AddComponent<AudioSource>();
        source.clip = chosenClip;
        source.volume = soundEffect.Volume * volumeMultiplier;
        if (soundEffect.Vary) source.pitch += Random.Range(-0.1f, 0.1f);
        source.pitch += 0.05f * modifier; 
        source.Play();
    }
}

[System.Serializable]
public struct SoundEffect
{
    private string name;
    public string SoundName;
    public List<AudioClip> Clips;
    [Range(0, 1)] public float Volume;
    public bool Vary;
    //public void OnValidate() => name = Type.ToString();
}