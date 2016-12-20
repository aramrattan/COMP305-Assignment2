/*  
 * File Name: GameController
 * Author Name: Omid Khataee
 * Last Modified Date: 12/18/2016
 * Last Modified By: Arlina Ramrattan
 * Cavemans Quest
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    // PRIVATE INSTANCE VARIABLES
	private int _livesValue;
	private int _scoreValue;


    // PUBLIC INSTANCE VARIABLES (TESTING)
    public AudioSource DeathSound;

    [Header("UI Objects")]
    public Text LivesLabel;
    public Text ScoreLabel;
 

    // PUBLIC PROPERTIES
    public int LivesValue {
        get {
            return this._livesValue;
        }

        set {
            this._livesValue = value;
            if(this._livesValue == 0) {
                this.DeathSound.Play();
				endGame ();
            }
            else {
                this.LivesLabel.text = "Lives: " + this._livesValue;
            }
        }
    }

    public int ScoreValue {
        get {
            return this._scoreValue;
        }
        set {
            this._scoreValue = value;
            this.ScoreLabel.text = "Score: " + this._scoreValue;
        }
    }
	// Use this for initialization
	void Start () {

		this.LivesValue = 5;
		this.ScoreValue = 0;

	}
	
	// Update is called once per frame
	void Update () {

        
    }
	//this method sers the value of score and then swtiches scenes
	void endGame () {
		PlayerPrefs.SetInt("Score", this._scoreValue);
		SceneManager.LoadScene("GameOver");

	}

    
}
