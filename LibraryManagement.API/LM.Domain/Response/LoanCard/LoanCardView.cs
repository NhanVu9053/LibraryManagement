﻿using System;

namespace LM.Domain.Response.LoanCard
{
    public class LoanCardView : ResView
    {
        public int LoanCardId { get; set; }
        public DateTime LoanOfDate { get; set; }
        public string LoanOfDateStr { get; set; }
        public DateTime ReturnOfDate { get; set; }
        public string ReturnOfDateStr { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public int Books { get; set; }
        public string AvatarPath { get; set; }
    }
}
