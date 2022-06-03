using UnityEngine;

/// <summary>
/// Class <c>AudioManager</c> is a Unity script used to manage the general game audio behavior.
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Singleton

    // Singleton
    public static AudioManager Instance { get; private set; }

    #endregion

    #region Fields / Properties

    /// <summary>
    /// Instance field <c>levelMusic</c> is a Unity <c>AudioSource</c> component representing the level music audio source.
    /// </summary>
    [SerializeField]
    private AudioSource _levelMusic;

    /// <summary>
    /// Instance field <c>levelSetList</c> is an array of Unity <c>AudioClip</c> ressources representing the different level music audio clip.
    /// </summary>
    [SerializeField]
    private AudioClip[] _levelSetList;

    /// <summary>
    /// Instance field <c>isBossLevel</c> represents the is boss level status of the game.
    /// </summary>
    public bool isBossLevel;

    /// <summary>
    /// Instance field <c>parentBossRoomController</c> is a Unity <c>RoomController</c> representing the room controller of the boss room center game object.
    /// </summary>
    [SerializeField]
    private RoomController _parentBossRoomController;

    /// <summary>
    /// Instance field <c>levelBossMusic</c> is a Unity <c>AudioSource</c> component representing the boss level music audio source.
    /// </summary>
    [SerializeField]
    private AudioSource _levelBossMusic;

    /// <summary>
    /// Instance field <c>gameOverMusic</c> is a Unity <c>AudioSource</c> component representing the game over music audio source.
    /// </summary>
    [SerializeField]
    private AudioSource _gameOverMusic;

    /// <summary>
    /// Instance field <c>winMusic</c> is a Unity <c>AudioSource</c> component representing the level win music audio source.
    /// </summary>
    [SerializeField]
    private AudioSource _winMusic;

    /// <summary>
    /// Instance field <c>sfxSources</c> is an array of Unity <c>AudioSource</c> component representing the different game SFX.
    /// </summary>
    [SerializeField]
    private AudioSource[] _sfxSources;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if (!isBossLevel)
        {
            _levelMusic.clip = _levelSetList[Random.Range(0, _levelSetList.Length)];
            _levelMusic.Play();
        }
    }

    #endregion

    #region Public

    /// <summary>
    /// This function is responsible for playing the game over music.
    /// </summary>
    public void PlayGameOver()
    {
        if (!isBossLevel)
        {
            _levelMusic.Stop();
        }
        else
        {
            _levelBossMusic.Stop();
        }
        StopSFX();

        _gameOverMusic.Play();
    }

    public void StopMusic()
    {
        if (!isBossLevel)
        {
            _levelMusic.Stop();
        }
        else
        {
            _levelBossMusic.Stop();
        }
        StopSFX();
    }

    /// <summary>
    /// This function is responsible for playing the level win music.
    /// </summary>
    public void PlayLevelWin()
    {
        _levelMusic.Stop();
        _winMusic.Play();
    }

    public void PlayBossMusic()
    {
        _levelBossMusic.Play();
    }

    /// <summary>
    /// This function is responsible for playing the SFX sound based on a given SFX index value.
    /// </summary>
    public void PlaySFX(int sfxIndex)
    {
        _sfxSources[sfxIndex].Stop();
        _sfxSources[sfxIndex].Play();
    }

    /// <summary>
    /// This function is responsible for playing in loop the SFX sound based on a given SFX index value.
    /// </summary>
    public void PlaySFXLoop(int sfxIndex, bool doPlay)
    {
        if (doPlay && !_sfxSources[sfxIndex].isPlaying.Equals(doPlay))
        {
            _sfxSources[sfxIndex].Play();
        }
        else if (!doPlay)
        {
            _sfxSources[sfxIndex].Stop();
        }
    }

    /// <summary>
    /// This function is responsible for stoping any the SFX sound currently playing.
    /// </summary>
    public void StopSFX()
    {
        foreach (AudioSource sfxSource in _sfxSources)
        {
            if (sfxSource.isPlaying)
            {
                sfxSource.Stop();
            }
        }

    }

    #endregion
}