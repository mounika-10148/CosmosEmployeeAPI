using System.Text.Json.Serialization;
using Newtonsoft.Json;



namespace CosmosEmployeeAPI.Models
{
   public class Employee
{
    
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
}
}
