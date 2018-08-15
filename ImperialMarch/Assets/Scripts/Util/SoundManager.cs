using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : Singleton<SoundManager>
{
    public Sound[] sounds = new Sound[1];

    private void Awake()
    {
        Init();
    }

    public void Init() 
	{
        foreach (Sound s in sounds)
        {
            if (s.multipleSound)
                continue;

            CreateSoundComponent(s);
        }
    }

    AudioSource CreateSoundComponent(Sound s)
    {
        s.source = gameObject.AddComponent<AudioSource>();

        SetSoundSource(s.source, s);

        return s.source;
    }

    void SetSoundSource(AudioSource _source, Sound _sound)
    {
        _source.clip = _sound.clip;
        _source.volume = _sound.volume;
        _source.pitch = _sound.pitch;
        _source.loop = _sound.loop;

        if(_sound.audioMixerGroup != null)
        {
            _source.outputAudioMixerGroup = _sound.audioMixerGroup;
        }
    }

    public AudioSource Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }

        if(!s.multipleSound)
            s.source.Play();
        else
        {
            //同时可能有多个的声音，而且会单独关闭
            AudioSource source = CreateSoundComponent(s);
            source.Play();
            return source;
        }

        return null;
    }

    public void StopPlay(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    public GameObject PlayAtPoint(string _name, Vector3 _pos)
    {
        Sound s = Array.Find(sounds, sound => sound.name == _name);

        GameObject go = ObjectPoolManager.Instance().SpawnObject("SoundSource", _pos, Quaternion.identity);

        SetSoundSource(go.GetComponent<AudioSource>(), s);
        go.GetComponent<AudioSource>().Play();

        StartCoroutine(PutbackWhenSoundStop(go));

        return go;
    }

    IEnumerator PutbackWhenSoundStop(GameObject _go)
    {
        yield return new WaitForSeconds(_go.GetComponent<AudioSource>().clip.length);

        ObjectPoolManager.Instance().PutbackObject(_go);
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;
    [Range(.1f, 3f)]
    public float pitch = 1;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    public AudioMixerGroup audioMixerGroup;

    public bool multipleSound;
}

