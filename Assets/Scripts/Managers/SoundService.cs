using UnityEngine;

[System.Serializable]
public class SoundsLibrary
{
    public int _id;
    public AudioClip _audioClip;
}

public class SoundService : MonoBehaviour
{
    public static SoundService Instance { get; private set; }

    [SerializeField] private SoundsLibrary[] _sounds;
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
                newSound.Play();
                Destroy(newSoundObj, newSound.clip.length + 1f);
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
                newSound.Play();
                Destroy(newSoundObj, newSound.clip.length + 1f);
            }
        }
    }

    public void PlaySound3D(SoundID id, Vector3 position)
    {
        foreach (var sound in _sounds)
        {
            if (sound._id == ((int)id))
            {
                AudioSource.PlayClipAtPoint(sound._audioClip, position, 1);
            }
        }

    }
    public void PlaySound3D(SoundID id, Vector3 position, float volume)
    {
        foreach (var sound in _sounds)
        {
            if (sound._id == ((int)id))
            {
                AudioSource.PlayClipAtPoint(sound._audioClip, position, volume);
            }
        }

    }
}

public enum SoundID
{
    #region Game Sounds 1-100
    step1 = 1,
    step2 = 2,
    jump = 3,
    crouch = 4,
    zoom = 5,
    hookThrow = 6,
    hookReturn = 7,
    hookScroll = 8,
    hookCollide = 9,
    flashlight = 10,
    grounded = 11,
    #endregion

    #region UI Sounds 101-200
    buttonPress = 101,
    #endregion
}