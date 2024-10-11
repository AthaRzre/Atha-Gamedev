using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioDice3d : MonoBehaviour
{

    [SerializeField]
    public AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnMouseDown()
    {
        audioSource.Play();
    }

}
