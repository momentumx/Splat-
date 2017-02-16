using UnityEngine;

public class MovingObjects : MonoBehaviour {
    public enum Type {
        delete,
        wrapping,
        turning
    }
	[SerializeField]
	Type whatHappensAtEdge;
    public Vector3 dir;
    [HideInInspector]
    public float myWidth;
    void Awake () {
        myWidth = GetComponentInChildren<SpriteRenderer> ().bounds.extents.x;
    }

    public void SwitchDirections () {
        dir.x *= -1;
        transform.localScale = new Vector3 ( transform.localScale.x * -1, transform.localScale.y, transform.localScale.z );
    }

    protected virtual void FixedUpdate () {
        if (GameController.playing) {
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
                        SwitchDirections ();
                        transform.position += dir;
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

    protected void RandomizePosition (float _yDis) {
        Vector3 randScreenPos = Camera.main.WorldToScreenPoint( transform.position);
        randScreenPos.x = Random.Range ( myWidth, Screen.width - myWidth );
        randScreenPos.y -= _yDis + Random.Range ( 0f, 2f * _yDis );
		transform.position = Camera.main.ScreenToWorldPoint ( randScreenPos );
    }
}
