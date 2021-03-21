using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace SubRipAdjuster.Models
{

    public class SubRipFile
    {
        /// <summary>
        /// List of all subtitles in the file.
        /// </summary>
        public List<Subtitle> Subtitles = new List<Subtitle>();



        /// <summary>
        /// Parses an .srt file.
        /// </summary>
        /// <param name="path">Path to the file</param>
        public SubRipFile(HttpPostedFileBase file)
        {
            try
            {
                BinaryReader b = new BinaryReader(file.InputStream);
                byte[] binData = b.ReadBytes(file.ContentLength);

                string result = System.Text.Encoding.UTF8.GetString(binData);

                string[] lines = Regex.Replace(result, "\r\n?", "\n").Split('\n');

                int state = 0;
                bool wasEmptyLine = false;

                Subtitle currentSubtitle = null;

                for (int i = 0; i < lines.Length; i++)
                {
                    int id;
                    bool isLineNumber = int.TryParse(lines[i], out id);

                    if (isLineNumber && wasEmptyLine)
                    {
                        state = 0;
                        wasEmptyLine = false;

                        Subtitles.Add(currentSubtitle);
                        currentSubtitle = null;
                    }
                    switch (state)
                    {
                        case 0: //Start of new subtitle
                            if (lines[i] != "")
                            {
                                currentSubtitle = new Subtitle(id);
                                state++;
                            }
                            break;
                        case 1: //Subtitle time
                            currentSubtitle.ParseTime(lines[i]);
                            state++;
                            break;
                        case 2:
                            if (lines[i] != "")  //Subtitle
                            {
                                currentSubtitle.Lines.Add(lines[i]);
                            }
                            else //Empty Line
                            {
                                wasEmptyLine = true;
                            }
                            break;
                    }
                }
                if (currentSubtitle != null) 
                    Subtitles.Add(currentSubtitle);
            }
            catch (Exception ex)
            {
                throw new Exception("Arquivo .srt invalido ou inexistente", ex);
            }
        }


        /// <summary>
        /// Renders this object to a .srt file format.
        /// </summary>
        /// <returns>string containing the contents of the whole file</returns>
        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < Subtitles.Count; i++) 
                sb.Append(Subtitles[i].ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Offset the times of this object by a number of milliseconds.
        /// </summary>
        public void Add(int ms)
        {
            foreach (var item in Subtitles)
                item.Add(ms);
        }

        /// <summary>
        /// Writes the render of this object to specified path.
        /// </summary>
        public void SaveAs(string path)
        {
            File.WriteAllText(path, Render());
        }
    }
}
