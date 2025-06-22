using Microsoft.Azure.Cosmos;
using CosmosEmployeeAPI.Models;

namespace CosmosEmployeeAPI.Services
{
    public class CosmosDbService
    {
        private Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

  public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        // IMPORTANT: Pass partition key value matching employee.Department
        var response = await _container.CreateItemAsync(employee, new PartitionKey(employee.Department));
        return response.Resource;
    }



        public async Task<Employee> GetEmployeeAsync(string id, string department)
        {
            try
            {
                ItemResponse<Employee> response = await _container.ReadItemAsync<Employee>(id, new PartitionKey(department));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(string department)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.department = @department")
                .WithParameter("@department", department);

            FeedIterator<Employee> resultSet = _container.GetItemQueryIterator<Employee>(query);

            List<Employee> employees = new List<Employee>();

            while (resultSet.HasMoreResults)
            {
                FeedResponse<Employee> response = await resultSet.ReadNextAsync();
                employees.AddRange(response);
            }
            return employees;
        }

        public async Task UpdateEmployeeAsync(string id, Employee employee)
        {
            await _container.UpsertItemAsync(employee, new PartitionKey(employee.Department));
        }

        public async Task DeleteEmployeeAsync(string id, string department)
        {
            await _container.DeleteItemAsync<Employee>(id, new PartitionKey(department));
        }
    }
}
