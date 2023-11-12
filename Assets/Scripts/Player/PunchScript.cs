using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Avatar = Alteruna.Avatar;

public class PunchScript : AttributesSync
{
    [Header("Setup")]
    public float range = 1.5f;
    public float KnockbackForce = 1;
    private Camera playerCamera;

    private void Awake()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootRaycast();
        }
    }

    private void ShootRaycast()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            if (hit.collider.tag != "Player") return;
            BroadcastRemoteMethod("Knockback", transform.forward);
        }
    }

    [SynchronizableMethod]
    private void Knockback(Vector3 forward)
    {
        transform.position -= forward * KnockbackForce;
    }
}
