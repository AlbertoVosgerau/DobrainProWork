using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;

namespace Dobrain.contents
{
    public class MissionManager : MonoBehaviour
    {
        public SimpleSQLManager sqlManager;

        // Use this for initialization
        void Start()
        {
            sqlManager = GetComponent<SimpleSQLManager>();
        }

        public void Record(string date, int chap, int index, string type, float score)
        {
            sqlManager.BeginTransaction();
            sqlManager.Execute("INSERT INTO QuestionRecord VALUES (?,?,?,?)",date,chap+"_"+index,type,score);
            sqlManager.Commit();
        }
    }
}