using UnityEngine;

public class FatBoyScript : EnemyScript {
	public AudioClip eating;

	void Eat ( Transform _other ) {
		_other.tag = "Untagged";
		_other.SetParent ( transform.GetChild ( 0 ) );
		_other.localPosition = Vector2.zero;
		SetSpeed0 ();
		Destroy ( _other.gameObject, 1f );
		GetComponent<Animator> ().SetTrigger ( "Extra" );

	}

	protected override void Nana ( Collider2D _other ) {
		Eat ( _other.transform );
	}

	protected override void Mine ( Collider2D _other ) {
		Eat ( _other.transform );
	}

	protected override void Trap ( Collider2D _other ) {
		Eat ( _other.transform );
	}

	protected override void Bomb ( Collider2D _other ) {
		Eat ( _other.transform );
	}

	protected override void Poison ( Collider2D _other ) {
		Eat ( _other.transform );
	}
}
