using Quokka;
using Quokka.ListItems;
using System.Windows.Media.Imaging;


namespace Plugin_EnglishDictionary {

  class DefinitionItem : ListItem {

    internal string word;
    internal string example;
    internal string partOfSpeech;
    internal List<string> synonyms;
    internal List<string> antonyms;
    internal List<Phonetic> phonetics;

    public DefinitionItem(string word, string definition, string example, string partOfSpeech, List<string> synonyms, List<string> antonyms, List<Phonetic> phonetics) {
      this.Name = definition;
      this.Description = "Part of Speech: " + partOfSpeech;
      this.Icon = new BitmapImage(new Uri(
          Environment.CurrentDirectory + "\\PlugBoard\\Plugin_EnglishDictionary\\Plugin\\dictionary.png"));

      if (example != "") {
        this.Description += " | Example: " + example;
      }

      this.word = word;
      this.example = example;
      this.partOfSpeech = partOfSpeech;
      this.synonyms = synonyms;
      this.antonyms = antonyms;
      this.phonetics = phonetics;
    }

    public override void Execute() {
      System.Windows.Clipboard.SetText(Name);
      App.Current.MainWindow.Close();
    }
  }

}
