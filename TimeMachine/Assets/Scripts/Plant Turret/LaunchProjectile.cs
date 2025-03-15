using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour, IRewindable
{
    public GameObject projectile;
    private Animator anim;
    private float timeElapsed = 0;
    private bool dir;
    public int direction;

    private LinkedList<float> firingHistory = new LinkedList<float>();

    void Start()
    {
        dir = GetComponent<SpriteRenderer>().flipX;
        anim = gameObject.transform.parent.GetComponent<Animator>();
        if(dir) direction = 1;
        else direction = -1;
    }
    void Update()
    {
        timeElapsed += Time.deltaTime;
        //Debug.Log(timeElapsed);
        if(timeElapsed >= 2){ //&& !Input.GetKey(KeyCode.Q)){
            anim.SetBool("isAttacking", true);
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if(stateInfo.IsName("Attack") && stateInfo.normalizedTime >= .4){
                GameObject shot = Instantiate(projectile, transform.position, transform.rotation);
                shot.GetComponent<MoveProjectile>().dir = direction;
                timeElapsed = 0;
                shot.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            }
        } else{
            anim.SetBool("isAttacking", false);
        }
    }

    void IRewindable.SaveState()
    {
        firingHistory.AddLast(timeElapsed);
    }

    void IRewindable.RewindState()
    {
        timeElapsed = firingHistory.Last.Value;
        firingHistory.RemoveLast();
    }

    void IRewindable.ClearHistory()
    {
        firingHistory.Clear();
    }
}
