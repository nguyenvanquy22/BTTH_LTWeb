using Lab2.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Data
{
    public class DbInitializer
    {
        public static void Initialize (IServiceProvider serviceProvider)
        {
            using (var context = new SchoolContext(serviceProvider
                .GetRequiredService<DbContextOptions<SchoolContext>>()))
            {
                context.Database.EnsureCreated();
                if (context.Learners.Any())
                {
                    return;
                }

                var majors = new Major[]
                {
                    new Major {MajorName="IT"},
                    new Major {MajorName="Economics"},
                    new Major {MajorName="Mathematics"},
                };
                foreach (Major major in majors) 
                {
                    context.Majors.Add(major);
                }
                context.SaveChanges();

                var learners = new Learner[]
                {
                    new Learner {FirstMidName="Carson", LastName="Alexander", EnrollmentDate=DateTime.Parse("2005-09-01"), MajorID=1},
                    new Learner {FirstMidName="Economics", LastName="Alexander", EnrollmentDate=DateTime.Parse("2002-09-01"), MajorID=2},
                };
                foreach (Learner learner in learners)
                {
                    context.Learners.Add(learner);
                }
                context.SaveChanges();

                var courses = new Course[]
                {
                    new Course {CourseID=1050, Title="Chemistry", Credits=3},
                    new Course {CourseID=4022, Title="Microeconomics", Credits=3},
                    new Course {CourseID=4041, Title="Macroeconomics", Credits=3},
                };
                foreach (Course course in courses)
                {
                    context.Courses.Add(course);
                }
                context.SaveChanges();

                var enrollments = new Enrollment[]
                {
                    new Enrollment {LearnerID=1, CourseID=1050, Grade=5.5f},
                    new Enrollment {LearnerID=1, CourseID=4022, Grade=7f},
                    new Enrollment {LearnerID=2, CourseID=1050, Grade=8.5f},
                    new Enrollment {LearnerID=2, CourseID=4041, Grade=6.2f},
                };
                foreach (Enrollment enrollment in enrollments)
                {
                    context.Enrollments.Add(enrollment);
                }
                context.SaveChanges();
            }
        }
    }
}
