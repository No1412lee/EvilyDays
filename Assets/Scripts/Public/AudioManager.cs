using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {


    public AudioData audioData;
    private AudioSource audioSource;
    
    private bool isDecrease=false;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isDecrease == true )
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, -0.3f, Time.deltaTime);
            if(audioSource.volume<0.01)
            {
                isDecrease = false;
            }
        }
        
	}
    public void SetMute()
    {
        audioSource.mute = true;
    }

    public void UIAudioSuccess()
    {
        audioSource.clip = audioData.audioClips[0];
        audioSource.volume = 0.3f;
        audioSource.Play();
    }
    public void UIAudioWrong()
    {
        audioSource.clip = audioData.audioClips[1];
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void UIAudioWin()
    {
        audioSource.clip = audioData.audioClips[2];
        audioSource.volume = 0.4f;
        audioSource.Play();
    }
    public void UIAudioFail()
    {
        if (audioSource.clip == audioData.audioClips[3])
            return;
        audioSource.clip = audioData.audioClips[3];
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void UIAudioError()
    {
        audioSource.clip = audioData.audioClips[4];
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void EnvAudioSelect()
    {
        audioSource.clip = audioData.audioClips[0];
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    public void EnvAudioGet()
    {
        audioSource.clip = audioData.audioClips[1];
        audioSource.volume = 0.8f;
        audioSource.Play();
    }
    public void EnvAudioStopSelect()
    {
     //   audioSource.clip = audioData.audioClips[0];
        if (audioSource.clip == audioData.audioClips[0] && audioSource.volume>0.05)
        {
            isDecrease = true;
         
        }
      //  audioSource.Play();
    }

    public void EnvAudioEnemyHit()
    {
        audioSource.clip = audioData.audioClips[4];
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    public void EnvAudioBuild()
    {
        audioSource.clip = audioData.audioClips[1];
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
    public void EnvAudioMergeSuccess()
    {
        audioSource.clip = audioData.audioClips[2];
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
    public void EnvAudioMergeFail()
    {

        audioSource.clip = audioData.audioClips[3];
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
    public void EnvAudioComing()
    {

        audioSource.clip = audioData.audioClips[5];
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
    public void EnvAudioDestroy()
    {
        audioSource.clip = audioData.audioClips[6];
        audioSource.volume = 0.6f;
        audioSource.Play();
    }

    public void EnAudioOpenMap()
    {
        audioSource.clip = audioData.audioClips[7];
        audioSource.volume = 0.6f;
        audioSource.Play();
    }

    public void BGMAudioRandomEnemyTime()
    {
        int temp = ((int)(Random.value*100)) %2+ 5;
        audioSource.clip = audioData.audioClips[temp];
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
    public void BGMAudioRandomNormalTime()
    {
        int temp = ((int)(Random.value * 100)) % 5;
        audioSource.clip = audioData.audioClips[temp];
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
    public void BGMUniversy()
    {
        audioSource.clip = audioData.audioClips[7];
        audioSource.volume = 0.5f;
        audioSource.Play();
    }


}
