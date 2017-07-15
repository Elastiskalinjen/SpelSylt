using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour, IFPSListener {

    private Text InteractText; 

    private WorldManager World;

    public Camera Camera { get; set; }

    // Use this for initialization
    void Start () {
        World = FindObjectOfType<WorldManager>();
        Camera = GetComponentInChildren<Camera>();
        InteractText = GameObject.Find("InteractText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        CheckInteract();
	}

    private void CheckInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward,  out hit, 2.2f))
        {
            if (hit.collider.tag == "RestoreObject" && World.CurrentWorldType == WorldManager.WorldType.Dark)
            {
                if (CrossPlatformInputManager.GetButtonDown("Interact"))
                {
                    hit.collider.transform.parent = World.LightWorld.transform;
                }
                else
                {
                    InteractText.enabled = true;
                    //GUI.Label(new Rect(10, 10, 64, 64), "Press E to interact");
                }
            }
        }
        else
        {
            InteractText.enabled = false;
        }
    }

    public void OnJump()
    {
        World.SwitchWorld();
    }
}
