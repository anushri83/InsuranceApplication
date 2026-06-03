//Check for expired policies.
//Mark them as Expired.
//Send expiration emails.
//Send reminder emails for policies expiring in 7 days.
//Repeat this process every 24 hours.

using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Insurance.Infrastructure.Data;

namespace Insurance.Api.BackgroundServices
{
    // BackgroundService is a built-in ASP.NET Core class that runs continuously in the background.
    public class PolicyExpiryWorker : BackgroundService
    {
        // IServiceProvider allows us to access registered services such as DbContext, UserService and EmailService.
        private readonly IServiceProvider _services;

        // Constructor Dependency Injection
        public PolicyExpiryWorker(IServiceProvider services)
        {
            _services = services;
        }

        // ExecuteAsync() automatically starts when the application starts independently of user requests.
        //A CancellationToken is used to notify a running task that the application is shutting down so it can stop gracefully.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Keep running until the application shuts down.
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    try
            //    {
            //        // Create a new scope because AppDbContext is a scoped service.
            //        using (var scope = _services.CreateScope())
            //        {
            //            // Get database context from dependency injection.
            //            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //            // Get email service from dependency injection.
            //            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            //            // Current UTC date and time.
            //            var today = DateTime.UtcNow;

            //            //  Find expired active policies
            //            var expiredPolicies = await context.CustomerPolicies

            //                // Load User information
            //                .Include(p => p.User)

            //                // Load Policy information
            //                .Include(p => p.Policy)

            //                // Find policies that:Are still Active and EndDate has already passed
            //                .Where(p =>p.Status == CustomerPolicyStatus.Active && p.EndDate < today)

            //                .ToListAsync(stoppingToken);

            //            //  Update status and send expiration email
            //            foreach (var policy in expiredPolicies)
            //            {
            //                // Change policy status
            //                policy.Status = CustomerPolicyStatus.Expired;

            //                // Update modification timestamp
            //                policy.UpdatedAt = today;

            //                // Send email notification
            //                if (policy.User != null &&
            //                    policy.Policy != null)
            //                {
            //                    await emailService.SendOtpEmailAsync(
            //                        policy.User.Email,

            //                        $"Your policy '{policy.Policy.PolicyName}' " +
            //                        $"has expired. Please log in and renew it."
            //                    );
            //                }
            //            }

            //            // Find policies expiring in 7 days
                        
            //            var warningDate = today.AddDays(7);

            //            var upcomingExpirations = await context.CustomerPolicies

            //                .Include(p => p.User)
            //                .Include(p => p.Policy)

            //                .Where(p =>
            //                    p.Status == CustomerPolicyStatus.Active &&
            //                    p.EndDate.Date == warningDate.Date)

            //                .ToListAsync(stoppingToken);

            //            //  Send renewal reminder email
            //            foreach (var policy in upcomingExpirations)
            //            {
            //                if (policy.User != null &&
            //                    policy.Policy != null)
            //                {
            //                    await emailService.SendOtpEmailAsync(
            //                        policy.User.Email,

            //                        $"Your policy '{policy.Policy.PolicyName}' " +
            //                        $"will expire in 7 days. Renew now to keep " +
            //                        $"your No-Claim Bonus (NCB) benefits."
            //                    );
            //                }
            //            }

            //            // status updates to database
            //            if (expiredPolicies.Any())
            //            {
            //                await context.SaveChangesAsync(stoppingToken);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
 
            //        // Prevents application crash if worker fails.
            //        Console.WriteLine(
            //            $"[CRITICAL] Policy Worker Error: {ex.Message}"
            //        );
            //    }

            //    //Sleep for 24 hours before checking again.
            //    await Task.Delay(
            //        TimeSpan.FromDays(1),
            //        stoppingToken
            //    );
            //}
        }
    }
}
