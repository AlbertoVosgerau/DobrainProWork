// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;

using Com.Dobrain.Dobrainproject.Manager;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class GetContentManager : FsmStateAction
    {
        [RequiredField]
        [ObjectTypeAttribute(typeof(ContentManager))]
        public FsmObject storeContentManager;

        public override void Reset()
        {
            storeContentManager = null;
        }

        public override void OnEnter()
        {
            storeContentManager.Value = ContentManager.instance;

            Finish();
        }

    }
}