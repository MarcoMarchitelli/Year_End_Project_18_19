using UnityEngine;
using Cinemachine;

public class Room : MonoBehaviour
{
    [SerializeField] bool CloseOnExit;
    public Transform SpawnPoint;
    public bool Unclosable;
    [SerializeField] UnityVoidEvent OnDiscover;
    [SerializeField] UnityVoidEvent OnClose;

    [HideInInspector]
    public bool Open = true;

    CinemachineVirtualCamera vCam;

    public void Setup()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        if (vCam)
            ToggleVCam(false);
        else
        {
            Debug.LogWarning(name + " cannot find a virtual camera!");
        }
    }

    public void Discover()
    {
        OnDiscover.Invoke();
    }

    public void Close()
    {
        Open = false;
        OnClose.Invoke();
    }

    public void ToggleVCam(bool _value)
    {
        vCam.enabled = _value;
    }

    public void Say(string _msg)
    {
        Debug.Log(name + " " + _msg);
    }
}