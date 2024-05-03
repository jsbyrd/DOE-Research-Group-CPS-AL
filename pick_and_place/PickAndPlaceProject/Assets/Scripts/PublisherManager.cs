using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublisherManager : MonoBehaviour
{
    public GameObject[] robots;
    public GameObject target;
    public GameObject[] targetPlacements;
    public GameObject endPlacement;
    public GameObject ROSLockObject;
    private ROSLock rosLock;
    NodeGraph graph;
    InstructionList instructionList;

    // Start is called before the first frame update
    void Start()
    {
        rosLock = ROSLockObject.GetComponent<ROSLock>();
        graph = new NodeGraph(targetPlacements, target, endPlacement, robots);
        this.instructionList = graph.Dijkstra(0);
    }

    public void StartPublishing() {
        StartCoroutine(ExecuteInstructions());
    }

    private IEnumerator ExecuteInstructions() {
        Instruction instruction = instructionList.head;
        while (instruction != null) {
            // First, make arm move target to endPlacement
            rosLock.Lock();
            instruction.ExecuteInstruction();
            yield return new WaitUntil(() => !rosLock.IsLocked()); // Wait for arm to move target completely
            // Move on to the next instruction
            instruction = instruction.next;
        }
        // We are done!
        Debug.Log("Done!");
    }
}
