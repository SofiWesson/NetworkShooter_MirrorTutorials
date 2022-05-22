using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LaserBeam : NetworkBehaviour
{
    public LineRenderer lineRenderer;
    public float coolDown;
    public ParticleSystem fireFX;
    int index = 1;

    // Use this for initialization
    void Start()
    {
        // turn off the linerenderer
        ShowLaser(false);
        CharacterMovement cm = GetComponent<CharacterMovement>();
        if (cm)
            index = cm.index;
    }

    // Update is called once per frame
    void Update()
    {
        // count down, and hide the laser after half a second
        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            if (coolDown < 0.5f)
                ShowLaser(false);
        }

        //only check controls if we're the local player
        if (!isLocalPlayer)
            return;

        // activate the laser if the space bar has been pressed and we're off cooldown
        if (Input.GetButtonDown("Fire" + index) && coolDown <= 0)
           CmdFire();
    }

    [Command]
    void CmdFire()
    {
        RpcFire();
    }

    [ClientRpc]
    void RpcFire()
    {
        DoLaser();
    }

    void DoLaser()
    {
        // trigger the visuals - this should happen on all machines individually
        ShowLaser(true);
        coolDown = 1.0f;

        // more visual fx, a burst around the firing nozzle
        if (fireFX)
            fireFX.Play();

        // do a raycast against anything which may have a health on it
        if (!isServer)
            return;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + new Vector3(0, 1.5f, 0), transform.forward), out hit, 10.0f))
        {
            Health health = hit.transform.GetComponent<Health>() ? hit.transform.GetComponent<Health>() : hit.transform.GetComponentInParent<Health>();
            if (health)
            {
                // subtract health from the other player
                health.ApplyDamage(20);
            }
        }
    }

    void ShowLaser(bool show)
    {
        lineRenderer.enabled = show;
    }
}
