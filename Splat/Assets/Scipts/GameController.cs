using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {
	public struct Item {
		public Animator animator;
		public GameObject dropItem;
		public Sprite kingsItem;
		public bool unlocked;
	}
	static public byte[] coolDowns = {5,5,5,5,5,5,5,5,5,5,5,5};
	static public byte[] indexes = {0,1,2,3,4};
	static public byte level = 0, minions = 0, currentItem, stars = 6, starStep;

	public static bool drag, cooling, playing;
	float scale, shootSpeed = .8f, starY;
	const uint starDist = 450U;
	static public string[] kills = {"Single Kill", "Double Kill", "Triple Kill", "Quadra Kill", "Penta Kill", "Crazy Kill!", "Insane Kill!!", "MURDER!!!", "UnbelIEVABLE!!!!", "UNREAL!!!!!", "WTF!!!!!!", "NO WAY!!!!!!!", "OMG!!!!!!!!" };
	static Item[] allItems;
	List<float> xs = new List<float>();
	static Animator anim;
	public static Transform cursor, camer;
	Quaternion rot;
	static public AudioSource audioSource;
	public AudioClip error;
	static SpriteRenderer kingItem;
	Vector3 cameraSpeed, kingPos;

	// Use this for initialization
	void Start () {
		//Time.timeScale = 0;
		starStep = 3;
		playing = false;
		cameraSpeed.z = .0005f;
		currentItem = 0;
		allItems = new Item [ 5 ];
		sbyte i = -1; while ( ++i != 5 ) {
			allItems [ i ].animator = GameObject.Find ( "Item" + i ).GetComponent<Animator> ();
			allItems [ i ].dropItem = ( GameObject )Resources.Load ( "items/Item" + indexes [ i ] );
			allItems [ i ].kingsItem = GameObject.Find ( "Item" + i ).GetComponent<UnityEngine.UI.Image> ().sprite;
			if ( indexes [ i ] * 6 < stars + 1 )
				allItems [ i ].unlocked = true;
		}
		audioSource = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		camer = Camera.main.transform;
		allItems [ 0 ].animator.SetBool ( "Pressed", true );
		cursor = GameObject.Find ( "Cursor" ).transform;
		rot = transform.rotation;
		scale = transform.localScale.x;
		kingItem = transform.GetChild ( 0 ).GetChild ( 0 ).GetComponent<SpriteRenderer> ();
		kingItem.sprite = allItems [ 0 ].kingsItem;
		//starY = Camera.main.ScreenToWorldPoint ( GameObject.Find ( "Star1" ).transform.position ).y;
		//mousepostext = GameObject.Find("mousepos").GetComponent<UnityEngine.UI.Text>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if ( playing ) {
			kingPos = transform.position;
			if ( drag ) {
				cursor.position = new Vector3 ( Camera.main.ScreenToWorldPoint ( new Vector3 ( Input.mousePosition.x, 0, -camer.position.z ) ).x, cursor.position.y, cursor.position.z );
				shootSpeed += .01f;
				if ( shootSpeed > 5 )
					shootSpeed = 5;
			}
			float xToAim = xs.Count == 0?cursor.position.x : xs[0];
			float dis = Mathf.Clamp( kingPos.x - xToAim,-10f,10f);
			if ( Mathf.Abs ( dis ) > .4f )
				kingPos.x -= dis * .2f;
			else {
				dis = 0f;
				if ( xs.Count != 0 ) {
					xs.RemoveAt ( 0 );
					rot.z = 0f;
					transform.rotation = rot;
					Fire ();
				}
			}

			Vector3 cursorScreenPos = Camera.main.WorldToScreenPoint(cursor.position);
			if ( cursorScreenPos.x > Screen.width )
				cursor.position -= new Vector3 ( .02f, 0 );
			else if ( cursorScreenPos.x < 0f )
				cursor.position += new Vector3 ( .02f, 0 );

			//if ( starStep == 3 && Camera.main.WorldToScreenPoint ( transform.position ).y - starY < starDist )
			//	SetStarAnimation ();
			//else if ( starStep == 2 && Camera.main.WorldToScreenPoint ( transform.position ).y - starY < starDist )
			//	SetStarAnimation ();
			//else if ( starStep == 1 && Camera.main.WorldToScreenPoint ( transform.position ).y - starY < starDist )
			//	SetStarAnimation ();

			rot.z = Mathf.Lerp(rot.z, dis*.1f,.5f);
			transform.rotation = rot;
			//kingPos.z = -camer.position.z;
			kingPos = Camera.main.WorldToScreenPoint ( kingPos );
			kingPos.y = Screen.height - 30; ;
			transform.position = Camera.main.ScreenToWorldPoint(kingPos);
			camer.position -= cameraSpeed * camer.position.z;
		}
	}

	void Fire () {
		if ( !allItems [ currentItem ].animator.GetCurrentAnimatorStateInfo ( 0 ).IsName ( "Activated" ) ) {

			audioSource.Play ();
			( ( GameObject )Instantiate ( allItems [ currentItem ].dropItem, transform.position, transform.rotation ) ).GetComponent<ItemScript> ().speed *= shootSpeed;
			anim.SetFloat ( "Speed", shootSpeed );
			anim.SetTrigger ( "Atk" );
			allItems [ currentItem ].animator.SetTrigger ( "Activated" );
			allItems [ currentItem ].animator.GetComponent<ButtonScript> ().SetTimer ();
			shootSpeed = .8f;

			byte i= 0; while ( ++i != 5 )
				if ( !allItems [ ( currentItem + i ) % 5 ].animator.GetCurrentAnimatorStateInfo ( 0 ).IsName ( "Activated" ) && allItems [ ( currentItem + i ) % 5 ].unlocked ) {
					currentItem = ( byte )( ( currentItem + i ) % 5 );
					kingItem.sprite = allItems [ currentItem ].kingsItem;
					break;
				}
			if ( i == 5 ) {
				anim.SetBool ( "Cooled", false );
				cooling = true;
			} else
				allItems [ currentItem ].animator.GetComponent<EventTrigger> ().OnPointerClick ( new PointerEventData ( EventSystem.current ) );
		} else
			audioSource.PlayOneShot ( error );
	}

	void SetStarAnimation () {
		if ( playing ) {
			GameObject.Find ( "Star" + starStep ).GetComponent<Animator> ().SetTrigger ( "die" + Random.Range ( 0, 3 ) );
			--starStep;
			if ( starStep == 0 ) {
				Lose ();
				return;
			}
			starY = GameObject.Find ( "Star" + starStep ).transform.position.y;
		}
	}

	public void SetDrag () {
		drag = true;
		if ( !cooling )
			anim.SetBool ( "Charge", true );
	}

	public void Lose () {
		UnityEngine.SceneManagement.SceneManager.LoadScene ( "SelectItemScene" );
	}

	public void StopDrag () {
		drag = false;
		if ( indexes [ currentItem ] == 4 )
			Fire ();
		else if ( !cooling )
			xs.Add ( new Vector2 ( Camera.main.ScreenToWorldPoint ( new Vector3 ( Input.mousePosition.x, 0, -camer.position.z ) ).x, cursor.position.y ).x );
	}

	public void SetItem ( int _item ) {
		currentItem = ( byte )_item;
	}

	public static void Refresh ( byte _item ) {
		if ( cooling ) {
			cooling = false;
			anim.SetBool ( "Cooled", true );
			currentItem = _item;
			allItems [ currentItem ].animator.GetComponent<EventTrigger> ().OnPointerClick ( new PointerEventData ( EventSystem.current ) );
			kingItem.sprite = allItems [ currentItem ].kingsItem;
		}

	}

	public void ShakeCall ( float magnitude ) {
		StopCoroutine ( "Shake" );
		if ( playing )
			StartCoroutine ( Shake ( magnitude ) );
	}

	public void CheckWin ( Vector3 _pos ) {
		--minions;
		if ( minions <= 0 ) {
			++level;
			//set Camera win couroutine
			//camer.forward = (_pos - camer.position).normalized;
			//camer.Rotate ( transform.right, -22.8f );
			playing = false;
			StartCoroutine ( ZoomIn ( _pos ) );
		}
	}

	IEnumerator Shake ( float magnitude ) {
		float camY = camer.position.y;
		Vector3 shakePos;
		shakePos.z = 0;
		// map value to [-1, 1]
		while ( magnitude > 0 ) {
			if ( !playing )
				yield break;
			shakePos.x = Random.Range ( -magnitude * 1.6f, magnitude * 1.6f );
			shakePos.y = Random.Range ( 0, magnitude );

			camer.position += shakePos;

			yield return new WaitForSeconds ( .04f );
			camer.position = new Vector3 ( 0, camY, camer.position.z );
			magnitude -= .3f;
		}
	}

	IEnumerator ZoomIn ( Vector3 _pos ) {
		float xDistance = (_pos.x - camer.position.x);
		float zDistance = (_pos.z - camer.position.z);
		// map value to [-1, 1]
		while ( zDistance > 22 ) {
			if ( Time.timeScale > .3f )
				Time.timeScale -= .007f;
			cameraSpeed.x = xDistance / 30f;
			cameraSpeed.z = Mathf.Pow ( zDistance, 2.2f ) / 16000f;
			yield return new WaitForSeconds ( .1f );
			xDistance = ( _pos.x - camer.position.x );
			zDistance = ( _pos.z - camer.position.z );
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene ( "SelectItemScene" );
		Time.timeScale = 1;
	}
}
