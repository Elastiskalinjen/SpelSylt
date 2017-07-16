using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour, IFPSListener {

    [SerializeField]
    private AudioClip DeathSound;

    private Text InteractText;

    public WorldManager World {get; private set; }

    public Camera Camera { get; set; }

    private CharacterController controller;

    private WorldManager.WorldType checkpointWorld;
    private Vector3 checkpointPosition;

    // Use this for initialization
    void Start () {
        World = FindObjectOfType<WorldManager>();
        Camera = GetComponentInChildren<Camera>();
        InteractText = GameObject.Find("InteractText").GetComponent<Text>();

        controller = GetComponent<CharacterController>();
        //Distance is slightly larger than the
        distance = controller.radius + 0.2f;

        checkpointPosition = transform.position;
        checkpointWorld = WorldManager.WorldType.Light;

        StartCoroutine(FadeLevelText(5, 1, GameObject.Find("LevelName").GetComponent<Text>()));
        StartCoroutine(FadeLevelText(4.4f, 1, GameObject.Find("WelcomeText").GetComponent<Text>()));
    }

    float distance;
    private void UpdateSweep()
    {
        RaycastHit hit;

        //Bottom of controller. Slightly above ground so it doesn't bump into slanted platforms. (Adjust to your needs)
        Vector3 p1 = transform.position + Vector3.up * 0.25f;
        //Top of controller
        Vector3 p2 = p1 + Vector3.up * controller.height;

        //Check around the character in a 360, 10 times (increase if more accuracy is needed)
        for (int i = 0; i < 360; i += 36)
        {
            //Check if anything with the platform layer touches this object
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, distance))
            {
                //If the object is touched by a platform, move the object away from it
                if (!hit.collider.isTrigger)
                    controller.Move(hit.normal * (distance - hit.distance));
            }
        }

        //[Optional] Check the players feet and push them up if something clips through their feet.
        //(Useful for vertical moving platforms)
        //if (Physics.Raycast(transform.position - Vector3.up, -Vector3.up, out hit, 0.1f))
        //{
        //    Debug.Log(hit.collider.name);
        //   controller.Move((Vector3.up * (1 - hit.distance)) * Time.deltaTime);
        //}
    }

    // Update is called once per frame
    void Update () {
        CheckInteract();

        if (CrossPlatformInputManager.GetButtonUp("Respawn"))
        {
            Respawn();
        }
	}

    void FixedUpdate()
    {
        UpdateSweep();
    }

    public void Respawn()
    {
        GetComponent<FirstPersonCustomController>().m_MoveDir.y = 0;
        transform.position = checkpointPosition;
        World.SwitchWorld(checkpointWorld);
        AudioSource.PlayClipAtPoint ( DeathSound, transform.position);
        ///GetComponent<AudioSource>().Play();
    }

    public void Checkpoint(Checkpoint p)
    {
        checkpointPosition = p.transform.position;
        checkpointPosition.y += 2;
        checkpointWorld = World.CurrentWorldType;
    }

    private void CheckInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward,  out hit, 3.5f))
        {
            if (hit.collider.tag == "RestoreObject" && World.CurrentWorldType == WorldManager.WorldType.Dark)
            {
                if (CrossPlatformInputManager.GetButtonDown("Interact"))
                {
                    hit.collider.transform.parent = World.LightWorld.transform;
                    World.AddRestoreObject(hit.collider.gameObject);

                    var extra = GameObject.Find("ExtraRestore");
                    foreach (Transform child in extra.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                else
                {
                    InteractText.enabled = true;
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
        if (World.enabled)
            World.SwitchWorld();
    }


    public IEnumerator FadeLevelText(float delay, float t, Text i)
    {
        yield return new WaitForSeconds(delay);

        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        i.enabled = false;
    }
}
