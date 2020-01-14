using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServerHw2.Domain
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string User { get; set; }
        public string Text { get; set; }
    }
}
