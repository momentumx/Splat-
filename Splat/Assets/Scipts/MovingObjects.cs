using UnityEngine;

public class MovingObjects : MonoBehaviour {
    public enum Type {
        delete,
        wrapping,
        turning
    }
    public Type whatHappensAtEdge;
    public Vector3 dir;
    public float myWidth;
    void Awake () {
        myWidth = GetComponentInChildren<SpriteRenderer> ().bounds.extents.x;
    }
    // Update is called once per frame
    protected virtual void FixedUpdate () {
        if ( !GameController.intro ) {
            transform.position += dir;
            float width = Screen.width * .5f;
            switch ( whatHappensAtEdge ) {
                case Type.wrapping:
                    if ( Mathf.Abs ( Camera.main.WorldToScreenPoint ( transform.position ).x - width ) - width > myWidth * 2 ) {
                        transform.position = new Vector3 ( -transform.position.x, transform.position.y, transform.position.z );
                        transform.position += dir;
                    }
                    break;
                case Type.turning:
                    if ( Mathf.Abs ( Camera.main.WorldToScreenPoint ( transform.position ).x - width ) - width > -myWidth ) {
                        dir *= -1;
                        transform.position += dir;
                        transform.localScale = new Vector3 ( transform.localScale.x * -1, transform.localScale.y, transform.localScale.z );
                    }
                    break;
                case Type.delete:
                    if ( Mathf.Abs ( Camera.main.WorldToScreenPoint ( transform.position ).x - width ) - width > 100 ) {
                        Die ();
                    }
                    break;
            }
        }
    }

    public virtual void Die () {
        Destroy ( gameObject );
    }
}
