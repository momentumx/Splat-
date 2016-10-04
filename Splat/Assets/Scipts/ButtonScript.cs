using UnityEngine;
using System.Text.RegularExpressions;

public class ButtonScript : MonoBehaviour {

    public float coolDown, timer;
    public byte item;
    UnityEngine.UI.Text text;
	// Use this for initialization
	void Start () {
        text = transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        Sprite[] mysprite = Resources.LoadAll<Sprite>("spriteSheet");
        GetComponent<UnityEngine.UI.Image>().sprite = mysprite[GameController.items[int.Parse(Regex.Match(gameObject.name, @"\d+").Value)]];
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
        if (Input.GetButtonDown("Select" + item))
            GetComponent<UnityEngine.EventSystems.EventTrigger>().OnPointerClick(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current));
    }
    public void SetTimer () {
        timer = coolDown;
    }
}
