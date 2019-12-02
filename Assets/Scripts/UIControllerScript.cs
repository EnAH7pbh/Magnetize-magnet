using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIControllerScript : MonoBehaviour {
    public GameObject Panel;
    public GameObject resumeBtn;
    public Text levelClearTxt;
    private Scene currActiveScene;
    public Text info;
    public GameObject gate;
    // Start is called before the first frame update
    void Start () {
        currActiveScene = SceneManager.GetActiveScene ();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            pauseGame ();
        }
    }

    public void pauseGame () {
        Time.timeScale = 0;
        Panel.SetActive (true);
    }

    public void playerDead () {
        Time.timeScale = 0;
        Panel.SetActive (true);
        Panel.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "You are dead";
        Panel.transform.GetChild (1).gameObject.SetActive (false);
    }
    public void resumeGame () {
        Time.timeScale = 1;
        Panel.SetActive (false);
    }

    public void restartLevel () {
        Time.timeScale = 1;
        SceneManager.LoadScene (currActiveScene.name);
        Data.score = 0;
    }

    public void nextLvl () {
        info.text = "Go to the next Level !!";
        gate.SetActive (true);
        info.color = Color.red;
        info.fontSize = 24;
        info.color = new Color (info.color.r, info.color.g, info.color.b, Mathf.Sin (Time.time * 5));
    }

    public void endGame () {
        Panel.SetActive (true);
        resumeBtn.SetActive (false);
        levelClearTxt.text = "You win";
    }
    public void home () {
        Time.timeScale = 1;
        SceneManager.LoadScene ("Menu");
    }
}