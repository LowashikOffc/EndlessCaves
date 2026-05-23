using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundsLibrary
{
    public int _id;
    public AudioClip _audioClip;
    public AudioMixerGroup _group;
}

public class SoundService : MonoBehaviour
{
    public static SoundService Instance { get; private set; }

    [SerializeField] private SoundsLibrary[] _sounds;
    private int _destroyTimme = 5;


    private GameObject[] _soundPool;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _soundPool = new GameObject[20];
    }

    public void PlaySound(SoundID id)
    {
        foreach (var sound in _sounds)
        {
            if (sound._id == ((int)id))
            {
                GameObject newSoundObj = new GameObject($"Sound_{sound._audioClip.name}");
                AudioSource newSound = newSoundObj.AddComponent<AudioSource>();
                newSound.clip = sound._audioClip;
                newSound.spread = 360;
                newSound.outputAudioMixerGroup = sound._group;
                newSound.Play();
                Destroy(newSoundObj, newSound.clip.length + _destroyTimme);
            }
        }
    }
    public void PlaySound(SoundID id, float volume)
    {
        foreach (var sound in _sounds)
        {
            if (sound._id == ((int)id))
            {
                GameObject newSoundObj = new GameObject($"Sound_{sound._audioClip.name}");
                AudioSource newSound = newSoundObj.AddComponent<AudioSource>();
                newSound.clip = sound._audioClip;
                newSound.spread = 360;
                newSound.volume = volume;
                newSound.outputAudioMixerGroup = sound._group;
                newSound.Play();
                Destroy(newSoundObj, newSound.clip.length + _destroyTimme);
            }
        }
    }

    public void PlaySound3D(SoundID id, Vector3 position)
    {
        foreach (var sound in _sounds)
        {
            if (sound._id == ((int)id))
            {
                GameObject newSoundObj = new GameObject($"Sound_{sound._audioClip.name}");
                newSoundObj.transform.position = position;
                AudioSource newSound = newSoundObj.AddComponent<AudioSource>();
                newSound.clip = sound._audioClip;
                newSound.spatialBlend = 1;
                newSound.outputAudioMixerGroup = sound._group;
                newSound.Play();
                Destroy(newSoundObj, newSound.clip.length + _destroyTimme);
            }
        }

    }
    public void PlaySound3D(SoundID id, Vector3 position, float volume)
    {
        foreach (var sound in _sounds)
        {
            if (sound._id == ((int)id))
            {
                GameObject newSoundObj = new GameObject($"Sound_{sound._audioClip.name}");
                newSoundObj.transform.position = position;
                AudioSource newSound = newSoundObj.AddComponent<AudioSource>();
                newSound.clip = sound._audioClip;
                newSound.spatialBlend = 1;
                newSound.volume = volume;
                newSound.outputAudioMixerGroup = sound._group;
                newSound.Play();
                Destroy(newSoundObj, newSound.clip.length + _destroyTimme);
            }
        }

    }
}

public enum SoundID
{
    #region Game Sounds 1-100
    step1 = 1,
    step2 = 2,
    step3 = 3,
    step4 = 4,
    zoom = 5,
    hookThrow = 6,
    hookReturn = 7,
    ropePull = 8,
    hookCollide = 9,
    flashlight = 10,
    grounded = 11,
    jump = 12,
    horrosSteps = 13,
    DosienerClick = 14,
    DosienerAlarm = 15,
    #endregion

    #region UI Sounds 101-200
    uiPress = 101,
    uiHover = 102,
    uiHoverExit = 103,
    #endregion
}