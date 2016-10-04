using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {
    struct Item {
        public Animator animators;
        public GameObject dropItem;
        public Sprite kingsItem;
    }
    Item[] allItems;
    
    public static byte[] items = { 114, 117, 113, 112, 115 };
    List<float> xs = new List<float>();
    float speed, scale, shootSpeed = .8f, dis;
    int stars = 3;
    static Animator anim;
    static Transform cursor, camer;
    Quaternion rot;
    AudioSource audioSource;
    public AudioClip error;
    public static byte currentItem;
    static bool drag, cooling;

	// Use this for initialization
	void Start () {
        allItems = new Item [ 5 ];
        sbyte i = -1; while ( ++i!=5 ) {
            allItems[i].animators = GameObject.Find ( "Item" + i ).GetComponent<Animator> ();
            allItems[i].dropItem = (GameObject)Resources.Load ( "items/Item" + currentItem );
            allItems[i].kingsItem = GameObject.Find ( "Item" + i ).GetComponent<UnityEngine.UI.Image> ().sprite;
        }
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        camer = Camera.main.transform;
        GameObject.Find("Item0").GetComponent<Animator>().SetBool("Pressed", true);
        cursor = GameObject.Find("Cursor").transform;
        rot = transform.rotation;
        scale = transform.localScale.x;
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
                Animator buttAnim = GameObject.Find("Item" + currentItem).GetComponent<Animator>();
                if (!buttAnim.GetCurrentAnimatorStateInfo(0).IsName("Activated"))
                {
                    audioSource.Play();
                    GameObject newItem = (GameObject)Instantiate(Resources.Load("items/Item" + currentItem), transform.position, transform.rotation);
                    newItem.GetComponent<ItemScript>().speed *= shootSpeed;
                    anim.SetFloat("Speed", shootSpeed);
                    anim.SetTrigger("Atk");
                    buttAnim.SetTrigger("Activated");
                    buttAnim.GetComponent<ButtonScript>().SetTimer();
                    shootSpeed = .8f;

                    int i = 0; while (++i != 5)
                    {
                        buttAnim = GameObject.Find("Item" + ((i + currentItem) % 5)).GetComponent<Animator>();
                        if (!buttAnim.GetCurrentAnimatorStateInfo(0).IsName("Activated"))
                            break;
                    }
                    buttAnim.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
                    if (i == 5)
                    {
                        anim.SetBool("Cooled", false);
                        cooling = true;
                    }
                }
                else
                    audioSource.PlayOneShot(error);
            }
        }
        transform.position += new Vector3(speed,- .05f);
        camer.position += new Vector3(0,0,.02f);

        Vector3 cursorScreenPos = Camera.main.WorldToScreenPoint(cursor.position);
        if (cursorScreenPos.x > Screen.width-10)
            cursor.position -= new Vector3(.02f, 0);
        else if (cursorScreenPos.x < 10)
            cursor.position += new Vector3(.02f, 0);
        if (stars == 3 && Camera.main.WorldToScreenPoint(transform.position).y < 675)
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
    }

    public void SetDrag()
    {
        drag = true;
        if(!cooling)
            anim.SetBool("Charge", true);
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
            GameObject.Find("Item"+currentItem).GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        }
        
    }

    public void ShakeCall(float magnitude)
    {
        StartCoroutine(Shake(magnitude));

    }

    IEnumerator Shake(float magnitude)
    {
        float camY = camer.position.y;

        // map value to [-1, 1]
        while (magnitude > 0)
        {
            float x = Random.Range(-magnitude*1.6f, magnitude*1.6f);
            float y = Random.Range(0, magnitude);

            camer.position += new Vector3(x, y);

            yield return new WaitForSeconds(.04f);
            camer.position = new Vector3(0, camY, camer.position.z);
            magnitude -= .3f;
        }
    }
}
