using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Words_Learning.Models
{
    public class Words
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Transcriptions { get; set; }
        public string Translation { get; set; }
        public byte[] Image { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
