using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * PlayerController.cs
 * Takes input from the local player.
 * Controls the local player's character.
 * Sends data to remote clients about this character, and is read by that Client's "RemotePlayer.cs"
 */

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;

    #region Input Variables
    private Vector2 ControlVec;
    #endregion

    #region Movement Variables
    [Header("Movement Settings")]
    [SerializeField]
    [Tooltip("Controls how fast the character will move.")]
    private float MovementSpeed = 10.0f;
    [SerializeField]
    [Tooltip("Controls how fast the character will turn.")]
    private float TurningSpeed = 10.0f;
    #endregion

    #region External Reference Variables
    [Header("External References")]
    [Tooltip("Character Controller Reference")]
    public CharacterInterface Chara;
    #endregion

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Fixed Update is called once per frame for Physics stuff
    void FixedUpdate()
    {
        Control();
    }

    // Updates the position of the crab by taking input from the player
    void Control()
    {
        bool bMoving = false;

        // TODO: Get this infor from our Input Script
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            ControlVec.x = Input.GetAxis("Vertical") * -1.0f;
            ControlVec.y = Input.GetAxis("Horizontal");
            bMoving = true;
        }
        
        // Movement & Rotation
        if (bMoving)
        {
            Vector3 CamRot = CameraController.cam.transform.eulerAngles;
            CamRot.x = 0;
            CamRot.y += 180;
            CamRot.z = 0;
            if(Chara != null)
                Chara.transform.rotation = Quaternion.Lerp(Chara.transform.rotation, Quaternion.Euler(0, -90, 0) * Quaternion.Euler(CamRot) * Quaternion.Euler(new Vector3(0, Mathf.Atan2(ControlVec.x, ControlVec.y) * 180 / Mathf.PI, 0)), TurningSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + -(Vector3.Normalize(this.transform.forward * ControlVec.y + this.transform.right * ControlVec.x) * (MovementSpeed * Time.deltaTime)));
        }
    }
}