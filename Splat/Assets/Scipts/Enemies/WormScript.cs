using UnityEngine;
public class WormScript : EnemyScript {

    protected override void Start () {
        base.Start ();
        SetSpeed0 ();
    }

    protected override void FixedUpdate () {
        if ( 1 == Random.Range ( 0, 60 ) ) {
            dir.x *= -1;

        }
        base.FixedUpdate ();
    }

    void KnockUp ( Rigidbody2D _other ) {
        _other.tag = "Untagged";
        _other.velocity = new Vector2 ( 0f, -20f );
        _other.GetComponent<Animator> ().SetTrigger ( "Crash" );

    }

    protected override void Nana ( Collider2D _other ) {
        KnockUp ( _other.GetComponent<Rigidbody2D>() );
    }

    protected override void Mine ( Collider2D _other ) {
        KnockUp ( _other.GetComponent<Rigidbody2D> () );
    }

    protected override void Trap ( Collider2D _other ) {
        KnockUp ( _other.GetComponent<Rigidbody2D> () );
    }

    protected override void Bomb ( Collider2D _other ) {
        KnockUp ( _other.GetComponent<Rigidbody2D> () );
    }
}
