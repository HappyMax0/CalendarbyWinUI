﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.Globalization;
using Calendar = Windows.Globalization.Calendar;
using DayOfWeek = Windows.Globalization.DayOfWeek;

namespace CalendarWinUI3.Models.Utils
{
    public static class Helper
    {
        public static List<Day> GetDayList(Calendar calendar)
        {
            DateTime time = calendar.GetDateTime().DateTime;

            List<Day> dayList = new List<Day>();

            DateTime today = DateTime.Today;

            int month = calendar.Month;
            int year = calendar.Year;
            int daysCount = calendar.NumberOfDaysInThisMonth;

            DateTime firstDay = new DateTime(year, month, 1);
            int firstDayWeek = (int)firstDay.DayOfWeek;

            //lastMonth
            int lastMonth = calendar.LastMonthInThisYear;
            int lastMonthYear = month > 1 ? year : year - 1;
            int lastMounthDaysCount = DateTime.DaysInMonth(lastMonthYear, lastMonth);

            //nextMonth
            int nextMonth = month < 12 ? month + 1 : 1;
            int nextMonthYear = month < 12 ? year : year + 1;

            for (int i = 0; i < firstDayWeek; i++)
            {
                Calendar cal = new Calendar();
                cal.SetDateTime(new DateTime(lastMonthYear, month - 1 > 0 ? month - 1 : 12, lastMounthDaysCount - (firstDayWeek - i) + 1));
                DateTime dateTime = cal.GetDateTime().DateTime;
                ChineseLunisolarCalendar chineseCalendar = new();
                int lunarYear = chineseCalendar.GetYear(dateTime);
                int lunarMonth = chineseCalendar.GetMonth(dateTime);
                int lunarDay = chineseCalendar.GetDayOfMonth(dateTime);

                dayList.Add(new Day(dateTime.Year, dateTime.Month, dateTime.Day) { LunarDay = lunarDay });
            }

            for (int day = 1; day <= daysCount; day++)
            {
                Calendar cal = new Calendar();
                cal.SetDateTime(new DateTime(year, month, day));
                DayOfWeek week = cal.DayOfWeek;
                
                bool isToday = day == today.Day && month == today.Month;

                // 创建农历日历实例
                ChineseLunisolarCalendar chineseCalendar = new();
                int lunarYear = chineseCalendar.GetYear(cal.GetDateTime().DateTime); 
                int lunarMonth = chineseCalendar.GetMonth(cal.GetDateTime().DateTime); 
                int lunarDay = chineseCalendar.GetDayOfMonth(cal.GetDateTime().DateTime);

                dayList.Add(new Day(year, month, day) { Week = week, IsToday = isToday, IsCurrentMonth = true, LunarDay = lunarDay });
            }

            for (int i = 1; i < 35 - (dayList.Count - 1); i++)
            {
                Calendar cal = new Calendar();
                cal.SetDateTime(new DateTime(nextMonthYear, nextMonth, i));
                DateTime dateTime = cal.GetDateTime().DateTime;
                ChineseLunisolarCalendar chineseCalendar = new();
                int lunarYear = chineseCalendar.GetYear(dateTime);
                int lunarMonth = chineseCalendar.GetMonth(dateTime);
                int lunarDay = chineseCalendar.GetDayOfMonth(dateTime);
                string str = chineseCalendar.ToString();

                dayList.Add(new Day(dateTime.Year, dateTime.Month, dateTime.Day) { LunarDay = lunarDay });
            }


            return dayList;

        }


        public static List<Week> GetWeeks(Calendar calendar)
        {
            DateTime time = calendar.GetDateTime().DateTime;


            DateTime today = DateTime.Today;

            int timeDay = time.Day;

            int timeWeek = (int)time.DayOfWeek;

            int daysCount = DateTime.DaysInMonth(time.Year, time.Month);

            List<Week> weekList = new List<Week>();
            for (int i = 0; i < 7; i++)
            {
                DayOfWeek week = (DayOfWeek)i;

                int day = timeDay - timeWeek + i;

                if (day > daysCount)
                    day = day - daysCount;

                weekList.Add(new Week() { WeekNo = week, DayNo = day, IsToday = day == today.Day });
            }
            return weekList;
        }

        public static Day GetDay(Calendar calendar)
        {
            DateTime time = calendar.GetDateTime().DateTime;

            Day day = new Day();

            day.YearNo = calendar.Year;
            day.MonthNo = calendar.Month;
            day.DayNo = calendar.Day;
            day.Week = calendar.DayOfWeek;

            List<Time> timeList = new List<Time>();

            DateTime dateTime = new DateTime(time.Year, time.Month, time.Day);

            for (int i = 0; i < 24; i++)
            {
                timeList.Add(new Time() { TimeNo = dateTime.AddHours(i), Event = String.Empty });
            }

            day.EventList = timeList;

            return day;
        }

        // 辅助函数：查找 Visual Tree 中的子元素
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

    }
}