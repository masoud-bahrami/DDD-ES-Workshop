using System;
using System.Globalization;

namespace Zero.EventSourcing.InMemoryEventStore
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcDateTimeNow();
        DateTimeOffset Yesterday();
        DateTimeOffset Friday();
        DateTimeOffset OneWeekFromNow();
        (short PersianYear, short PersianMonth) PersianYearMonth();
    }

    namespace Quantum.Core
    {
        public class DateTimeProvider : IDateTimeProvider
        {

            public DateTimeOffset UtcDateTimeNow()
                => Now();

            public DateTimeOffset Yesterday()
                => Now().AddDays(-1);
            public DateTimeOffset Friday()
                => Now().AddDays(1);
            public DateTimeOffset OneWeekFromNow()
                => Now().AddDays(7);

            public virtual DateTimeOffset Now() => DateTimeOffset.UtcNow;


            public (short PersianYear, short PersianMonth) PersianYearMonth()
            {
                var dateTime = UtcDateTimeNow().DateTime;
                var persianCalendar = new PersianCalendar();
                return new((short)persianCalendar.GetYear(dateTime), (short)persianCalendar.GetMonth(dateTime));
            }
        }
    }

}