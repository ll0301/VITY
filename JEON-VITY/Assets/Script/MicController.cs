using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MicController : MonoBehaviour
{
    public GameObject onButton;
    public GameObject offButton;
    public bool _useMicrophone;
    public string microphoneName;
    AudioClip clip;
    AudioSource audioSource;
    float[] samplesBuffer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log(device);
            microphoneName = device;
        }
        
    }

    public void OnMic()
    {
        onButton.SetActive(false);
        offButton.SetActive(true);
        _useMicrophone = true;
        clip = Microphone.Start(microphoneName, true, 1000, AudioSettings.outputSampleRate);
        samplesBuffer = new float[clip.channels];
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void OffMic()
    {
        onButton.SetActive(true);
        offButton.SetActive(false);
        _useMicrophone = false;
        audioSource.Stop();
        Microphone.End(microphoneName);
        audioSource.Stop();
    }



}
