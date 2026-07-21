using System.Collections.ObjectModel;

namespace PluginEnglishDictionary
{

  /// <summary>
  /// a definition for the word
  /// </summary>
  public class Definition
  {
    /// <summary>
    /// a definition for the word
    /// </summary>
#pragma warning disable IDE1006 // Naming Styles - matches API response
    public string definition { get; set; } = "";

    /// <summary>
    /// synonyms for the word in this context
    /// </summary>
    public Collection<string> synonyms { get; } = new();
    /// <summary>
    /// antonyms for the word in this context
    /// </summary>
    public Collection<string> antonyms { get; } = new();
    /// <summary>
    /// relevant example of the word being used in this context
    /// </summary>
    public string example { get; set; } = "";
  }

  /// <summary>
  /// A License, data is under
  /// </summary>
  public class License
  {
    /// <summary>
    /// name of license
    /// </summary>
    public string name { get; set; } = "";
    /// <summary>
    /// link to license details
    /// </summary>
#pragma warning disable CA1056 // Uri properties should not be strings
    public string url { get; set; } = "";
#pragma warning restore CA1056
  }

  /// <summary>
  /// The different definitions of the word when used in a specific part of speech
  /// </summary>
  public class Meaning
  {
    /// <summary>
    /// a part of speech this word can be used in
    /// </summary>
    public string partOfSpeech { get; set; } = "";
    /// <summary>
    /// definitions for the word when it is used in this part of speech
    /// </summary>
    public Collection<Definition> definitions { get; } = new();
    /// <summary>
    /// synonyms for the word in this context
    /// </summary>
    public Collection<string> synonyms { get; } = new();
    /// <summary>
    /// antonyms for the word in this context
    /// </summary>
    public Collection<string> antonyms { get; } = new();
  }

  /// <summary>
  /// A pronunciation of the word
  /// </summary>
  public class Phonetic
  {
    /// <summary>
    /// Phonetic notation for this pronunciation
    /// </summary>
    public string text { get; set; } = "";
    /// <summary>
    /// A link to an audio file to hear the pronunciation
    /// </summary>
    public string audio { get; set; } = "";
    /// <summary>
    /// The source of the data
    /// </summary>
#pragma warning disable CA1056 // Uri properties should not be strings
    public string sourceUrl { get; set; } = "";
#pragma warning restore CA1056
    /// <summary>
    /// The License the data is under
    /// </summary>
    public License license { get; set; } = new();
  }

  /// <summary>
  /// Part of the response from the dictionary API, representing a single word for a specific part of speech
  /// </summary>
  public class ApiResponse
  {
    /// <summary>
    /// The word defined
    /// </summary>
    public string word { get; set; } = "";
    /// <summary>
    /// Phonetic notation of the word
    /// </summary>
    public string phonetic { get; set; } = "";
    /// <summary>
    /// Different phonetic notations for different pronunciations
    /// </summary>
    public Collection<Phonetic> phonetics { get; } = new();
    /// <summary>
    /// The different definitions of the word when used in a specific part of speech
    /// </summary>
    public Collection<Meaning> meanings { get; } = new();
    /// <summary>
    /// The License the data is under
    /// </summary>
    public License license { get; set; } = new();
    /// <summary>
    /// The source of the data
    /// </summary>
    public Collection<string> sourceUrls { get; } = new();
  }
#pragma warning restore IDE1006 // Naming Styles
}
