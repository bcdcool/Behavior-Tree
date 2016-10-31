using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree : MonoBehaviour
{
    
    public static int movePointsSize;
    public GameObject[] movePoints = new GameObject[movePointsSize];

    public static int participantSize;  
    public GameObject[] participants = new GameObject[movePointsSize];

    

    public GameObject ball; 


	private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
        movePoints = new GameObject[movePointsSize];
        participants = new GameObject[participantSize];

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
        return new Sequence(participants[0].GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }


    protected Node ST_ThrowAndCatch(GameObject thrower, GameObject catcher)
    {

        Val<Vector3> throwPosition = Val.V(() => catcher.transform.position);
        return new Sequence(participants[0].GetComponent<BehaviorMecanim>().Node_GoTo(throwPosition), new LeafWait(1000));

    }

    protected Node ST_StartleAndRun()
    {
        Val<Vector3> position = Val.V(() => ball.transform.position);
        return new Sequence(participants[0].GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));

    }

    protected Node BuildTreeRoot()
    {
        Node roaming = new DecoratorLoop(
            this.ST_ApproachAndWait(this.movePoints[0].transform, participants[0]));
        return roaming; 
    }

    /*
    protected Node BuildTreeRoot()
	{
		Node roaming = new DecoratorLoop (
						new SequenceShuffle(s
						this.ST_ApproachAndWait(this.wander1.transform),
						this.ST_ApproachAndWait(this.wander2.transform),
						this.ST_ApproachAndWait(this.wander3.transform)));
		return roaming;
	}
    */
}
