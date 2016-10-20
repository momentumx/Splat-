using UnityEngine;

public class EnemyScript : MovingObjects {
    float myWidth, timer, maxSpeed;
    public float sleepTime;
    public AudioClip soundFX, hurt;
	// Use this for initialization
	void Start () {
        if ( Random.Range ( 0, 2 ) == 0 ) {
            dir *= -1;
            transform.localScale = new Vector3 ( transform.localScale.x * Mathf.Sign ( dir.x ), transform.localScale.y, transform.localScale.z );
        }
        transform.position = new Vector2 ( Random.Range ( -35, 35 ), transform.position.y );
        myWidth = GetComponent<SpriteRenderer> ().bounds.extents.x*.5f;
    }

    protected override void FixedUpdate () {
        base.FixedUpdate ();
        if ( timer != 0 ) {
            timer -= Time.fixedDeltaTime;
            if ( timer <= 0 ) {
                timer = 0;
                dir.x = maxSpeed;
                GetComponent<Animator> ().SetTrigger ( "Cured" );
            }
        }
    }

    protected void Collide (Animator _anim, string _trigger, AudioClip _SFX) {
        GameObject.Find ( "Balloon" ).GetComponent<AudioSource> ().PlayOneShot ( _SFX );
        _anim.SetBool ( "Hit", true );
        GetComponent<Animator> ().SetTrigger ( _trigger );
        maxSpeed = dir.x;
        dir.x = 0;
    }

    void OnTriggerEnter2D ( Collider2D _other ) {
        
        switch ( _other.tag ) {
            case "Kill":
                Collide ( _other.GetComponent<Animator> (), "Collided", soundFX );
                break;
            case "Nana":
                timer = sleepTime;
                Collide ( _other.GetComponent<Animator> (), "Hurt", hurt );
                break;
            case "Box":
                if ( Mathf.Abs ( transform.position.x - _other.transform.position.x ) > myWidth + 2 ) {
                    timer = sleepTime;
                    Collide ( _other.GetComponent<Animator> (), "Hurt", hurt );
                } else {
                    Collide ( _other.GetComponent<Animator> (), "Collided", soundFX );
                    ++_other.GetComponent<ItemScript> ().kills;
                }
                break;
            case "Trap":
                dir.x = 0;
                break;
            default:
                break;
        }
    }
}
