using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.SqlEfCore.ValueConverters;

public sealed class DateTimeToUtcValueConverter()
    : ValueConverter<DateTime, DateTime>(
        to => to.ToUniversalTime(),
        from => DateTime.SpecifyKind(from, DateTimeKind.Utc));