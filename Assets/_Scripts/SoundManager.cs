using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private const string SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    [SerializeField] private AudioClipRefsSO _audioClipsRefSO;

    private bool _isGamePlaying;
    private bool _isMoving;
    private bool _wasAlreadyMoving;
    private float _volume = 1f;
    private bool _hasMoved = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one SoundManager");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (PlayerPrefs.HasKey(SOUND_EFFECTS_VOLUME))
        {
            _volume = PlayerPrefs.GetFloat(SOUND_EFFECTS_VOLUME);
        }
        else
        {
            _volume = 1f;
        }
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused; ;
    }

    private void Update()
    {
        _isMoving = PlayerController.Instance.IsMoving();

        if (_isMoving && _hasMoved)
        {
            PlayMovingSound(PlayerController.Instance.GetPlayerAudioSource());
            _hasMoved = false;
        }
        else
        {
            _hasMoved = true;
        }
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        if (!_wasAlreadyMoving)
        {
            _isMoving = true;
            _wasAlreadyMoving = true;
        }
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        if (_wasAlreadyMoving && _isMoving)
        {
            _wasAlreadyMoving = false;
            _isMoving = false;
        }
    }



    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f, bool isGamePlayingRequired = false)
    {
        _isGamePlaying = GameManager.Instance.IsGamePlaying();

        if (!isGamePlayingRequired || _isGamePlaying)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, _volume * volumeMultiplier);
        }
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f, bool isGamePlayingRequired = false)
    {
        _isGamePlaying = GameManager.Instance.IsGamePlaying();

        if (!isGamePlayingRequired || _isGamePlaying)
        {
            PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volumeMultiplier, isGamePlayingRequired);
        }
    }

    private IEnumerator PlayMovingSoundOnLoop(AudioSource playerAudioSource, AudioClip audioClip)
    {
        while (_isMoving)
        {
            playerAudioSource.Stop();
            playerAudioSource.clip = audioClip;
            playerAudioSource.Play();

            while (playerAudioSource.isPlaying)
            {
                yield return null;
                if (!_isMoving)
                {
                    playerAudioSource.Stop();
                    break;
                }
            }
        }
    }

    public void PlayPickupSound(Junk junk)
    {
        PlaySound(_audioClipsRefSO.PickUp, junk.transform.position);
    }

    public void PlayHitSound(Asteroid asteroid)
    {
        PlaySound(_audioClipsRefSO.Hit, asteroid.transform.position);
    }


    public void PlayMovingSound(AudioSource playerAudioSource)
    {
        _wasAlreadyMoving = true;
        _isMoving = true;

        StartCoroutine(PlayMovingSoundOnLoop(playerAudioSource, _audioClipsRefSO.MoveFire));
    }

    public void ChangeVolume()
    {
        _volume += .1f;

        if (_volume > 1.01f) // Due to odd precision of float
        {
            _volume = 0f;
        }

        PlayerPrefs.SetFloat(SOUND_EFFECTS_VOLUME, _volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return _volume;
    }

    public void StopMoving()
    {
        _isMoving = false;
    }
}
