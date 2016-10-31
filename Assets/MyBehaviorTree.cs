using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree : MonoBehaviour
{
    public GameObject[] movePoints; 

    public GameObject[] participants;

    public GameObject ball; 


	private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	protected Node ST_ApproachAndWait(Transform target, GameObject agent)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( agent.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

    protected Node ST_ApproachAndPickupBall()
    {
        Val<Vector3> position = Val.V(() => ball.transform.position);
        return new Sequence(participant1.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }


    protected Node ST_ThrowAndCatch()
    {
        Val<Vector3> position = Val.V(() => ball.transform.position);
        return new Sequence(participant1.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));

    }

    protected Node ST_StartleAndRun()
    {
        Val<Vector3> position = Val.V(() => ball.transform.position);
        return new Sequence(participant1.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));

    }

    protected Node BuildTreeRoot()
    {
        Node roaming = new DecoratorLoop(
            this.ST_ApproachAndWait(this.wander1.transform);
        return roaming; 
    }

    /*
    protected Node BuildTreeRoot()
	{
		Node roaming = new DecoratorLoop (
						new SequenceShuffle(
						this.ST_ApproachAndWait(this.wander1.transform),
						this.ST_ApproachAndWait(this.wander2.transform),
						this.ST_ApproachAndWait(this.wander3.transform)));
		return roaming;
	}
    */
}
