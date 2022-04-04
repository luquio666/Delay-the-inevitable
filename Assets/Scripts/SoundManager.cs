using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

	public List<AudioSource> Sounds;
    private void Awake()
    {
		GameEvents.OnPlaySound += PlaySound;
		GameEvents.OnStopSound += StopSound;
		GameEvents.OnStopAllSounds += StopAllSounds;

		ConfigSounds();
	}
	private void OnDisable()
	{
		GameEvents.OnPlaySound -= PlaySound;
		GameEvents.OnStopSound -= StopSound;
		GameEvents.OnStopAllSounds -= StopAllSounds;
	}



	private void PlaySound(string soundName, bool loop)
	{
		var sound = FindAudio(soundName);
		Debug.Log("sound name: " + soundName);
		if (sound == null) return;
		if (sound.isPlaying)
			sound.Stop();
		sound.loop = loop;
		sound.Play();
	}

	private void StopSound(string soundName)
	{
		var sound = FindAudio(soundName);
		sound.Stop();
	}
	
	private void StopAllSounds()
	{
		foreach (var sound in Sounds)		
		{
			sound.Stop();
		}
	}

	private AudioSource FindAudio(string soundName)
    {
		return Sounds.FirstOrDefault(x => x.clip.name == soundName);
    }

	[ContextMenu("SetDefaultSettings")]
	public void ConfigSounds()
	{
		Sounds.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
			var child = transform.GetChild(i).GetComponentInChildren<AudioSource>();
			if (child != null && child.clip != null)
			{
				child.name = child.clip.name;
				child.playOnAwake = false;
				child.loop = false;
				Sounds.Add(child);
			}
			else
				child.name = "ERROR! :: (missing audio clip)";
		}
	}
}
