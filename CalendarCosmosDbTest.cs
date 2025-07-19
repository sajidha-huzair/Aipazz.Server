using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Aipazz.Domian;
using Aipazz.Domian.Calender;
using Aipazz.Infrastructure.Calendar;
using AIpazz.Infrastructure.Calender;

namespace CalendarCosmosDbTest
{
    public class CalendarRepositoryTest
    {
        private readonly CosmosClient _cosmosClient;
        private readonly CosmosDbOptions _cosmosDbOptions;

        public CalendarRepositoryTest()
        {
            _cosmosDbOptions = new CosmosDbOptions
            {
                AccountEndpoint = "https://sajidha.documents.azure.com:443/",
                AuthKey = "jmGmjb2etmN4KhQKz4BaoI9nRkiSlD6F9AIjrSvefhKjApOKwlXDCL5RwU8Gjrmg7kDqLhvGBA5CACDb35hpjQ==",
                DatabaseName = "Aipazz",
                Containers = new Dictionary<string, string>
                {
                    { "CourtDateForm", "CourtDateForm" },
                    { "FilingsDeadlineForm", "FilingsDeadlineForm" },
                    { "TeamMeetingForm", "TeamMeetingForm" },
                    { "ClientMeeting", "ClientMeeting" }
                }
            };

            _cosmosClient = new CosmosClient(_cosmosDbOptions.AccountEndpoint, _cosmosDbOptions.AuthKey);
        }

        public async Task TestCourtDateFormRepository()
        {
            var options = Options.Create(_cosmosDbOptions);
            var repository = new CourtDateFormRepository(_cosmosClient, options);

            // Test adding a court date form
            var courtDateForm = new CourtDateForm
            {
                CaseNumber = "TEST-001",
                CourtName = "Test Court",
                Date = DateTime.UtcNow.AddDays(10),
                Description = "Test court hearing"
            };

            try
            {
                await repository.AddCourtDateForm(courtDateForm);
                Console.WriteLine("✓ CourtDateForm added successfully");

                // Test retrieving by ID
                var retrieved = await repository.GetById(courtDateForm.Id);
                if (retrieved != null)
                {
                    Console.WriteLine("✓ CourtDateForm retrieved successfully");
                }

                // Test getting all
                var all = await repository.GetAll();
                Console.WriteLine($"✓ Retrieved {all.Count} court date forms");

                // Test updating
                courtDateForm.Description = "Updated description";
                var updated = await repository.UpdateCourtDateForm(courtDateForm.Id, courtDateForm);
                if (updated != null)
                {
                    Console.WriteLine("✓ CourtDateForm updated successfully");
                }

                // Test deleting
                var deleted = await repository.DeleteCourtDateForm(courtDateForm.Id);
                if (deleted)
                {
                    Console.WriteLine("✓ CourtDateForm deleted successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error testing CourtDateForm: {ex.Message}");
            }
        }

        public async Task TestFilingsDeadlineFormRepository()
        {
            var options = Options.Create(_cosmosDbOptions);
            var repository = new FilingsDeadlineFormRepository(_cosmosClient, options);

            var form = new FilingsDeadlineForm
            {
                Title = "Test Filing",
                Date = DateTime.UtcNow.AddDays(5),
                Time = "10:00 AM",
                Reminder = "1 day before",
                Description = "Test filing deadline",
                AssignedMatter = "TEST-MATTER-001"
            };

            try
            {
                await repository.Add(form);
                Console.WriteLine("✓ FilingsDeadlineForm added successfully");

                var retrieved = await repository.GetById(form.Id);
                if (retrieved != null)
                {
                    Console.WriteLine("✓ FilingsDeadlineForm retrieved successfully");
                }

                var all = await repository.GetAll();
                Console.WriteLine($"✓ Retrieved {all.Count} filing deadline forms");

                form.Description = "Updated filing description";
                var updated = await repository.Update(form.Id, form);
                if (updated != null)
                {
                    Console.WriteLine("✓ FilingsDeadlineForm updated successfully");
                }

                var deleted = await repository.Delete(form.Id);
                if (deleted)
                {
                    Console.WriteLine("✓ FilingsDeadlineForm deleted successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error testing FilingsDeadlineForm: {ex.Message}");
            }
        }

        public async Task TestTeamMeetingFormRepository()
        {
            var options = Options.Create(_cosmosDbOptions);
            var repository = new TeamMeetingFormRepository(_cosmosClient, options);

            var form = new TeamMeetingForm
            {
                Title = "Test Team Meeting",
                Date = DateTime.UtcNow.AddDays(3),
                Time = "2:00 PM",
                Repeat = "Weekly",
                Reminder = "15 minutes before",
                Description = "Test team meeting description",
                VideoConferencingLink = "https://teams.microsoft.com/test",
                LocationLink = "https://maps.google.com/test",
                TeamMembers = new List<string> { "test@example.com", "test2@example.com" }
            };

            try
            {
                await repository.Add(form);
                Console.WriteLine("✓ TeamMeetingForm added successfully");

                var retrieved = await repository.GetById(form.Id);
                if (retrieved != null)
                {
                    Console.WriteLine("✓ TeamMeetingForm retrieved successfully");
                }

                var all = await repository.GetAll();
                Console.WriteLine($"✓ Retrieved {all.Count} team meeting forms");

                form.Description = "Updated team meeting description";
                var updated = await repository.Update(form.Id, form);
                if (updated != null)
                {
                    Console.WriteLine("✓ TeamMeetingForm updated successfully");
                }

                var deleted = await repository.Delete(form.Id);
                if (deleted)
                {
                    Console.WriteLine("✓ TeamMeetingForm deleted successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error testing TeamMeetingForm: {ex.Message}");
            }
        }

        public async Task TestClientMeetingRepository()
        {
            var options = Options.Create(_cosmosDbOptions);
            var repository = new Clientmeetingrepository(_cosmosClient, options);

            var meeting = new ClientMeeting(
                id: Guid.NewGuid(),
                title: "Test Client Meeting",
                date: DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                time: TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                repeat: false,
                reminder: TimeSpan.FromMinutes(30),
                description: "Test client meeting description",
                meetingLink: "https://teams.microsoft.com/test-client",
                location: "Test Location",
                teamMembers: new List<string> { "team@example.com" },
                clientEmail: "client@example.com"
            );

            try
            {
                await repository.AddClientMeeting(meeting);
                Console.WriteLine("✓ ClientMeeting added successfully");

                var retrieved = await repository.GetClientMeetingByID(meeting.Id);
                if (retrieved != null)
                {
                    Console.WriteLine("✓ ClientMeeting retrieved successfully");
                }

                var all = await repository.GetAllClientMeetings();
                Console.WriteLine($"✓ Retrieved {all.Count} client meetings");

                var updated = await repository.UpdateClientMeeting(meeting);
                if (updated != null)
                {
                    Console.WriteLine("✓ ClientMeeting updated successfully");
                }

                var deleted = await repository.DeleteClientMeeting(meeting.Id);
                if (deleted)
                {
                    Console.WriteLine("✓ ClientMeeting deleted successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error testing ClientMeeting: {ex.Message}");
            }
        }

        public async Task RunAllTests()
        {
            Console.WriteLine("=== Calendar Cosmos DB Repository Tests ===");
            Console.WriteLine();

            Console.WriteLine("Testing CourtDateFormRepository...");
            await TestCourtDateFormRepository();
            Console.WriteLine();

            Console.WriteLine("Testing FilingsDeadlineFormRepository...");
            await TestFilingsDeadlineFormRepository();
            Console.WriteLine();

            Console.WriteLine("Testing TeamMeetingFormRepository...");
            await TestTeamMeetingFormRepository();
            Console.WriteLine();

            Console.WriteLine("Testing ClientMeetingRepository...");
            await TestClientMeetingRepository();
            Console.WriteLine();

            Console.WriteLine("=== Tests Completed ===");
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var test = new CalendarRepositoryTest();
            await test.RunAllTests();
        }
    }
}
