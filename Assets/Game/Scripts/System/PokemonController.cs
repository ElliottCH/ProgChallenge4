using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System.Net;
using System.IO;
using System.Threading.Tasks;

public class PokemonController : MonoBehaviour
{
    private string URL = "https://pokeapi.co/api/v2/pokemon/{0}/";

    public InputField inputField;

    public Image pokeImage;

    int currentIndex = -1;
    
    private bool isLookingForPokemon = false;
    // Start is called before the first frame update
    Texture2D tex;
    private void Start()
    {
        tex = new Texture2D(96, 96);
    }

    public void FixedUpdate()
    {
        if (inputField != null)
        {
            int value = 0;
            int.TryParse(inputField.text, out value);
            if (isLookingForPokemon == false && value > 0 && currentIndex != value)
            {
                currentIndex = value;
                isLookingForPokemon = true;
                StartCoroutine(ChangePokemonImage());
            }
        }
    }

    IEnumerator ChangePokemonImage()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                string.Format(URL, currentIndex)
            );

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();

        Pokemon pokemon = JsonUtility.FromJson<Pokemon>(jsonResponse);

        string name = pokemon.name;
        Debug.Log(name);

        UnityWebRequest www = UnityWebRequest.Get("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            byte[] results = www.downloadHandler.data;
            tex.LoadImage(results);

            pokeImage.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        isLookingForPokemon = false;
    }
}
