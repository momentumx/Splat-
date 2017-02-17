using UnityEngine;

public class EnemyScript : MovingObjects {
	bool available = true, special;
	[HideInInspector]
	public float timer, maxSpeed;
	public float sleepTime;
	public AudioClip die, hurt;
	[SerializeField]
	Color color;
	// Use this for initialization
	protected virtual void Start () {
		if ( Random.Range ( 0, 2 ) == 0 ) {
			SwitchDirections ();
		}
		RandomizePosition ( 0f );
		++GameController.minions;
	}

	protected override void FixedUpdate () {
		if ( special ) {
			Vector3 newPos = transform.position;
			Mathf.Lerp ( newPos.x, Camera.main.transform.position.x, .3f );
			Mathf.Lerp ( newPos.y, Camera.main.transform.position.y, .3f );
			Mathf.Lerp ( newPos.z, Camera.main.transform.position.z + 30f, .3f );
			transform.position = newPos;
		} else {
			base.FixedUpdate ();
			if ( timer != 0 ) {
				timer -= Time.fixedDeltaTime;
				if ( timer <= 0 ) {
					timer = 0;
					SetSpeed ();
					GetComponent<Animator> ().SetTrigger ( "Cured" );
				}
			}
		}
	}

	public void MakeAvailable () {
		available = true;
	}

	public void SetSpeed () {
		dir.x = maxSpeed;
	}

	public void SetSpeed0 () {
		maxSpeed = dir.x;
		dir.x = 0;
	}

	void Collide ( ItemScript item, bool itemAct = false ) {
		available = false;
		GetComponent<Collider2D> ().enabled = false;
		GetComponent<Animator> ().SetTrigger ( "Collided" );
		GameController.audioSource.PlayOneShot ( die );
		SetSpeed0 ();
		++item.kills;
		if ( itemAct ) {
			GameController.audioSource.PlayOneShot ( item.collideSFX );
			item.GetComponent<Animator> ().SetBool ( "Hit", true );
		}
		GameController.audioSource.GetComponent<GameController> ().CheckWin ( transform.position );
		Instantiate (
			Resources.Load<GameObject> ( "Splat" )
			, Camera.main.WorldToScreenPoint ( transform.position )
			, Quaternion.identity
			, GameObject.Find ( "Canvas" ).transform
			).GetComponent<UnityEngine.UI.Image> ().color = color;
	}

	void CollideInjur ( ItemScript item, bool itemAct = false ) {
		available = false;
		GetComponent<Animator> ().SetTrigger ( "Hurt" );
		GameController.audioSource.PlayOneShot ( hurt );
		timer = sleepTime;
		SetSpeed0 ();
		if ( itemAct ) {
			GameController.audioSource.PlayOneShot ( item.collideSFX );
			item.GetComponent<Animator> ().SetBool ( "Hit", true );
		}
	}

	protected virtual void Nana ( Collider2D _other ) {

		CollideInjur ( _other.GetComponent<ItemScript> (), true );
		_other.tag = "Untagged";
	}

	protected virtual void Box ( Collider2D _other ) {
		if ( timer == 0 && Mathf.Abs ( transform.position.x - _other.transform.position.x ) > myWidth + 2 ) {

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

	protected virtual void Poison ( Collider2D _other ) {
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
		SetSpeed0 ();
		_other.tag = "Untagged";
		ItemScript item = _other.GetComponent<ItemScript> ();
		GameController.audioSource.PlayOneShot ( item.collideSFX );
		item.GetComponent<Animator> ().SetBool ( "Hit", true );
	}

	void OnTriggerStay2D ( Collider2D _other ) {

		if ( available ) {
			switch ( _other.tag ) {
				case "Box":
					Box ( _other );
					break;
				case "Nana":
					if ( timer == 0 )
						Nana ( _other );
					break;
				case "Spear":
					Spear ( _other );
					break;
				case "Ninja":
					Ninja ( _other );
					break;
				case "Trap":
					if ( timer == 0 && Mathf.Abs ( transform.position.x - _other.transform.position.x ) < .2f )
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
				case "Poison":
					Poison ( _other );
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
