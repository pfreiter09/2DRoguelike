﻿//Mike Fortin, Basic Player Script (*Edited by Clinton Oka)
//All code in this file written by Mike Fortin thru 10/5, Edited on 10/22 by Clinton Oka
//Ranged attack will add ammo after each melee hit with limitations
//Clinton-> included player dead bool, player stregth float ( see Inventory for Strength Implenetation)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
//[RequireComponent(typeof(Animator))]
public class PlayerScript2 : MonoBehaviour
{

    public float playerSpeed = 3f; //Used to set the players speed when moving
    public float playerHP = 100; //Used to store adjust and update the players health
    public bool playerdead;
    public float maxHP = 100;
    public float parryTime = 2f;
    private string directionString = "";
    public float ammo = 2;
    private Rigidbody2D playerBody; //Used to find and adjust the body of the player
    private Animator animator; //Used to set animation triggers off depending on movement
    public bool canTakeDamage = true;
    public bool canParryAgain = true;
    private Vector2 currentPosition;
    private bool inFantasyWorld = true;
    public float strength;
    public float coins = 500;             //currency the player will use 

    // Used for initialization
    void Start()
    {
        playerBody = FindObjectOfType<Rigidbody2D>();
        playerBody.freezeRotation = true; //IMPORTANT! PLAYER TYPE MUST BE DYNAMIC WITH FREEZE ROTATION SELECTED
        animator = GetComponent<Animator>();
        canTakeDamage = true;
        directionString = "d";
        currentPosition.Set(transform.position.x, transform.position.y);
    }

    // This is how the player's movement and status will update 
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R) && canParryAgain == true) //Handles parrying, binds to R key for now
        {
            animator.SetTrigger("parry");
            canTakeDamage = false;
            canParryAgain = false;
            StartCoroutine(parryDelay());
            StartCoroutine(preventUnlimitedParry());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(inFantasyWorld == true)
            {
                transform.position = new Vector3(transform.position.x + 2000, transform.position.y, transform.position.z);
                inFantasyWorld = false;
            }
            else
            {
                transform.position = new Vector3(transform.position.x - 2000, transform.position.y, transform.position.z);
                inFantasyWorld = true;
            }
        }

        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        GetComponent<Rigidbody2D>().velocity = targetVelocity * playerSpeed; //moves the player

        //This series of ifs uses animation triggers according to directional movement
        //Priority animation given to left and right on diagonal movement
        if (targetVelocity.x < 0)
        {
            animator.SetTrigger("left");
            directionString = "l";
        }
        else if (targetVelocity.x > 0)
        {
            animator.SetTrigger("right");
            directionString = "r";
        }
        else if (targetVelocity.y > 0)
        {
            animator.SetTrigger("back");
            directionString = "u";
        }
        else if (targetVelocity.y < 0)
        {
            animator.SetTrigger("forward");
            directionString = "d";
        }

       
        //Kills the player if his health drops below 0
        if (playerHP == 0) {
            animator.SetTrigger("die");
            playerdead = true;  //used in playerScore as a means to add up score.

        }

        if (playerHP < 0)
        {
            playerdead = true;
            //SceneManager.LoadScene("ScoreScreen"); instead of changing screen will open up score board
        }
        currentPosition.Set(transform.position.x, transform.position.y);
    }
    IEnumerator parryDelay()
    {
        yield return new WaitForSeconds(parryTime); //limits the parrying to a short time
        canTakeDamage = true;
    }

    IEnumerator preventUnlimitedParry()
    {
        yield return new WaitForSeconds(3f); //Can be changed,
        canParryAgain = true;                  //prevents spamming of the parry button for invincibility
    }

    public string getDirectionString()
    {
        return directionString;
    }

    public bool getParrying()
    {
        return !canParryAgain;
    }
    public Vector2 getCurrentPosition()
    {
        return currentPosition;
    }
}