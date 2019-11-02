using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject tPlayer;
    public Transform tFollowTarget;
    public Transform tSpawnPoint;
    private CinemachineVirtualCamera vcam;
    [SerializeField] private CinemachineVirtualCamera vcam2;
    private float timeToSearch = 0f;
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (tPlayer == null)
        {
            vcam2.m_Priority = 11;
            tPlayer = GameObject.FindWithTag("Player");
            if (tPlayer != null)
            {
                tFollowTarget = tPlayer.transform;
                vcam.Follow = tFollowTarget;
                vcam2.m_Priority = 9;
            }
        }
    }
}
