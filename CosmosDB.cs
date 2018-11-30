using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubProcessingNet
{
    class CosmosDB
    {
        //FROM THE DB,SELECT KEYS , SELECT THE PRIMARY KEY AND ENDPOINT URL
        private const string EndpointUrl = "https://temperature-simulated.documents.azure.com:443/";
        private const string PrimaryKey = "fOTVcm4kjxUJm3qjQxOIYRwwQm9fTaX70Rq327U14rUscaFE9MuLsSrIY7BqQNZUTQjmyTh90Bj4CS7RJ76A0g==";

        //names that are decided
        private const string DBName = "TemperatureOutputDB";
        private const string DBCollectionName = "temperatureCollection";

        //Create a client to communicate with API
        private DocumentClient client;

        public  async Task Init()
        {
            try
            {
                //initiate the client
                client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
                await client.CreateDatabaseIfNotExistsAsync(new Database { Id = DBName });
                await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DBName), new DocumentCollection { Id = DBCollectionName });
                
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("DB Client successfully connected");
                
            }
            
           
        }
        public  async Task pushTemperatureDocument(MessageBody payload)
        {
            try

            {
                Console.WriteLine("entering into method");
                await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DBName, DBCollectionName), payload);
                Console.WriteLine("exit from method");
            }
            catch (DocumentClientException de)

            {
                Console.WriteLine("The exception is {0}", de);
            }
        }

        

    }
}
