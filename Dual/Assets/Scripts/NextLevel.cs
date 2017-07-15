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
        var curr = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(curr + 1);
    }

}
