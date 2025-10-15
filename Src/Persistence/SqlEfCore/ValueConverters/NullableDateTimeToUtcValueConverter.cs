using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.SqlEfCore.ValueConverters;

public sealed class NullableDateTimeToUtcValueConverter() : 
    ValueConverter<DateTime?, DateTime?>(
        to => to.HasValue ? to.Value.ToUniversalTime() : to,
        from => from.HasValue ? DateTime.SpecifyKind(from.Value, DateTimeKind.Utc) : from);
