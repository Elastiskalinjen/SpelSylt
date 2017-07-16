using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void PlayerCollide(Player hit = null)
    {
        var fader = FindObjectOfType<CrossFader>();
        if (fader != null)
            fader.CrossFade(hit.World.LightMusic, 1, 0.5f);

        var curr = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(curr + 1);
    }

}
