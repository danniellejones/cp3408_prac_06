using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK, SLEEP
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected NavMeshAgent agent;

    // State that gets to run after the current running STATE
    protected State nextState;

    // When player is within distance, then npc should be able to see with angle and distance to attack
    float visDist = 10.0f;
    float visAngle = 30.0f;
    float shootDist = 7.0f;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }

    // Skeleton code for each of the different phases of a state
    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    // Progress state through different stages
    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;  // Keep returning same state
    }


}

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        // Trigger idle animation
        anim.SetTrigger("isIdle");

        base.Enter();
    }

    public override void Update()
    {
        // Transition to next state, leave update loop
        if (Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        base.Update();
    }

    public override void Exit()
    {
        // If animation does not run, then it can cause problems when triggered - reset stops this
        anim.ResetTrigger("isIdle");

        base.Exit();
    }
}

public class Patrol : State
{
    // Index to count through waypoints
    int currentIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;  // Nav mesh agent property if on path
        agent.isStopped = false;  // Change if needs to stop on a path
    }

    public override void Enter()
    {
        currentIndex = 0;
        // Get list of waypoints
        anim.SetTrigger("isWalking");

        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            // Reach end of waypoints and switch back to start
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;

            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }
        base.Enter();
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Enter();
    }

}
