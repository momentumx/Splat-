using UnityEngine;

public class ButtonScript : MonoBehaviour {

    float coolDown, timer;
    byte item;
    bool unlocked = true;
    UnityEngine.UI.Text text;
	// Use this for initialization
	void Awake () {
        item = byte.Parse ( System.Text.RegularExpressions.Regex.Match ( gameObject.name, @"\d+" ).Value );
        //this will change when i actually have the items sprtie sheet
        GetComponent<UnityEngine.UI.Image> ().sprite = Resources.LoadAll<Sprite>("spriteSheet") [ GameController.sprites [ GameController.indexes [ item] ] ];
        if(item > GameController.level + 1 ) {
            unlocked = false;
            Color color = GetComponent<UnityEngine.UI.Image> ().color;
            color.a = 0f;
            GetComponent<UnityEngine.UI.Image> ().color = color;
            GetComponent<Animator> ().enabled = false;
            GetComponent<UnityEngine.EventSystems.EventTrigger> ().enabled = false;
        }
        coolDown = GameController.coolDowns [ GameController.indexes [ item ] ];
        text = transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
	}

    void FixedUpdate()
    {
        if (timer != 0)
        {
            timer -= Time.fixedDeltaTime;
            text.text = timer.ToString("F2");
            if (timer<=0)
            {
                timer = 0;
                GetComponent<Animator>().SetTrigger("Cooled");
                GameController.Refresh(item);
            }
        }
        else
            text.text = "";
    }

    void Update()
    {
        if ( Input.GetButtonDown ( "Select" + item ) && unlocked )
            GetComponent<UnityEngine.EventSystems.EventTrigger> ().OnPointerClick ( new UnityEngine.EventSystems.PointerEventData ( UnityEngine.EventSystems.EventSystem.current ) );
    }
    public void SetTimer () {
        timer = coolDown;
    }
}
