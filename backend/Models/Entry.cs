using System;
using System.ComponentModel.DataAnnotations;

namespace BabyPoll.Models
{
    public class Entry
    {
        [Key]
        public Guid? EntryId { get; set; }
        public Guid? PollId { get; set; }
        public string Participant { get; set; }
        public DateTime Guess { get; set; }
    }
}