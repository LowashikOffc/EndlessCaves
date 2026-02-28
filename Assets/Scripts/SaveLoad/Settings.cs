using System;

[Serializable]
public class Settings
{
    #region Input
    public byte _mouseSensitivity;
    public bool _invertMouseX;
    public bool _invertMouseY;
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

}
