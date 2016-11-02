using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK; 

public class BrandonsBehaviorTree : MonoBehaviour {


    public static int parSize;
    public static int wanderSize;
    public static int leaveSize;
    public GameObject[] participant = new GameObject[parSize];
    public Transform[] wander = new Transform[wanderSize];
    public Transform[] leave = new Transform[leaveSize];
    public InteractionObject obj;
    public GameObject ball;
    


    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected Node ST_ApproachAndWait(GameObject agent, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(agent.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
    {
        int pickUp = 0;//UnityEngine.Random.Range(0, 2); 
        Node roaming = new Sequence(
                        //this.ST_Enter(),
                        this.ST_Greetings(),
                        this.ST_PickUpBall(participant[pickUp]),
                        new LeafWait(2000),
                        this.ST_Leave(pickUp)
                        );
        return roaming;
    }

    protected Node ST_Enter()
    {
        Node enter = new SequenceParallel(
                this.ST_ApproachAndWait(participant[0], wander[0]),
                this.ST_ApproachAndWait(participant[1], wander[1]),
                this.ST_ApproachAndWait(participant[2], wander[2])
            );

        return enter; 
    }

    protected Node ST_Leave(int not)
    {
        Node enter;
        switch (not)
        {
            case 0:
                enter = new SequenceParallel(
                    this.ST_ApproachAndWait(participant[1], leave[1]),
                    this.ST_ApproachAndWait(participant[2], leave[2]),
                    new LeafWait(5000),
                    this.ST_End()
                );
                break;
            case 1:
                enter = new SequenceParallel(
                    this.ST_ApproachAndWait(participant[0], leave[0]),
                    this.ST_ApproachAndWait(participant[2], leave[2]),
                    new LeafWait(5000),
                    this.ST_End()
                );
                break;
            case 2:
                enter = new SequenceParallel(
                    this.ST_ApproachAndWait(participant[0], leave[0]),
                    this.ST_ApproachAndWait(participant[1], leave[1]),
                    new LeafWait(5000),
                    this.ST_End()
                );
                break;
            default:
                enter = new Sequence(); 
                break; 
        }

        return enter;
    }

    protected Node ST_End()
    {
        Application.Quit(); 
        return null; 
    }

    protected Node ST_Greetings()
    {
        Node greetings = new SequenceShuffle(
                    this.ST_LookAtAndWave(participant[0], participant[1]),
                    this.ST_LookAtAndWave(participant[0], participant[2]),
                    this.ST_LookAtAndWave(participant[1], participant[2])
                                 
            );
        return greetings; 
    }

    protected Node ST_LookAtAndWave(GameObject agent_1, GameObject agent_2)
    {
        Vector3 p1 = agent_1.transform.position;
        Vector3 p2 = agent_2.transform.position;

        return
            new Sequence(agent_1.GetComponent<BehaviorMecanim>().ST_TurnToFace(p2),
                         agent_2.GetComponent<BehaviorMecanim>().ST_TurnToFace(p1),
                         new LeafWait(1000),
                         agent_1.GetComponent<BehaviorMecanim>().Node_HandAnimation("WAVE", true),
                         agent_2.GetComponent<BehaviorMecanim>().Node_HandAnimation("WAVE", true),
                         new LeafWait(2000),
                         agent_1.GetComponent<BehaviorMecanim>().Node_HandAnimation("WAVE", false),
                         agent_2.GetComponent<BehaviorMecanim>().Node_HandAnimation("WAVE", false)
                         );
    }


    protected Node ST_PickUpBall(GameObject agent)
    {
        FullBodyBipedEffector hand = FullBodyBipedEffector.RightHand; 
        Val<Vector3> ballPosition = Val.V(() => ball.transform.position);
        Node pickUpBall = new Sequence(
                agent.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(ballPosition, 5f),
                agent.GetComponent<BehaviorMecanim>().Node_StartInteraction(hand, obj),
                new LeafWait(1000)
            );
        return pickUpBall; 
    }

    public Node ST_TurnThrowAndCatch(GameObject thrower, GameObject ball, GameObject catcher)
    { 
        return null; 
    }

    public Node ST_Play()
    {
        return null; 
    }
}
