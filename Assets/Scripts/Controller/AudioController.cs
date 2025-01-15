using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;
    }

    public Sound[] sounds;

    private Dictionary<string, AudioSource> soundDictionary;

    void Awake()
    {
        // Implementação do padrão Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        soundDictionary = new Dictionary<string, AudioSource>();

        // Cria um AudioSource para cada som e armazena no dicionário
        foreach (Sound s in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.pitch = s.pitch;
            source.loop = s.loop;
            soundDictionary[s.name] = source;
        }
    }

    // Método para reproduzir um som pelo nome
    public void Play(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioSource source))
        {
            source.Play();
        }
        else
        {
            Debug.LogWarning($"Som '{soundName}' não encontrado!");
        }
    }

    // Método para parar um som pelo nome
    public void Stop(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioSource source))
        {
            source.Stop();
        }
        else
        {
            Debug.LogWarning($"Som '{soundName}' não encontrado!");
        }
    }
}
