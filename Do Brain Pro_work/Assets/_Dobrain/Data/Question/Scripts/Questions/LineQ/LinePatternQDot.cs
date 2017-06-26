using UnityEngine;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    // TODO: Add IButtonSoundHandler interface to LinePatternQDot class.
    public class LinePatternQDot : MonoBehaviour
    {
        private RectTransform selectedCover;
        private bool selected = false;
        private bool fix = false;
        private int dotId;
        private LinePatternQManager manager;
        public bool Fix
        {
            set { fix = value; }
            get { return fix; }
        }
        public int DotId
        {
            get { return dotId; }
        }
        public bool Selected
        {
            get { return selected; }
        }
        public void ResetSelected()
        {
            selected = false;
        }
        public void SelectDot()
        {
            selected = true;
        }
        void Start()
        {
            selectedCover = transform.GetChild(0).GetComponent<RectTransform>();
            dotId = GetDotIndex(gameObject.name);
        }

        void Update()
        {
            selectedCover.gameObject.SetActive(selected || fix);
        }

        void OnMouseOver()
        {
            manager.IsIn = dotId;
        }
        void OnMouseExit()
        {
            manager.IsIn = -1;
        }
        public void SetManager(LinePatternQManager manager)
        {
            this.manager = manager;
        }
        int GetDotIndex(string dotName)
        {
            string[] dotNameSplit = dotName.Split(' ');
            int ascii_num = dotNameSplit[1].ToCharArray()[0];
            int ret = 0;
            if (ascii_num > 64)
                ret = ascii_num - 55;
            else
                ret = int.Parse(dotNameSplit[1]);
            return ret;
        }
    }
}