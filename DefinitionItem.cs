using Quokka;
using Quokka.ListItems;
using Quokka.PluginArch;


using System.Collections.ObjectModel;

namespace PluginEnglishDictionary
{

  sealed class DefinitionItem : ListItem
  {

    internal string Word { get; }
    internal string Example { get; }
    internal string PartOfSpeech { get; }
    internal Collection<string> Synonyms { get; }
    internal Collection<string> Antonyms { get; }
    internal Collection<Phonetic> Phonetics { get; }

    public DefinitionItem(string word, string definition, string example, string partOfSpeech,
                          Collection<string> synonyms, Collection<string> antonyms, Collection<Phonetic> phonetics)
    {
      Name = definition;
      Description = "Part of Speech: " + partOfSpeech;
      Icon = IconCache.GetOrAdd(
        Environment.CurrentDirectory + "\\PlugBoard\\PluginEnglishDictionary\\Plugin\\dictionary.png"
      );

      if (!string.IsNullOrEmpty(example))
      {
        Description += " | Example: " + example;
      }

      Word = word;
      Example = example;
      PartOfSpeech = partOfSpeech;
      Synonyms = synonyms;
      Antonyms = antonyms;
      Phonetics = phonetics;
    }

    public override void Execute()
    {
      System.Windows.Clipboard.SetText(Name);
      App.Current.MainWindow.Close();
    }
  }

}
