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

    public CinemachineVirtualCamera vCam;

    public void Setup()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        if (vCam)
            CameraManager.Instance.SetActiveCamera(vCam);
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

    public void Say(string _msg)
    {
        Debug.Log(name + " " + _msg);
    }
}