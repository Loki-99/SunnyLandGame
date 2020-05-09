using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManerger : MonoBehaviour
{
    public static MusicManerger instance; //生成一个静态类
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip jumpAudio, hurtAudio, cherryAudio;


    //初始化实例
    private void Awake()
    { 
        instance = this;
    }

    public void JumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void HurtAudio()
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }

    public void CherryAudio()
    {
        audioSource.clip = cherryAudio;
        audioSource.Play();
    }
}
