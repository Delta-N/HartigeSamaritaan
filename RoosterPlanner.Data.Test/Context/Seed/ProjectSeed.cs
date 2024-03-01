using System;
using System.Collections.Generic;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class ProjectSeed
    {
        public static List<Project> Seed()
        {
            List<Project> projects = new List<Project>
            {
                new(Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe"))
                {
                    Name = "Voorburg 2020", Address = "Stationsplein 2", City = "Voorburg",
                    Description = "Leuk project in Voorburg",
                    ParticipationStartDate = DateTime.Now.AddDays(1),
                    ParticipationEndDate = DateTime.Now.AddDays(29),
                    ProjectStartDate = DateTime.Now,
                    ProjectEndDate = DateTime.Now.AddDays(30),
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now,
                    PictureUriId = Guid.Parse("ad6e3143-ba49-48b5-af06-97ef6cea08aa"),
                },

                new(Guid.Parse("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"))
                {
                    Name = "Den Haag 2018", Address = "Laan van Waalhaven 450", City = "Den Haag",
                    Description = "Leuk project in Den Haag",
                    ParticipationStartDate = DateTime.Now.AddDays(1),
                    ParticipationEndDate = DateTime.Now.AddDays(29),
                    ProjectStartDate = DateTime.Now,
                    ProjectEndDate = DateTime.Now.AddDays(30),
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now,
                    PictureUriId = Guid.Parse("ad6e3143-ba49-48b5-af06-97ef6cea08aa"),
                },
                new(Guid.Parse("9d9e80f6-2995-446d-ac71-b16eac2870bf"))
                {
                    Name = $"Project_{DateTime.Now:yyyyddMM-HHmm}",
                    Description = "Description",
                    Address = "Address",
                    City = "City",
                    WebsiteUrl = "Https://website.com",
                    Closed = false,
                    ContactAdres = "Contact@adres.com",
                    ParticipationStartDate = DateTime.Today.AddDays(-7),
                    ProjectStartDate = DateTime.Today.AddDays(-7),
                    ParticipationEndDate = DateTime.Today,
                    ProjectEndDate = DateTime.Today,
                    LastEditBy = "System",
                    LastEditDate = DateTime.UtcNow.Date,
                    PictureUriId = Guid.Parse("ad6e3143-ba49-48b5-af06-97ef6cea08aa"),
                }
            };
            

            foreach (Project project in projects)
            {
                
                //add random number of shifts each day
                DateTime current = project.ParticipationStartDate;
                while (current <= project.ParticipationEndDate)
                {
                    int numberOfShiftsThisDay = Helper.RandomNumberFromRange(0,4);
                    for (int i = 0; i < numberOfShiftsThisDay; i++)
                    {
                        Task task = Helper.ReturnRandomEntity(TaskSeed.Seed());
                        int startHour = Helper.RandomNumberFromRange(0, 24);
                        Shift shift = new Shift(Guid.NewGuid())
                        {
                            Date = current,
                            StartTime = new TimeSpan(startHour, 0, 0),
                            EndTime = new TimeSpan(Helper.RandomNumberFromRange(startHour, 24), 0, 0),
                            TaskId = task.Id,
                            ParticipantsRequired = Helper.RandomNumberFromRange(0, 10),
                            ProjectId = project.Id
                            
                        };
                        //add availabilities to shift
                        foreach (Person person in PersonSeed.Seed())
                        {
                            //fill availabilities list
                            shift.Availabilities.Add(new Availability(Guid.NewGuid())
                            {

                                ParticipationId = Helper.ConcatGuid(project.Id,person.Id),
                                Shift = shift,
                                ShiftId = shift.Id,
                                Type = Helper.RandomType(),
                                Preference = Helper.RandomNumberFromRange(0, 1) == 1,
                                PushEmailSend = false
                            });
                        }

                        project.Shifts.Add(shift);
                    }

                    current = current.AddDays(1);
                }

                foreach (Shift shift in project.Shifts)
                {
                    bool found = false;
                    foreach (ProjectTask projectTask in project.ProjectTasks)
                    {
                        if (projectTask.TaskId == shift.TaskId)
                            found = true;
                    }

                    if (!found)
                        project.ProjectTasks.Add(new ProjectTask(Guid.NewGuid())
                        {
                            Project = project,
                            ProjectId = project.Id,
                            Task = shift.Task,
                            TaskId = (Guid) shift.TaskId
                        });
                }
            }

            return projects;
        }
    }
}