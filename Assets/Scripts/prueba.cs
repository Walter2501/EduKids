using GraphQL;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class prueba : MonoBehaviour
{
    private GraphS _graphQLService; //instancia mi clase GraphS para obtener los metodos necesarios para hacer consultas 
    public TextMeshProUGUI resultText; 


    void Start()
    {
        _graphQLService = new GraphS();
        FetchData().ConfigureAwait(false);
    }


    private string ParseData(dynamic data)
    {
        string result = "";
        foreach (var country in data.countries)
        {
            result += $"Country: {country.name}, Code: {country.code}, Continent: {country.continent.name}\n";
            result += "Languages: ";
            foreach (var language in country.languages)
            {
                result += $"{language.name} ";
            }
            result += "\n\n";
        }
        return result;
    }





    public async Task FetchData()
    {
        var request = new GraphQLRequest
        {
            Query = @"
            {
              countries {
                code
                name
                continent {
                  name
                }
                languages {
                  name
                }
              }
            }"
        };
        var result = await _graphQLService.ExecuteCustomQuery(request);
        if (result != null)
        {
            string formattedResult = ParseData(result);
            resultText.text = formattedResult;
        }
    }

    /* 
     * 
     *                     EJEMPLO DE COMO USAR UN MUTATION 
     *
     * 
             public async Task PerformMutation(string nombres, string apellidos, string estado, int totalmeritos)
        {
            var mutation = new GraphQLRequest
            {
                Query = $@"
                mutation Edukid {{
                  edukid {{
                    usuarioCreate(input: {{apellidos: ""{apellidos}"", estado: ""{estado}"", nombres: ""{nombres}"", totalmeritos: {totalmeritos}}}) {{
                      returning {{
                        nombres
                        apellidos
                        id
                      }}
                      rowCount
                    }}
                  }}
                }}"
            };
            
                ------------ el var sólo es para poder validar si es nulo o no la acción realizada  --------------
            var result = await _graphQLService.ExecuteCustomMutation(mutation);



                ------- Esto es para que sea vea en todo caso en una caja de texto, osea es visual no es necesario para la funcionalidad de la mutación ---------
            if (result != null)
            {
                string formattedResult = $"User Created: {result.edukid.usuarioCreate.returning[0].nombres} {result.edukid.usuarioCreate.returning[0].apellidos}, ID: {result.edukid.usuarioCreate.returning[0].id}";
                resultText.text = formattedResult;
            }
     */

}
