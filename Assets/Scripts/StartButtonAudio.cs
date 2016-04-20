using UnityEngine;
using System.Collections;

public class StartButtonAudio : MonoBehaviour {

    private AudioSource source;
    public AudioClip start;
    public AudioClip startAlready;
    public AudioClip startJingle;

    private bool startPressed = false;


    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
	
    void Start()
    {
        source.PlayOneShot(start);
        StartCoroutine(StartAlready());
    }


    IEnumerator StartAlready()
    {
        yield return new WaitForSeconds(10f);
        if (!startPressed)
        {
            source.PlayOneShot(startAlready);
            yield return new WaitForSeconds(30f);

            while (!startPressed)
            {
                source.PlayOneShot(startAlready);
                yield return new WaitForSeconds(30f);
            }
        }
    }


    public void PlayStartJingle()
    {
        startPressed = true;
        Animator anim = GetComponent<Animator>();
        source.Stop();
        source.PlayOneShot(startJingle);
        anim.Stop();
    }

}
