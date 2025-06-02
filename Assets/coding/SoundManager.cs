using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    AudioClip pyrMoveEff;
    AudioSource pyrMoveEffS;

    private static SoundManager soundManager = null;

    const string PLAYER_MOVING_SOUND_EFFECT_PATH = "Audio/tung";
    void Start()
    {
        soundManager = this;

        pyrMoveEff = Resources.Load<AudioClip>(PLAYER_MOVING_SOUND_EFFECT_PATH);

        pyrMoveEffS = gameObject.AddComponent<AudioSource>();

        pyrMoveEffS.clip = pyrMoveEff;

        if (pyrMoveEff == null)
        {
            Debug.LogError("無法加載音頻文件: " + PLAYER_MOVING_SOUND_EFFECT_PATH);
            return;
        }
        if (pyrMoveEffS == null)
        {
            Debug.LogError("無法加載音頻文件: " + PLAYER_MOVING_SOUND_EFFECT_PATH);
            return;
        }
    }

    public void PlayWalkSoundEffect()
    {
        //Debug.Log(pyrMoveEffS.name);
        pyrMoveEffS.Play();
    }
    public static SoundManager SoundInstance
    {
        get
        {
            return soundManager;
        }
    }

    // Update is called once per frame
   
}
