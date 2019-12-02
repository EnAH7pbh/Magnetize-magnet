using UnityEngine;
using UnityEngine.UI;
public class PlayerControler : MonoBehaviour {
    private Rigidbody2D rb2d;
    private float moveSpeed = 5f;
    private float pullForce = 100f;
    private float rotateSpeed = 360f;
    private GameObject closestTower;
    private GameObject hookedTower;
    private bool isPulled = false;
    private UIControllerScript uIController;
    private AudioSource myAudio;
    private bool isCrashed = false;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private float dir;
    public float maxHealth = 100;
    public float currentHealth = 100;
    public Image hp;
    // Start is called before the first frame update
    void Start () {
        //move the object
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb2d = this.gameObject.GetComponent<Rigidbody2D> ();
        uIController = GameObject.Find ("Canvas").GetComponent<UIControllerScript> ();
        myAudio = this.gameObject.GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update () {
        rb2d.velocity = -this.transform.up * moveSpeed;
        hp.fillAmount = currentHealth / maxHealth;
        if (currentHealth == 0) {
            uIController.playerDead ();
        }
        if (Input.GetKey (KeyCode.Z) && !isPulled || Input.GetMouseButtonDown (0)) {
            if (closestTower != null && hookedTower == null) {
                hookedTower = closestTower;
            }
            if (hookedTower) {
                float distance = Vector2.Distance (this.transform.position, hookedTower.transform.position);

                //Gravitation towards object
                Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;
                float newPullForce = pullForce / distance;
                rb2d.AddForce (pullDirection * newPullForce);

                //Angular velocity
                rb2d.angularVelocity = dir * rotateSpeed / distance;

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
        if (Data.score >= 200) {
            uIController.nextLvl ();
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
                currentHealth -= 20;
            }
        }
    }

    public void OnTriggerEnter2D (Collider2D collider) {
        if (collider.gameObject.tag.Equals ("Finish")) {
            uIController.endGame ();
            this.gameObject.SetActive (false);
        }
        if (collider.gameObject.tag.Equals ("Coin")) {
            Data.score += 15;
            Destroy (collider.gameObject);
        }
    }

    public void OnTriggerStay2D (Collider2D collider) {
        if (collider.gameObject.tag.Equals ("Tower")) {
            closestTower = collider.gameObject;
            GameObject child = collider.transform.GetChild (0).gameObject;
            dir = child.GetComponent<Rotator> ().direction;
        }
    }

    public void OnTriggerExit2D (Collider2D collider) {
        if (isPulled) {
            return;
        }
        if (collider.gameObject.tag.Equals ("Tower")) {
            closestTower = null;
            hookedTower = null;
        }
    }

    public void restartPosition () {
        //set to restart position
        this.transform.position = startPosition;

        //restart rotation
        this.transform.rotation = startRotation;

        //set isCrashed false
        isCrashed = false;

        if (closestTower) {
            closestTower = null;
            hookedTower = null;
            isPulled = false;
            Input.ResetInputAxes ();
        }
    }

    void OnBecameInvisible () {
        restartPosition ();
        currentHealth -= 20;
    }
}