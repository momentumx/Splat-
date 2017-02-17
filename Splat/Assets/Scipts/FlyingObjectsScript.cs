using UnityEngine;

public class FlyingObjectsScript : MovingObjects {
	[SerializeField]
	AudioClip soundFX;
	[SerializeField]
	string fileName;
	// Use this for initialization
	protected void Start () {
		dir.x = Random.Range ( .12f, .34f );
		if ( Random.Range ( 0, 2 ) == 0 ) {
			dir *= -1;
			transform.localScale = new Vector3 ( transform.localScale.x * Mathf.Sign ( dir.x ), transform.localScale.y, transform.localScale.z );
		}
		RandomizePosition ( 15f );
		if ( fileName != "" )
			GetComponent<SpriteRenderer> ().sprite = Resources.LoadAll<Sprite> ( fileName ) [ Random.Range ( 0, 1 ) ];
	}

	void OnTriggerEnter2D ( Collider2D _other ) {
		GameController.audioSource.PlayOneShot ( soundFX );
		GetComponent<Animator> ().SetTrigger ( "Collided" );
	}
}
