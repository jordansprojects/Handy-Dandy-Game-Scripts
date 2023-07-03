using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using EmeraldAI.Example;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public string deathScene;
    public ScreenFader screenFade;
    private EmeraldAIPlayerHealth health;
    public Grabber rGrabber;
    public Grabber lGrabber;

    public float VibrateFrequency = 0.3f;
    public float VibrateAmplitude = 0.1f;
    public float VibrateDuration = 0.1f;
    
    public Transform playerOrientation;
    private AudioSource[] allAudioSources;
    void Start()
    {
        health = GetComponent<EmeraldAIPlayerHealth>();
    
    }

    public void ScreenFadeDamage()
    {
        if (health.CurrentHealth > 0)
        {
            StartCoroutine(GotHit());
        }
    }

    public void stopAllAudio(){
	    allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
  	    foreach (AudioSource audioS in allAudioSources) {
         	audioS.Stop();
     }
    }
    public void OnDeath()
    {
        stopAllAudio();
	    rGrabber.TryRelease();
        lGrabber.TryRelease();

      //  GetComponent<CharacterController>().enabled = false;//edit
        // Deactivate all items with environment tag
			GameObject [] env = GameObject.FindGameObjectsWithTag("Environment");
			foreach (GameObject thing in env){
				thing.SetActive(false);
			}


        Death();

    }

    

    public IEnumerator GotHit()
    {
        InputBridge.Instance.VibrateController(VibrateFrequency, VibrateAmplitude, VibrateDuration, rGrabber.HandSide);
        InputBridge.Instance.VibrateController(VibrateFrequency, VibrateAmplitude, VibrateDuration, lGrabber.HandSide);

        screenFade.DoFadeIn();
        yield return new WaitForSeconds(0.2f);
        screenFade.DoFadeOut();
    }

    private void Death()
    {
       StopCoroutine(GotHit());
       screenFade.DoFadeIn();

        
        // Play death scene 
        SceneManager.LoadScene(deathScene);
        playerOrientation.rotation = Quaternion.Euler(new Vector3(0,0,0));
    }
}
