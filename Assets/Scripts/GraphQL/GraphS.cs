using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Threading.Tasks;
using UnityEngine;
public class GraphS 
{
    private readonly GraphQLHttpClient _client;


    //Recibo el url de mi api, el endpoint es la url recibida
    public GraphS(string endpoint)
    {
        _client = new GraphQLHttpClient(endpoint, new NewtonsoftJsonSerializer());
    }


    //ejecuto la consulta a mi api y regreso la respuesta de la misma, necesita un valor GraphQLRequest para funcionar y el request es la query que le estare pasando
    public async Task<dynamic> ExecuteQueryAsync(GraphQLRequest request)
    {
        var response = await _client.SendQueryAsync<dynamic>(request).ConfigureAwait(false);
        if (response.Errors != null)
        {
            foreach (var error in response.Errors)
            {
                // Log error or handle it accordingly
                Debug.LogError(error.Message);
            }
            return null;
        }
        return response.Data;
    }


    //Manejo un query personalizado que le pasaré a mi metodo ExecuteQueryAsync, para funcionar necesito un parametro tipo GraphQLRequest query y retorno la data encontrada
    public async Task<dynamic> ExecuteCustomQuery(GraphQLRequest query)
    {
        return await ExecuteQueryAsync(query).ConfigureAwait(false);
    }
}
