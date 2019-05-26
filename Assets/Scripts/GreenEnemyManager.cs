/*
 *This script has Green plane working functionallity. This make instance of Green plane of group in a list and start transforming on their position.
 * In certain time this plane can get near position of the player plane.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemyManager : MonoBehaviour
{
    //private
    GameObject player;                                      
    bool stopInstanceGreenPlane, greenPlaneTransform;       //flag to check condision in realtime.
    Vector2 curr = new Vector2(0, 6);                       //This value provide initial posion of the plane.
    Vector2 next = new Vector2(0, 0);                       //Value can help to transform plane to zero postion into the screen.
    float speedOfEnemy;                                     //Here enemy speed perform with deltaTime method. NOT EDITABLE
    float greenAttackTimer, greenAttackSpeed, greenPlaneDistance;//This helps to get Attack speed, distance and attack timer variable NOT EDITABLE 
    int randomGreenPlaneNumber;                             //This generates random number between the list.
    int numberOfGreenPlane = 5;                             //This is EDITABLE but need to check postion for every change.

    //public    
    public List<GameObject>  greenPlaneList = new List<GameObject>();//This is list to store green planes.
    public GameObject greenPlanePrefab;
    public Animator energyBeamAnimObj;                      //plane fire energy beam and here object can parse.

    //Class Objects
    GameManager gameManagerObj;

    //Looking for Player tag object into the scene.
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManagerObj = FindObjectOfType<GameManager>();
    }
    //Here boolean check for is Game start yet.
    void Update() {
        if (gameManagerObj.startGame )
        {
            player = GameObject.FindGameObjectWithTag("Player");                    //This perform every frame to find Player object in scene.
            //This is main condition to get attack green plane to Player.
            if (greenPlaneList.Count >= 0 && !gameManagerObj.currentPlayerDead)
            {
                GreenPlaneAttack();                                                 //Function inherited.
            }
            //This check How many of instance of objects required into the scene.
            if (!stopInstanceGreenPlane)
            {
                StartCoroutine(SpwanPlane(numberOfGreenPlane, curr));           //Plane Instantiate into the scene.
                        
            }
            //Plane get initial position which setted into the vector variable above..
            if (greenPlaneTransform)
            {
                speedOfEnemy += Time.deltaTime;                                      //start timer to get smooth tranform speed.
               
                for (int i = 0; i < greenPlaneList.Count; i++)
                {
                    next = new Vector2(0, 4);                                       //this define row and column of position.
                    next.x = Mathf.Clamp(transform.position.x + 1 + i, -4.3f, 4.3f);//This defines the area for the tranformation.
                    greenPlaneList[i].transform.position = Vector2.Lerp(curr, next, 1 * speedOfEnemy);//The planes get transform to position smoothly.
                }
                if (speedOfEnemy >= 1 && speedOfEnemy <= 1.2)                       //if within speed Condition gets falls to stop transformation of the plane.
                {
                    greenPlaneTransform = false;
                    speedOfEnemy = 0;
                }
            }
            
        }
    }

    //This function has Instantiation functionallity to set position. This has 2 overload method.
    IEnumerator SpwanPlane(int  greenValue, Vector2 spwanPos)
    {
        //This stops when i equals to Green value and instance specific objects.
        for (int i = 0; i < greenValue; i++)
        {
            GameObject goG = Instantiate(greenPlanePrefab, spwanPos, transform.localRotation) as GameObject;//Instantiation process.
            greenPlaneList.Add(goG);                                                                        //Add into the list which defined.
            goG.transform.parent = gameObject.transform;                                                    //this moved to parent object into the hierarchy
            //Condision to stop instantiation process.
            if (i <= greenValue)
                stopInstanceGreenPlane = true;
            else
                stopInstanceGreenPlane = false;
        }
        //This enumerator start tranformation of the object after 2 seconds.       
        yield return new WaitForSeconds(2);
        greenPlaneTransform = true;
    }

    //Attck functionallity to Player with certain delay. 
    void GreenPlaneAttack()
    {
        greenAttackTimer += Time.deltaTime;   
        if (greenAttackTimer >= 10.4f && greenAttackTimer <= 13f)                           //Within every 10-13 time this enemy attack to player. EDITABLE.
        {
            try         //Because of List of planes sometime gets remove or empty so this TRY n CATCH stop throwing exceptions.
            {
                greenAttackSpeed += Time.deltaTime / 50;                                    //Speed of plane while attacking
                greenPlaneDistance = Vector2.Distance(greenPlaneList[randomGreenPlaneNumber].transform.position, player.transform.position);    //Distance get between player and current green enemy.
                greenPlaneList[randomGreenPlaneNumber].transform.position = Vector2.Lerp(greenPlaneList[randomGreenPlaneNumber].transform.position, player.transform.position, greenAttackSpeed);
                //Funtion inherited when distance less than 4
                if (greenPlaneDistance < 4f)
                {
                    StartCoroutine(EneryBeamController());
                }
            }
            catch (Exception e)
            {
                //print(e);
            }
            
        }
        if (greenAttackTimer > 13)  //This condition help to check if timer reset hasnt worked then here by default set it to 0.
        {
            randomGreenPlaneNumber = 0;                         //Plane Number within list = 0
            greenAttackTimer = 0;
        }
    }

    //Function controls the Energy Beam object and animation.
    IEnumerator EneryBeamController()
    {
        greenPlaneList[randomGreenPlaneNumber].transform.parent = null;         //This remove the parenting betn green plane and parent object.
        //temp animator object instance into the scene and playes the state of animation controller.
        Animator gO = Instantiate(energyBeamAnimObj, greenPlaneList[randomGreenPlaneNumber].transform.position, greenPlaneList[randomGreenPlaneNumber].transform.localRotation);
        //gO.gameObject.transform.parent = greenPlaneList[randomGreenPlaneNumber].transform;
        gO.SetBool("playEnergyBeam", true);
        greenAttackSpeed = 0;        
        greenAttackTimer = 0;        
        yield return new WaitForSeconds(3);                                     //Stop to catch player into the beam.
        gO.SetBool("playEnergyBeam", false);                                    //Play reverse animation of beam.
        greenPlaneList[randomGreenPlaneNumber].GetComponent<Rigidbody2D>().gravityScale = 1.5f; // Start falling down using rigidbody feature.    
        greenPlaneList.Remove(greenPlaneList[randomGreenPlaneNumber]);          //This removes the object from the list to find next.
        randomGreenPlaneNumber = UnityEngine.Random.Range(0, greenPlaneList.Count); //This generate next plane number into the list.

    }
}
