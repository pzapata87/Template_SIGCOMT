using System.Data.Entity;

namespace SIGCOMT.Persistence.EntityFramework
{
    public class ContextInitializer : DropCreateDatabaseAlways<DbContextBase>
    {
        protected override void Seed(DbContextBase context)
        {
            context.Database.ExecuteSqlCommand(@"
            CREATE TABLE [Log4Net] (
                [Id] [int] IDENTITY (1, 1) NOT NULL,
                [Date] [datetime] NOT NULL,
                [Thread] [varchar](255) NOT NULL,
                [Level] [varchar] (50) NOT NULL,
                [Logger] [varchar] (255) NOT NULL,
                [UserName] [varchar] (100) NULL,
                [Message] [varchar] (4000) NOT NULL,
                [Exception] [varchar] (4000) NULL
            )");
        }
    }
}