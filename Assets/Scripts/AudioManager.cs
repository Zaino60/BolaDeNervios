using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    float _originalMusicVolume;
    [Header("UI")]
    //[Range(.0001f, 1f)][SerializeField] float _initMusicVol = 1f; //volumen inicial de musica
    //[Range(.0001f, 1f)][SerializeField] float _initSFXVol = 1f; //volumen inicial de sonidos
    [Header("Audio")]
    [SerializeField] AudioSource _musicSource, _effectsSource;
    //[SerializeField] AudioMixer _musicMixer, _sfxMixer;
    [SerializeField] GameObject _soundPrefab;
    public AudioClip[] sounds;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }

    }

    private void Start()
    {
        //SetMusicVolume(_initMusicVol);
        //SetSFXVolume(_initSFXVol);
        _originalMusicVolume = _musicSource.volume;
    }

    //public void SetMusicVolume(float value)
    //{
    //    _musicMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    //}

    //public void SetSFXVolume(float value)
    //{
    //    _sfxMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    //}

    //Metodo para reproducir sonidos.
    public void Play(string name, float volume = 1f, float pitch = 0f)
    {
        AudioClip s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError(s.name + " no existe");
            return;
        }
        if (pitch == 0) try { _effectsSource.PlayOneShot(s, volume); } catch { }
        else
        {
            GameObject newSound = Instantiate(_soundPrefab.gameObject);
            newSound.GetComponent<Sound>().audioS.clip = s;
            newSound.GetComponent<Sound>().audioS.volume = volume;
            newSound.GetComponent<Sound>().audioS.pitch = pitch;
        }
    }

    public void Play(int id, float volume = 1f, float pitch = 0f)
    {
        if(id >= sounds.Length)
        {
            Debug.LogError("out of bounds");
            return;
        }
        if (pitch == 0) try { _effectsSource.PlayOneShot(sounds[id], volume); } catch { }
        else
        {
            GameObject newSound = Instantiate(_soundPrefab.gameObject);
            newSound.GetComponent<Sound>().audioS.clip = sounds[id];
            newSound.GetComponent<Sound>().audioS.volume = volume;
            newSound.GetComponent<Sound>().audioS.pitch = pitch;
        }
    }

    public void ChangeLevelMusic(AudioClip clip)
    {
        if (_musicSource.clip != clip)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
            _musicSource.loop = true;
        }
        if (!clip) _musicSource.Stop();
    }

    //public void ButtonPressedSound()
    //{
    //    instance.Play("Buttonpress2");
    //}

    public AudioClip GetCurrentMusicClip()
    {
        return _musicSource.clip;
    }

    public void PauseMusic()
    {
        _musicSource.Stop();
    }

    public void ResumeMusic()
    {
        _musicSource.Play();
    }

}

