using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;
using Com.Dobrain.Dobrainproject.Manager;

namespace Dobrain.LMS
{
    public struct ThisWeekBest
    {
        public string Type { get; set; }
        public float avg { get; set; }
    }

    public struct Progress
    {
        public int count { get; set; }
    }

    public class Analysis : MonoBehaviour
    {

        public SimpleSQLManager sqlManager;

        public ThisWeekBest[] thisWeekBest;

        public int todaycount = 0;
        public int weekcount = 0;
        public int monthcount = 0;

        public int todaytotalcount = 0;
        public int weektotalcount = 0;
        public int monthtotalcount = 0;

        public int todaymax = 8;
        public int weekmax = 16;
        public int monthmax = 64;

        public float thisweekRepeat = 0;
        // Use this for initialization
        void Awake()
        {
            sqlManager = GetComponent<SimpleSQLManager>();
            WeekCount();

            TodayProgress();
            TodayTotal();
            ThisWeekProgress();
            ThisWeekTotal();
            ThisMonthProgress();
            ThisMonthTotal();
            ThisWeekBest();
            Repeat();
        }

        void WeekCount()
        {
            //Debug.Log(LMSManager.instance.startDate.ToString("yyyy-MM-dd"));
        }
        //오늘 푼 문제
        void TodayProgress()
        {
            List<Progress> list;
            if (sqlManager.databaseFile)
            {
                list = sqlManager.Query<Progress>
                    ("SELECT COUNT(DISTINCT Chapter_index) 'count' FROM QuestionRecord WHERE Date = ?", UnbiasedTime.Instance.Now().ToString("yyyy-MM-dd"));

                todaycount = list[0].count;
            }
        }
        void TodayTotal()
        {
            List<Progress> list;
            if (sqlManager.databaseFile)
            {
                list = sqlManager.Query<Progress>
                    ("SELECT COUNT(*) 'count' FROM QuestionRecord WHERE Date = ?", UnbiasedTime.Instance.Now().ToString("yyyy-MM-dd"));

                todaytotalcount = list[0].count;
            }
        }

        //이번 주 푼 문제
        void ThisWeekProgress()
        {
            List<Progress> list;
            if (sqlManager.databaseFile)
            {
                list = sqlManager.Query<Progress>
                    ("SELECT COUNT(DISTINCT Chapter_index) 'count' FROM " + "(" + ThisWeek() + ")");

                weekcount = list[0].count;
            }
        }

        void ThisWeekTotal()
        {
            List<Progress> list;
            if (sqlManager.databaseFile)
            {
                list = sqlManager.Query<Progress>
                    ("SELECT COUNT(*) 'count' FROM " + "(" + ThisWeek() + ")");

                weektotalcount = list[0].count;
            }
        }
        //이번 달 푼 문제
        void ThisMonthTotal()
        {
            List<Progress> list;
            if (sqlManager.databaseFile)
            {
                list = sqlManager.Query<Progress>
                    ("SELECT COUNT(*) 'count' FROM QuestionRecord WHERE Date BETWEEN ? AND ?",
                    UnbiasedTime.Instance.Now().AddDays(1 - System.DateTime.Today.Day).ToString("yyyy-MM-dd"),
                    UnbiasedTime.Instance.Now().AddMonths(1).AddDays(0 - System.DateTime.Today.Day).ToString("yyyy-MM-dd"));

                monthtotalcount = list[0].count;
            }
        }

        void ThisMonthProgress()
        {
            List<Progress> list;
            if (sqlManager.databaseFile)
            {
                list = sqlManager.Query<Progress>
                    ("SELECT COUNT(DISTINCT Chapter_index) 'count' FROM QuestionRecord WHERE Date BETWEEN ? AND ?",
                    UnbiasedTime.Instance.Now().AddDays(1 - System.DateTime.Today.Day).ToString("yyyy-MM-dd"),
                    UnbiasedTime.Instance.Now().AddMonths(1).AddDays(0 - System.DateTime.Today.Day).ToString("yyyy-MM-dd"));

                monthcount = list[0].count;
            }
        }
        //이번 주 잘한 것 3개
        void ThisWeekBest()
        {
            List<ThisWeekBest> list;

            if (sqlManager.databaseFile)
            {
                if (weekcount > 0)
                {
                    list = sqlManager.Query<ThisWeekBest>
                         ("SELECT Type, AVG(Score) 'avg' FROM (" + ThisWeek() + ") " + "GROUP BY Type ORDER BY avg DESC LIMIT 3");

                }
                else
                {
                    ThisWeekBest twb;
                    list = new List<ThisWeekBest>();

                    twb.Type = "-";
                    twb.avg = 1f;
                    list.Add(twb);
                    twb.Type = "-";
                    twb.avg = 1f;
                    list.Add(twb);
                    twb.Type = "-";
                    twb.avg = 1f;
                    list.Add(twb);
                }

                thisWeekBest = new ThisWeekBest[3];

                if (list.Count <= 3)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Type != null)
                            thisWeekBest[i].Type = list[i].Type;
                        else
                            thisWeekBest[i].Type = "";
                        if (!float.IsNaN(list[i].avg))
                            thisWeekBest[i].avg = list[i].avg;
                        else
                            thisWeekBest[i].avg = 0;
                    }


                }

                if(list.Count == 2)
                {
                    thisWeekBest[2].Type = "-";
                    thisWeekBest[2].avg = 0;
                }
                else if (list.Count == 1)
                {
                    thisWeekBest[2].Type = "-";
                    thisWeekBest[2].avg = 0;
                    thisWeekBest[1].Type = "-";
                    thisWeekBest[1].avg = 0;
                }            
            }
        }
        //이번주 복습률
        
        void Repeat()
        {
            List<Progress> list;
            if (sqlManager.databaseFile)
            {

                list = sqlManager.Query<Progress>
                    ("SELECT Count(*) AS 'count' FROM (" + ThisWeek() + ") GROUP BY Chapter_index HAVING count(*)>1");

                thisweekRepeat = (float)list.Count / (float)weekmax;
                
            }
        }
        
        //이번 주 날짜 구하기
        string ThisWeek()
        {
            string firstDay;
            string lastDay;

            int currentDay;

            switch (UnbiasedTime.Instance.Now().DayOfWeek)
            {
                case System.DayOfWeek.Monday:
                    currentDay = 0;
                    break;
                case System.DayOfWeek.Tuesday:
                    currentDay = 1;
                    break;
                case System.DayOfWeek.Wednesday:
                    currentDay = 2;
                    break;
                case System.DayOfWeek.Thursday:
                    currentDay = 3;
                    break;
                case System.DayOfWeek.Friday:
                    currentDay = 4;
                    break;
                case System.DayOfWeek.Saturday:
                    currentDay = 5;
                    break;
                case System.DayOfWeek.Sunday:
                    currentDay = 6;
                    break;
                default:
                    currentDay = 0;
                    break;
            }

            firstDay = UnbiasedTime.Instance.Now().AddDays(-currentDay).ToString("yyyy-MM-dd");
            lastDay = UnbiasedTime.Instance.Now().AddDays(6 - currentDay).ToString("yyyy-MM-dd");

            return ("SELECT* FROM QuestionRecord WHERE Date BETWEEN '" + firstDay + "' AND '" + lastDay + "'");
        }
    }
}