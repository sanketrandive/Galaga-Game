/*
 *This script has Blue plane working functionallity. This make instance of blue plane of group in a list and start transforming on their position.
 * In certain time this plane can attack on player plane.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemyManager : MonoBehaviour
{
    //Private
    GameObject player;
    bool stopInstanceBluePlane, bluePlaneTransform;             //flag to check condision in realtime.
    Vector2 curr = new Vector2(0, 6);                           //This value provide initial posion of the plane.
    Vector2 next = new Vector2(0, 0);                           //Value can help to transform plane to zero postion into the screen.
    float speedOfEnemy;                                         //Here enemy speed perform with deltaTime method. NOT EDITABLE
    float blueAttackTimer, blueAttackSpeed, bluePlaneDistance;  //This helps to get Attack speed, distance and attack timer variable NOT EDITABLE 
    int randomBluePlaneNumber;                                  //This generates random number between the list.
    int numberOfBluePlane = 14;                                 //This is EDITABLE but need to check postion for every change.

    //Public
    public List<GameObject> bluePlaneList = new List<GameObject>(); //This is list to store blue planes.
    public GameObject bluePlanePrefab; 
    public bool blueAlive = false;                              //Flag to check is all planes are Active in the screen or not.

    //Class Objects
    GameManager gameManagerObj;
    
    //Find Player object.
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManagerObj = FindObjectOfType<GameManager>();        
    }

    void Update() {
        //Here boolean check for is Game start yet.
        if (gameManagerObj.startGame )
        {
            player = GameObject.FindGameObjectWithTag("Player");            //This perform every frame to find Player object in scene.
            //This is main condition to get attack blue plane to Player.
            if (bluePlaneList.Count >= 0 && !gameManagerObj.currentPlayerDead)
            {
                BluePlaneAttack();                                          //Function inherited.
            }
            
            //This check How many of instance of objects required into the scene.
            if (!stopInstanceBluePlane)
            {
                StartCoroutine(SpwanBluePlane(numberOfBluePlane, curr));    //Plane Instantiate into the scene.             
            }
            //Plane get initial position which setted into the vector variable above..
            if (bluePlaneTransform)
            {
                speedOfEnemy += Time.deltaTime;                             //start timer to get smooth tranform speed.
                for (int i = 0; i < bluePlaneList.Count; i++)
                {
                    next = new Vector2(0, 0);                               //this define row and column of position.
                    next.x = Mathf.Clamp(transform.position.x + i, -4.3f, 4.3f);//This defines the area for the tranformation.
                    bluePlaneList[i].transform.position = Vector2.Lerp(curr, next, 1 * speedOfEnemy);   //The half of the planes get first row position.
                }
                for (int j = 7; j < bluePlaneList.Count; j++)
                {
                    next = new Vector2(0, 1);
                    next.x = Mathf.Clamp(transform.position.x + j - 7, -4.3f, 4.3f);
                    bluePlaneList[j].transform.position = Vector2.Lerp(curr, next, 1 * speedOfEnemy);   //Here, The half of the planes get second row position.
                }
                if (speedOfEnemy >= 1 && speedOfEnemy <= 1.2f)          //if within speed condition gets false to stop transformation of the plane.
                {
                    bluePlaneTransform = false;
                    speedOfEnemy = 0;
                }
            }
            //This ping pong to the parent object to move all set of plane with the parent. This can only perform in one axis.
            transform.position = Vector2.Lerp(new Vector2(-4f, 0), new Vector2(-2f, 0), Mathf.PingPong(Time.time * 1, 1.0f)); 
        }
    }

    //This function has Instantiation functionallity to set position. This has 2 overload method.
    IEnumerator SpwanBluePlane(int blueValue, Vector2 spwanPos) {
        //This stops when i equals to Blue value and instance specific objects.
        for (int i = 0; i < blueValue; i++)
        {
            GameObject goB = Instantiate(bluePlanePrefab, spwanPos, transform.localRotation) as GameObject; //Instantiation process.
            bluePlaneList.Add(goB);                                                                         //Add into the list which defined.
            goB.transform.parent = gameObject.transform;                                                    //this moved to parent object into the hierarchy
            //Condision to stop instantiation process.
            if (i <= blueValue)
                stopInstanceBluePlane = true;
            else
                stopInstanceBluePlane = false;
        }
        //This enumerator start tranformation of the object after 2 seconds.
        yield return new WaitForSeconds(2);
        bluePlaneTransform = true;
        blueAlive = true;                                                                                   //This help to check is all type of plane active.
    }

    //Attck functionallity to Player with certain delay. 
    void BluePlaneAttack()
    {        
        blueAttackTimer += Time.deltaTime;                                          //Timer to attack in certain time.
        if (blueAttackTimer >= 6 && blueAttackTimer <= 9f)                          //Within every 6-9 time this enemy attack to player. EDITABLE. 
        {
            try             //Because of List of planes sometime gets remove or empty so this TRY n CATCH stop throwing exceptions.
            {
                blueAttackSpeed += Time.deltaTime / 50;                             //Speed of plane while attacking
                bluePlaneDistance = Vector2.Distance(bluePlaneList[randomBluePlaneNumber].transform.position, player.transform.position);   //Distance get between player and current blue enemy.
                bluePlaneList[randomBluePlaneNumber].transform.position = Vector2.Lerp(bluePlaneList[randomBluePlaneNumber].transform.position, player.transform.position, blueAttackSpeed);
                //Here start attacking to player position using Lerp method.
                //Whenever Distance lessthan 3 this stops attacking and following to player postion and add self gravity.
                if (bluePlaneDistance < 3)
                {                    
                    bluePlaneList[randomBluePlaneNumber].transform.parent = null;
                    bluePlaneList[randomBluePlaneNumber].GetComponent<Rigidbody2D>().gravityScale = 1.5f;
                    bluePlaneList.Remove(bluePlaneList[randomBluePlaneNumber]);             //This remove current gameobject from the list.
                    blueAttackSpeed = 0;
                    blueAttackTimer = 0;
                    randomBluePlaneNumber = UnityEngine.Random.Range(0, bluePlaneList.Count); //This generate next plane number into the list.                         
                }
            }
            catch (Exception e)
            {
                //print(e);
            }
        }
        //This condition help to check if timer reset hasnt worked then here by default set it to 0.
        if ( blueAttackTimer>12)
        {
            randomBluePlaneNumber = 0;            //Plane Number within list = 0
            blueAttackTimer = 0;
        }
        
    }

   
}
