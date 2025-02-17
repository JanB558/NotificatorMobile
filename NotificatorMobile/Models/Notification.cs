﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificatorMobile.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime TimeAndDate { get; set; }
        public bool IsRecurring { get; set; }
    }
}
