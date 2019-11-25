using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {
    private Rigidbody2D rb2d;
    public float moveSpeed = 5f;
    public float pullForce = 100f;
    public float rotateSpeed = 360f;
    public GameObject closestTower;
    public GameObject hookedTower;
    private bool isPulled = false;
    private UIControllerScript uIController;
    private AudioSource myAudio;
    private bool isCrashed = false;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start () {
        //move the object
        startPosition = this.transform.position;
        rb2d = this.gameObject.GetComponent<Rigidbody2D> ();
        uIController = GameObject.Find ("Canvas").GetComponent<UIControllerScript> ();
        myAudio = this.gameObject.GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update () {
        rb2d.velocity = -transform.up * moveSpeed;

        if (Input.GetKey (KeyCode.Z) && !isPulled || Input.GetMouseButtonDown (0)) {
            if (closestTower != null && hookedTower == null) {
                hookedTower = closestTower;
            }
            if (hookedTower) {
                float distance = Vector2.Distance (transform.position, hookedTower.transform.position);

                //Gravitation towards object
                Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;
                float newPullForce = Mathf.Clamp (pullForce / distance, 20, 50);
                rb2d.AddForce (pullDirection * newPullForce);

                //Angular velocity
                rb2d.angularVelocity = -rotateSpeed / distance;

                isPulled = true;
            }
        }
        if (Input.GetKeyUp (KeyCode.Z) || Input.GetMouseButtonUp (0)) {
            rb2d.angularVelocity = 0;
            isPulled = false;
        }
        if (isCrashed) {
            if (!myAudio.isPlaying) {
                //restart scene
                restartPosition ();
            }
        }
    }

    public void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.tag == "Wall") {
            if (!isCrashed) {
                //play sfx
                myAudio.Play ();
                rb2d.velocity = new Vector3 (0f, 0f, 0f);
                rb2d.angularVelocity = 0f;
                isCrashed = true;
            }
        }
    }

    public void OnTriggerEnter2D (Collider2D collider) {
        if (collider.gameObject.tag == "Goal") {
            uIController.endGame ();
            this.gameObject.SetActive (false);
        }
    }

    public void OnTriggerStay2D (Collider2D collider) {
        if (collider.gameObject.tag == "Tower") {
            closestTower = collider.gameObject;

            collider.gameObject.GetComponent<SpriteRenderer> ().color = Color.green;
        }
    }

    public void OnTriggerExit2D (Collider2D collider) {
        if (isPulled) {
            return;
        }
        if (collider.gameObject.tag == "Tower") {
            closestTower = null;

            collider.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
        }
    }

    public void restartPosition () {
        //set to restart position
        this.transform.position = startPosition;

        //restart rotation
        this.transform.rotation = Quaternion.Euler (0f, 0f, 90f);

        //set isCrashed false
        isCrashed = false;

        if (closestTower) {
            closestTower.GetComponent<SpriteRenderer> ().color = Color.white;
            closestTower = null;
        }
    }
}