// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    [Tooltip("Finds the Child of a GameObject by Name.\nNote, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger. If you need to specify a tag, use GetChild.")]
    public class FindGameObjectOfType : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [RequiredField]
        public FsmObject type;

        [UIHint(UIHint.Variable)]
        [RequiredField]
        public FsmObject storeComponent;

        public override void Reset()
        {
            type = null;
            storeComponent = null;   
        }

        public override void OnEnter()
        {
            storeComponent.Value = GameObject.FindObjectOfType(type.ObjectType);

            Finish();
        }
    }
}