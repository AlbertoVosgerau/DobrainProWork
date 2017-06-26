// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;

using Com.Dobrain.Dobrainproject.Manager;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class GetResourceManager : FsmStateAction
    {
        [RequiredField]
        [ObjectTypeAttribute(typeof(ResourceManager))]
        public FsmObject storeLMSManager;

        public override void Reset()
        {
            storeLMSManager = null;
        }

        public override void OnEnter()
        {
            storeLMSManager.Value = ResourceManager.instance;

            Finish();
        }

    }
}