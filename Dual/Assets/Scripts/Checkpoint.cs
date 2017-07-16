using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

  
    public void OnTriggerEnter( Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
            player.Checkpoint(this);
    }
}
