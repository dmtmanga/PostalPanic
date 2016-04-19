using UnityEngine;
using System.Collections;

public class DevScreenAudioMix : MonoBehaviour {

    private AudioSource audioSource;
    public AudioClip[] mixer = new AudioClip[3];


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        int clip = Random.Range(0,3);
        audioSource.PlayOneShot(mixer[clip]);
    }
	
}
