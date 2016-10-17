using UnityEngine;

public class BouncingObjectScript : MovingObjects {

    public AudioClip hurt;

    protected override void FixedUpdate () {
        base.FixedUpdate ();
        float width = Screen.width * .5f;
        if ( Mathf.Abs ( Camera.main.WorldToScreenPoint ( transform.position ).x - width ) - width > 21 ) {
            dir *= -1;
            transform.position = new Vector3 ( transform.position.x + dir, transform.position.y, transform.position.z );
            transform.localScale = new Vector3 ( transform.localScale.x * -1, transform.localScale.y, transform.localScale.z );
        }
    }

    void OnTriggerEnter2D ( Collider2D _other ) {
        dir = 0;
        if ( Mathf.Abs ( transform.position.x - _other.transform.position.x ) > myWidth*.5f +2 ) {
            GameObject.Find ( "Balloon" ).GetComponent<AudioSource> ().PlayOneShot ( hurt );
            GetComponent<Animator> ().SetTrigger ( "Hurt" );
        } else {

            GameObject.Find ( "Balloon" ).GetComponent<AudioSource> ().PlayOneShot ( soundFX );
            GetComponent<Animator> ().SetTrigger ( "Collided" );
        }
    }
}
