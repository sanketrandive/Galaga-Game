/*
 * This play role of energy beam fire from green plane.
 *
 */
using System;
using UnityEngine;


public class EnergyBeamFunction : MonoBehaviour {

    PlayerManager PlayerClassObj;           //Player classobject
    GameObject player;
    GameManager gameManagerObj;             //Gamemanager class object.
    float timer = 0;                        //timer to transform player object into the beam
    bool tempFLag;                          //To check is player in beam or not.
    GameObject tempObject;                  //store player object into temp object. 
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerClassObj = player.GetComponent<PlayerManager>();
        gameManagerObj = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        //This check is game start yet ot not.
        if (gameManagerObj.startGame)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            PlayerClassObj = player.GetComponent<PlayerManager>();
            gameManagerObj.currentPlayerDead = false;
        }
        //Check for player transfrom postion to green plane object postion with set value.
        if (tempFLag)
        {
            try
            {
                Vector2 pos = new Vector2(transform.position.x, transform.position.y + 1f);
                tempObject.transform.position = Vector2.Lerp(tempObject.transform.position, pos, timer * 3);
                tempObject.transform.parent = this.transform;
                PlayerClassObj.playerControl = false;
            }
            catch(Exception e)
            {

            }
        }

    }

    //Here check is collided object tag match with Player tag if yes then grab that.
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerCollisionFunction(other.gameObject);
            timer += Time.deltaTime;
            tempFLag = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        tempFLag = false;
    }

    //this false the player collider to avoid other plane collision.
    void PlayerCollisionFunction(GameObject playerObj)
    {
        
        tempObject = playerObj;
        playerObj.GetComponent<Collider2D>().enabled = false;
        //Destroy(playerObj, 6);
    }
}
