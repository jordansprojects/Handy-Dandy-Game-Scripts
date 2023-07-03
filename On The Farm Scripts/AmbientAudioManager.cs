using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbientAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusicDay;
    [SerializeField] private AudioClip backgroundMusicNight1;
    [SerializeField] private AudioClip backgroundMusicNight2;
    [SerializeField] private List<AudioSource> audioSources;


    private void Start()
    {

	   // begin with day music for tutorial
        playDayMusic();
        if (audioSources[0] == null || audioSources[1] == null || audioSources[2] == null)
        {
            Debug.LogError("One or more AudioSource references are missing in AudioManager.");
            return;
        }

    }



    public void playDayMusic(){
	    // stop night music
    	    audioSources[1].Stop();
	    audioSources[2].Stop();
 	   // now play day music! 
 	    PlayClip(audioSources[0], backgroundMusicDay);
    
    }

    public void playNightMusic(){
	//stop day music
        audioSources[0].Stop();

	// now play night music
        PlayClip(audioSources[1], backgroundMusicNight1);
        PlayClip(audioSources[2], backgroundMusicNight2);

    }


    private void PlayClip(AudioSource audioSource, AudioClip clip)
    {

        if(audioSource != audioSources[2])
        {
            audioSources[1].Stop();
            audioSources[2].Stop();
        }


        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnDestroy()
    {
       
    }
}
