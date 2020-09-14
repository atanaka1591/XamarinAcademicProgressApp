using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace C971Project
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public enum Status { InProgress, Completed, Upcoming }
        public Status TermStatus { get; set; }
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }

        public Term(string title, Status termstatus, DateTime termstart, DateTime termend)
        {
            Title = title;
            TermStatus = termstatus;
            TermStart = termstart;
            TermEnd = termend;
        }

        public Term()
        {

        }
    }
}
