using UnityEngine;

[System.Serializable]
public class Settings
{
    public static Settings Instance { get; private set; }
    #region Input
    public byte _mouseSensitivity = 100;
    public bool _invertMouseX = false;
    public bool _invertMouseY = false;

    public KeyCode _front = KeyCode.W;
    public KeyCode _left = KeyCode.A;
    public KeyCode _back = KeyCode.S;
    public KeyCode _right = KeyCode.D;
    public KeyCode _jump = KeyCode.Space;
    public KeyCode _sprint = KeyCode.LeftShift;
    public KeyCode _crouch = KeyCode.LeftControl;
    public KeyCode _flashlight = KeyCode.F;
    public KeyCode _zoom = KeyCode.Z;
    public KeyCode _interact = KeyCode.E;
    public KeyCode _drop = KeyCode.Q;
    #endregion

    #region Graphics
    public bool _vignette;
    public bool _grain;
    public bool _ambientOcclusion;
    public byte _gamma;
    public bool _vSync;
    public byte _maxFps;
    public byte _viewDistance;
    public byte _shadowsQuality;
    public byte _particleQuality;
    public byte _TextureQuality;
    public byte _antiAliasing;
    #endregion

    #region Audio
    public byte _masterVolume;
    public byte _ambient;
    public byte _music;
    public byte _guiSounds;
    #endregion

    #region UI
    public byte _scale;
    public string _language;
    #endregion
    public Settings()
    {
        Instance = this;
    }
}
