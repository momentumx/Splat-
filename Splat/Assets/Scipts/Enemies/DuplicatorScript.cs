using UnityEngine;

public class DuplicatorScript : EnemyScript {

    protected override void Start () {
        ++GameController.minions;
    }

    protected override void FixedUpdate () {
        if (GameController.minions < 20 && 1 == Random.Range ( 0, 800 ) ) {
            ((GameObject)Instantiate ( gameObject, transform.position, transform.rotation )).GetComponent<DuplicatorScript>().dir = dir;
            SetSpeed0 ();
            GetComponent<Animator> ().SetTrigger ( "Extra" );
        }
            base.FixedUpdate ();
    }
}
