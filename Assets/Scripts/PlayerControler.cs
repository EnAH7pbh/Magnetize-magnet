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
    // Start is called before the first frame update
    void Start () {
        //move the object
        rb2d = this.gameObject.GetComponent<Rigidbody2D> ();
        uIController = GameObject.Find("Canvas").GetComponent<UIControllerScript>();
    }

    // Update is called once per frame
    void Update () {
        rb2d.velocity = -transform.up * moveSpeed;

        if (Input.GetKey (KeyCode.Z) && !isPulled) {
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
        if (Input.GetKeyUp (KeyCode.Z)) {
            rb2d.angularVelocity = 0;
            isPulled = false;
        }
    }

    public void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.tag == "Wall") {
            this.gameObject.SetActive (false);
        }
    }

    public void OnTriggerEnter2D (Collider2D collider) {
        if (collider.gameObject.tag == "Goal") {
            uIController.endGame();
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
}