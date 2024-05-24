using GraphQL;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class prueba : MonoBehaviour
{
    private GraphS _graphQLService; //instancia mi clase GraphS para obtener mis dos metodos 
    public TextMeshProUGUI resultText; // Referencia al objeto Text en la UI


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
}
