﻿using System;
using System.Collections.Generic;

namespace DBFirstApp.Models
{
    public partial class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int LearnerId { get; set; }
        public float Grade { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Learner Learner { get; set; } = null!;
    }
}
