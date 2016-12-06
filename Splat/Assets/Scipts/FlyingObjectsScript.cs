using UnityEngine;

public class FlyingObjectsScript : MovingObjects {
    public AudioClip soundFX;
	// Use this for initialization
	protected void Start () {
        dir.x = Random.Range(.2f, .3f);
        if ( Random.Range ( 0, 2 ) == 0 ) {
            dir *= -1;
            transform.localScale = new Vector3 ( transform.localScale.x * Mathf.Sign ( dir.x ), transform.localScale.y, transform.localScale.z );
        }
        // x = ( _x + 1 ) * R_WIDTH * .5f;
        // x*2 / R_WIDTH - 1 = _x
        transform.position = new Vector2 ( (Random.Range(0f,480f)*2f/480f - 1f)*480f, transform.position.y + Random.Range ( -15f, 15f ) );
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        GameObject.Find ( "Balloon" ).GetComponent<AudioSource> ().PlayOneShot ( soundFX );
        GetComponent<Animator> ().SetTrigger ( "Collided" );
    }
}
