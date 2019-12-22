using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    #region Serialized Variables
        [SerializeField] private float jumpHeight, gravity;
        [SerializeField] private GameObject jumpEffect, startPrompt;
        [SerializeField] private GameManager theGameManager;
        [SerializeField] private ShopManager theShopManager;
    #endregion
    
    #region Private Variables
        private Rigidbody2D myRigidBody;
        private int currentSprite;
        private StairManager theStairManager;
        private float jumpVelocity;
        private bool isDragging = false, isPlaying = false;
        private Vector2 touchPosition, playerPosition, dragPosition;
    #endregion
    
    //Gets executed as soon as the game obejct is initialized
    private void Awake()
    {
        currentSprite = PlayerPrefs.GetInt("Sprite", 0);
        Debug.Log("The Current Sprite Index is:" + currentSprite);
        gameObject.GetComponent<SpriteRenderer>().sprite = theShopManager.sprites[currentSprite];
        gameObject.SetActive(true);
        myRigidBody = GetComponent<Rigidbody2D>();
        theStairManager = GameObject.FindObjectOfType<StairManager>();
        startPrompt.SetActive(true);
        isPlaying = false;
    }

    //Gets executed before the very first frame
    private void Start()
    {
    }

    //Gets executed once per frame
    private void Update()
    {
        if(CheckIfPlaying()){
            AddGravityToPlayer();
            GetInput();
            MovePlayer();
            DeadCheck();
        }
    }

    /// <summary>
    /// Deals with the tap to play mechanic of the game.
    /// </summary>
    /// <returns>Returns whether the game is playing or not</returns>
    private bool CheckIfPlaying(){
        //if game not running yet
        if(!isPlaying){
            //check for input
            if(Input.GetMouseButtonDown(0)){
                //if there is input, play the game
                Time.timeScale = 1f;
                //hide the prompt
                startPrompt.SetActive(false);
                isPlaying = true;
            }
            //if there is no input...
            else{
                Time.timeScale = 0f;
                isPlaying = false;
            }
        }
        return isPlaying;
    }
    ///<summary>
    ///Controls all of the inputs from the player
    ///</summary>
    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            playerPosition = transform.position;
            isPlaying = true;

        }else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    ///<summary>
    ///A function that controls the movement of the player
    ///</summary>
    private void MovePlayer()
    {
        if (isDragging)
        {
            dragPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            transform.position = new Vector2(playerPosition.x + (dragPosition.x - touchPosition.x), transform.position.y);

            if(transform.position.x <= -5) //If the ball is on the left wall
            {
                transform.position = new Vector2(-4.7f, transform.position.y);
            }
            else if(transform.position.x >= 5) //If the ball is on the right wall
            {
                transform.position = new Vector2(4.7f, transform.position.y);
            }
        }
    }

    ///<summary>
    ///A function that controls the jumping of the ball
    ///<para>Gets executed when the player hits the platform</para>
    ///</summary>
    private void Jump()
    {
        jumpVelocity = gravity * jumpHeight;
        theGameManager.AddScore(1);
        myRigidBody.velocity = new Vector2(0, jumpVelocity);
        //play the jump animation
        Destroy(Instantiate(jumpEffect, transform.position, Quaternion.identity), 1f);
        gravity += 0.01f;
        ChooseSoundEffect();
    }

    private void ChooseSoundEffect()
    {
        int random = Random.Range(0, 2);
        SoundManager.Instance.PlaySFX(random == 0 ? "Jump_1" : "Jump_2");
    }

    private void MakeDestroyStair(Collider2D collision)
    {
        Destroy(collision.gameObject);
        theStairManager.MakeNewStair();
    }

    private void DeadCheck()
    {
        if(transform.position.y < Camera.main.transform.position.y - 25)
        {
            SoundManager.Instance.PlaySFX("Death");
            theGameManager.IsDead = true;
            gameObject.SetActive(false);
        }
    }


    private void AddGravityToPlayer()
    {
        myRigidBody.velocity = new Vector2(0, myRigidBody.velocity.y - (gravity * gravity));
    }

    private void ChangeBackgroundColour(Collider2D stair)
    {
        if (stair.gameObject.name != "Start")
        {
            Color backgroundColour = stair.gameObject.GetComponent<SpriteRenderer>().color;
            float h, s, v;
            Color.RGBToHSV(backgroundColour, out h, out s, out v);
            s -= 0.3f;
            if(s == 0)
            {
                s = 0.1f;
            }
            backgroundColour = Color.HSVToRGB(h, s, v);
            Camera.main.backgroundColor = backgroundColour;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "stair" && collision.name != "start")
        {
            if(myRigidBody.velocity.y <= 0)
            {
                Jump();
                ChangeBackgroundColour(collision);
                MakeDestroyStair(collision);
            }
        }
        if (collision.gameObject.tag == "gem")
        {
            SoundManager.Instance.PlaySFX("Gem");
            theGameManager.Gems += 1;
            theGameManager.AddScore(3);
            Destroy(collision.gameObject);
        }
    }
}
