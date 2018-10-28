using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace MMI.Libraries
{
    /// <summary>DateTime Methods Library</summary>
    /// <company>Marktest - Markdata</company>
    /// <department>MMI - Markdata Media Internet</department>
    /// <author>Valter Lima</author>
    /// <date_created>04-05-2009</date_created>
    /// <modifiers>Valter Lima</modifiers>
    /// <date_modified>01-02-2013</date_modified>
    public class LibraryDateTime : IDisposable
    {
        #region Variables
        public enum DateTimeType { NONE = 0, DEFAULT, DATE, TIME, DATEandTIME }
        public enum DateFormatType { NONE = 0, DEFAULT, yyyymmdd, yyyyMMdd, ddmmyyyy, ddMMyyyy }
        public enum TimeFormatType { NONE = 0, DEFAULT, hhmmss, HHmmss, hhmmssms, HHmmssms }
        public enum Language { PT = 0, EN }
        private Boolean _Disposed;
        #endregion Variables

        #region Constructor / Destructor

        /// <summary>Class constructor</summary>
        public LibraryDateTime()
        {
            _Disposed = false;
        }

        /// <summary>Class destructor which clean up unmanaged resources.</summary>
        //~LibraryDateTime()
        //{
        //    Disposing(false);
        //}

        /// <summary>Clean up all managed and unmanaged resources</summary>
        public void Dispose()
        {
            Disposing(true);
            //GC.SuppressFinalize(this); //Garbage Collector don't need to call its destructor because we have already done the clean up.
        }

        /// <summary>Dispose resources</summary>
        /// <param name="bDisposing">
        /// True: Clean up managed resources -> Deterministic finalization
        /// False: Clean up unmanaged resources -> Finalizer (Garbage Collector) cleanup  -> Non-deterministic finalization
        /// </param>
        private void Disposing(Boolean bDisposing)
        {
            #region Check to see if Dispose has already been called
            if (this._Disposed)
                return;
            #endregion Check to see if Dispose has already been called
            #region Dispose all managed and unmanaged resources
            #region Dispose managed resources
            if (bDisposing)
            {
                
            }
            #endregion Dispose managed resources
            #region Dispose unmanaged resources
            #endregion Dispose unmanaged resources
            #endregion Dispose all managed and unmanaged resources
            this._Disposed = true;
        }

        #endregion Constructor / Destructor

        #region Public Methods

        /// <summary></summary>
        /// <creator>Valter Lima</creator>
        /// <date created>25-10-2012</date created>
        /// <modifier>Valter Lima</modifier>
        /// <date modified>01-02-2013</date modified>
        public Boolean IsDateTimeNull(DateTime dt)
        {
            try { return ((dt.ToString().Trim() == "01-01-0001 00:00:00")) ? true : false; }
            catch { return true; }
        }

        /// <summary>Convert the Date String to DateTime format</summary>
        /// <param name="sDate">Date String</param>
        /// <returns>Date time format</returns>
        //public DateTime ConvertStringDateToDateTime(String sDate)
        //{
        //    try
        //    {
        //        DateTime objDateTime = new DateTime();
        //        objDateTime = objDateTime.AddYears(Convert.ToInt32(sDate.Substring(0, 4)) - 1);
        //        objDateTime = objDateTime.AddMonths(Convert.ToInt32(sDate.Substring(4, 2)) - 1);
        //        objDateTime = objDateTime.AddDays(Convert.ToDouble(sDate.Substring(6, 2)) - 1);
        //        return objDateTime;
        //    }
        //    catch { return new DateTime(); }
        //}

        /// <summary>Convert Time String to TimeSpan format</summary>
        /// <param name="sTime">Time String</param>
        /// <returns>TimeSpan format</returns>
        public TimeSpan ConvertStringTimeToTimeSpan(String sTime)
        {
            try
            {
                Int32 iH = 0, iM = 0, iS = 0;
                if (sTime.Length == 6)
                {
                    iH = Convert.ToInt32(sTime.Substring(0, 2));
                    iM = Convert.ToInt32(sTime.Substring(2, 2));
                    iS = Convert.ToInt32(sTime.Substring(4, 2));
                }
                else if (sTime.Length == 5)
                {
                    iH = Convert.ToInt32(sTime.Substring(0, 1));
                    iM = Convert.ToInt32(sTime.Substring(1, 2));
                    iS = Convert.ToInt32(sTime.Substring(3, 2));
                }
                if (iH >= 24)
                    iH -= 24;
                return new TimeSpan(iH, iM, iS);
            }
            catch { return new TimeSpan(0, 0, 0); }
        }

        /// <summary>Returns a list with the given TimeSpan elements</summary>
        /// <param name="objTimeSpan">TimeSpan object</param>
        /// <returns>TimeSpan elements list</returns>
        public ArrayList GetTimeList(TimeSpan objTimeSpan)
        {
            try
            {
                ArrayList objArrayListTimeSpan = new ArrayList();
                Int32 iTimeHours = objTimeSpan.Hours;
                Int32 iTimeMinutes = objTimeSpan.Minutes;
                Int32 iTimeSeconds = objTimeSpan.Seconds;
                String sTimeHours = "", sTimeMinutes = "", sTimeSeconds = "";
                if (iTimeHours < 10)
                    sTimeHours += "0";
                if (iTimeMinutes < 10)
                    sTimeMinutes += "0";
                if (iTimeSeconds < 10)
                    sTimeSeconds += "0";
                sTimeHours += iTimeHours.ToString();
                sTimeMinutes += iTimeMinutes.ToString();
                sTimeSeconds += iTimeSeconds.ToString();
                objArrayListTimeSpan.Add(sTimeHours);
                objArrayListTimeSpan.Add(sTimeMinutes);
                objArrayListTimeSpan.Add(sTimeSeconds);
                return objArrayListTimeSpan;
            }
            catch { return null; }
        }

        /// <summary>Returns a TimeSpan from the given Total Seconds</summary>
        /// <param name="iTotalSeconds">Total seconds</param>
        /// <returns>TimeSpan object</returns>
        public TimeSpan GetTimeSpanFromTotalSeconds(Int32 iTotalSeconds)
        {
            try
            {
                Int32 iH = iTotalSeconds / 3600;
                Int32 iM = (iTotalSeconds - iH * 3600) / 60;
                Int32 iS = iTotalSeconds - iH * 3600 - iM * 60;
                return new TimeSpan(iH, iM, iS);
            }
            catch { return new TimeSpan(0, 0, 0); }
        }

        /// <summary>Returns a TimeSpan from the given Total Miliseconds</summary>
        /// <param name="iTotalSeconds">Total miliseconds</param>
        /// <returns>TimeSpan object</returns>
        public TimeSpan GetTimeSpanFromTotalMiliseconds(Int32 iTotalMiliseconds)
        {
            try
            {
                Int32 iH = iTotalMiliseconds / 3600000;
                Int32 iM = (iTotalMiliseconds - iH * 3600000) / 60000;
                Int32 iS = iTotalMiliseconds - iH * 3600000 - iM * 60000;
                return new TimeSpan(iH, iM, iS);
            }
            catch { return new TimeSpan(0, 0, 0); }
        }

        /// <summary>Convert a TimeSpan into a string</summary>
        /// <param name="objTimeSpan">Time span</param>
        /// <param name="eLanguage">Language definition</param>
        /// <returns>Time string text</returns>
        public String ConvertTimeSpanIntoString(TimeSpan objTimeSpan, TimeSpanLanguageCollection.Language eLanguage)
        {
            try
            {
                String sTimeSpan = "";
                TimeSpanLanguageCollection objTimeSpanLanguageCollection = new TimeSpanLanguageCollection();
                objTimeSpanLanguageCollection.LanguageType = eLanguage;
                #region Set TimeSpan Language Strings
                switch (eLanguage)
                {
                    #region Portuguese TimeSpan Language Strings
                    case TimeSpanLanguageCollection.Language.PT:
                        TimeLanguageStrings objTimeLanguageStringsPT = new TimeLanguageStrings();
                        #region Days
                        TimeLanguageString objTimeLanguageStringDaysPT = new TimeLanguageString();
                        objTimeLanguageStringDaysPT.TimeElement = TimeLanguageString.TimeSpanElement.DAYS;
                        objTimeLanguageStringDaysPT.SingularDescription = "dia";
                        objTimeLanguageStringDaysPT.PluralDescription = "dias";
                        objTimeLanguageStringsPT.ListStrings.Add(objTimeLanguageStringDaysPT);
                        #endregion Days
                        #region Hours
                        TimeLanguageString objTimeLanguageStringHoursPT = new TimeLanguageString();
                        objTimeLanguageStringHoursPT.TimeElement = TimeLanguageString.TimeSpanElement.HOURS;
                        objTimeLanguageStringHoursPT.SingularDescription = "hora";
                        objTimeLanguageStringHoursPT.PluralDescription = "horas";
                        objTimeLanguageStringsPT.ListStrings.Add(objTimeLanguageStringHoursPT);
                        #endregion Hours
                        #region Minuts
                        TimeLanguageString objTimeLanguageStringMinutsPT = new TimeLanguageString();
                        objTimeLanguageStringMinutsPT.TimeElement = TimeLanguageString.TimeSpanElement.MINUTS;
                        objTimeLanguageStringMinutsPT.SingularDescription = "minuto";
                        objTimeLanguageStringMinutsPT.PluralDescription = "minutos";
                        objTimeLanguageStringsPT.ListStrings.Add(objTimeLanguageStringMinutsPT);
                        #endregion Minuts
                        #region Seconds
                        TimeLanguageString objTimeLanguageStringSecondsPT = new TimeLanguageString();
                        objTimeLanguageStringSecondsPT.TimeElement = TimeLanguageString.TimeSpanElement.SECONDS;
                        objTimeLanguageStringSecondsPT.SingularDescription = "segundo";
                        objTimeLanguageStringSecondsPT.PluralDescription = "segundos";
                        objTimeLanguageStringsPT.ListStrings.Add(objTimeLanguageStringSecondsPT);
                        #endregion Seconds
                        #region Miliseconds
                        TimeLanguageString objTimeLanguageStringMilisecondsPT = new TimeLanguageString();
                        objTimeLanguageStringMilisecondsPT.TimeElement = TimeLanguageString.TimeSpanElement.MILISECONDS;
                        objTimeLanguageStringMilisecondsPT.SingularDescription = "dia";
                        objTimeLanguageStringMilisecondsPT.PluralDescription = "dias";
                        objTimeLanguageStringsPT.ListStrings.Add(objTimeLanguageStringMilisecondsPT);
                        #endregion Miliseconds
                        objTimeSpanLanguageCollection.Add(objTimeLanguageStringsPT);
                        break;
                    #endregion Portuguese TimeSpan Language Strings
                    #region English TimeSpan Language Strings
                    case TimeSpanLanguageCollection.Language.EN:
                        TimeLanguageStrings objTimeLanguageStringsEN = new TimeLanguageStrings();
                        #region Days
                        TimeLanguageString objTimeLanguageStringDaysEN = new TimeLanguageString();
                        objTimeLanguageStringDaysEN.TimeElement = TimeLanguageString.TimeSpanElement.DAYS;
                        objTimeLanguageStringDaysEN.SingularDescription = "day";
                        objTimeLanguageStringDaysEN.PluralDescription = "days";
                        objTimeLanguageStringsEN.ListStrings.Add(objTimeLanguageStringDaysEN);
                        #endregion Days
                        #region Hours
                        TimeLanguageString objTimeLanguageStringHoursEN = new TimeLanguageString();
                        objTimeLanguageStringHoursEN.TimeElement = TimeLanguageString.TimeSpanElement.HOURS;
                        objTimeLanguageStringHoursEN.SingularDescription = "hour";
                        objTimeLanguageStringHoursEN.PluralDescription = "hours";
                        objTimeLanguageStringsEN.ListStrings.Add(objTimeLanguageStringHoursEN);
                        #endregion Hours
                        #region Minuts
                        TimeLanguageString objTimeLanguageStringMinutsEN = new TimeLanguageString();
                        objTimeLanguageStringMinutsEN.TimeElement = TimeLanguageString.TimeSpanElement.MINUTS;
                        objTimeLanguageStringMinutsEN.SingularDescription = "minute";
                        objTimeLanguageStringMinutsEN.PluralDescription = "minutes";
                        objTimeLanguageStringsEN.ListStrings.Add(objTimeLanguageStringMinutsEN);
                        #endregion Minuts
                        #region Seconds
                        TimeLanguageString objTimeLanguageStringSecondsEN = new TimeLanguageString();
                        objTimeLanguageStringSecondsEN.TimeElement = TimeLanguageString.TimeSpanElement.SECONDS;
                        objTimeLanguageStringSecondsEN.SingularDescription = "second";
                        objTimeLanguageStringSecondsEN.PluralDescription = "seconds";
                        objTimeLanguageStringsEN.ListStrings.Add(objTimeLanguageStringSecondsEN);
                        #endregion Seconds
                        #region Miliseconds
                        TimeLanguageString objTimeLanguageStringMilisecondsEN = new TimeLanguageString();
                        objTimeLanguageStringMilisecondsEN.TimeElement = TimeLanguageString.TimeSpanElement.MILISECONDS;
                        objTimeLanguageStringMilisecondsEN.SingularDescription = "milisecond";
                        objTimeLanguageStringMilisecondsEN.PluralDescription = "miliseconds";
                        objTimeLanguageStringsEN.ListStrings.Add(objTimeLanguageStringMilisecondsEN);
                        #endregion Miliseconds
                        objTimeSpanLanguageCollection.Add(objTimeLanguageStringsEN);
                        break;
                    #endregion English TimeSpan Language Strings
                }
                #endregion Set TimeSpan Language Strings
                #region Get TimeSpan String
                foreach (TimeLanguageString objTimeLanguageString in objTimeSpanLanguageCollection.ListTimeStrings[0].ListStrings)
                {
                    switch (objTimeLanguageString.TimeElement)
                    {
                        case TimeLanguageString.TimeSpanElement.DAYS:
                            sTimeSpan = AddTimeSpanElementString(sTimeSpan, objTimeSpan.Days, objTimeLanguageString, ", ");
                            break;
                        case TimeLanguageString.TimeSpanElement.HOURS:
                            sTimeSpan = AddTimeSpanElementString(sTimeSpan, objTimeSpan.Hours, objTimeLanguageString, ", ");
                            break;
                        case TimeLanguageString.TimeSpanElement.MINUTS:
                            sTimeSpan = AddTimeSpanElementString(sTimeSpan, objTimeSpan.Minutes, objTimeLanguageString, ", ");
                            break;
                        case TimeLanguageString.TimeSpanElement.SECONDS:
                            sTimeSpan = AddTimeSpanElementString(sTimeSpan, objTimeSpan.Seconds, objTimeLanguageString, ", ");
                            break;
                        case TimeLanguageString.TimeSpanElement.MILISECONDS:
                            sTimeSpan = AddTimeSpanElementString(sTimeSpan, objTimeSpan.Milliseconds, objTimeLanguageString, ", ");
                            break;
                    }
                }
                #endregion Get TimeSpan String
                return sTimeSpan;
            }
            catch { return ""; }
        }

        /// <summary>Convert given DateTime to string with specified format patterns</summary>
        /// <param name="objDateTime">DateTime object</param>
        /// <param name="eDateTimeType"></param>
        /// <param name="eDateFormatType">Date format pattern</param>
        /// <param name="eTimeFormatType">Time format pattern</param>
        /// <param name="sDateTimeSeparationChar">Separation character between Date and Time</param>
        /// <param name="sDateSeparationChar">Date separation character</param>
        /// <param name="sTimeSeparationChar">Time separation character</param>
        /// <returns>Formatted DateTime string</returns>
        //public String ConvertDateTimeToString(DateTime objDateTime, DateTimeType eDateTimeType, DateFormatType eDateFormatType, TimeFormatType eTimeFormatType, String sDateTimeSeparationChar, String sDateSeparationChar, String sTimeSeparationChar)
        //{
        //    try
        //    {
        //        String sDateTime = "";
        //        String sDate = "";
        //        String sTime = "";
        //        if (eDateTimeType == DateTimeType.DATEandTIME || eDateTimeType == DateTimeType.DATE)
        //        {
        //            String sDateFormatType = "";
        //            String sDateFormatPattern = "";
        //            switch (eDateFormatType)
        //            {
        //                case DateFormatType.ddmmyyyy:
        //                case DateFormatType.ddMMyyyy:
        //                    sDateFormatType = Enum.GetName(typeof(DateFormatType), eDateFormatType);
        //                    sDateFormatPattern = sDateFormatType.Substring(0, 2) + sDateSeparationChar + sDateFormatType.Substring(2, 2) + sDateSeparationChar + sDateFormatType.Substring(4, 4);
        //                    break;
        //                case DateFormatType.yyyymmdd:
        //                case DateFormatType.yyyyMMdd:
        //                case DateFormatType.DEFAULT:
        //                    sDateFormatType = Enum.GetName(typeof(DateFormatType), eDateFormatType);
        //                    sDateFormatPattern = sDateFormatType.Substring(0, 4) + sDateSeparationChar + sDateFormatType.Substring(4, 2) + sDateSeparationChar + sDateFormatType.Substring(6, 2);
        //                    break;
        //                case DateFormatType.NONE:
        //                    sDateFormatType = Enum.GetName(typeof(DateFormatType), eDateFormatType);
        //                    sDateFormatPattern = sDateFormatType.Substring(0, 4) + sDateFormatType.Substring(4, 2) + sDateFormatType.Substring(6, 2);
        //                    break;
        //            }
        //            sDate = objDateTime.ToString(sDateFormatPattern, CultureInfo.InvariantCulture);
        //        }
        //        if (eDateTimeType == DateTimeType.DATEandTIME || eDateTimeType == DateTimeType.TIME)
        //        {
        //            String sTimeFormatType = "";
        //            String sTimeFormatPattern = "";
        //            switch (eTimeFormatType)
        //            {
        //                case TimeFormatType.hhmmss:
        //                case TimeFormatType.HHmmss:
        //                case TimeFormatType.DEFAULT:
        //                    sTimeFormatType = Enum.GetName(typeof(TimeFormatType), eTimeFormatType);
        //                    sTimeFormatPattern = sTimeFormatType.Substring(0, 2) + sTimeSeparationChar + sTimeFormatType.Substring(2, 2) + sTimeSeparationChar + sTimeFormatType.Substring(4, 2);
        //                    break;
        //                case TimeFormatType.hhmmssms:
        //                case TimeFormatType.HHmmssms:
        //                    sTimeFormatType = Enum.GetName(typeof(TimeFormatType), eTimeFormatType);
        //                    sTimeFormatPattern = sTimeFormatType.Substring(0, 2) + sTimeSeparationChar + sTimeFormatType.Substring(2, 2) + sTimeSeparationChar + sTimeFormatType.Substring(4, 2) + sTimeSeparationChar + sTimeFormatType.Substring(6, 2);
        //                    break;
        //                case TimeFormatType.NONE:
        //                    sTimeFormatType = Enum.GetName(typeof(TimeFormatType), eTimeFormatType);
        //                    sTimeFormatPattern = sTimeFormatType.Substring(0, 2) + sTimeFormatType.Substring(2, 2) + sTimeFormatType.Substring(4, 2);
        //                    break;
        //            }
        //            sTime = objDateTime.ToString(sTimeFormatPattern, CultureInfo.InvariantCulture);
        //        }
        //        if (eDateTimeType == DateTimeType.DEFAULT)
        //        {
        //            sDate = objDateTime.ToString(Program.DB_Date_Format_String, CultureInfo.InvariantCulture);
        //            sTime = objDateTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
        //        }
        //        if (eDateTimeType == DateTimeType.NONE)
        //        {
        //            sDate = objDateTime.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        //            sTime = objDateTime.ToString("HHmmss", CultureInfo.InvariantCulture);
        //        }
        //        if (!String.IsNullOrEmpty(sDate))
        //            sDateTime += sDate;
        //        if (!String.IsNullOrEmpty(sDate))
        //            sDateTime += sDateTimeSeparationChar + sTime;
        //        return sDateTime;
        //    }
        //    catch { return ""; }
        //}

        /// <summary>Format given militar time string</summary>
        /// <param name="sTime">Militar format time</param>
        /// <param name="cSeparationChar">Separation character</param>
        /// <returns>Formatted time string</returns>
        public String FormatMilitarTimeToFormattedTime(String sTime, char cSeparationChar)
        {
            try
            {
                String sTimeFormatted = "";
                if (sTime.Length == 1)
                    sTimeFormatted = "00" + cSeparationChar + "00" + cSeparationChar + "0" + sTime;
                else if (sTime.Length == 2)
                    sTimeFormatted = "00" + cSeparationChar + "00" + cSeparationChar + sTime;
                else if (sTime.Length == 3)
                    sTimeFormatted = "00" + cSeparationChar + "0" + sTime.Substring(0, 1) + cSeparationChar + sTime.Substring(1, 2);
                else if (sTime.Length == 4)
                    sTimeFormatted = "00" + cSeparationChar + sTime.Substring(0, 2) + cSeparationChar + sTime.Substring(2, 2);
                else if (sTime.Length == 5)
                    sTimeFormatted = "0" + sTime.Substring(0, 1) + cSeparationChar + sTime.Substring(1, 2) + cSeparationChar + sTime.Substring(3, 2);
                else if (sTime.Length == 6)
                    sTimeFormatted = sTime.Substring(0, 2) + cSeparationChar + sTime.Substring(2, 2) + cSeparationChar + sTime.Substring(4, 2);
                return sTimeFormatted;
            }
            catch { return null; }
        }

        /// <summary>Format given militar date to given format date</summary>
        /// <param name="sDate">Militar date (8 digits string)</param>
        /// <param name="objInputDateFormatType">Input string date format pattern</param>
        /// <param name="objOutputDateFormatType">Output string date format pattern</param>
        /// <param name="cSeparationChar">Separation character</param>
        /// <returns>Formatted date string</returns>
        public String FormatMilitarDateToFormattedDate(String sDate, DateFormatType objInputDateFormatType, DateFormatType objOutputDateFormatType, char cSeparationChar)
        {
            try
            {
                String sTimeFormatted = "";
                if (sDate.Length != 8)
                    return sDate;
                switch (objInputDateFormatType)
                {
                    case DateFormatType.ddmmyyyy:
                    case DateFormatType.ddMMyyyy:
                        switch (objOutputDateFormatType)
                        {
                            case DateFormatType.ddmmyyyy:
                            case DateFormatType.ddMMyyyy:
                            case DateFormatType.DEFAULT:
                                sTimeFormatted = sDate.Substring(0, 2) + cSeparationChar + sDate.Substring(2, 2) + cSeparationChar + sDate.Substring(4, 4);
                                break;
                            case DateFormatType.yyyymmdd:
                            case DateFormatType.yyyyMMdd:
                                sTimeFormatted = sDate.Substring(4, 4) + cSeparationChar + sDate.Substring(2, 2) + cSeparationChar + sDate.Substring(0, 2);
                                break;
                            case DateFormatType.NONE:
                                return sDate;
                        }
                        break;
                    case DateFormatType.yyyymmdd:
                    case DateFormatType.yyyyMMdd:
                    case DateFormatType.DEFAULT:
                        switch (objOutputDateFormatType)
                        {
                            case DateFormatType.ddmmyyyy:
                            case DateFormatType.ddMMyyyy:
                                sTimeFormatted = sDate.Substring(6, 2) + cSeparationChar + sDate.Substring(4, 2) + cSeparationChar + sDate.Substring(0, 4);
                                break;
                            case DateFormatType.yyyymmdd:
                            case DateFormatType.yyyyMMdd:
                            case DateFormatType.DEFAULT:
                                sTimeFormatted = sDate.Substring(0, 4) + cSeparationChar + sDate.Substring(4, 2) + cSeparationChar + sDate.Substring(6, 2);
                                break;
                            case DateFormatType.NONE:
                                return sDate;
                        }
                        break;
                    case DateFormatType.NONE:
                        return sDate;
                }
                return sTimeFormatted;
            }
            catch { return sDate; }
        }

        /// <summary>Get the current day time saudation</summary>
        /// <param name="eLanguage">Text language</param>
        /// <returns>Current day time saudation</returns>
        //public String GetCurrentDayTimeSaudation(Language eLanguage)
        //{
        //    try
        //    {
        //        String sSaudation = null;
        //        DateTime dtNowTime = DateTime.Now;
        //        String sGoodMorningText = "";
        //        String sGoodAfternoonText = "";
        //        String sGoodNightText = "";
        //        switch (eLanguage)
        //        { 
        //            case Language.PT:
        //                sGoodMorningText = "Bom dia";
        //                sGoodAfternoonText = "Boa tarde";
        //                sGoodNightText = "Boa noite";
        //                break;
        //            case Language.EN:
        //                sGoodMorningText = "Good morning";
        //                sGoodAfternoonText = "Good afternoon";
        //                sGoodNightText = "Good night";
        //                break;
        //        }
        //        if (dtNowTime.Hour >= 0 && dtNowTime.Hour < 12)
        //            sSaudation += sGoodMorningText;
        //        else if (dtNowTime.Hour >= 12 && dtNowTime.Hour < 20)
        //            sSaudation += sGoodAfternoonText;
        //        else
        //            sSaudation += sGoodNightText;
        //        return sSaudation;
        //    }
        //    catch { return null; }
        //}

        #endregion Public Methods

        #region Private Methods

        /// <summary>Split the elements of a TimeSpan object and add it to a text tring</summary>
        /// <param name="sTimeSpan">Time span object</param>
        /// <param name="iTimeSpanElementValue">Time span element</param>
        /// <param name="objTimeLanguageString">Language definition</param>
        /// <param name="sTimeSpanElementSeparation">String separation character</param>
        /// <returns>Time text string</returns>
        private String AddTimeSpanElementString(String sTimeSpan, Int32 iTimeSpanElementValue, TimeLanguageString objTimeLanguageString, String sTimeSpanElementSeparation)
        {
            try
            {
                if (iTimeSpanElementValue > 0)
                {
                    if (!String.IsNullOrEmpty(sTimeSpan))
                        sTimeSpan += sTimeSpanElementSeparation;
                    sTimeSpan += iTimeSpanElementValue.ToString() + " ";
                    if (iTimeSpanElementValue == 1)
                        sTimeSpan += objTimeLanguageString.SingularDescription;
                    else
                        sTimeSpan += objTimeLanguageString.PluralDescription;
                }
                return sTimeSpan;
            }
            catch { return ""; }
        }

        #endregion Private Methods
    }

    #region TimeSpanLanguageCollection

    public class TimeSpanLanguageCollection
    {
        public enum Language { PT = 0, EN }
        public Language LanguageType;
        public List<TimeLanguageStrings> ListTimeStrings;
        public TimeSpanLanguageCollection()
        {
            this.LanguageType = Language.PT;
            this.ListTimeStrings = new List<TimeLanguageStrings>();
        }
        public void Add(TimeLanguageStrings objTimeLanguageStrings)
        {
            this.ListTimeStrings.Add(objTimeLanguageStrings);
        }
    }

    public class TimeLanguageStrings
    {
        public List<TimeLanguageString> ListStrings;
        public TimeLanguageStrings()
        {
            this.ListStrings = new List<TimeLanguageString>();
        }
        public void Add(TimeLanguageString objTimeLanguageString)
        {
            this.ListStrings.Add(objTimeLanguageString);
        }
    }

    public class TimeLanguageString
    {
        public enum TimeSpanElement { NONE = 0, DAYS, HOURS, MINUTS, SECONDS, MILISECONDS }
        public TimeSpanElement TimeElement;
        public String SingularDescription;
        public String PluralDescription;
        public TimeLanguageString()
        {
            this.TimeElement = TimeSpanElement.NONE;
            this.SingularDescription = "";
            this.PluralDescription = "";
        }
    }

    #endregion TimeSpanLanguageCollection
}