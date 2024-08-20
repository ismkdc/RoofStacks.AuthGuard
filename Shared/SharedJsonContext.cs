using System.Text.Json.Serialization;
using Shared.Models;

namespace Shared;

[JsonSerializable(typeof(Employee))]
[JsonSerializable(typeof(IReadOnlyCollection<Employee>))]
public partial class SharedJsonContext : JsonSerializerContext
{
}