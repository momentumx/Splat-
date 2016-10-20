using UnityEngine;

public class FlyingObjectsScript : MovingObjects {
    public AudioClip soundFX;
	// Use this for initialization
	protected void Start () {
        dir.x = Random.Range(.1f, .2f);
        if ( Random.Range ( 0, 2 ) == 0 ) {
            dir *= -1;
            transform.localScale = new Vector3 ( transform.localScale.x * Mathf.Sign ( dir.x ), transform.localScale.y, transform.localScale.z );
        }
        transform.position = new Vector2 ( Random.Range ( -35, 35 ), transform.position.y + Random.Range ( -15, 16 ) );
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        GameObject.Find ( "Balloon" ).GetComponent<AudioSource> ().PlayOneShot ( soundFX );
        GetComponent<Animator> ().SetTrigger ( "Collided" );
    }
}
