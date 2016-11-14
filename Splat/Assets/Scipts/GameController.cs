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
    static public byte level = 0, minions = 0, currentItem;

    static bool drag, cooling, playing = true;
    float speed, scale, shootSpeed = .8f, dis, starY;
    const uint starDist = 10;
    byte stars = 3;
    static public string[] kills = {"Single Kill", "Double Kill", "Triple Kill", "Quadra Kill", "Penta Kill", "Crazy Kill", "Insane Kill", "Murder" };
    static Item[] allItems;
    List<float> xs = new List<float>();
    static Animator anim;
    static Transform cursor, camer;
    Quaternion rot;
    public AudioSource audioSource;
    public AudioClip error;
    static SpriteRenderer kingItem;
    Vector3 cameraSpeed;

	// Use this for initialization
	void Start () {
        cameraSpeed = new Vector3 ( 0f, 0f, .02f );
        currentItem = 0;
        allItems = new Item [ 5 ];
        sbyte i = -1; while ( ++i!=5 ) {
            allItems[i].animator = GameObject.Find ( "Item" + i ).GetComponent<Animator> ();
            allItems[i].dropItem = (GameObject)Resources.Load ( "items/Item" + indexes [ i] );
            allItems[i].kingsItem = GameObject.Find ( "Item" + i ).GetComponent<UnityEngine.UI.Image> ().sprite;
            if ( i < level + 2 )
                allItems [ i ].unlocked = true;
        }
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        camer = Camera.main.transform;
        allItems[0].animator.SetBool("Pressed", true);
        cursor = GameObject.Find("Cursor").transform;
        rot = transform.rotation;
        scale = transform.localScale.x;
        kingItem = transform.GetChild ( 0 ).GetChild ( 0 ).GetComponent<SpriteRenderer> ();
        kingItem.sprite = allItems [ 0 ].kingsItem;
        starY = GameObject.Find ( "Star1" ).transform.position.y;
        //mousepostext = GameObject.Find("mousepos").GetComponent<UnityEngine.UI.Text>();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        if (drag)
        {
            cursor.position = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, -camer.position.z)).x, cursor.position.y);
            shootSpeed += .01f;
            if (shootSpeed > 5)
                shootSpeed = 5;
        }

        //mousepostext.text = Input.mousePosition.x.ToString("F1") +", " + Input.mousePosition.y;
        if(xs.Count == 0)
            dis = (cursor.position.x - transform.position.x);
        else
            dis = (xs[0] - transform.position.x);
        float disMag = Mathf.Abs(dis);
        if (disMag > .4f)
        {
            if (disMag > 30)
                speed += .1f*Mathf.Sign(dis);
            else
                speed = dis*.07f;

            if (Mathf.Abs(speed) > 2)
                speed = 2 * Mathf.Sign(speed);
            else if (Mathf.Abs(speed) < .3f)
                speed = .3f * Mathf.Sign(speed);
        }
        else
        {
            speed = 0;
            rot.z = speed;
            transform.rotation = rot;
            if (xs.Count > 0)
            {
                xs.RemoveAt(0);
                if (!allItems[currentItem].animator.GetCurrentAnimatorStateInfo(0).IsName("Activated"))
                {
                    audioSource.Play();
                    ((GameObject)Instantiate( allItems [ currentItem ].dropItem, transform.position, transform.rotation)).GetComponent<ItemScript>().speed *=shootSpeed;
                    anim.SetFloat("Speed", shootSpeed);
                    anim.SetTrigger("Atk");
                    allItems [ currentItem ].animator.SetTrigger("Activated");
                    allItems [ currentItem ].animator.GetComponent<ButtonScript>().SetTimer();
                    shootSpeed = .8f;

                    byte i= 0; while (++i != 5)
                        if ( !allItems [ (currentItem+ i)%5 ].animator.GetCurrentAnimatorStateInfo ( 0 ).IsName ( "Activated" ) && allItems [ ( currentItem + i ) % 5 ] .unlocked) {
                            currentItem = ( byte )(( currentItem + i ) % 5);
                            kingItem.sprite = allItems [ currentItem ].kingsItem;
                            break;
                        }
                    if (i == 5)
                    {
                        anim.SetBool("Cooled", false);
                        cooling = true;
                    }
                    else
                        allItems [ currentItem ].animator.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
                }
                else
                    audioSource.PlayOneShot(error);
            }
        }
        transform.position += new Vector3(speed,- .05f);
        camer.position += cameraSpeed;

        Vector3 cursorScreenPos = Camera.main.WorldToScreenPoint(cursor.position);
        if (cursorScreenPos.x > Screen.width-10)
            cursor.position -= new Vector3(.02f, 0);
        else if (cursorScreenPos.x < 10)
            cursor.position += new Vector3(.02f, 0);
        if (stars == 3 && Camera.main.WorldToScreenPoint(transform.position).y - starY < starDist)
                SetStarAnimation();
            else if (stars == 2 && Camera.main.WorldToScreenPoint(transform.position).y < 500)
                SetStarAnimation();
            else if (stars == 1 && Camera.main.WorldToScreenPoint(transform.position).y < 250)
                SetStarAnimation();

        rot.z = speed*.25f;
        transform.rotation = rot;
    }

    void SetStarAnimation()
    {
        GameObject.Find("Star" + stars).GetComponent<Animator>().SetTrigger("die" + Random.Range(0, 3));
        --stars;
        if ( stars == 0 ) {
            Lose ();
            return;
        }
        starY = GameObject.Find ( "Star" + stars ).transform.position.y;
    }

    public void SetDrag()
    {
        drag = true;
        if(!cooling)
            anim.SetBool("Charge", true);
    }

    public void Lose () {

    }

    public void StopDrag()
    {
        drag = false;
        if(!cooling)
            xs.Add(new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, -camer.position.z)).x, cursor.position.y).x);
    }

    public void SetItem(int _item)
    {
        currentItem = (byte)_item;
    }

    public static void Refresh(byte _item)
    {
        if (cooling)
        {
            cooling = false;
            anim.SetBool("Cooled", true);
            currentItem = _item;
            allItems [ currentItem ].animator.GetComponent<EventTrigger> ().OnPointerClick ( new PointerEventData ( EventSystem.current ) );
            kingItem.sprite = allItems [ currentItem ].kingsItem;
        }
        
    }

    public void ShakeCall(float magnitude)
    {
        if(playing)
        StartCoroutine(Shake(magnitude));
    }

    public void CheckWin (Vector3 _pos) {
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

    IEnumerator Shake(float magnitude)
    {
        float camY = camer.position.y;
        Vector3 shakePos;
        shakePos.z = 0;
        // map value to [-1, 1]
        while (magnitude > 0)
        {
            shakePos.x = Random.Range(-magnitude*1.6f, magnitude*1.6f);
            shakePos.y = Random.Range(0, magnitude);

            camer.position += shakePos;

            yield return new WaitForSeconds(.04f);
            camer.position = new Vector3(0, camY, camer.position.z);
            magnitude -= .3f;
        }
    }

    IEnumerator ZoomIn ( Vector3 _pos ) {
        float xDistance = (_pos.x - camer.position.x);
        float zDistance = (_pos.z - camer.position.z);
        camer.position = new Vector3 ( _pos.x, camer.position.y, camer.position.z );
        // map value to [-1, 1]
        while ( zDistance > 30 ) {
            if( Time.timeScale >.3f)
                Time.timeScale -= .05f;
            cameraSpeed.x = xDistance / 10f;
            cameraSpeed.z = Mathf.Pow(zDistance,2) / 4000f;
            yield return new WaitForSeconds ( .1f );
            xDistance = ( _pos.x - camer.position.x );
            zDistance = (_pos.z - camer.position.z);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene ( 0 );
        Time.timeScale = 1;
    }
}
