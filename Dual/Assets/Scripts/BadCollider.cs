using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCollider : MonoBehaviour {

    private bool working = true;
    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null && working)
        {
            //Debug.Log(other.name);
            player.World.SwitchWorld();
            working = false;
            Invoke("ResetTime", 0.4f);
        }
    }

    private void ResetTime()
    {
        working = true;
    }

    //public void OnTriggerStay(Collider other)
    //{
       
    //}
}
