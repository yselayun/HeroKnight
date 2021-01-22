using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform respawmPoint;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;

    private CinemachineVirtualCamera cvc;

    private void Start()
    {
        cvc = GameObject.Find("Main Camera").GetComponent<CinemachineVirtualCamera>();
    }
    private void Update()
    {
        checkRespawn();
    }
    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void checkRespawn()
    {
        if(Time.time >= respawnTimeStart - respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawmPoint);
            cvc.m_Follow = playerTemp.transform;
            respawn = true;
        }
    }
}
