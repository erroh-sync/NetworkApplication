using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Singleton Setup
    public static CameraController cam;
    #endregion

    #region Speed Settings
    [Header("Speed Settings")]
    [SerializeField]
    [Tooltip("How fast will the camera move towards the player.")]
    private float SpeedToPlayer = 10.0f;
    #endregion

    #region External References
    private GameObject localPlayer;
    #endregion

    // Use this for initialization
    void Start () {
        cam = this;
        localPlayer = FindObjectOfType<PlayerController>().gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        //TODO: Should check if we're focusing on something else in case...we are
        transform.position = Vector3.Lerp(this.transform.position, localPlayer.transform.position, SpeedToPlayer * Time.deltaTime);
	}
}
