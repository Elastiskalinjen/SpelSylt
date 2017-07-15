using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour, IFPSListener {

    private WorldManager World;

    public Camera Camera { get; set; }

    // Use this for initialization
    void Start () {
        World = FindObjectOfType<WorldManager>();
        Camera = GetComponentInChildren<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnJump()
    {
        World.SwitchWorld();
    }
}
