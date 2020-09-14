using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971Project
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }      
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public enum Status { InProgress, Completed, Upcoming, None }
        public Status CurrentStatus { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string OAName { get; set; }
        public DateTime OAStart { get; set; }
        public DateTime OAEnd { get; set; }
        public Status OAStatus { get; set; }
        public string OANotes { get; set; }
        public string PAName { get; set; }
        public DateTime PAStart { get; set; }
        public DateTime PAEnd { get; set; }
        public Status PAStatus { get; set; }
        public string PANotes { get; set; }

    }
}
