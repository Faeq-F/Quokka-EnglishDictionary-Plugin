using Newtonsoft.Json;
using Quokka.ListItems;
using Quokka.PluginArch;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;

namespace PluginEnglishDictionary
{

  /// <summary>
  /// The English Dictionary Plugin
  /// </summary>
#pragma warning disable CA1711 // Type name ends in Dictionary
  public partial class EnglishDictionary : Plugin
  {

    private static PluginSettings pluginSettings = new();
    internal static PluginSettings PluginSettings { get => pluginSettings; set => pluginSettings = value; }

    /// <summary>
    /// Loads Plugin specific settings
    /// </summary>
    public EnglishDictionary()
    {
      string fileName = Environment.CurrentDirectory + "\\PlugBoard\\PluginEnglishDictionary\\Plugin\\settings.json";
      PluginSettings = JsonConvert.DeserializeObject<PluginSettings>(File.ReadAllText(fileName))!;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override string PluginName { get; set; } = "EnglishDictionary";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="query"><inheritdoc/></param>
    /// <returns>
    /// An empty collection - Using the command signifier is the only way to get a result from this plugin,
    /// as to not needlessly send queries to the dictionary API
    /// </returns>
    public override Collection<ListItem> OnQueryChange(string query) { return new Collection<ListItem>(); }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>
    /// The DictionarySignifier from plugin settings
    /// </returns>
    public override Collection<string> CommandSignifiers()
    {
      return new Collection<string>() { PluginSettings.DictionarySignifier };
    }

    private static Collection<ListItem> ParseDefinitions(string obj)
    {
      Collection<ListItem> definitions = new();
      List<ApiResponse> response = JsonConvert.DeserializeObject<List<ApiResponse>>(obj)!;
      foreach (ApiResponse word in response)
      {
        foreach (Meaning meaning in word.meanings)
        {
          foreach (Definition definition in meaning.definitions)
          {
            Collection<string> synonyms = new(definition.synonyms.Concat(meaning.synonyms).ToList());
            Collection<string> antonyms = new(definition.antonyms.Concat(meaning.antonyms).ToList());

            definitions.Add(
              new DefinitionItem(word: word.word, definition: definition.definition, example: definition.example,
              partOfSpeech: meaning.partOfSpeech, synonyms: synonyms,
              antonyms: antonyms, phonetics: word.phonetics));
          }
        }

      }
      return definitions;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="command">The DictionarySignifier (Since there is only 1 signifier for this plugin), followed by the word being defined</param>
    /// <returns>Collection of definitions that possibly match what is being searched for</returns>
    public override Collection<ListItem> OnSignifier(string command)
    {
      command ??= "";
      command = command.Substring(PluginSettings.DictionarySignifier.Length);
      try
      {
        var uri = new Uri("https://api.dictionaryapi.dev/api/v2/entries/en/" + command);
        WebRequest request = WebRequest.CreateHttp(uri);
        request.ContentType = "application/json; charset=utf-8";
        string definitions;
        var response = (HttpWebResponse)request.GetResponse();
        using (var sr = new StreamReader(response.GetResponseStream()))
        {
          definitions = sr.ReadToEnd();
        }
        return new Collection<ListItem>(FuzzySearch.Sort(command, ParseDefinitions(definitions)).ToList());
      }
      catch (Exception)
      {
        return new Collection<ListItem>();
      }
    }

  }

}
