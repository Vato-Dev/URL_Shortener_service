using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.SqlEfCore.ValueConverters;

public sealed class GuidToByteValueConverter() 
    : ValueConverter<Guid, byte[]>
        (  to => to.ToByteArray(),
        from => new Guid(from)  );
