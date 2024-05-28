using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorbellChat : MonoBehaviour
{
    public static DoorbellChat instance;
    public SoDoorBellChat stats;
    private float _timeChatting;
    private bool _endChat=false;
    private AudioSource _audioChat;
    private int _chatIndex;
    #region DoorObject
    private XRGrabInteractable _grabInteractable;
    private Rigidbody _doorRig;
    #endregion
    #region GetStats
    private List<AudioClip> _chatList = new List<AudioClip>();
    private Image _doorBellSoudEffect;
    #endregion
    void Start()
    {
        instance = this;
        _audioChat = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        
        GetReferences();
        GetDoorReferences();
    }
    private void GetDoorReferences()
    {
        _grabInteractable = GameObject.FindGameObjectWithTag("Door").GetComponent<XRGrabInteractable>();
        _grabInteractable.enabled = false;
        _doorRig = GameObject.FindGameObjectWithTag("Door").GetComponent<Rigidbody>();
        _doorRig.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void GetReferences()
    {
        _chatList = stats.AudioList;
        _doorBellSoudEffect = stats.ImageSoundAnimation;
    }
    private  IEnumerator ChattingSequence()
    {
        _audioChat.clip = _chatList[_chatIndex];
        _audioChat.Play();
        _timeChatting = _audioChat.clip.length +1;
        _chatIndex++;
        yield return new WaitForSeconds(_timeChatting/20);
        _audioChat.clip = _chatList[_chatIndex];
        _audioChat.Play();
        _timeChatting = _audioChat.clip.length + 1;
        _chatIndex++;
        yield return new WaitForSeconds(_timeChatting);
        _audioChat.clip = _chatList[_chatIndex];
        _audioChat.Play();
        _timeChatting = _audioChat.clip.length + 1;
        _chatIndex++;
        _doorRig.constraints = RigidbodyConstraints.None;
        _grabInteractable.enabled = true;
        _endChat = true;
    }
    public void StartChat()
    {
        StartCoroutine(ChattingSequence());
    }
}
