using UnityEngine;

public class Rotator : MonoBehaviour {
    // Update is called once per frame
    public float direction;
    void FixedUpdate () {
        this.transform.eulerAngles += new Vector3 (0, 0, direction);
    }
}