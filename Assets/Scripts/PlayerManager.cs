/*
 * Player controller with condition of dead or alive
 * script attached to player prefab.
 */
 
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public float playerSpeed;               //This is confirgable.
    public Rigidbody2D bullet;              //this initiallize the bullet object and fire 
    public GameObject bulletSpwnObject;     //Bullet spawn point of the player
    private int bulletSpeed = 500;          //This is speed of bullet 
    public bool playerControl = false, playerDead;  //This is public flag to check is player dead or alive and has control or not.

	// Use this for initialization
	void Start () {
        playerControl = true;
        playerDead = false;
    }
	
	// Update is called once per frame
	void Update () {
        //Check for player controller enabled or not if yes then perform the controller else make dead condition.
        if (playerControl)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");                             //within horizontal control
            transform.Translate(moveHorizontal * Time.deltaTime * playerSpeed, 0, 0);       //Player movement with config speed.
            Vector2 movementClamp = transform.position;
            movementClamp.x = Mathf.Clamp(transform.position.x, -4.3f, 4.3f);           //Make a region within player are.
            transform.position = movementClamp;                                         //tranform within clamp area.

            //This is basic cotroller when by pressing spacebar bullets fire in up direction.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rigidbody2D bulletObject = Instantiate(bullet, bulletSpwnObject.transform.position, bulletSpwnObject.transform.localRotation);
                bulletObject.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Force);
            }
        }
        else
        {
            playerDead = true;
        }
	}
}
