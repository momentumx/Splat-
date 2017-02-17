using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatScript : MonoBehaviour {
	Vector2 splatPos, endScale;
	float timer;
	// Use this for initialization
	void Start () {
		timer = Time.time+.5f;
		transform.localScale *= .01f;
		endScale = Vector2.one * Random.Range ( .5f, 1.5f );
		UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image> ();
		Color temp = image.color;
		temp.a = .9f;
		image.sprite = Resources.LoadAll<Sprite> ( "Splats" ) [ Random.Range ( 0, 28 ) ];
		image.color = temp;
		Destroy ( gameObject, 6f );
		splatPos.x = Random.Range ( 20, Screen.width ) - 10;
		splatPos.y = Random.Range ( 450, Screen.height ) - 300;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if ( Time.time<timer ) {
			transform.localScale = Vector3.LerpUnclamped ( transform.localScale, endScale, .2f );
			transform.position = Vector2.LerpUnclamped ( transform.position, splatPos, .2f );
			return;
		}
		transform.position -= new Vector3 ( 0f, .15f );
		transform.localScale += Vector3.up * .003f;

	}
}