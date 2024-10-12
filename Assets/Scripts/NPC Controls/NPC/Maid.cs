using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using UnityEngine.AI;
public class Maid : MonoBehaviour, IGoap
{
    public GameObject duster;
    public float moveSpeed;
    private FurnitureComponent[] furniture;
    private PlantComponent[] plants;
    private float dirtyFurnCount = 0;
    private float thirstyPlants;
    public Animator maid_anim;
    private List<HashSet<KeyValuePair<string, object>>> goals;
    private int currentGoalIndex;
    private Rigidbody2D maidRB;
    public bool isWalking;

    void Start()
    {
        moveSpeed = 1f;
        maid_anim = GetComponent<Animator>();
        maidRB = GetComponent<Rigidbody2D>();
        goals = new List<HashSet<KeyValuePair<string, object>>>();
        HashSet<KeyValuePair<string, object>> firstGoal = new HashSet<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("dirtyFurnitureCount", false)
        };
        HashSet<KeyValuePair<string, object>> secondGoal = new HashSet<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("plantThirsty", false)
        };
        goals.Add(firstGoal);
        goals.Add(secondGoal);
        currentGoalIndex = 0;
    }

    void Update()
    {

    }

    public HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData =  new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("hasDuster", (duster != null)));
        worldData.Add(new KeyValuePair<string, object>("hasWaterCan", true));
        worldData.Add(new KeyValuePair<string, object>("dirtyFurnitureCount", GetDirtyFurnitureCount()>0));
        worldData.Add(new KeyValuePair<string, object>("plantThirsty", GetThirstyPlantsCount() > 0));
        Debug.Log(DebugHashSet(worldData));
        return worldData;
    }

    public HashSet<KeyValuePair<string, object>> createGoalState()
    {
        if (currentGoalIndex < goals.Count)
        {
            HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> (goals[currentGoalIndex]);
            return goal;
        }

        return null;

        //goal.Add(new KeyValuePair<string, object>("dirtyFurnitureCount", false));
        //goal.Add(new KeyValuePair<string, object>("plantThirsty", false));
    }
    public void planFailed (HashSet<KeyValuePair<string, object>> failedGoal)
    {
        
    }

    public void planFound (HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
        // Yay we found a plan for our goal
        Debug.Log ("<color=green>Plan found</color> "+GoapAgent.prettyPrint(actions));
    }
    public void actionsFinished ()
    {
        // Everything is done, we completed our actions for this gool. Hooray!
        Debug.Log ("<color=blue>Actions completed</color>");
        if (GetDirtyFurnitureCount() == 0)
        {
            currentGoalIndex++;
        }
    }

    public void planAborted (GoapAction aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.
        //Debug.Log ("<color=red>Plan Aborted</color> "+GoapAgent.prettyPrint(aborter));
    }
    public bool moveAgent(GoapAction nextAction)
    {
        isWalking = true;
        maid_anim.SetBool("isWalking",isWalking);
        Vector2 destination = nextAction.target.transform.position;
        if (this.transform.position.x < destination.x-2)
        {
            maid_anim.SetFloat("moveY",0);
            maid_anim.SetFloat("moveX",1);
            maidRB.velocity = new Vector2(moveSpeed, 0);
            
        }
        else if (this.transform.position.x > destination.x+2)
        {
            maid_anim.SetFloat("moveY",0);
            maid_anim.SetFloat("moveX",-1);
            maidRB.velocity = new Vector2(-moveSpeed, 0);
            
        }
        if (this.transform.position.y < destination.y-2)
        {
            maid_anim.SetFloat("moveX",0);
            maid_anim.SetFloat("moveY",1);
            maidRB.velocity = new Vector2(0,moveSpeed);
            
        }
        else if (this.transform.position.y > destination.y+2)
        {
            maid_anim.SetFloat("moveX",0);
            maid_anim.SetFloat("moveY",-1);
            maidRB.velocity = new Vector2(0,-moveSpeed);
            
        }

        float distance = Vector2.Distance(gameObject.transform.position, nextAction.target.transform.position);
        if (distance<=3f)
        {
            isWalking = false;
            maid_anim.SetBool("isWalking",isWalking);
            maidRB.velocity = new Vector2(0,0);
            // we are at the target location, we are done
            nextAction.setInRange(true);
            return true;
        } else
            return false;
    }

    private float GetDirtyFurnitureCount()
    {
        furniture = ((FurnitureComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(FurnitureComponent)))
            .Where(f => f.dirty)
            .ToArray();
        dirtyFurnCount = furniture.Length;
        return dirtyFurnCount;
    }
    string DebugHashSet(HashSet<KeyValuePair<string, object>> hashSet)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("HashSet contents:");
        foreach (var kvp in hashSet)
        {
            sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
        }

        return sb.ToString();
    }
    private float GetThirstyPlantsCount()
    {
        plants = ((PlantComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(PlantComponent)))
            .Where(f => f.thirsty)
            .ToArray();
        thirstyPlants = plants.Length;
        return thirstyPlants;
    }
}
