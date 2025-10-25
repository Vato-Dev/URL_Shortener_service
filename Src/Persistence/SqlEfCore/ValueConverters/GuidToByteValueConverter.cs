using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.SqlEfCore.ValueConverters;

/*
public sealed class GuidToByteValueConverter() //Useless because .Net is inversing half of Guid and when you manually(using EF) take it from db it doesn't fix
    : ValueConverter<Guid, byte[]>
        (  to => to.ToByteArray(),
        from => new Guid(from)  );*/
