// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class UnloadLevel : FsmStateAction
    {
        [RequiredField]
        public FsmString levelName;

        public override void Reset()
        {
            levelName = "";
        }

        public override void OnEnter()
        {
            SceneManager.UnloadSceneAsync(levelName.Value);

            Finish();
        }

    }
}