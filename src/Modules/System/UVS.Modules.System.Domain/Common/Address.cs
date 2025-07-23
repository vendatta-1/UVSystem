using Microsoft.EntityFrameworkCore;

namespace UVS.Domain.Common;

[Owned]
public sealed record Address(string City, string ZipCode, string? Town);