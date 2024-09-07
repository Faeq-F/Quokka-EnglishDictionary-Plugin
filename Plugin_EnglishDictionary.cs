using Newtonsoft.Json;
using Quokka.ListItems;
using Quokka.PluginArch;
using System.IO;
using System.Net;

namespace Plugin_EnglishDictionary {

  /// <summary>
  /// The English Dictionary Plugin
  /// </summary>
  public partial class EnglishDictionary : Plugin {

    private static PluginSettings pluginSettings = new();
    internal static PluginSettings PluginSettings { get => pluginSettings; set => pluginSettings = value; }

    /// <summary>
    /// Loads Plugin specific settings
    /// </summary>
    public EnglishDictionary() {
      string fileName = Environment.CurrentDirectory + "\\PlugBoard\\Plugin_EnglishDictionary\\Plugin\\settings.json";
      PluginSettings = JsonConvert.DeserializeObject<PluginSettings>(File.ReadAllText(fileName))!;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override string PluggerName { get; set; } = "EnglishDictionary";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="query"><inheritdoc/></param>
    /// <returns>
    /// An empty list - Using the command signifier is the only way to get a result from this plugin,
    /// as to not needlessly send queries to the dictionary API
    /// </returns>
    public override List<ListItem> OnQueryChange(string query) { return new List<ListItem>(); }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>
    /// The DictionarySignifier from plugin settings
    /// </returns>
    public override List<string> CommandSignifiers() {
      return new List<string>() { PluginSettings.DictionarySignifier };
    }

    private List<ListItem> parseDefinitions(string obj) {
      List<ListItem> definitions = new List<ListItem>();
      List<ApiResponse> response = JsonConvert.DeserializeObject<List<ApiResponse>>(obj)!;
      foreach (ApiResponse word in response) {
        foreach (Meaning meaning in word.meanings) {
          foreach (Definition definition in meaning.definitions) {
            List<string> synonyms = definition.synonyms;
            List<string> antonyms = definition.antonyms;
            synonyms.AddRange(meaning.synonyms);
            antonyms.AddRange(meaning.antonyms);
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
    /// <returns>List of definitions that possibly match what is being searched for</returns>
    public override List<ListItem> OnSignifier(string command) {
      command = command.Substring(PluginSettings.DictionarySignifier.Length);
      try {
        WebRequest request = WebRequest.CreateHttp("https://api.dictionaryapi.dev/api/v2/entries/en/" + command);
        request.ContentType = "application/json; charset=utf-8";
        string definitions;
        var response = (HttpWebResponse) request.GetResponse();
        using (var sr = new StreamReader(response.GetResponseStream())) {
          definitions = sr.ReadToEnd();
        }
        return parseDefinitions(definitions);
      } catch (Exception) {
        return new List<ListItem>();
      }
    }

  }

}
