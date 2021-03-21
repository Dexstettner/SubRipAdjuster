using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubRipAdjuster.Models
{
    public class Subtitle
    {
        private int id;
        private SubRipTime startTime, endTime;
        private List<string> lines = new List<string>();

        public int Id { get => id; set => id = value; }
        public SubRipTime StartTime { get => startTime; set => startTime = value; }
        public SubRipTime EndTime { get => endTime; set => endTime = value; }
        public List<string> Lines { get => lines; set => lines = value; }



        /// <summary>
        /// Gets or sets the text of the subtitle
        /// </summary>
        /// 

        public string Text
        {
            get
            {
                return string.Join(Environment.NewLine, Lines.ToArray());
            }
            set
            {
                Lines.Clear();
                Lines.AddRange(value.Split(new string[] { "\r\n" }, StringSplitOptions.None));
            }
        }

        public Subtitle(int seqNum)
        {
            Id = seqNum;
        }
        public void ParseTime(string line)
        {
            int markerindex = line.IndexOf(" --> ");
            StartTime = new SubRipTime(line.Substring(0, markerindex));
            EndTime = new SubRipTime(line.Substring(markerindex + 5, line.Length - markerindex - 5));
        }


        /// <summary>
        /// Offset the times of this subtitle by a number of milliseconds.
        /// </summary>
        public void Add(int ms)
        {
            StartTime.Add(ms);
            EndTime.Add(ms);
        }

        /// <summary>
        /// Render the object to .srt format.
        /// </summary>
        /// <returns>the rendered subtitle, with sequence number, times and subtitle itself</returns>
        public string Render()
        {
            return string.Format("{0}\r\n{1} --> {2}\r\n{3}\r\n\r\n",
                Id,
                StartTime.RenderToString(),
                EndTime.RenderToString(),
                Text);
        }

        public override string ToString()
        {
            return Render();
        }
    }
}