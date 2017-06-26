using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dobrain.contents.weekend
{
    public class Wave : MonoBehaviour
    {
        public float speed = 0.5f;
        public RectTransform thisT;
        public int siblingindex;

        // Use this for initialization
        void Start()
        {
            thisT = GetComponent<RectTransform>();
            //Debug.Log(transform.GetSiblingIndex());
            //transform.SetSiblingIndex(siblingindex);
        }

        // Update is called once per frame
        void Update()
        {
            if (thisT.anchoredPosition.x <= -4000)
                thisT.anchoredPosition = new Vector2(4000, thisT.anchoredPosition.y);
            else
                thisT.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }
    }
}