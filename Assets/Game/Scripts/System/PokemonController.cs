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

    public Pokemon pokemon;

    Texture2D texture;
    private void Start()
    {
        texture = new Texture2D(96, 96);
    }

    public void Search()
    {
        if (inputField != null)
        {
            int value = 0;
            int.TryParse(inputField.text, out value);
            if (value > 0 && currentIndex != value)
            {
                currentIndex = value;
                pokemon.id = value;
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

        UnityWebRequest www = UnityWebRequest.Get("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/"+pokemon.id+".png");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            byte[] results = www.downloadHandler.data;
            texture.LoadImage(results);

            pokeImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

    }
    [System.Serializable]
    public class Pokemon
    {
        public int id;
        public string name;
        public PokeSprite sprites;
    }

    [System.Serializable]
    public class PokeSprite
    {
        public string front_default;
    }
}
