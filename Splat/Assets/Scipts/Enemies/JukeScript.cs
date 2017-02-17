using UnityEngine;

public class JukeScript : EnemyScript {
	// Use this for initialization

    protected override void FixedUpdate () {
        if ( 1 == Random.Range ( 0, 60 ) )
            dir.x *= -1;
        base.FixedUpdate ();
    }
}
