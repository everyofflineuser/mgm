using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Avatar = Alteruna.Avatar;

public class PunchScript : AttributesSync
{
    private Avatar _avatar;
    [Header("Setup")]
    public float range = 3f;
    public float KnockbackForce = 500;
    private Camera playerCamera;

    private void Awake()
    {
        _avatar = GetComponent<Avatar>();
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
            BroadcastRemoteMethod("Knockback");
        }
    }

    [SynchronizableMethod]
    private void Knockback()
    {
        transform.position -= transform.forward * Time.deltaTime * KnockbackForce;
    }
}
