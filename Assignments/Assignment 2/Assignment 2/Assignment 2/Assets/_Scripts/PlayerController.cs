/*  
 * File Name: PlayerController
 * Author Name: Omid Khataee/Arlina Ramrattan
 * Last Modified Date: 12/18/2016
 * Last Modified By: Arlina Ramrattan
 * Cavemans Quest
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    // PRIVATE INSTANCE VARIABLES
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private float _move;
    private float _jump;
    private bool _isFacingRight;
    private bool _isGrounded;
    private GameObject _gameControllerObject;
    

    // PUBLIC INSTANCE VARIABLES (FOR TESTING)
    public float Velocity;
    public float JumpForce;
    public Camera MainCamera;
    public GameObject background; 
    public Transform SpawnPoint;
	public GameController _gameController;


    [Header("Sound Clips")]
    public AudioSource JumpSound;
    public AudioSource HitSound;
    public AudioSource EnemyDeath;
    public AudioSource FoodSound;

    // Use this for initialization
    void Start () {
        this._initialize();          
	}

    // Update is called once per frame
    void FixedUpdate() {
        // Check if Input is one of Left/Right or A/D and put the value in _move
        // If _move > 0f player will go right
            this._move = Input.GetAxis("Horizontal");
            if (this._move > 0f) {
                this._move = 1;
                this._isFacingRight = true;
                this._flip();
            }
            // If _move < 0f player will go Left
            else if (this._move < 0f) {
                this._move = -1;
                this._isFacingRight = false;
                this._flip();
            }
            else {
                this._move = 0f;
            }

            // If Space is pressed, the jump sound is played and _jump = 1
            if (Input.GetKeyDown(KeyCode.Space)) {
                if(this._isGrounded == true) {
                    this.JumpSound.Play();
                    this._jump = 1;
                }
            }

            // If the player is NOT on a platform, _jump = 0 regardless of spacebar being pressed
            if (this._isGrounded == false) {
                this._jump = 0f;
            }

        // This is the force that moves the player
        this._rigidbody.AddForce(new Vector2(
                this._move * this.Velocity,
                this._jump * this.JumpForce),
                ForceMode2D.Force);
        
        // Ensures that the Main Camera and Background follow the player
        this.background.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1f, 0f);
        this.MainCamera.transform.position = new Vector3(this._transform.position.x, this._transform.position.y + 1f, -10f); 
	}

    // PRIVATE METHODS
    /*
     * This method initialized variables and objects when called
     */
    private void _initialize() {
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._move = 0f;
        this._isFacingRight = true;
        this._isGrounded = false;

        this._gameControllerObject = GameObject.Find("Game Controller");
        //this._gameController = this._gameControllerObject.GetComponent<GameController>() as GameController;
 
    }
    /*
    * This method will flip the character Left or Right depending on the value of _isFacingRight
    */
    private void _flip() {
        if (this._isFacingRight) {
            this._transform.localScale = new Vector2(-0.13f, 0.13f);
        }
        else {
            this._transform.localScale = new Vector2(0.13f, 0.13f);
        }
    }

    /*
    * This method check whenever the player collides with the DeathPlane, Enemy, NextLevelPlane, or the Boss.
    */
    private void OnCollisionEnter2D(Collision2D other) {
        // Lives -1
        // Player will be moved back to the spawnpoint
        if (other.gameObject.CompareTag("DeathPlane")) {
            this.HitSound.Play();
            this._transform.position = this.SpawnPoint.position;
            this._gameController.LivesValue -= 1;
        }

        // Lives -1
        if (other.gameObject.CompareTag("Enemy")) {          
            this._gameController.LivesValue -= 1;
            this.HitSound.Play();
        }
        
        // Player gets transitioned to Level 2
		if (other.gameObject.CompareTag ("NextLevelPlane")) {
			SceneManager.LoadScene("Level2");
			PlayerPrefs.SetInt ("Player Score", this._gameController.LivesValue);
			PlayerPrefs.SetInt ("Player Lives", this._gameController.ScoreValue);
			PlayerPrefs.SetInt ("Player Food", this._gameController.FoodValue);

		}

        // Lives -1
        if (other.gameObject.CompareTag("Boss")) {
            this._gameController.LivesValue -= 1;
        }
    }

    /*
    * This method check whenever the player collides with a the TRIGGER of a Food, Boss, Enemy, Jump, Speed, and Life Powerups.
    */
    private void OnTriggerEnter2D(Collider2D other) {

        // Score +100, Food +1
        if (other.gameObject.CompareTag("Scroll")) {
            Destroy(other.gameObject);
            this.FoodSound.Play();
            this._gameController.ScoreValue += 100;
        }
        


        // Enemy Dies
        if (other.gameObject.CompareTag("Enemy")) {
            this.EnemyDeath.Play();
            Destroy(other.gameObject);
        }

        // JumpForce + 3
        if (other.gameObject.CompareTag("Jump")) {
            this.JumpForce += 3;
            Destroy(other.gameObject);
        }

        // Velocity + .3f
        if (other.gameObject.CompareTag("Speed")) {
            this.Velocity += .3f;
            Destroy(other.gameObject);
        }

        // Life + 5
        if (other.gameObject.CompareTag("Life")) {
            this._gameController.LivesValue += 5;
            Destroy(other.gameObject);
        }
    }

    /*
    * This method check whenever the player is touching another collider for an extended period of time
    */
    private void OnCollisionStay2D(Collision2D other) {
        // While the player is on a sprite that is a Tagged as a Platform, _isGrounded will remain true
        if (other.gameObject.CompareTag("Platform")) {
            this._isGrounded = true;
        }
    }

    /*
    * This method check whenever the player stops colliding with a collider
    */

    private void OnCollisionExit2D(Collision2D other) {
        this._isGrounded = false;
    }
}
