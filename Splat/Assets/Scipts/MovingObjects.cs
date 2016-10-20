using UnityEngine;

public class MovingObjects : MonoBehaviour {
    public enum Type {
        delete,
        wrapping,
        turning
    }
    public Type whatHappensAtEdge;
    public Vector3 dir;
    // Update is called once per frame
    protected virtual void FixedUpdate () {
        transform.position += dir;
        float width = Screen.width * .5f;
        if ( Mathf.Abs ( Camera.main.WorldToScreenPoint ( transform.position ).x - width ) - width > 21 ) {
            switch ( whatHappensAtEdge ) {
                case Type.wrapping:
                    transform.position = new Vector3 ( -transform.position.x, transform.position.y, transform.position.z );
                    transform.position += dir;
                    break;
                case Type.turning:
                    dir *= -1;
                    transform.position += dir;
                    transform.localScale = new Vector3 ( transform.localScale.x * -1, transform.localScale.y, transform.localScale.z );
                    break;
                case Type.delete:
                    Die();
                    break;
            }
        }
    }

    public virtual void Die () {
        Destroy ( gameObject );
    }
}
