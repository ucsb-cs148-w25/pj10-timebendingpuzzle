using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour, IRewindable
{
    public GameObject projectile;
    private Animator anim;
    private float timeElapsed = 0;
    private bool dir;
    public int direction;
    private SPM playerMovement;
    public GameObject activeProjectile = null;


    // Item1: timeElapsed
    // Item2: active projectile instance ID (-1 if none)
    // Item3: animator state fullPathHash
    // Item4: animator normalized time
    private LinkedList<Tuple<float, int, int, float>> firingHistory = new LinkedList<Tuple<float, int, int, float>>(); // contains lifetime and creation history of object

    void Start()
    {
        dir = GetComponent<SpriteRenderer>().flipX;
        anim = gameObject.transform.parent.GetComponent<Animator>();
        playerMovement = GameObject.Find("Player").GetComponent<SPM>();
        if(dir) direction = 1;
        else direction = -1;
    }
    void Update()
    {
        timeElapsed += Time.deltaTime;
        //Debug.Log(timeElapsed);
        if(timeElapsed >= 2 && activeProjectile == null){ //&& !Input.GetKey(KeyCode.Q)){
            anim.SetBool("isAttacking", true);
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if(stateInfo.IsName("Attack") && stateInfo.normalizedTime >= .4){ // BUG: if I bounce on projectile, and then rewind
                                                                                // while plant is midanimation for projectile,
                                                                                // it spawns several projectiles
                Debug.Log("FIRE" + timeElapsed);
                //isCreated = true;
                GameObject shot = Instantiate(projectile, transform.position, transform.rotation, transform);
                shot.GetComponent<MoveProjectile>().dir = direction;
                shot.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV();
                shot.GetComponent<MoveProjectile>().playerMovement = playerMovement;
                activeProjectile = shot;
                timeElapsed = 0;
            }
        } else{
            anim.SetBool("isAttacking", false);
        }
    }

    void IRewindable.SaveState()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        int activeProjectileID = activeProjectile != null ? activeProjectile.GetInstanceID() : -1;
        firingHistory.AddLast(new Tuple<float, int, int, float>(timeElapsed, activeProjectileID, stateInfo.fullPathHash, stateInfo.normalizedTime));
    }

    void IRewindable.RewindState()
    {
        var state = firingHistory.Last.Value;
        
        timeElapsed = state.Item1;
        //isCreated = state.Item2;
        int savedProjectileID = state.Item2;
        int animHash = state.Item3;
        float normalizedTime = state.Item4;
        anim.Play(animHash, 0, normalizedTime);
        firingHistory.RemoveLast();

        activeProjectile = null;
        if (savedProjectileID != -1)
        {
            foreach (Transform child in transform)
            {
                if(child.gameObject.GetInstanceID() == savedProjectileID)
                {
                    activeProjectile = child.gameObject;
                    break;
                }
            }
        }
    }

    void IRewindable.ClearHistory()
    {
        firingHistory.Clear();
    }
}
