using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Threading.Tasks;
using UnityEngine;
public class GraphS 
{
    private readonly GraphQLHttpClient _client;

    private static readonly string endpoint = "https://countries.trevorblades.com/"; //le paso una url estatica de mi api



    //Establesco la conexión con mi api
    public GraphS()
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

                Debug.LogError(error.Message);
            }
            return null;
        }
        return response.Data;
    }

    //Creo una mutación esto sirve para hacer crear,eliminar,editar valores en la bd, también me regresa algo de data si la mutacion lo necesitará 
    public async Task<dynamic> ExecuteMutationAsync(GraphQLRequest request)
    {
        var response = await _client.SendMutationAsync<dynamic>(request).ConfigureAwait(false);
        if (response.Errors != null)
        {
            foreach (var error in response.Errors)
            {

                UnityEngine.Debug.LogError(error.Message);
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

    //manejo mi mutacion personalizada y se la paso a mi metodo ExecuteMutationAsync, para funcionar necesito una GraphQLRequest mutation y retorno la respuesta 
    public async Task<dynamic> ExecuteCustomMutation(GraphQLRequest mutation)
    {
        return await ExecuteMutationAsync(mutation).ConfigureAwait(false);
    }
}
