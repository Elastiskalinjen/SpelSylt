using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour {

    public enum WorldType
    {
        Light, Dark, Shared
    }

    public GameObject LightWorld;

    public GameObject DarkWorld;

    [SerializeField]
    private int TotalRestores = 1;

    private int NumberOfRestores;
    private Text RestoreSuccessText;

    [SerializeField]
    private GameObject SharedWorld;

    [SerializeField]
    private PostProcessingProfile LightEffect;

    [SerializeField]
    private PostProcessingProfile DarkEffect;

    [SerializeField]
    private Material LightSky;

    [SerializeField]
    private Material DarkSky;


    [SerializeField]
    private AudioClip WarpSound;

    public WorldType CurrentWorldType {get; private set;}

    private PostProcessingBehaviour Effects;

    private Player Player;

    // Use this for initialization
    void Start () {
        Effects = FindObjectOfType<PostProcessingBehaviour>();
        Player = FindObjectOfType<Player>();
        _audio = GetComponent<AudioSource>();

        RestoreSuccessText = GameObject.Find("RestoreSuccess").GetComponent<Text>();
        RestoreSuccessText.enabled = false;
        OnNewLevel();
	}

    private void OnNewLevel()
    {
        LightWorld.SetActive(true);
        DarkWorld.SetActive(false);
        SharedWorld.SetActive(true);
        CurrentWorldType = WorldType.Light;
        RenderSettings.skybox = LightSky;
    }

    public void SwitchWorld(WorldType type)
    {
        StopAllCoroutines();
        StartCoroutine(SwapAsync(type));
    }

    public void SwitchWorld()
    {
        SwitchWorld(CurrentWorldType == WorldType.Dark ? WorldType.Light : WorldType.Dark);
        //SwitchWorld();
    }

    private void SetNewWorld(WorldType type)
    {
      // var type = CurrentWorldType == WorldType.Dark ? WorldType.Light : WorldType.Dark;

        if (type == WorldType.Light)
        {
            LightWorld.SetActive(true);
            DarkWorld.SetActive(false);
            Effects.profile = LightEffect;
            RenderSettings.skybox = LightSky;
            RenderSettings.fogDensity = 0;
           CurrentWorldType = WorldType.Light;
        }
        else if (type == WorldType.Dark)
        {
            LightWorld.SetActive(false);
            DarkWorld.SetActive(true);
            Effects.profile = DarkEffect;
            RenderSettings.skybox = DarkSky;
            RenderSettings.fogDensity = 0.01f;
           CurrentWorldType = WorldType.Dark;
        }

        //if (Physics.CheckCapsule(transform.position, transform.position + transform.up * 2, 1.2f, -1))
        //{
        //    SwitchWorld();
        //}
    }
	
	// Update is called once per frame
	//void Update () {
		
	//}


    //[SerializeField]
    //private Vingette _vingette;
    [SerializeField]
    private AnimationCurve _innerVingette;
    [SerializeField]
    private AnimationCurve _outerVingette;
    [SerializeField]
    private AnimationCurve _saturation;

    [SerializeField]
    private AnimationCurve _fov;
    [SerializeField]
    private AnimationCurve _timeScale;

    public void AddRestoreObject(GameObject obj)
    {
        NumberOfRestores++;
        RestoreSuccessText.text = "YOU RESTORED THE " + obj.name.ToUpper() + "!";
        RestoreSuccessText.enabled = true;
        Invoke("HideRestoreSuccess", 4);
    }

    private void HideRestoreSuccess()
    {
        RestoreSuccessText.enabled = false;
    }

    // [SerializeField]
    // private Transform _itemTransform;
    // [SerializeField]
    // private AnimationCurve _itemPosition;

    private bool _swapTiggered;
    private readonly float _swapTime = 0.45f;
    private AudioSource _audio;

    public static bool Swapping
    {
        get; private set;
    }

    /// <summary>
	/// Controls a bunch of stuff like vingette and FoV over time and calls the swap cameras function after a fixed duration.
	/// </summary>
	IEnumerator SwapAsync(WorldType type)
    {
        Swapping = true;
        _swapTiggered = false;

       // CurrentWorldType = type;

       // _audio.PlayOneShot(_audio.clip);
       _audio.PlayOneShot(WarpSound);

        for (float t = 0; t < 1.0f; t += Time.unscaledDeltaTime * 1.8f)
        {
                Player.Camera.fieldOfView = _fov.Evaluate(t);
            //_vingette.MinRadius = _innerVingette.Evaluate(t);
            ///_vingette.MaxRadius = _outerVingette.Evaluate(t);
            //_vingette.Saturation = _saturation.Evaluate(t);
            Time.timeScale = _timeScale.Evaluate(t);

           // _itemTransform.localPosition = new Vector3(-0.5f, -0.5f, _itemPosition.Evaluate(t));

            if (t > _swapTime && !_swapTiggered)
            {
                _swapTiggered = true;
                // _twinCameras.SwapCameras();
                SetNewWorld(type);
            }

            yield return null;
        }

        // technically a huge lag spike could cause this to be missed in the coroutine so double check it here.
        if (!_swapTiggered)
        {
            _swapTiggered = true;
            SetNewWorld(type);
            //_twinCameras.SwapCameras();
        }

           Player.Camera.fieldOfView = _fov.Evaluate(1.0f);

        //_vingette.MinRadius = _innerVingette.Evaluate(1.0f);
        //_vingette.MaxRadius = _outerVingette.Evaluate(1.0f);
       // _vingette.Saturation = 1.0f;
      //  _itemTransform.localPosition = new Vector3(-0.5f, -0.5f, 0.5f);

        Time.timeScale = 1.0f;

        Swapping = false;
    }
}
