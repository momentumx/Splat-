using UnityEngine;

public class NinjaScript : ItemScript {

	// Use this for initialization
	public override void Start () {
        Vector2 curspos = GameObject.Find("Cursor").transform.position;
        GetComponent<Rigidbody2D> ().velocity += new Vector2 ( curspos.x-transform.position.x, curspos.y-transform.position.y ).normalized*speed;
    }
}