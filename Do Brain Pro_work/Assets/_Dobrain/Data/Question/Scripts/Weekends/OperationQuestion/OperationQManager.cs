using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationQManager : MonoBehaviour {

    public string level = "A";

    public List<OperationQUnit> targetUnitList;
    public List<OperationQUnit> triggerUnitList;

    bool isHit;
    OperationQUnit hitTrigger;
    OperationQUnit hitTarget;


	void Start () 
    {
        for(int i = 0 ; i < triggerUnitList.Count ; i++)
        {
            triggerUnitList[i].OnHitEnter += Trigger_OnHitEnter;
            triggerUnitList[i].OnHitExit += Trigger_OnHitExit;
        }
	}

    void Update()
    {
        if(isHit && Input.GetMouseButtonUp(0))
        {
            int indexOfTargetUnit = targetUnitList.IndexOf(hitTarget);
            int indexOfTriggerUnit = triggerUnitList.IndexOf(hitTrigger);
            if(indexOfTargetUnit.Equals(indexOfTriggerUnit))
            {
                hitTrigger.OnHitEnter += Trigger_OnHitEnter;
                hitTrigger.OnHitExit += Trigger_OnHitExit;
                hitTrigger.enabled = false;
            }
            else
            {
                hitTrigger.ReturnToStartPosition();
            }
        }
    }

    void Trigger_OnHitEnter(OperationQUnit trigger, OperationQUnit target)
    {
        isHit = true;
        hitTrigger = trigger;
        hitTarget = target;
    }

    void Trigger_OnHitExit(OperationQUnit trigger, OperationQUnit target)
    {
        isHit = false;
    }

}
