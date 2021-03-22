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
        [Display(Name = "ID")]
        public int Id { get => id; set => id = value; }

        [Display(Name = "File Name")]
        public string ArquiveName { get => arquiveName; set => arquiveName = value; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime InsertDate { get => insertDate; set => insertDate = value; }

        [Display(Name = "File")]
        public byte[] ArquiveFile { get => arquiveFile; set => arquiveFile = value; }
        [Display(Name = "Milliseconds Shifted")] 
        public int Ms { get => ms; set => ms = value; }
    }
}