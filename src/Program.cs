using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnetcore3_cosmos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the EFCore Cosmos DB Provider...");

            for (int i = 0; i < 1000; i++)
            {
                var job = new Job
                {
                    Id = Guid.NewGuid(),
                    Address = new Address
                    {
                        Line1 = $"{i} Some Street {i}",
                        Line2 = $"Somewhere{i}",
                        Town = "Birmingham",
                        PostCode = "B90 {i}SS",
                    },
                    Contacts = new List<Contact>()
                    {
                        new Contact { Title = "Mr", FirstName = $"Craig {i}", LastName = "Mellon", TelephoneNumber = "34441234" },
                        new Contact { Title = "Mrs", FirstName = $"Cara {i}", LastName = "Mellon", TelephoneNumber = "53665554" }
                    },
                    AssignedResource = new Resource
                    {
                        Id = Guid.NewGuid(),
                        Title = "Mr",
                        FirstName = "Bob",
                        LastName = "Builder",
                        TelephoneNumber = "0800 1234567"
                    }
                };
                using (var context = new JobContext())
                {
                    context.Database.EnsureCreated();
                    context.Add(job);
                    context.SaveChanges();
                }
            }

            using (var context = new JobContext())
            {
                var job = context.Jobs.First();
                // now load the resource and assign it to the Job
                var resource1 = context.Resources.First(x => x.Id == job.AssignedResourceId);
                job.AssignedResource = resource1;
                Console.WriteLine($"Job created and retrieved with address: {job.Address.Line1}, {job.Address.PostCode}");
                Console.WriteLine($"  Contacts ({job.Contacts.Count()})");
                job.Contacts.ForEach(x =>
                {
                    Console.WriteLine($"    Name: {x.FirstName} {x.LastName}");
                });
                Console.WriteLine($"  Assigned Resource: {job.AssignedResource?.FirstName} {job.AssignedResource?.LastName}");
            }



            var resourceId = Guid.NewGuid();
            var resource = new Resource
            {
                Id = resourceId,
                Title = "Mr",
                FirstName = "Bob",
                LastName = "Builder",
                TelephoneNumber = "0800 1234567"
            };
            var job1 = new Job
            {
                Id = Guid.NewGuid(),
                Address = new Address
                {
                    Line1 = "Job 1 Address"
                },
                AssignedResource = resource
            };
            var job2 = new Job
            {
                Id = Guid.NewGuid(),
                Address = new Address
                {
                    Line1 = "Job 2 Address"
                },
                AssignedResource = resource
            };
            using (var context = new JobContext())
            {
                context.Database.EnsureCreated();
                context.Add(job1);
                context.Add(job2);
                context.SaveChanges();
            }
            using (var context = new JobContext())
            {
                var loadedResource = context.Resources.First(x => x.Id == resourceId);
                // Load all jobs with the same assigned resource id
                var jobs = context.Jobs.Where(x => x.AssignedResourceId == resourceId).ToList();
                jobs.ForEach(job =>
                {
                    Console.WriteLine($"Job: {job.Id} - Resource: {job.AssignedResource?.FirstName} {job.AssignedResource?.FirstName}");
                });
            }
        }
    }
}
