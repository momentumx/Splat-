using UnityEngine;

public class HardBackScript : EnemyScript {
    // Use this for initialization

    protected override void Ninja ( Collider2D _other ) {
        GameController.audioSource.PlayOneShot ( _other.GetComponent<ItemScript> ().collideSFX );
        _other.GetComponent<Animator> ().SetBool ( "Hit", true );
        _other.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
    }

    protected override void Spear ( Collider2D _other ) {
        GameController.audioSource.PlayOneShot ( _other.GetComponent<ItemScript> ().collideSFX );
        _other.GetComponent<Animator> ().SetBool ( "Hit", true );
        _other.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
    }
}
