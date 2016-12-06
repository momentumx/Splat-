using UnityEngine;

public class EnemyScript : MovingObjects {
    bool available = true;
    float timer, maxSpeed;
    public float sleepTime;
    public AudioClip die, hurt;
    public AudioSource king;
	// Use this for initialization
	void Start () {
        if ( Random.Range ( 0, 2 ) == 0 ) {
            dir *= -1;
            transform.localScale = new Vector3 ( transform.localScale.x * Mathf.Sign ( dir.x ), transform.localScale.y, transform.localScale.z );
        }
        transform.position = new Vector2 ( Random.Range ( -35, 35 ), transform.position.y );
        
        king = GameObject.Find ( "Balloon" ).GetComponent<AudioSource> ();
        ++GameController.minions;
    }

    protected override void FixedUpdate () {
        base.FixedUpdate ();
        //transform.LookAt ( GameController.camer );
        if ( timer != 0 ) {
            timer -= Time.fixedDeltaTime;
            if ( timer <= 0 ) {
                timer = 0;
                dir.x = maxSpeed;
                GetComponent<Animator> ().SetTrigger ( "Cured" );
            }
        }
    }

    public void MakeAvailable() {
        available = true;
    }

    void Collide (ItemScript item, bool itemAct = false) {
        available = false;
        GetComponent<Collider2D> ().enabled = false;
        GetComponent<Animator> ().SetTrigger ( "Collided" );
        king.PlayOneShot ( die );
        maxSpeed = dir.x;
        dir.x = 0;
        ++item.kills;
        if ( itemAct ) {
            king.PlayOneShot ( item.collideSFX );
            item.GetComponent<Animator> ().SetBool ( "Hit", true );
        }
        king.GetComponent<GameController> ().CheckWin ( transform.position );
    }

    void CollideInjur ( ItemScript item, bool itemAct = false ) {
        available = false;
        GetComponent<Animator> ().SetTrigger ( "Hurt" );
        king.PlayOneShot ( hurt );
        maxSpeed = dir.x;
        timer = sleepTime;
        dir.x = 0;
        if ( itemAct ) {
            king.PlayOneShot ( item.collideSFX );
            item.GetComponent<Animator> ().SetBool ( "Hit", true );
        }
    }

    protected virtual void Nana ( Collider2D _other ) {

        CollideInjur ( _other.GetComponent<ItemScript> (), true );
        _other.tag = "Untagged";
    }

    protected virtual void Box ( Collider2D _other ) {
        if (timer==0 && Mathf.Abs ( transform.position.x - _other.transform.position.x ) > myWidth + 2 ) {
            
            CollideInjur ( _other.GetComponent<ItemScript> () );
        } else {
            Collide ( _other.GetComponent<ItemScript> () );
        }
    }

    protected virtual void Pogo ( Collider2D _other ) {
        if ( timer == 0 && Mathf.Abs ( transform.position.x - _other.transform.position.x ) > myWidth + 2 ) {
            CollideInjur ( _other.GetComponent<ItemScript> () );
        } else {
            Collide ( _other.GetComponent<ItemScript> () );
        }
    }

    protected virtual void Fire ( Collider2D _other ) {
        if ( timer == 0 && Mathf.Abs ( transform.position.x - _other.transform.position.x ) > myWidth + 2 ) {
            CollideInjur ( _other.GetComponent<ItemScript> (), true );
        } else {
            Collide ( _other.GetComponent<ItemScript> () );
        }
    }

    protected virtual void Star ( Collider2D _other ) {
        Collide ( _other.GetComponent<ItemScript> () );
    }

    protected virtual void Bomb ( Collider2D _other ) {
        Collide ( _other.GetComponent<ItemScript> () );
    }

    protected virtual void Gas ( Collider2D _other ) {
        Collide ( _other.GetComponent<ItemScript> (), true );
        _other.tag = "Untagged";
    }

    protected virtual void Mine ( Collider2D _other ) {
        Collide ( _other.GetComponent<ItemScript> (), true );
        _other.tag = "Untagged";
    }

    protected virtual void Ninja ( Collider2D _other ) {
        if ( timer == 0 && Mathf.Abs ( transform.position.x - _other.transform.position.x ) > myWidth + 2 ) {
            CollideInjur ( _other.GetComponent<ItemScript> () );
        } else {
            Collide ( _other.GetComponent<ItemScript> () );
        }
    }

    protected virtual void Spear ( Collider2D _other ) {
        if ( timer != 0 || Mathf.Abs ( transform.position.x - _other.transform.position.x ) < myWidth + 2 ) {
            Collide ( _other.GetComponent<ItemScript> () );
        } else {
            CollideInjur ( _other.GetComponent<ItemScript> () );
        }
    }

    protected virtual void Trap ( Collider2D _other ) {
        dir.x = 0;
        _other.tag = "Untagged";
        ItemScript item = _other.GetComponent<ItemScript> ();
        king.PlayOneShot ( item.collideSFX );
        item.GetComponent<Animator> ().SetBool ( "Hit", true );
    }

    void OnTriggerStay2D ( Collider2D _other ) {

        if ( available ) {
            switch ( _other.tag ) {
                case "Box":
                    Box ( _other );
                    break;
                case "Nana":
                    if(timer==0)
                        Nana ( _other );
                    break;
                case "Spear":
                    Spear ( _other );
                    break;
                case "Ninja":
                    Ninja ( _other );
                    break;
                case "Trap":
                    if (timer == 0 && Mathf.Abs ( transform.position.x - _other.transform.position.x ) < .2f )
                        Trap ( _other );
                    break;
                case "Pogo":
                    Pogo ( _other );
                    break;
                case "Fire":
                    Fire ( _other );
                    break;
                case "Star":
                    Star ( _other );
                    break;
                case "Bomb":
                    Bomb ( _other );
                    break;
                case "Gas":
                    Gas ( _other );
                    break;
                case "Mine":
                    Mine ( _other );
                    break;
                default:
                    break;
            }
        }
    }
}
