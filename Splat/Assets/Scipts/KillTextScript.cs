using UnityEngine;
using System.Collections;

public class KillTextScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.SetParent ( GameObject.Find ( "Canvas" ).transform );
        Destroy ( gameObject, 2.3f );
	}

    void FixedUpdate () {
        transform.position += new Vector3 ( 0, .7f );
    }
}
