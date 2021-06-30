// <copyright file="TimersHelper.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiStorage.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// Time intervals.
    /// </summary>
    public enum TimeValuesEnum
    {
        /// <summary>Days.</summary>
        Days = 0,

        /// <summary>Hours.</summary>
        Hours = 1,

        /// <summary>Minutes.</summary>
        Minutes = 2,

        /// <summary>Seconds.</summary>
        Seconds = 3,

        /// <summary>Milliseconds.</summary>
        Milliseconds = 4,
    }

    /// <summary>
    /// Timers Helper.
    /// </summary>
    public static class TimeHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed.")]
        public static string? zzDebug { get; set; }

        /// <summary>
        /// TimeShodTrigger.
        /// </summary>
        /// <param name="lastrun">datetime of last run.</param>
        /// <param name="time">Time value to trigger on.</param>
        /// <param name="interval">time interval in minutes.</param>
        /// <returns>True if time has ended. false if time not have ended.</returns>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:BracesMustNotBeOmitted", Justification = "Reviewed.")]
        public static bool TimeShodTrigger(DateTime lastrun, TimeValuesEnum time, ushort interval)
        {
            if (time != TimeValuesEnum.Days &&
                time != TimeValuesEnum.Hours &&
                time != TimeValuesEnum.Minutes &&
                time != TimeValuesEnum.Seconds &&
                time != TimeValuesEnum.Milliseconds)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }

                throw new NotImplementedException();
            }

            if (time == TimeValuesEnum.Days && (DateTime.UtcNow - lastrun).TotalDays >= interval)
                return true;
            else if (time == TimeValuesEnum.Hours && (DateTime.UtcNow - lastrun).TotalHours >= interval)
                return true;
            else if (time == TimeValuesEnum.Minutes && (DateTime.UtcNow - lastrun).TotalMinutes >= interval)
                return true;
            else if (time == TimeValuesEnum.Seconds && (DateTime.UtcNow - lastrun).TotalSeconds >= interval)
                return true;
            else if (time == TimeValuesEnum.Milliseconds && (DateTime.UtcNow - lastrun).TotalMilliseconds >= interval)
                return true;

            return false;
        }
    }
}
