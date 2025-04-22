using UnityEngine;

public enum AudioType
{
    step,
    jumpLand,
    jumpHeavyLand,
    death,
    respawn,
    grapplingHookHit,
    grapplingHookThrow,
    wallCling,
    wallJump,
    checkpointReach,
    areaTransition,
    enduranceRunOut,
    collectibleGet,
    stalactiteFall,
    stalactiteRegrow,
    levelStart,
    uiButton,
    uiReturn,
    platformBreak,
    platformRespawn
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public struct AudioData
    {
        public AudioType type;
        public AudioSource source;
    }

    public AudioData[] audioData;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); }
        else                  { Instance = this;     }
    }

    public void PlaySound(AudioType type)
    {
        AudioData data = GetAudioData(type);
        if (type == AudioType.step 
         || type == AudioType.jumpLand 
         || type == AudioType.grapplingHookThrow 
         || type == AudioType.grapplingHookHit
         || type == AudioType.uiReturn
         || type == AudioType.platformBreak
         || type == AudioType.platformRespawn
         || type == AudioType.death
         || type == AudioType.uiButton)
        {
            data.source.pitch = Random.Range(0.75f, 1.5f);
        }

        else data.source.pitch = 1f;

        data.source.Play();
    }

    public void StopSound(AudioType type)
    {
        AudioData data = GetAudioData(type);
        if (data.source) data.source.Stop();
    }

    private AudioData GetAudioData(AudioType type)
    {
        for (int i = 0; i < audioData.Length; i++)
        {
            if (audioData[i].type == type)
            {
                return audioData[i];
            }
        }
        Debug.LogError("AudioManager: No clip found for type " + type);
        return new AudioData();
    }
}
