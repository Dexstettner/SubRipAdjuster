using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SubRipAdjuster.Models
{
    public class ArquiveHistory
    {
        private int id;
        private String arquiveName;
        private DateTime insertDate;
        private int ms;
        private byte[] arquiveFile;

        public ArquiveHistory()
        { }
            public ArquiveHistory(string arquiveName, DateTime insertDate, byte[] arquiveFile, int ms)
        {
            ArquiveName = arquiveName;
            InsertDate = insertDate;
            ArquiveFile = arquiveFile;
            Ms = ms;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get => id; set => id = value; }
        public string ArquiveName { get => arquiveName; set => arquiveName = value; }
        [DataType(DataType.Date)]
        public DateTime InsertDate { get => insertDate; set => insertDate = value; }
        public byte[] ArquiveFile { get => arquiveFile; set => arquiveFile = value; }
        public int Ms { get => ms; set => ms = value; }
    }
}