using UnityEngine;

public class Rotator : MonoBehaviour {
    // Update is called once per frame
    public float direction;
    void FixedUpdate () {
        this.transform.eulerAngles += new Vector3 (0, 0, direction);
    }

    public float back () {
        return -1;
    }

    public float forward () {
        return 1;
    }
}