using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.Dobrain.Dobrainproject.Manager
{
    public class LMSManager : MonoBehaviour {

        public static LMSManager instance;

        public class ContentData
        {
            public string date;
            public DayOfWeek dayOfWeek;
            public string type;
            public int uno;
            public Def.ContentState state;
            public int currentContentIndex = 0;
            public int weekendQuestionPoint = 0;

            public Dictionary<string, object> ToDictionary()
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("date", date);
                result.Add("dayOfWeek", (int)dayOfWeek);
                result.Add("type", type);
                result.Add("uno", uno);
                result.Add("state", (int)state);
                result.Add("currentContentIndex", currentContentIndex);
                result.Add("weekendQuestionPoint", weekendQuestionPoint);
                return result;
            }

            public static ContentData DictionaryToContentData(Dictionary<string, object> data)
            {
                ContentData result = new ContentData();
                result.date = data["date"].ToString();
                result.dayOfWeek = (DayOfWeek)int.Parse(data["dayOfWeek"].ToString());
                result.type = data["type"].ToString();
                result.uno = int.Parse(data["uno"].ToString());
                result.state = (Def.ContentState)int.Parse(data["state"].ToString());
                result.currentContentIndex = int.Parse(data["currentContentIndex"].ToString());
                result.weekendQuestionPoint = int.Parse(data["weekendQuestionPoint"].ToString());
                return result;
            }
        }

        void Awake()
        {
            instance = this;
        }



        public bool profileInited { 
            set { PlayerPrefs.SetInt("lms_profile_inited", Convert.ToInt16(value)); }
            get { return Convert.ToBoolean(PlayerPrefs.GetInt("lms_profile_inited")); } 
        }

        public string profileName {
            set { PlayerPrefs.SetString("lms_profile_name", value); }
            get { return PlayerPrefs.GetString("lms_profile_name"); }
        }

        public string profileLevel {
            set { PlayerPrefs.SetString("lms_profile_level", value); }
            get { return PlayerPrefs.GetString("lms_profile_level"); }
        }

        public DateTime startDate
        {
            set { 
                string date = value.ToString();
                PlayerPrefs.SetString("lms_start_date", date); 
            }
            get {
                return Convert.ToDateTime(PlayerPrefs.GetString("lms_start_date"));
            }
        }

        public int currentContentNo
        {
            set{ PlayerPrefs.SetInt("lms_current_content_no", value); }
            get{ return PlayerPrefs.GetInt("lms_current_content_no"); }
        }

        public int lastContentNo {
            set{ PlayerPrefs.SetInt("lms_last_content_no", value); }
            get{ return PlayerPrefs.GetInt("lms_last_content_no"); }
        }

        public int lastWeekdayContentNo {
            set{ PlayerPrefs.SetInt("lms_last_weekday_content_no", value); }
            get{ return PlayerPrefs.GetInt("lms_last_weekday_content_no"); }
        }

        public int lastWeekendContentNo {
            set{ PlayerPrefs.SetInt("lms_last_weekend_content_no", value); }
            get{ return PlayerPrefs.GetInt("lms_last_weekend_content_no"); }
        }

        public int contentLimitNum{
            set{ PlayerPrefs.SetInt("lms_content_limit_num", value); }
            get{ return PlayerPrefs.GetInt("lms_content_limit_num"); }
        }

        public bool inited
        {
            set{ PlayerPrefs.SetInt("lms_content_data_inited", Convert.ToInt32(value)); }
            get{ return Convert.ToBoolean(PlayerPrefs.GetInt("lms_content_data_inited")); }
        }

        public void Init()
        {
            if(!inited)
            {
                DateTime now = UnbiasedTime.Instance.Now();

                startDate = now;

                lastContentNo = 0;
                lastWeekdayContentNo = 0;
                lastWeekendContentNo = 0;
                currentContentNo = 1;
                contentLimitNum = 2;


                DayOfWeek dayOfWeek = UnbiasedTime.Instance.Now().DayOfWeek;
                switch(now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        now = now.AddDays(-2);
                        dayOfWeek = DayOfWeek.Saturday;
                        break;
                    case DayOfWeek.Wednesday:
                        now = now.AddDays(-1);
                        dayOfWeek = DayOfWeek.Tuesday;
                        break;
                    case DayOfWeek.Friday:
                        now = now.AddDays(-1);
                        dayOfWeek = DayOfWeek.Thursday;
                        break;
                    case DayOfWeek.Sunday:
                        now = now.AddDays(-1);
                        dayOfWeek = DayOfWeek.Saturday;
                        break;
                }
                string date = now.Year + "_" + now.Month + "_" + now.Day;

                SetContentData(1, date, dayOfWeek, "weekday", 1, Def.ContentState.Open);
                SetContentData(2, date, dayOfWeek, "weekday", 2, Def.ContentState.HalfOpen);
                lastContentNo = 2;
                AddContentData();


                inited = true;
            }
        }



        public void SetContentData(int contentNo, string date, DayOfWeek dayOfWeek, string type, int uno, Def.ContentState state, int currentContentIndex=0, int weekendQuestionPoint=0)
        {
            ContentData data = new ContentData();
            data.date = date;
            data.dayOfWeek = dayOfWeek;
            data.type = type;
            data.uno = uno;
            data.state = state;
            data.currentContentIndex = currentContentIndex;
            data.weekendQuestionPoint = weekendQuestionPoint;

            _SetContentData(contentNo, data);
        }

        public void AddContentData()
        {
            int contentNo = lastContentNo + 1;

            // Get Prev Content Data
            ContentData prevData = GetContentData(lastContentNo);
            char[] delimiterChars = { '_' };
            string[] prevDateStrArr = prevData.date.Split(delimiterChars);
            DateTime prevDate = new DateTime(int.Parse(prevDateStrArr[0]), int.Parse(prevDateStrArr[1]), int.Parse(prevDateStrArr[2]));

            //=== Set Add Content Data ===
            ContentData data = new ContentData();

            // Set Date, DayOfWeek, Type, UNO
//            DateTime date = DateTime.Now;
            DateTime date = UnbiasedTime.Instance.Now();
            switch(prevData.dayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    date = prevDate.AddDays(2);
                    data.dayOfWeek = DayOfWeek.Thursday;
                    data.type = "weekday";
                    data.uno = lastWeekdayContentNo + 1;
                    break;
                case DayOfWeek.Thursday:
                    date = prevDate.AddDays(2);
                    data.dayOfWeek = DayOfWeek.Saturday;
                    data.type = "weekend";
                    data.uno = lastWeekendContentNo + 1;
                    break;
                case DayOfWeek.Saturday:
                    date = prevDate.AddDays(3);
                    data.dayOfWeek = DayOfWeek.Tuesday;
                    data.type = "weekday";
                    data.uno = lastWeekdayContentNo + 1;
                    break;
            }
            data.date = date.Year + "_" + date.Month + "_" + date.Day;

            // Set State
            data.state = Def.ContentState.Lock;


            _SetContentData(contentNo, data);
            //=== Set Add Content Data : end ===


            lastContentNo++;
        }

        public void SetContentState(int contentNo, Def.ContentState state)
        {
            ContentData data = GetContentData(contentNo);
            data.state = state;

            _SetContentData(contentNo, data);
        }

        public string GetContentType(int contentNo)
        {
            return GetContentData(contentNo).type;
        }

        public int GetContentUNo(int contentNo)
        {
            return GetContentData(contentNo).uno;
        }

        public Def.ContentState GetContentState(int contentNo)
        {
            return GetContentData(contentNo).state;
        }

        public string GetContentDate(int contentNo)
        {
            return GetContentData(contentNo).date;
        }

        public int GetCurrentContentIndex(int contentNo)
        {
            return GetContentData(contentNo).currentContentIndex;
        }

        public void SetCurrentContentIndex(int contentNo, int index)
        {
            ContentData data = GetContentData(contentNo);
            data.currentContentIndex = index;
            _SetContentData(contentNo, data);
        }

        public int GetWeekendQuestionPoint(int contentNo)
        {
            ContentData data = GetContentData(contentNo);
            return data.weekendQuestionPoint;
        }

        public void SetWeekendQuestionPoint(int contentNo, int point)
        {
            ContentData data = GetContentData(contentNo);
            data.weekendQuestionPoint = point;
            _SetContentData(contentNo, data);
        }

        public string GetNowDate()
        {
            DateTime date = UnbiasedTime.Instance.Now();
            string dateStr = date.Year.ToString() + "_" + date.Month.ToString() + "_" + date.Day.ToString();
            return dateStr;
        }

        public int[] GetWaitingTime()
        {
            ContentData lastData = GetContentData(lastContentNo);

            int[] date = DateToIntArr(lastData.date);

            DateTime dateTime = new DateTime(date[0], date[1], date[2]);

            TimeSpan timeSpan = dateTime - UnbiasedTime.Instance.Now();

            return new int[3]{timeSpan.Days, timeSpan.Hours, timeSpan.Minutes};
        }

        public int DateCompare(string dateA, string dateB)
        {
            char[] delimiterChars = { '_' };
            string[] dateStrArr;

            dateStrArr = dateA.Split(delimiterChars);
            DateTime prevDate = new DateTime(int.Parse(dateStrArr[0]), int.Parse(dateStrArr[1]), int.Parse(dateStrArr[2]));

            dateStrArr = dateB.Split(delimiterChars);
            DateTime nextDate = new DateTime(int.Parse(dateStrArr[0]), int.Parse(dateStrArr[1]), int.Parse(dateStrArr[2]));

            // if dateA > dateB => 1
            // if dateA == dateB => 0
            // if dateA < dateB => -1
            return DateTime.Compare(prevDate, nextDate);
        }

        public bool IsTimePassed(int[] time)
        {
            if(time[0] < 0 || time[1] < 0 || time[2] < 0)
                return true;

            return false;
        }

        public string GetLastContentDate()
        {
            return GetContentData(lastContentNo).date;
        }

        public void SetProfile(string name, string level)
        {
            profileInited = true;
            profileName = name;
            profileLevel = level;
        }

        public string ConvertProfileLevelIntToString(int level)
        {
            string result = "";
            switch(level)
            {
                case 0:
                    result = "A";
                    break;
                case 1:
                    result = "B";
                    break;
                case 2:
                    result = "C";
                    break;
            }

            return result;
        }

        public int ConvertProfileLevelStringToInt(string level)
        {
            int result = -1;
            switch(level)
            {
                case "A":
                    result = 0;
                    break;
                case "B":
                    result = 1;
                    break;
                case "C":
                    result = 2;
                    break;
            }

            return result;
        }


        public ContentData GetContentData(int contentNo)
        {
            string json = PlayerPrefs.GetString("lms_content_data_list_" + contentNo.ToString());
            ContentData data = JsonUtility.FromJson<ContentData>(json);
            return data;
        }

        public Dictionary<string, object> GetStep()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("inited", inited);
            result.Add("profileInited", profileInited);
            result.Add("profileName", profileName);
            result.Add("profileLevel", profileLevel);
            result.Add("startDate", startDate.ToString());
            result.Add("currentContentNo", currentContentNo);
            result.Add("lastContentNo", lastContentNo);
            result.Add("lastWeekdayContentNo", lastWeekdayContentNo);
            result.Add("lastWeekendContentNo", lastWeekendContentNo);
            result.Add("contentLimitNum", contentLimitNum);
            return result;
        }

        public void SetStep(Dictionary<string, object> step)
        {
            inited = bool.Parse(step["inited"].ToString());
            profileInited = bool.Parse(step["profileInited"].ToString());
            profileName = step["profileName"].ToString();
            profileLevel = step["profileLevel"].ToString();
            startDate = Convert.ToDateTime(step["startDate"].ToString()); 
            currentContentNo = int.Parse(step["currentContentNo"].ToString()); 
            lastContentNo = int.Parse(step["lastContentNo"].ToString()); 
            lastWeekdayContentNo = int.Parse(step["lastWeekdayContentNo"].ToString()); 
            lastWeekendContentNo = int.Parse(step["lastWeekendContentNo"].ToString()); 
            contentLimitNum = int.Parse(step["contentLimitNum"].ToString()); 
        }

        public List<Dictionary<string, object>> GetContentDatas()
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            for(int i = 1 ; i <= lastContentNo ; i++)
                result.Add(GetContentData(i).ToDictionary());
            return result;
        }

        public void SetContentDatas(List<Dictionary<string, object>> contentDatas)
        {
            for(int i = 1 ; i <= contentDatas.Count ; i++)
            {
                ContentData data = ContentData.DictionaryToContentData(contentDatas[i - 1]);
                _SetContentData(i, data);
//                SetContentData(i, data.date, data.dayOfWeek, data.type, data.uno, data.state, data.currentContentIndex, data.weekendQuestionPoint);
            }
        }

        void _SetContentData(int contentNo, ContentData data)
        {
            if(data.type == "weekday")
                lastWeekdayContentNo = data.uno;
            else if(data.type == "weekend")
                lastWeekendContentNo = data.uno;

            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("lms_content_data_list_" + contentNo.ToString(), json);
        }

        int DayOfWeekToInt(DayOfWeek dayOfWeek)
        {
            int result = 0;

            switch(dayOfWeek)
            {
                case DayOfWeek.Monday:
                    result = 1;
                    break;
                case DayOfWeek.Tuesday:
                    result = 2;
                    break;
                case DayOfWeek.Wednesday:
                    result = 3;
                    break;
                case DayOfWeek.Thursday:
                    result = 4;
                    break;
                case DayOfWeek.Friday:
                    result = 5;
                    break;
                case DayOfWeek.Saturday:
                    result = 6;
                    break;
                case DayOfWeek.Sunday:
                    result = 7;
                    break;
            }

            return result;
        }

        int[] DateToIntArr(string date)
        {
            char[] delimiterChars = { '_' };
            string[] dateStrArr = date.Split(delimiterChars);
            int[] arr = new int[]{int.Parse(dateStrArr[0]), int.Parse(dateStrArr[1]), int.Parse(dateStrArr[2])};
            return arr;
        }

        //============================================================================
        //============================================================================
        public void TestInit()
        {
            if(!inited)
            {
                DateTime now = UnbiasedTime.Instance.Now();

                startDate = now;

                lastContentNo = 0;
                lastWeekdayContentNo = 0;
                lastWeekendContentNo = 0;
                currentContentNo = 1;

                DayOfWeek dayOfWeek = UnbiasedTime.Instance.Now().DayOfWeek;
                switch(now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        dayOfWeek = DayOfWeek.Saturday;
                        break;
                    case DayOfWeek.Wednesday:
                        dayOfWeek = DayOfWeek.Tuesday;
                        break;
                    case DayOfWeek.Friday:
                        dayOfWeek = DayOfWeek.Thursday;
                        break;
                    case DayOfWeek.Sunday:
                        dayOfWeek = DayOfWeek.Saturday;
                        break;
                }
                string date = now.Year + "_" + now.Month + "_" + now.Day;

                for(int i = 1 ; i <= 20 ; i++)
                {
                    int contentNo = i;
                    int uno = i;
                    SetContentData(contentNo, date, dayOfWeek, "weekday", uno, Def.ContentState.Cleared);
                    SetCurrentContentIndex(contentNo, 8);
                }
                lastWeekdayContentNo = 20;

                int unoCount = 0;
                for(int i = 21 ; i <= 28 ; i++)
                {
                    int contentNo = i;
                    unoCount++;
                    int uno = unoCount;
                    SetContentData(contentNo, date, dayOfWeek, "weekend", uno, Def.ContentState.Cleared);
                    SetCurrentContentIndex(contentNo, 8);
                }
                lastWeekendContentNo = 8;

                lastContentNo = 28;

//                TestAddContent();

                contentLimitNum = 30;

                inited = true;
            }
        }
       
        public void TestAddContent()
        {
            int contentNo = lastContentNo + 1;

            // Get Prev Content Data
            ContentData prevData = GetContentData(lastContentNo);
            char[] delimiterChars = { '_' };
            string[] prevDateStrArr = prevData.date.Split(delimiterChars);
            DateTime prevDate = new DateTime(int.Parse(prevDateStrArr[0]), int.Parse(prevDateStrArr[1]), int.Parse(prevDateStrArr[2]));

            //=== Set Add Content Data ===
            ContentData data = new ContentData();

            // Set Date, DayOfWeek, Type, UNO
            //            DateTime date = DateTime.Now;
            DateTime date = UnbiasedTime.Instance.Now();
            switch(prevData.dayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    data.dayOfWeek = DayOfWeek.Thursday;
                    data.type = "weekday";
                    data.uno = lastWeekdayContentNo + 1;
                    break;
                case DayOfWeek.Thursday:
                    data.dayOfWeek = DayOfWeek.Saturday;
                    data.type = "weekend";
                    data.uno = lastWeekendContentNo + 1;
                    break;
                case DayOfWeek.Saturday:
                    data.dayOfWeek = DayOfWeek.Tuesday;
                    data.type = "weekday";
                    data.uno = lastWeekdayContentNo + 1;
                    break;
            }
            date = prevDate.AddMinutes(5);
            data.date = date.Year + "_" + date.Month + "_" + date.Day;

            // Set State
            data.state = Def.ContentState.Lock;

            data.currentContentIndex = 9;

            _SetContentData(contentNo, data);
            //=== Set Add Content Data : end ===


            lastContentNo++;
        }
        //============================================================================
        //============================================================================

    }
}
