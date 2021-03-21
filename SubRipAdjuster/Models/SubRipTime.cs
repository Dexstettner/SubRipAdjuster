using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubRipAdjuster.Models
{
    public class SubRipTime
    {
        private static int
            msInSecond = 1000,
            msInMinute = 60 * msInSecond,
            msInHour = 60 * msInMinute;

        public int Hours, Minutes, Seconds, Milliseconds;

        /// <summary>
        /// Creates a new SRTTime object from a string.
        /// </summary>
        /// <param name="strTime">string in hh:mm:ss,milli format</param>
        public SubRipTime(string strTime)
        {
            Parse(strTime);
        }

        /// <summary>
        /// Offsets the time by a number of milliseconds.
        /// </summary>
        public void Add(int milliseconds)
        {
            TotalMilliseconds = TotalMilliseconds + milliseconds;
            if(TotalMilliseconds < 0)
            {
                throw new Exception("Tempo não pode ser negativo");
            }
        }

        /// <summary>
        /// Parses a string in hh:mm:ss,milli format
        /// </summary>
        public void Parse(string strTime)
        {
            strTime = strTime.Trim();
            int index1 = strTime.IndexOf(':');
            int index2 = strTime.IndexOf(':', index1 + 1);
            int index3 = strTime.IndexOf(',', index2 + 1);

            
            Hours = int.Parse(strTime.Substring(0, index1));
            Minutes = int.Parse(strTime.Substring(index1 + 1, index2 - index1 - 1));
            Seconds = int.Parse(strTime.Substring(index2 + 1, index3 - index2 - 1));
            Milliseconds = int.Parse(strTime.Substring(index3 + 1, strTime.Length - index3 - 1));
        }

        /// <summary>
        /// Gets or sets the total amount of milliseconds counted from 0:0:0,0
        /// </summary>
        public int TotalMilliseconds
        {
            get
            {
                return Milliseconds +
                Seconds * msInSecond +
                Minutes * msInMinute +
                Hours * msInHour;
            }
            set
            {
                Hours = (value - value % msInHour) / msInHour;
                value -= Hours * msInHour;

                Minutes = (value - value % msInMinute) / msInMinute;
                value -= Minutes * msInMinute;

                Seconds = (value - value % msInSecond) / msInSecond;
                value -= Seconds * msInSecond;

                Milliseconds = value;
            }
        }

        /// <summary>
        /// Renders SubRipTime object to a string hh:mm:ss,milli
        /// </summary>
        /// <returns></returns>
        public string RenderToString()
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2},{3:D3}", Hours, Minutes, Seconds, Milliseconds);
        }
        
    }
}
