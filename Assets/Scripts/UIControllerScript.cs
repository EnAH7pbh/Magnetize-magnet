using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIControllerScript : MonoBehaviour {
    public GameObject Panel;
    public Transform resumeBtn;
    public Transform nextLvlBtn;
    public Text levelClearTxt;
    public string currActiveScene;
    public string nextScene;
    public Text info;
    public GameObject gate;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            pauseGame ();
        }
    }

    public void startGame () {
        SceneManager.LoadScene ("Level1");
    }
    public void pauseGame () {
        Time.timeScale = 0;
        Panel.SetActive (true);
        nextLvlBtn.GetComponent<Button> ().interactable = false;
        resumeBtn.GetComponent<Button> ().interactable = true;
    }

    public void playerDead () {
        Time.timeScale = 0;
        Panel.SetActive (true);
        Panel.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "You are dead";
        nextLvlBtn.GetComponent<Button> ().interactable = false;
        resumeBtn.GetComponent<Button> ().interactable = false;
    }
    public void resumeGame () {
        Time.timeScale = 1;
        Panel.SetActive (false);
    }

    public void restartLevel () {
        Time.timeScale = 1;
        SceneManager.LoadScene (currActiveScene);
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
        resumeBtn.GetComponent<Button> ().interactable = false;
        nextLvlBtn.GetComponent<Button> ().interactable = true;
        levelClearTxt.text = "You win";
    }
    public void home () {
        Time.timeScale = 1;
        SceneManager.LoadScene ("Menu");
    }

    public void exit () {
        Application.Quit ();
    }

    public void nxtScene () {
        Time.timeScale = 1;
        SceneManager.LoadScene (nextScene);
        Data.score = 0;
    }
}