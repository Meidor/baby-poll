using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BabyPoll.Models
{
    public class Poll
    {
        [Key]
        public Guid PollId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsOpen { get; set; }
        public DateTime DueDate { get; set; }
        public List<Entry> Entries { get; set; } = new List<Entry>();
    }
}