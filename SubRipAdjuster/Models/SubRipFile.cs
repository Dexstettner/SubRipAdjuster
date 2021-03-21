using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;

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
        public SubRipFile(byte[] binData)
        {
            try
            {
                String file = System.Text.Encoding.UTF8.GetString(binData);
                string[] lines = Regex.Replace(file, "\r\n?", "\n").Split('\n');

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
        /// Writes the render of this file with a SaveFileDialog in a new thread.
        /// </summary>
        public Tuple<String, String, String> OpenDialogSaveAs()
        {

            SaveFileDialog sfd = new SaveFileDialog();
            Tuple<String, String, String> status = new Tuple<String, String, String>("","", "");

            Thread t = new Thread((ThreadStart)delegate
            {
                sfd.Filter = "SubRip Files (*.srt)|*.srt|All files (*.*)|*.*";
                sfd.DefaultExt = ".srt";
                sfd.FilterIndex = 2;
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, Render());
                    status = new Tuple<String, String, String>("Sucesso", "Arquivo salvo com sucesso em: " + Path.GetFullPath(sfd.FileName), sfd.FileName);
                }
                else
                {
                    status = new Tuple<String, String, String>("Error", "Ação salvar arquivo abortado","");
                }
            });

            t.TrySetApartmentState(ApartmentState.STA);

            //start the thread 
            t.Start();

            // Wait for thread to get started 
            while (!t.IsAlive) { Thread.Sleep(1); }
            // Wait a tick more
            Thread.Sleep(1);
            //wait for the dialog thread to finish 
            t.Join();

            return status;
        }

        /// <summary>
        /// Convert a string to byte array( byte[] )
        /// </summary>
        public static byte[] StringToByte(String stringData)
        {
            return Encoding.UTF8.GetBytes(stringData);
        }
        /// <summary>
        /// Convert a HttpPostedFileBase to byte array( byte[] )
        /// </summary>
        public static byte[] FileToByte(HttpPostedFileBase file)
        {
            BinaryReader b = new BinaryReader(file.InputStream);
            return b.ReadBytes(file.ContentLength);
        }

    }
}
