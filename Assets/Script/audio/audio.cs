using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class audio : MonoBehaviour
{
    public AudioSource lagu;

    public void stoplagu()
    {
        lagu.Stop();

    }

    public void playlagu()
    {
        lagu.Play();
    }
 
}
