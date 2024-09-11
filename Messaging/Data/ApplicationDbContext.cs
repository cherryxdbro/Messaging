using Messaging.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Data;

/// <summary>
/// The database context for the application, managing access to the database tables.
/// This context includes messages and data protection keys.
/// </summary>
/// <param name="dbContextOptions">Configuration options for the database context.</param>
public class ApplicationDatabaseContext(DbContextOptions dbContextOptions)
    : DbContext(options: dbContextOptions),
        IDataProtectionKeyContext
{
    /// <summary>
    /// A dataset used to store data protection keys.
    /// The keys are necessary for protecting sensitive information such as cookies and authentication tokens.
    /// </summary>
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    /// <summary>
    /// A dataset containing chat messages.
    /// Manages the addition, updating, and deletion of message records.
    /// </summary>
    public DbSet<Message> Messages { get; set; }
}
