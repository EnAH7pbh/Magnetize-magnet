using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIControllerScript : MonoBehaviour {
    public GameObject pausePanel;
    public GameObject resumeBtn;
    public Text levelClearTxt;
    private Scene currActiveScene;
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
        pausePanel.SetActive (true);
    }

    public void resumeGame () {
        Time.timeScale = 1;
        pausePanel.SetActive (false);
    }

    public void restartGame () {
        Time.timeScale = 1;
        SceneManager.LoadScene (currActiveScene.name);
    }

    public void endGame () {
        pausePanel.SetActive (true);
        resumeBtn.SetActive (false);
        levelClearTxt.text = "You win";
    }
}