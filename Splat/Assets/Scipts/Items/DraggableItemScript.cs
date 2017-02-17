using UnityEngine;

public class DraggableItemScript : MonoBehaviour {
    bool drag = false, set = false, unlocked = true;
    byte index, setIndex;
    Vector2 pos, returnPos;
	// Use this for initialization
	void Start () {
        index = byte.Parse ( System.Text.RegularExpressions.Regex.Match ( transform.name, @"\d+" ).Value );
        if ( index > GameController.stars / 6 + 1 ) {
            unlocked = false;
            Color color = GetComponent<UnityEngine.UI.Image> ().color;
            color.a = .2f;
            GetComponent<UnityEngine.UI.Image> ().color = color;
        }
        returnPos = transform.position;
        if ( unlocked ) {
            sbyte i =-1; while ( ++i != 5 )
                if ( index == GameController.indexes [ i ] ) {
                    pos = ( GameObject.Find ( "ItemPick" + i ).transform.position );
                    setIndex = ( byte )i;
                    set = true;
                    transform.position = pos;
                    return;
                }
        }
        pos = returnPos;
    }

    void FixedUpdate () {
        if ( drag ) {
            Vector3 poss = Input.mousePosition;
            poss.z = 2;
            transform.position = poss;
        }
    }

    public void StartDrag () {
        //this makes the item appear on top of everything else
        if ( unlocked ) {
            transform.SetSiblingIndex ( transform.parent.childCount - 1 );
            drag = true;
        }
    }

    public void StopDrag () {
        drag = false;
        Collider2D coll= Physics2D.OverlapCircle ( transform.position, 40f );
        if ( coll ) {
            DraggableItemScript draggable = coll.GetComponent<DraggableItemScript> ();
            if ( draggable.unlocked ) {
                if ( draggable.set ) {
                    if ( set ) {
                        setIndex ^= draggable.setIndex;
                        draggable.setIndex ^= setIndex;
                        setIndex ^= draggable.setIndex;
                        GameController.indexes [ setIndex ] = index;
                        GameController.indexes [ draggable.setIndex ] = draggable.index;
                        Vector2 temp = pos;
                        pos = draggable.pos;
                        draggable.pos = temp;
                        coll.transform.position = temp;
                    } else {
                        draggable.set = false;
                        set = true;
                        setIndex = draggable.setIndex;
                        GameController.indexes [ setIndex ] = index;
                        pos = draggable.pos;
                        draggable.pos = draggable.returnPos;
                        coll.transform.position = draggable.pos;
                    }
                } else if ( set ) {
                    set = false;
                    draggable.set = true;
                    draggable.setIndex = setIndex;
                    GameController.indexes [ draggable.setIndex ] = draggable.index;
                    draggable.pos = pos;
                    pos = returnPos;
                    coll.transform.position = draggable.pos;
                }
            }
        }
        transform.position = pos;
    }
}